using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StarMage.Core.Audio;
using StarMage.Core.Databases;
using StarMage.Core.Enums;
using StarMage.Core.Tiles;

using System;

namespace StarMage.Core.Actors.Projectiles
{
    internal sealed class WaveSpell : Actor
    {
        internal Vector2 Direction { get; set; }

        private float velocityX;
        private float velocityY;

        private const float SPEED = 265.0f;
        private const float GRAVITY = 460.0f;
        private const float MAX_FALL_SPEED = 500.0f;

        internal WaveSpell(ActorIndex index, ActorType type, ActorTeam team, ActorManager actorManager, TileMap tileMap) : base(index, type, team, actorManager, tileMap)
        {
            Reset();
        }

        public override void Reset()
        {
            this.velocityX = SPEED;
            this.velocityY = SPEED;
            this.Direction = Vector2.Zero;
        }

        internal override void OnTileCollision(bool isHorizontalAxis)
        {
            SoundEffectPlayer.Play(SoundEffectIndex.ShotDestroyed);
            this.ActorManager.Destroy(this);
        }

        internal override void OnOutsideLevelBounds(Direction direction, int distance)
        {
            if (direction is Enums.Direction.Down)
            {
                this.ActorManager.Destroy(this);
            }
        }

        internal override void OnRemoved()
        {
            if (this.ActorManager.TryCreate(ActorIndex.GoodExplosion, out Actor explosionActor))
            {
                explosionActor.Position = this.Position;
            }
        }

        internal override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            this.velocityX = MathF.Max(this.velocityX - (GRAVITY * deltaTime), 0.0f);

            // We calculated how much gravity has already affected the speed.
            float gravityEffect = this.velocityY - SPEED + (GRAVITY * deltaTime);
            this.velocityY = SPEED + gravityEffect;

            // The actual speed is the sum of the direction plus the base speed plus the accumulated gravity (always downwards).
            float currentSpeedY = MathF.Min((SPEED * this.Direction.Y) + gravityEffect, MAX_FALL_SPEED);

            MoveHorizontally(this.velocityX * this.Direction.X * deltaTime);
            MoveVertically(currentSpeedY * deltaTime);
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(AssetDatabase.GetTexture(TextureIndex.Actors), new(this.PositionX, this.PositionY), new(128, 48, 16, 16), Color.White, 0.0f, new(3, 3), Vector2.One, SpriteEffects.None, 0.0f);
        }
    }
}
