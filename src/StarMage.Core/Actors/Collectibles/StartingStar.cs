using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StarMage.Core.Actors.Players;
using StarMage.Core.Audio;
using StarMage.Core.Databases;
using StarMage.Core.Enums;
using StarMage.Core.Handlers;
using StarMage.Core.Managers;
using StarMage.Core.Tiles;

namespace StarMage.Core.Actors.Collectibles
{
    internal sealed class StartingStar : Actor
    {
        private MagePlayer magePlayer;

        private byte animationFrameCounter;
        private byte animationFrameIndex;

        private static readonly Rectangle[] animationFrameRectangles =
        [
            new(128, 00, 16, 16),
            new(144, 00, 16, 16),
            new(128, 16, 16, 16),
            new(144, 16, 16, 16)
        ];

        private const byte ANIMATION_FRAME_DELAY = 4;
        private const float SPEED = 68.0f;

        internal StartingStar(ActorIndex index, ActorType type, ActorTeam team, ActorManager actorManager, TileMap tileMap) : base(index, type, team, actorManager, tileMap)
        {
            Reset();
        }

        public override void Reset()
        {
            this.animationFrameCounter = 0;
            this.animationFrameIndex = 0;
        }

        internal override void OnCreated()
        {
            this.magePlayer = (MagePlayer)this.ActorManager.GetActor(ActorIndex.MagePlayer);
            this.magePlayer.Position = this.Position;
            CameraHandler.Position = Point.Zero;
        }

        internal override void OnTileCollision(bool isHorizontalAxis)
        {
            SoundEffectPlayer.Play(SoundEffectIndex.CollectedStar);
            this.ActorManager.Destroy(this);
            this.magePlayer.Position = this.Position;
            this.magePlayer.PrepareForLevel();
        }

        private void UpdateAnimation()
        {
            if (++this.animationFrameCounter >= ANIMATION_FRAME_DELAY)
            {
                this.animationFrameCounter = 0;
                this.animationFrameIndex = (byte)((this.animationFrameIndex + 1) % animationFrameRectangles.Length);
            }
        }

        internal override void Update(GameTime gameTime)
        {
            UpdateAnimation();
            MoveVertically(SPEED * (float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(AssetDatabase.GetTexture(TextureIndex.Actors), new(this.PositionX, this.PositionY), animationFrameRectangles[this.animationFrameIndex], Color.White, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.0f);
        }
    }
}
