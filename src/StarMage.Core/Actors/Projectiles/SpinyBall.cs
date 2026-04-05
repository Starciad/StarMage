using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StarMage.Core.Audio;
using StarMage.Core.Databases;
using StarMage.Core.Enums;
using StarMage.Core.Tiles;

using System;

namespace StarMage.Core.Actors.Projectiles
{
    internal sealed class SpinyBall : Actor
    {
        internal float VelocityX { get; set; }
        internal float VelocityY { get; set; }

        private byte animationFrameCounter;
        private byte durability;
        private bool isFirstFrame;

        private const byte DEFAULT_DURABILITY = 3;
        private const byte ANIMATION_FRAME_DELAY = 5;
        private const float GRAVITY = 650.0f;
        private const float MAX_FALL_SPEED = 500.0f;

        internal SpinyBall(ActorIndex index, ActorType type, ActorTeam team, ActorManager actorManager, TileMap tileMap) : base(index, type, team, actorManager, tileMap)
        {
            Reset();
        }

        public override void Reset()
        {
            this.VelocityX = 0.0f;
            this.VelocityY = 0.0f;
            this.durability = DEFAULT_DURABILITY;
            this.animationFrameCounter = 0;
            this.isFirstFrame = true;
        }

        internal override void OnTileCollision(bool isHorizontalAxis)
        {
            if (isHorizontalAxis)
            {
                this.VelocityX *= -1.0f;
            }
            else
            {
                this.VelocityY *= -1.0f;
            }

            if (this.durability > 0)
            {
                this.durability--;
            }
            else
            {
                SoundEffectPlayer.Play(SoundEffectIndex.EnemyExploded);
                this.ActorManager.Destroy(this);
            }
        }

        internal override void OnRemoved()
        {
            if (this.ActorManager.TryCreate(ActorIndex.BadExplosion, out Actor explosionActor))
            {
                explosionActor.Position = this.Position;
            }
        }

        private void UpdateAnimation()
        {
            if (++this.animationFrameCounter >= ANIMATION_FRAME_DELAY)
            {
                this.animationFrameCounter = 0;
                this.isFirstFrame = !this.isFirstFrame;
            }
        }

        internal override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            UpdateAnimation();

            this.VelocityY = MathF.Min(this.VelocityY + (GRAVITY * deltaTime), MAX_FALL_SPEED);

            MoveHorizontally(this.VelocityX * deltaTime);
            MoveVertically(this.VelocityY * deltaTime);
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(AssetDatabase.GetTexture(TextureIndex.Actors), new(this.PositionX, this.PositionY), this.isFirstFrame ? new(0, 112, 16, 16) : new(16, 112, 16, 16), Color.White, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.0f);
        }
    }
}
