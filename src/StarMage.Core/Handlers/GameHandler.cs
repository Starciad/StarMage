using Microsoft.Xna.Framework;

using StarMage.Core.Actors.Players;
using StarMage.Core.Enums;
using StarMage.Core.Levels;
using StarMage.Core.Managers;
using StarMage.Core.Tiles;

using System;

namespace StarMage.Core.Handlers
{
    internal static class GameHandler
    {
        internal static Scene CurrentScene { get; set; }
        internal static LevelIndex CurrentLevelIndex { get; private set; }
        internal static int TotalRemainingSeconds { get; private set; }
        internal static int BigCrystalCollected { get; private set; }
        internal static bool IsGameOver { get; private set; }
        internal static bool IsFreezeActive { get; private set; }
        internal static bool IsFreezeTemporary { get; private set; }
        internal static bool ShouldCameraFollowPlayer { get; private set; }

        private static bool bossAppeared;

        private static float cameraMovementDelay;
        private static float freezeDuration;
        private static float secondsAccumulator;

        private static LevelProfile levelProfile;

        private static MagePlayer magePlayer;
        private static ActorManager actorManager;
        private static TileMap tileMap;

        private static int GetLevelTime()
        {
            return Constants.DEFAULT_LEVEL_SECONDS + (CurrentLevelIndex * 5) + (CurrentLevelIndex.LevelInWorld % 4 == 0 ? Constants.DEFAULT_LEVEL_SECONDS : 0);
        }

        internal static void Initialize(MagePlayer magePlayer, ActorManager actorManager, TileMap tileMap)
        {
            IsGameOver = false;
            CurrentLevelIndex = new();
            TotalRemainingSeconds = GetLevelTime();
            BigCrystalCollected = 0;
            ShouldCameraFollowPlayer = true;
            bossAppeared = false;

            GameHandler.magePlayer = magePlayer;
            GameHandler.actorManager = actorManager;
            GameHandler.tileMap = tileMap;

            magePlayer.Reset();
            magePlayer.CanUpdate = false;
            magePlayer.CanDraw = false;

            levelProfile = LevelGenerator.Generate(actorManager, tileMap);
        }

        internal static void NextLevel()
        {
            CurrentLevelIndex++;
            BigCrystalCollected = 0;
            TotalRemainingSeconds = GetLevelTime();
            ShouldCameraFollowPlayer = true;
            bossAppeared = false;

            magePlayer.PrepareForLevel();
            magePlayer.CanUpdate = false;
            magePlayer.CanDraw = false;

            levelProfile = LevelGenerator.Generate(actorManager, tileMap);
        }

        private static void UpdateTimer(float deltaTime)
        {
            secondsAccumulator += deltaTime;

            if (secondsAccumulator > 1.0f && TotalRemainingSeconds > Constants.MINIMUM_LEVEL_SECONDS)
            {
                TotalRemainingSeconds--;
                secondsAccumulator = 0.0f;

                if (TotalRemainingSeconds <= Constants.MINIMUM_LEVEL_SECONDS)
                {
                    TotalRemainingSeconds = Constants.MINIMUM_LEVEL_SECONDS;
                    magePlayer.Kill();
                }
            }
        }

        private static void UpdateFreeze(float deltaTime)
        {
            if (IsFreezeActive && IsFreezeTemporary)
            {
                freezeDuration -= deltaTime;
                if (freezeDuration <= 0.0f)
                {
                    IsFreezeActive = false;
                    freezeDuration = 0.0f;
                }
            }
        }

        private static void UpdateBossAppearance(float deltaTime)
        {
            cameraMovementDelay += deltaTime;

            if (!bossAppeared || cameraMovementDelay < 1.25f)
            {
                return;
            }

            int totalLevelWidth = (levelProfile.HorizontalChunkCount - 1) * Constants.CHUNK_WIDTH_TILES * Constants.TILE_SIZE;

            Point cameraPosition = CameraHandler.Position;
            cameraPosition.X = Math.Min(cameraPosition.X + 1, totalLevelWidth);

            if (cameraPosition.X == totalLevelWidth)
            {
                bossAppeared = false;
                IsFreezeActive = false;
            }

            CameraHandler.Position = cameraPosition;
        }

        internal static void Update(GameTime gameTime, MagePlayer magePlayer)
        {
            if (IsGameOver)
            {
                return;
            }

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            UpdateTimer(deltaTime);
            UpdateFreeze(deltaTime);
            UpdateBossAppearance(deltaTime);
        }

        internal static void CollectBigCrystal()
        {
            BigCrystalCollected++;
        }

        internal static void GameOver()
        {
            IsGameOver = true;
        }

        internal static void Freeze()
        {
            IsFreezeActive = true;
            IsFreezeTemporary = false;
            freezeDuration = 0.0f;
        }

        internal static void Freeze(float duration)
        {
            IsFreezeActive = true;
            IsFreezeTemporary = true;
            freezeDuration = duration;
        }

        internal static void BossAppear()
        {
            Freeze();
            bossAppeared = true;
            ShouldCameraFollowPlayer = false;
        }

        internal static void BossDefeated()
        {
            bossAppeared = false;
        }
    }
}
