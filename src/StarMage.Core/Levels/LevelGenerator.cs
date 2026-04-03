using Microsoft.Xna.Framework;

using StarMage.Core.Actors;
using StarMage.Core.Databases;
using StarMage.Core.Enums;
using StarMage.Core.Handlers;
using StarMage.Core.Managers;
using StarMage.Core.Tiles;

using System;
using System.Collections.Generic;

namespace StarMage.Core.Levels
{
    internal static class LevelGenerator
    {
        #region HELPER ENUMS

        [Flags]
        private enum SpawnLocations : byte
        {
            None = 0,
            Ground = 1 << 0,
            LeftWall = 1 << 1,
            RightWall = 1 << 2,
            Ceiling = 1 << 3,
            Aerial = 1 << 4,
            All = Ground | LeftWall | RightWall | Ceiling | Aerial
        }

        #endregion

        #region HELPER CLASSES

        private sealed class EnemyProfile(ActorIndex index, SpawnLocations spawnLocations, int spawnChance, Func<bool> canAllocate)
        {
            internal ActorIndex Index => index;
            internal SpawnLocations SpawnLocations => spawnLocations;
            internal int SpawnChance => spawnChance;
            internal bool CanAllocate => canAllocate();
        }
        private sealed class SpawnPool(SpawnLocations locations, List<Point> positions)
        {
            internal SpawnLocations Locations => locations;
            internal List<Point> Positions => positions;
        }
        private sealed class AllocationEntry<T>(T item, int weight, int capacity) where T : notnull
        {
            internal T Item { get; } = item;
            internal int Weight { get; } = weight;
            internal int Capacity { get; } = capacity;
            internal int Count { get; set; }
            internal double Remainder { get; set; }
        }
        private sealed class LevelSpawnData
        {
            internal List<Point> BigCrystalPositions { get; } = [];
            internal List<Point> TreasureChestPositions { get; } = [];

            internal List<Point> GroundEnemyPositions { get; } = [];
            internal List<Point> CeilingEnemyPositions { get; } = [];
            internal List<Point> LeftWallEnemyPositions { get; } = [];
            internal List<Point> RightWallEnemyPositions { get; } = [];
            internal List<Point> AerialEnemyPositions { get; } = [];

            internal int StartingStarHorizontalPosition { get; set; }
            internal Point FinishingStarPosition { get; set; }
            internal Point BossSpawnPosition { get; set; }

            private static void SortByX(List<Point> positions)
            {
                positions.Sort((a, b) => a.X.CompareTo(b.X));
            }

            internal void Sort()
            {
                SortByX(this.BigCrystalPositions);
                SortByX(this.TreasureChestPositions);
                SortByX(this.GroundEnemyPositions);
                SortByX(this.CeilingEnemyPositions);
                SortByX(this.LeftWallEnemyPositions);
                SortByX(this.RightWallEnemyPositions);
                SortByX(this.AerialEnemyPositions);
            }
        }

        #endregion

        private static readonly EnemyProfile[] enemyProfiles =
        [
            new EnemyProfile(ActorIndex.FloatingBall, SpawnLocations.Aerial, 32, () => GameHandler.CurrentLevelIndex >= 13),
            new EnemyProfile(ActorIndex.HappyMask, SpawnLocations.Aerial, 64, () => GameHandler.CurrentLevelIndex >= 3),
            new EnemyProfile(ActorIndex.Slime, SpawnLocations.Ground, 96, () => true),
            new EnemyProfile(ActorIndex.Spino, SpawnLocations.Ground | SpawnLocations.LeftWall | SpawnLocations.RightWall | SpawnLocations.Ceiling, 64, () => GameHandler.CurrentLevelIndex >= 5),
            new EnemyProfile(ActorIndex.Spitter, SpawnLocations.Ground | SpawnLocations.LeftWall | SpawnLocations.RightWall | SpawnLocations.Ceiling, 64, () =>  GameHandler.CurrentLevelIndex >= 9)
        ];

