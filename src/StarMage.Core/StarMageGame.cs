using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

using StarMage.Core.Actors;
using StarMage.Core.Actors.Players;
using StarMage.Core.Audio;
using StarMage.Core.Databases;
using StarMage.Core.Enums;
using StarMage.Core.Handlers;
using StarMage.Core.InputSystem;
using StarMage.Core.Managers;
using StarMage.Core.TextSystem;
using StarMage.Core.Tiles;

using System;

namespace StarMage.Core
{
    public sealed class StarMageGame : Game
    {
        private bool isActivated;
        private bool isPaused;

        private byte oceanAnimationFrameCounter;
        private byte oceanAnimationIndex;

        private byte titleAnimationFrameCounter;
        private byte titleAnimationIndex;

        private MagePlayer magePlayer;

        private SpriteBatch spriteBatch;
        private RenderTarget2D mainRenderTarget2D;

        private readonly ActorManager actorManager;
        private readonly TileMap tileMap;

        private const float OCEAN_SPEED = 1.15f;

        private const byte OCEAN_ANIMATION_DELAY = 12;
        private const byte OCEAN_SPRITES_LENGTH = 4;

        private const byte TITLE_ANIMATION_DELAY = 16;
        private const byte TITLE_SPRITES_LENGTH = 4;

        public StarMageGame()
        {
            _ = new GraphicsDeviceManager(this)
            {
                GraphicsProfile = GraphicsProfile.Reach,
                PreferredBackBufferFormat = SurfaceFormat.Color,
                PreferredBackBufferWidth = 1280,
                PreferredBackBufferHeight = 720,
                SynchronizeWithVerticalRetrace = true,
                HardwareModeSwitch = true,
                IsFullScreen = false,
                PreferHalfPixelOffset = true,
                PreferMultiSampling = false,
                PreferredDepthStencilFormat = DepthFormat.None,
                SupportedOrientations = DisplayOrientation.Default
            };

            this.IsMouseVisible = false;
            this.InactiveSleepTime = TimeSpan.FromMilliseconds(100.0f);
            this.MaxElapsedTime = TimeSpan.FromSeconds(0.5f);
            this.TargetElapsedTime = TimeSpan.FromSeconds(Constants.GAME_FPS);
            this.IsFixedTimeStep = true;

            this.Window.Title = "Star Mage";
            this.Window.AllowAltF4 = true;
            this.Window.AllowUserResizing = true;
            this.Window.IsBorderless = false;

            this.Content.RootDirectory = "Content";

            this.tileMap = new();
            this.actorManager = new(this.tileMap);
        }

        protected override void Initialize()
        {
            this.mainRenderTarget2D = new(this.GraphicsDevice, Constants.SCREEN_WIDTH, Constants.SCREEN_HEIGHT, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.DiscardContents, false);

            TileProperty.Initialize();
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.5f;
            SoundEffect.MasterVolume = 0.5f;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.spriteBatch = new(this.GraphicsDevice);
            AssetDatabase.LoadContent(this.Content);
            ActorDatabase.LoadContent(this.actorManager, this.tileMap);
        }

        private void CreatePlayer()
        {
            if (this.actorManager.TryCreate(ActorIndex.MagePlayer, out Actor playerActor))
            {
                this.magePlayer = (MagePlayer)playerActor;
                this.magePlayer.CanUpdate = false;
                this.magePlayer.CanDraw = false;
                this.magePlayer.Position = new(Constants.SCREEN_WIDTH / 2, Constants.SCREEN_HEIGHT / 2);
            }
        }

        protected override void BeginRun()
        {
            CreatePlayer();
            base.BeginRun();
        }

        private void UpdateMainMenu()
        {
            if (++this.titleAnimationFrameCounter > TITLE_ANIMATION_DELAY)
            {
                this.titleAnimationFrameCounter = 0;
                if (++this.titleAnimationIndex >= TITLE_SPRITES_LENGTH)
                {
                    this.titleAnimationIndex = 0;
                }
            }

            if (GameHandler.CurrentScene is not Scene.Gameplay && Controller.IsStartStarted())
            {
                GameHandler.CurrentScene = Scene.Gameplay;
                GameHandler.Initialize(this.magePlayer, this.actorManager, this.tileMap);
            }
        }

