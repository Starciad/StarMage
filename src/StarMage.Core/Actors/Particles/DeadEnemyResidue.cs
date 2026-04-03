using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StarMage.Core.Databases;
using StarMage.Core.Enums;
using StarMage.Core.Managers;
using StarMage.Core.Tiles;

using System;

namespace StarMage.Core.Actors.Particles
{
    internal sealed class DeadEnemyResidue : Actor
    {
        internal Vector2 Direction { get; set; }

        private float velocityX;
        private float velocityY;

        private const float SPEED = 260.0f;
        private const float GRAVITY = 260.0f;
        private const float MAX_FALL_SPEED = 500.0f;

        internal DeadEnemyResidue(ActorIndex index, ActorType type, ActorTeam team, ActorManager actorManager, TileMap tileMap) : base(index, type, team, actorManager, tileMap)
        {
            Reset();
        }

        public override void Reset()
        {
            this.velocityX = SPEED;
            this.velocityY = SPEED;
        }

        internal override void OnOutsideCameraBounds()
        {
            this.ActorManager.Destroy(this);
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
            spriteBatch.Draw(AssetDatabase.GetTexture(TextureIndex.Actors), new(this.PositionX, this.PositionY), new(0, 128, 8, 8), Color.White, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.0f);
        }
    }
}