        #region HELPER METHODS

        private static bool IsCompatible(SpawnLocations allowedLocations, SpawnLocations location)
        {
            return (allowedLocations & location) != SpawnLocations.None;
        }
        private static List<SpawnPool> GetCompatiblePools(EnemyProfile profile, List<SpawnPool> pools)
        {
            List<SpawnPool> result = [];

            foreach (SpawnPool pool in pools)
            {
                if (pool.Positions.Count > 0 && IsCompatible(profile.SpawnLocations, pool.Locations))
                {
                    result.Add(pool);
                }
            }

            return result;
        }
        private static int GetCompatiblePoolCount(EnemyProfile profile, List<SpawnPool> pools)
        {
            int count = 0;

            foreach (SpawnPool pool in pools)
            {
                if (pool.Positions.Count > 0 && IsCompatible(profile.SpawnLocations, pool.Locations))
                {
                    count++;
                }
            }

            return count;
        }
        private static int GetCompatiblePositionCount(EnemyProfile profile, List<SpawnPool> pools)
        {
            int count = 0;

            foreach (SpawnPool pool in pools)
            {
                if (pool.Positions.Count > 0 && IsCompatible(profile.SpawnLocations, pool.Locations))
                {
                    count += pool.Positions.Count;
                }
            }

            return count;
        }
        private static Dictionary<T, int> AllocateWeightedCounts<T>(int totalCount, IReadOnlyList<T> items, Func<T, int> weightSelector, Func<T, int> capacitySelector) where T : notnull
        {
            Dictionary<T, int> allocations = [];

            // Return empty allocations if there are no items or a zero count is requested
            if (totalCount <= 0 || items.Count == 0)
            {
                return allocations;
            }

            List<AllocationEntry<T>> entries = [];
            int totalWeight = 0;
            int totalCapacity = 0;

            // Calculate active capacities and weights, filtering out invalid items
            foreach (T item in items)
            {
                int weight = Math.Max(0, weightSelector(item));
                int capacity = Math.Max(0, capacitySelector(item));

                if (weight <= 0 || capacity <= 0)
                {
                    continue;
                }

                entries.Add(new AllocationEntry<T>(item, weight, capacity));
                totalWeight += weight;
                totalCapacity += capacity;
            }

            // If no valid entries are left, return empty allocations
            if (entries.Count == 0 || totalWeight <= 0 || totalCapacity <= 0)
            {
                return allocations;
            }

            // Ensure we do not try to allocate more than the maximum capacity
            totalCount = Math.Min(totalCount, totalCapacity);

            int assigned = 0;

            // Phase 1: Distribute based on ideal mathematical ratio
            foreach (AllocationEntry<T> entry in entries)
            {
                double ideal = totalCount * (entry.Weight / (double)totalWeight);
                entry.Count = Math.Min(entry.Capacity, (int)Math.Floor(ideal));
                entry.Remainder = ideal - entry.Count;

                allocations[entry.Item] = entry.Count;
                assigned += entry.Count;
            }

            int remaining = totalCount - assigned;

            // Phase 2: Distribute remaining counts based on the highest fractional remainder
            while (remaining > 0)
            {
                AllocationEntry<T> best = null;

                foreach (AllocationEntry<T> entry in entries)
                {
                    if (entry.Count >= entry.Capacity || entry.Remainder <= 0)
                    {
                        continue;
                    }

                    if (best is null ||
                        entry.Remainder > best.Remainder ||
                        (Math.Abs(entry.Remainder - best.Remainder) < 0.000001 && entry.Weight > best.Weight))
                    {
                        best = entry;
                    }
                }

                if (best is null)
                {
                    break;
                }

                best.Count++;
                best.Remainder = 0; // Prevent duplicate allocation for the same remainder
                allocations[best.Item] = best.Count;
                remaining--;
            }

            // Phase 3: Fallback distribution for any further remaining counts using a score ratio
            while (remaining > 0)
            {
                AllocationEntry<T> best = null;
                double bestScore = double.NegativeInfinity;

                foreach (AllocationEntry<T> entry in entries)
                {
                    if (entry.Count >= entry.Capacity)
                    {
                        continue;
                    }

                    double score = entry.Weight / (double)(entry.Count + 1);

                    if (score > bestScore)
                    {
                        bestScore = score;
                        best = entry;
                    }
                }

                // Break early if we cannot allocate anymore
                if (best is null)
                {
                    break;
                }

                best.Count++;
                allocations[best.Item] = best.Count;
                remaining--;
            }

            return allocations;
        }
        private static List<int> GetEvenlySpacedIndices(int count, int selectionCount)
        {
            List<int> indices = [];

            // Return early if there is nothing to select
            if (count <= 0 || selectionCount <= 0)
            {
                return indices;
            }

            // Cap the selection count to the available number of items
            selectionCount = Math.Min(selectionCount, count);

            // Special case for a single selection: pick the exact middle
            if (selectionCount == 1)
            {
                indices.Add(count / 2);
                return indices;
            }

            // Calculate the step size between each chosen index
            float step = (count - 1) / (float)(selectionCount - 1);
            bool[] used = new bool[count];

            for (int i = 0; i < selectionCount; i++)
            {
                // Find the nearest target index
                int index = (int)MathF.Round(i * step);
                index = Math.Clamp(index, 0, count - 1);

                // If the index is already used, find the nearest available neighbor
                if (used[index])
                {
                    int left = index - 1;
                    int right = index + 1;

                    while (left >= 0 || right < count)
                    {
                        if (left >= 0 && !used[left])
                        {
                            index = left;
                            break;
                        }

                        if (right < count && !used[right])
                        {
                            index = right;
                            break;
                        }

                        left--;
                        right++;
                    }
                }

                // Mark the index as used and add it to the final result
                used[index] = true;
                indices.Add(index);
            }

            indices.Sort();
            return indices;
        }