        private void UpdateCamera()
        {
            if (!GameHandler.ShouldCameraFollowPlayer || this.magePlayer == null || this.magePlayer.IsDead)
            {
                return;
            }

            Point cameraPosition = CameraHandler.Position;
            Point cameraOrigin = new(Constants.SCREEN_WIDTH / 2, Constants.SCREEN_HEIGHT / 2);

            int cameraLevelWidth = (this.tileMap.Width * Constants.TILE_SIZE) - Constants.SCREEN_WIDTH;
            float newPositionX = MathHelper.Lerp(cameraPosition.X - cameraOrigin.X, this.magePlayer.PositionX, 0.5f);

            newPositionX = MathF.Floor(newPositionX);
            newPositionX = Math.Clamp(newPositionX, 0, cameraLevelWidth);

            CameraHandler.Position = new((int)newPositionX, 0);
        }

        private void UpdateOceanForeground()
        {
            if (++this.oceanAnimationFrameCounter > OCEAN_ANIMATION_DELAY)
            {
                this.oceanAnimationFrameCounter = 0;

                if (++this.oceanAnimationIndex >= OCEAN_SPRITES_LENGTH)
                {
                    this.oceanAnimationIndex = 0;
                }
            }
        }

        private void UpdatePauseState()
        {
            if (Controller.IsStartStarted())
            {
                SoundEffectPlayer.Play(SoundEffectIndex.CollectedStar);
                this.isPaused = !this.isPaused;
            }
        }

        private void UpdateGameplay(GameTime gameTime)
        {
            GameHandler.Update(gameTime);
            UpdateCamera();

            if (!GameHandler.IsFreezeActive)
            {
                this.actorManager.Update(gameTime);
                UpdateOceanForeground();
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (!this.isActivated)
            {
                base.Update(gameTime);
                return;
            }

            Input.Update();

            switch (GameHandler.CurrentScene)
            {
                case Scene.MainMenu:
                    this.isPaused = false;
                    UpdateMainMenu();
                    break;

                case Scene.Gameplay:
                    UpdatePauseState();

                    if (!this.isPaused)
                    {
                        UpdateGameplay(gameTime);
                    }

                    break;

                default:
                    break;
            }

            base.Update(gameTime);
        }

        #region DRAWING

        private static Rectangle AdjustRenderTargetOnScreen(GraphicsDevice graphicsDevice, RenderTarget2D renderTarget)
        {
            float scale = MathF.Min(
                (float)graphicsDevice.PresentationParameters.Bounds.Width / renderTarget.Width,
                (float)graphicsDevice.PresentationParameters.Bounds.Height / renderTarget.Height
            );

            Vector2 newSize = new(
                renderTarget.Width * scale,
                renderTarget.Height * scale
            );

            Point newPosition = new(
                (int)((graphicsDevice.PresentationParameters.Bounds.Width - newSize.X) / 2.0f),
                (int)((graphicsDevice.PresentationParameters.Bounds.Height - newSize.Y) / 2.0f)
            );

            return new(newPosition, newSize.ToPoint());
        }

        private void DrawMainMenu()
        {
            PaletteHandler.SetPalette(PaletteIndex.TechNeonSprite);

            this.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, AssetDatabase.GetEffect(EffectIndex.PaletteIndexed), Matrix.CreateTranslation(0, 0, 0));

            // Background
            this.spriteBatch.Draw(AssetDatabase.GetTexture(TextureIndex.UI), new(0.0f, 128.0f), new Rectangle(0, 336, 256, 80), Color.White, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.0f);

            // Title
            this.spriteBatch.Draw(AssetDatabase.GetTexture(TextureIndex.UI), new(0.0f, 16.0f), new Rectangle(0, 144 + (this.titleAnimationIndex * 44), 256, 48), Color.White, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.0f);

            // Copyright
            TextRenderer.DrawText(this.spriteBatch, "© 2026 STARCIAD", new(Constants.SCREEN_WIDTH - 4, 68), TextRenderSettings.RightAligned);

            // Label
            TextRenderer.DrawText(this.spriteBatch, "PUSH START BUTTON", new(128, 200), TextRenderSettings.Centered);

            this.spriteBatch.End();
        }

