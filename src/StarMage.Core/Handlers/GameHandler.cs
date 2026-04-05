using Microsoft.Xna.Framework;

using StarMage.Core.Actors;
using StarMage.Core.Actors.Players;
using StarMage.Core.Audio;
using StarMage.Core.Enums;
using StarMage.Core.Levels;
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

        private static float gameOverDelay;
        private static float cameraMovementDelay;
        private static float freezeDuration;
        private static float secondsAccumulator;

        private static LevelProfile levelProfile;

        private static MagePlayer magePlayer;
        private static ActorManager actorManager;
        private static TileMap tileMap;

        private const float GAME_OVER_SCREEN_DURATION = 5.0f;

        private static int GetLevelTime()
        {
            return Constants.DEFAULT_LEVEL_SECONDS + (CurrentLevelIndex * 5) + (CurrentLevelIndex.LevelInWorld % 4 == 0 ? Constants.DEFAULT_LEVEL_SECONDS : 0);
        }

        internal static void Initialize(MagePlayer magePlayer, ActorManager actorManager, TileMap tileMap)
        {
            actorManager.Clear(x => x.Type is ActorType.Player);

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
            magePlayer.CanDraw = false;
            magePlayer.CanUpdate = false;

            SongPlayer.Play(SongIndex.World1);
            levelProfile = LevelGenerator.Generate(actorManager, tileMap);
        }

        internal static void NextLevel()
        {
            if (CurrentLevelIndex == Constants.FINAL_LEVEL)
            {
                GameCompleted();
                return;
            }

            CurrentLevelIndex++;
            BigCrystalCollected = 0;
            TotalRemainingSeconds = GetLevelTime();
            ShouldCameraFollowPlayer = true;
            bossAppeared = false;

            magePlayer.PrepareForLevel();
            magePlayer.CanUpdate = false;
            magePlayer.CanDraw = false;

            actorManager.Clear(x => x.Type is ActorType.Player);

            switch (CurrentLevelIndex.World)
            {
                case 1:
                    SongPlayer.Play(SongIndex.World1);
                    break;

                case 2:
                    SongPlayer.Play(SongIndex.World2);
                    break;

                case 3:
                    SongPlayer.Play(SongIndex.World3);
                    break;

                case 4:
                    SongPlayer.Play(SongIndex.World4);
                    break;

                default:
                    SongPlayer.Stop();
                    break;
            }

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

                actorManager.Clear(x => x.Type is ActorType.Player or ActorType.Boss);

                SongPlayer.Play(SongIndex.BossBattle);
            }

            CameraHandler.Position = cameraPosition;
        }

        internal static void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (IsGameOver)
            {
                gameOverDelay -= deltaTime;

                if (gameOverDelay <= 0.0f)
                {
                    gameOverDelay = 0.0f;
                    CurrentScene = Scene.MainMenu;
                }
            }
            else
            {
                UpdateTimer(deltaTime);
                UpdateFreeze(deltaTime);
                UpdateBossAppearance(deltaTime);
            }
        }

        internal static void CollectBigCrystal()
        {
            BigCrystalCollected++;
        }

        internal static void GameOver()
        {
            IsGameOver = true;
            gameOverDelay = GAME_OVER_SCREEN_DURATION;
            SongPlayer.Stop();
        }

        internal static void GameCompleted()
        {
            IsGameOver = true;
            SongPlayer.Stop();
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
            SongPlayer.Stop();
        }

        internal static void BossDefeated()
        {
            bossAppeared = false;
            SongPlayer.Stop();
        }
    }
}