        #endregion

        #region MAIN GENERATION LOGIC

        private static void InitializeLevel(ActorManager actorManager, TileMap tileMap, int horizontalChunkCount)
        {
            tileMap.Resize(Constants.CHUNK_WIDTH_TILES * horizontalChunkCount, Constants.CHUNK_HEIGHT_TILES);
            actorManager.Clear(x => x.Type is ActorType.Player);

            int currentChunkX = 0;

            // Generate the level layout by applying chunks to the tile map
            ChunkDatabase.GetRandomChunk(x => x.Type is ChunkType.Beginning).ApplyToTileMap(tileMap, currentChunkX, 0);
            currentChunkX += Constants.CHUNK_WIDTH_TILES;

            for (int i = 0; i < horizontalChunkCount - 2; i++)
            {
                ChunkDatabase.GetRandomChunk(x => x.Type is ChunkType.Normal).ApplyToTileMap(tileMap, currentChunkX, 0);
                currentChunkX += Constants.CHUNK_WIDTH_TILES;
            }

            // Every 4th level features an arena chunk as the climax, while
            // the others feature an ending chunk that likely serves as a transition
            // to the next segment of the game, creating a pacing rhythm where players
            // can expect a major challenge every 4 levels followed by a resolution or
            // change of scenery in the subsequent levels
            if (GameHandler.CurrentLevelIndex % 4 == 0)
            {
                ChunkDatabase.GetRandomChunk(x => x.Type is ChunkType.Arena).ApplyToTileMap(tileMap, currentChunkX, 0);
            }
            else
            {
                ChunkDatabase.GetRandomChunk(x => x.Type is ChunkType.Ending).ApplyToTileMap(tileMap, currentChunkX, 0);
            }
        }
        private static void ProcessSpawnTile(LevelSpawnData spawnData, TileType tile, Point worldPosition)
        {
            switch (tile)
            {
                case TileType.C01:
                    spawnData.BigCrystalPositions.Add(worldPosition);
                    break;

                case TileType.C02:
                    spawnData.TreasureChestPositions.Add(worldPosition);
                    break;

                case TileType.C03:
                    spawnData.StartingStarHorizontalPosition = worldPosition.X;
                    break;

                case TileType.C04:
                    spawnData.FinishingStarPosition = worldPosition;
                    break;

                case TileType.E01:
                    spawnData.GroundEnemyPositions.Add(worldPosition);
                    break;

                case TileType.E02:
                    spawnData.CeilingEnemyPositions.Add(worldPosition);
                    break;

                case TileType.E03:
                    spawnData.LeftWallEnemyPositions.Add(worldPosition);
                    break;

                case TileType.E04:
                    spawnData.RightWallEnemyPositions.Add(worldPosition);
                    break;

                case TileType.E05:
                    spawnData.AerialEnemyPositions.Add(worldPosition);
                    break;

                case TileType.E06:
                    spawnData.BossSpawnPosition = worldPosition;
                    break;
            }
        }
        private static LevelSpawnData CollectSpawnData(TileMap tileMap)
        {
            LevelSpawnData spawnData = new();

            for (int y = 0; y < tileMap.Height; y++)
            {
                for (int x = 0; x < tileMap.Width; x++)
                {
                    ProcessSpawnTile(spawnData, tileMap[x, y], new(x * Constants.TILE_SIZE, y * Constants.TILE_SIZE));
                }
            }

            spawnData.Sort();
            return spawnData;
        }
        private static void SpawnCoreActors(ActorManager actorManager, LevelSpawnData spawnData)
        {
            // Player
            actorManager.GetActor(ActorIndex.MagePlayer).Position = Point.Zero;

            // Starting Star
            if (actorManager.TryCreate(ActorIndex.StartingStar, out Actor startingStarActor))
            {
                startingStarActor.PositionX = spawnData.StartingStarHorizontalPosition;
                startingStarActor.PositionY = -32;
            }

            // Finishing Star (only spawn if the level index is not a multiple of 4,
            // which creates a pacing rhythm where every 4th level has no finishing star
            // and likely relies on other mechanics to complete)
            if (GameHandler.CurrentLevelIndex % 4 != 0 && actorManager.TryCreate(ActorIndex.FinishingStar, out Actor finishingStarActor))
            {
                finishingStarActor.Position = spawnData.FinishingStarPosition;
            }
        }
        private static void SpawnBigCrystals(ActorManager actorManager, List<Point> availablePositions)
        {
            int availablePositionsCount = availablePositions.Count;

            if (Constants.DEFAULT_BIG_CRYSTALS_PER_LEVEL <= 0 || availablePositionsCount <= 0)
            {
                return;
            }

            int crystalCount = Math.Min(Constants.DEFAULT_BIG_CRYSTALS_PER_LEVEL, availablePositionsCount);
            int startIndex = 0;
            int endIndex = availablePositionsCount - 1;

            if (availablePositionsCount >= crystalCount + 2)
            {
                startIndex = 1;
                endIndex = availablePositionsCount - 2;
            }

            int availableRangeSize = endIndex - startIndex + 1;

            // If there's only one crystal to place, put it in the middle
            // of the available range for better visual balance
            if (crystalCount == 1)
            {
                int middleIndex = startIndex + (availableRangeSize / 2);

                if (actorManager.TryCreate(ActorIndex.BigCrystal, out Actor bigCrystal))
                {
                    bigCrystal.Position = availablePositions[middleIndex];
                }

                return;
            }

            // Distribute crystals evenly across the available range of positions,
            // rounding to the nearest index to avoid bias towards the start
            float step = (availableRangeSize - 1) / (float)(crystalCount - 1);

            for (int i = 0; i < crystalCount; i++)
            {
                int index = startIndex + (int)MathF.Round(i * step);

                if (actorManager.TryCreate(ActorIndex.BigCrystal, out Actor bigCrystal))
                {
                    bigCrystal.Position = availablePositions[index];
                }
            }
        }
        private static void SpawnTeasureChest(ActorManager actorManager, List<Point> availablePositions)
        {
            if (availablePositions.Count == 0)
            {
                return;
            }

            int index = availablePositions.Count / 2;

            if (actorManager.TryCreate(ActorIndex.TreasureChest, out Actor treasureChest))
            {
                treasureChest.Position = availablePositions[index];
            }
        }
        private static void SpawnEnemies(ActorManager actorManager, float spawnRate, LevelSpawnData spawnData)
        {
            List<SpawnPool> spawnPools =
            [
                new(SpawnLocations.Ground, spawnData.GroundEnemyPositions),
                new(SpawnLocations.LeftWall, spawnData.LeftWallEnemyPositions),
                new(SpawnLocations.RightWall, spawnData.RightWallEnemyPositions),
                new(SpawnLocations.Ceiling, spawnData.CeilingEnemyPositions),
                new(SpawnLocations.Aerial, spawnData.AerialEnemyPositions)
            ];

            List<EnemyProfile> availableEnemyProfiles = [];

            // Get list of enemy profiles that can be allocated
            // and have at least one compatible spawn position
            // available
            foreach (EnemyProfile enemyProfile in enemyProfiles)
            {
                if (enemyProfile.CanAllocate && GetCompatiblePositionCount(enemyProfile, spawnPools) > 0)
                {
                    availableEnemyProfiles.Add(enemyProfile);
                }
            }

            // If there are no valid enemy profiles or spawn positions,
            // skip enemy spawning
            if (availableEnemyProfiles.Count == 0)
            {
                return;
            }

            // Calculate total available spawn positions and total spawn
            // capacity based on compatible profiles and pools to determine
            // how many enemies can actually be spawned
            int totalAvailableEnemyPositions = 0;

            foreach (SpawnPool spawnPool in spawnPools)
            {
                totalAvailableEnemyPositions += spawnPool.Positions.Count;
            }

            int totalSpawnCapacity = 0;

            foreach (EnemyProfile enemyProfile in availableEnemyProfiles)
            {
                totalSpawnCapacity += GetCompatiblePositionCount(enemyProfile, spawnPools);
            }

            // If there are no available spawn positions or the spawn capacity
            // is zero (which could happen if all profiles have zero spawn chance),
            // skip spawning to avoid unnecessary calculations
            if (totalAvailableEnemyPositions <= 0 || totalSpawnCapacity <= 0)
            {
                return;
            }

            // Determine the maximum number of enemies that can be spawned based on
            // available positions and spawn capacity, then calculate the desired
            // enemy count using the spawn rate while ensuring it's at least 1 and
            // does not exceed the maximum
            int maxEnemyCount = Math.Min(totalAvailableEnemyPositions, totalSpawnCapacity);
            int desiredEnemyCount = Math.Clamp((int)MathF.Round(totalAvailableEnemyPositions * spawnRate), 1, maxEnemyCount);

            Dictionary<EnemyProfile, int> profileAllocations = AllocateWeightedCounts(
                desiredEnemyCount,
                availableEnemyProfiles,
                profile => profile.SpawnChance,
                profile => GetCompatiblePositionCount(profile, spawnPools)
            );

            // Sort enemy profiles by the number of compatible spawn
            // positions (ascending) and then by spawn chance (descending)
            availableEnemyProfiles.Sort((a, b) =>
            {
                int aCapacity = GetCompatiblePositionCount(a, spawnPools);
                int bCapacity = GetCompatiblePositionCount(b, spawnPools);

                int result = aCapacity.CompareTo(bCapacity);
                if (result != 0)
                {
                    return result;
                }

                result = GetCompatiblePoolCount(a, spawnPools).CompareTo(GetCompatiblePoolCount(b, spawnPools));
                return result != 0 ? result : b.SpawnChance.CompareTo(a.SpawnChance);
            });

            // Allocate enemies to spawn pools based on the profile allocations and
            // the number of compatible positions in each pool, then spawn the
            // allocated enemies at evenly spaced positions within their assigned pools
            foreach (EnemyProfile enemyProfile in availableEnemyProfiles)
            {
                // If the profile allocation is zero or negative (which could happen if the
                // spawn chance is very low and gets rounded down), skip to the next profile
                if (!profileAllocations.TryGetValue(enemyProfile, out int enemyCount) || enemyCount <= 0)
                {
                    continue;
                }

                // Get list of spawn pools that are compatible with the enemy profile and
                // have available positions
                List<SpawnPool> compatiblePools = GetCompatiblePools(enemyProfile, spawnPools);

                if (compatiblePools.Count == 0)
                {
                    continue;
                }

                // Allocate the profile's enemies to the compatible spawn pools based on
                // the number of available positions in each pool, then spawn the allocated
                // enemies at evenly spaced positions within their assigned pools
                Dictionary<SpawnPool, int> poolAllocations = AllocateWeightedCounts(
                    enemyCount,
                    compatiblePools,
                    pool => pool.Positions.Count,
                    pool => pool.Positions.Count
                );

                // Iterate through the compatible spawn pools in reverse order to
                // prioritize pools with fewer compatible profiles, and spawn the
                // allocated enemies at evenly spaced positions within each pool
                foreach (SpawnPool spawnPool in compatiblePools)
                {
                    if (!poolAllocations.TryGetValue(spawnPool, out int poolEnemyCount) || poolEnemyCount <= 0 || spawnPool.Positions.Count == 0)
                    {
                        continue;
                    }

                    List<int> selectedIndices = GetEvenlySpacedIndices(spawnPool.Positions.Count, poolEnemyCount);

                    for (int i = selectedIndices.Count - 1; i >= 0; i--)
                    {
                        int index = selectedIndices[i];
                        Point spawnPosition = spawnPool.Positions[index];
                        spawnPool.Positions.RemoveAt(index);

                        if (actorManager.TryCreate(enemyProfile.Index, out Actor enemyActor))
                        {
                            enemyActor.Position = spawnPosition;
                        }
                    }
                }
            }
        }
        private static void SpawnBoss(ActorManager actorManager, Point spawnPosition)
        {
            // Only spawn the boss if the current level index is a multiple of 4,
            // which creates a pacing rhythm where every 4th level features a boss
            // encounter that likely serves as a climax or major challenge for that
            // segment of the game
            if (GameHandler.CurrentLevelIndex % 4 != 0)
            {
                return;
            }

            if (actorManager.TryCreate(ActorIndex.GreatDarkWizard, out Actor bossActor))
            {
                bossActor.Position = spawnPosition;
            }
        }

        internal static LevelProfile Generate(ActorManager actorManager, TileMap tileMap)
        {
            int horizontalChunkCount = int.Clamp(7 + (GameHandler.CurrentLevelIndex / 2), 7, 32);
            float spawnRate = 0.265f + (GameHandler.CurrentLevelIndex * 0.01f);

            InitializeLevel(actorManager, tileMap, horizontalChunkCount);

            LevelSpawnData spawnData = CollectSpawnData(tileMap);

            SpawnCoreActors(actorManager, spawnData);
            SpawnBigCrystals(actorManager, spawnData.BigCrystalPositions);

            if (GameHandler.CurrentLevelIndex % 2 == 0)
            {
                SpawnTeasureChest(actorManager, spawnData.TreasureChestPositions);
            }

            SpawnEnemies(actorManager, spawnRate, spawnData);
            SpawnBoss(actorManager, spawnData.BossSpawnPosition);

            return new(horizontalChunkCount);
        }

        #endregion
    }
}