        private void DrawGameplayContent()
        {
            // Tile Map
            PaletteHandler.SetPalette((PaletteIndex)(GameHandler.CurrentLevelIndex.World % 4));

            this.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, AssetDatabase.GetEffect(EffectIndex.PaletteIndexed), CameraHandler.GetViewMatrix());
            this.tileMap.Draw(this.spriteBatch);
            this.spriteBatch.End();

            // Good Actors
            PaletteHandler.SetPalette(PaletteIndex.TechNeonSprite);

            this.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, AssetDatabase.GetEffect(EffectIndex.PaletteIndexed), CameraHandler.GetViewMatrix());
            this.actorManager.Draw(this.spriteBatch, ActorTeam.Good);
            this.spriteBatch.End();

            // Bad Actors
            PaletteHandler.SetPalette((PaletteIndex)((GameHandler.CurrentLevelIndex.World % 3) + 4));

            this.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, AssetDatabase.GetEffect(EffectIndex.PaletteIndexed), CameraHandler.GetViewMatrix());
            this.actorManager.Draw(this.spriteBatch, ActorTeam.Bad);
            this.spriteBatch.End();

            // Ocean (foreground)
            PaletteHandler.SetPalette(PaletteIndex.TechNeonSprite);

            this.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, AssetDatabase.GetEffect(EffectIndex.PaletteIndexed), CameraHandler.GetViewMatrix());

            int parallaxX = (int)(CameraHandler.Position.X * OCEAN_SPEED);
            int offsetX = -(parallaxX % Constants.TILE_SIZE);

            for (int i = offsetX - Constants.TILE_SIZE; i < Constants.SCREEN_WIDTH + Constants.TILE_SIZE; i += Constants.TILE_SIZE)
            {
                int posX = CameraHandler.Position.X + i;
                int posY = Constants.SCREEN_HEIGHT - (Constants.TILE_SIZE * 4);

                this.spriteBatch.Draw(AssetDatabase.GetTexture(TextureIndex.Tiles), new(posX, posY), new(this.oceanAnimationIndex * 16, 112, 16, 16), Color.White, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.0f);
            }

            this.spriteBatch.End();
        }

        private void DrawGameplayHUD()
        {
            PaletteHandler.SetPalette(PaletteIndex.TechNeonSprite);

            this.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, AssetDatabase.GetEffect(EffectIndex.PaletteIndexed), Matrix.CreateTranslation(0, 176, 0));

            // Background
            this.spriteBatch.Draw(AssetDatabase.GetTexture(TextureIndex.UI), Vector2.Zero, new Rectangle(0, 16, Constants.SCREEN_WIDTH, 48), Color.White, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.0f);

            #region LEFT

            // Player Portrait
            this.spriteBatch.Draw(AssetDatabase.GetTexture(TextureIndex.UI), new Vector2(9, 9), new Rectangle(this.magePlayer.Health * 32, 64, 32, 32), Color.White, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.0f);

            // Player Label
            TextRenderer.DrawText(this.spriteBatch, "MAGE", new(48, 11), TextRenderSettings.LeftAligned);

            // Player Health
            for (int i = 0; i < MagePlayer.DEFAULT_HEALTH; i++)
            {
                Rectangle sourceRectangle = new(16, 0, 16, 16);
                Vector2 position = new(46 + (i * 13), 22);

                if (i >= this.magePlayer.Health)
                {
                    sourceRectangle.X += 16;
                }

                this.spriteBatch.Draw(AssetDatabase.GetTexture(TextureIndex.UI), position, sourceRectangle, Color.White, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.0f);
            }

            #endregion

            #region CENTER

            // Stage Label
            TextRenderer.DrawText(this.spriteBatch, "STAGE", new(Constants.SCREEN_WIDTH / 2, 6), TextRenderSettings.Centered);
            TextRenderer.DrawText(this.spriteBatch, GameHandler.CurrentLevelIndex.ToString(), new(Constants.SCREEN_WIDTH / 2, 17), TextRenderSettings.Centered);

            // Big Crystals Collected
            for (int i = 0; i < Constants.DEFAULT_BIG_CRYSTALS_PER_LEVEL; i++)
            {
                Rectangle sourceRectangle = new(96, 0, 16, 16);
                Vector2 position = new((Constants.SCREEN_WIDTH / 2.0f) + (i * 17.0f) - (Constants.DEFAULT_BIG_CRYSTALS_PER_LEVEL * 16.0f / 2.0f), 27.0f);

                if (i >= GameHandler.BigCrystalCollected)
                {
                    sourceRectangle.X += 16;
                }

                this.spriteBatch.Draw(AssetDatabase.GetTexture(TextureIndex.UI), position, sourceRectangle, Color.White, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.0f);
            }

            #endregion

            #region RIGHT

            // Weapon Portrait
            this.spriteBatch.Draw(AssetDatabase.GetTexture(TextureIndex.UI), new Vector2(215, 9), new Rectangle(128 + ((int)this.magePlayer.SpellType * 32), 64, 32, 32), Color.White, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.0f);

            // Time Label
            TextRenderer.DrawText(this.spriteBatch, "TIME", new(209, 11), TextRenderSettings.RightAligned);
            TextRenderer.DrawText(this.spriteBatch, $"{GameHandler.TotalRemainingSeconds / 60:00}:{GameHandler.TotalRemainingSeconds % 60:00}", new(209, 22), TextRenderSettings.RightAligned);

            #endregion

            this.spriteBatch.End();
        }

        private void DrawPauseOverlay()
        {
            if (!this.isPaused)
            {
                return;
            }

            PaletteHandler.SetPalette(PaletteIndex.TechNeonSprite);

            this.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, AssetDatabase.GetEffect(EffectIndex.PaletteIndexed), Matrix.CreateTranslation(0, 0, 0));
            TextRenderer.DrawText(this.spriteBatch, "PAUSED", new(Constants.SCREEN_WIDTH / 2, Constants.SCREEN_HEIGHT / 2), TextRenderSettings.Centered);
            this.spriteBatch.End();
        }

        protected override void Draw(GameTime gameTime)
        {
            #region Content

            this.GraphicsDevice.SetRenderTarget(this.mainRenderTarget2D);
            this.GraphicsDevice.Clear(Constants.DEFAULT_COLOR_PALETTE[(byte)ColorIndex.RoyalBlue]);

            switch (GameHandler.CurrentScene)
            {
                case Scene.MainMenu:
                    DrawMainMenu();
                    break;

                case Scene.Gameplay:
                    DrawGameplayContent();
                    DrawGameplayHUD();
                    DrawPauseOverlay();
                    break;

                default:
                    break;
            }

            #endregion

            #region Screen

            this.GraphicsDevice.SetRenderTarget(null);
            this.GraphicsDevice.Clear(Color.Black);

            this.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone);
            this.spriteBatch.Draw(this.mainRenderTarget2D, AdjustRenderTargetOnScreen(this.GraphicsDevice, this.mainRenderTarget2D), Color.White);
            this.spriteBatch.End();

            #endregion

            base.Draw(gameTime);
        }

        #endregion

        protected override void UnloadContent()
        {
            this.mainRenderTarget2D.Dispose();
            base.UnloadContent();
        }

        protected override void OnActivated(object sender, EventArgs args)
        {
            MediaPlayer.Resume();

            this.isActivated = true;
            base.OnActivated(sender, args);
        }

        protected override void OnDeactivated(object sender, EventArgs args)
        {
            MediaPlayer.Pause();

            this.isActivated = false;
            base.OnDeactivated(sender, args);
        }
    }
}
