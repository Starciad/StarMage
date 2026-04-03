using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StarMage.Core.Databases;
using StarMage.Core.Enums;
using StarMage.Core.Managers;
using StarMage.Core.Tiles;

using System;

namespace StarMage.Core.Actors.Projectiles
{
    internal sealed class BounceSpell : Actor
    {
        internal sbyte HorizontalDirection { get; set; }

        private float velocityY;

        private const float DEFAULT_VELOCITY_X = 250.0f;
        private const float DEFAULT_VELOCITY_Y = 120.0f;
        private const float GRAVITY = 650.0f;
        private const float MAX_FALL_SPEED = 500.0f;

        internal BounceSpell(ActorIndex index, ActorType type, ActorTeam team, ActorManager actorManager, TileMap tileMap) : base(index, type, team, actorManager, tileMap)
        {
            Reset();
        }

        public override void Reset()
        {
            this.HorizontalDirection = 0;
            this.velocityY = DEFAULT_VELOCITY_Y;
        }

        internal override void OnTileCollision(bool isHorizontalAxis)
        {
            if (isHorizontalAxis)
            {
                this.ActorManager.Destroy(this);
            }
            else
            {
                this.velocityY *= -1.0f;
            }
        }

        internal override void OnOutsideLevelBounds(Direction direction, int distance)
        {
            this.ActorManager.Destroy(this);
        }

        internal override void OnOutsideCameraBounds()
        {
            this.ActorManager.Destroy(this);
        }

        internal override void OnDestroyed()
        {
            if (this.ActorManager.TryCreate(ActorIndex.GoodExplosion, out Actor explosionActor))
            {
                explosionActor.Position = this.Position;
            }
        }

        internal override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            this.velocityY = MathF.Min(this.velocityY + (GRAVITY * deltaTime), MAX_FALL_SPEED);

            MoveHorizontally(DEFAULT_VELOCITY_X * this.HorizontalDirection * deltaTime);
            MoveVertically(this.velocityY * deltaTime);
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(AssetDatabase.GetTexture(TextureIndex.Actors), new(this.PositionX, this.PositionY), new(128, 48, 16, 16), Color.White, 0.0f, new(3, 3), Vector2.One, SpriteEffects.None, 0.0f);
        }
    }
}
