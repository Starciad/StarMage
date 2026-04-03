using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StarMage.Core.Databases;
using StarMage.Core.Enums;
using StarMage.Core.Handlers;
using StarMage.Core.Managers;
using StarMage.Core.Tiles;

using System;

namespace StarMage.Core.Actors.Collectibles
{
    internal sealed class BigCrystal : Actor
    {
        private bool isCollected;
        private float time;
        private Vector2 originalPosition;

        private const float AMPLITUDE = 4.5f;
        private const float SPEED = 1.5f;

        internal BigCrystal(ActorIndex index, ActorType type, ActorTeam team, ActorManager actorManager, TileMap tileMap) : base(index, type, team, actorManager, tileMap)
        {
            Reset();
        }

        public override void Reset()
        {
            this.isCollected = false;
        }

        internal override void OnCreated()
        {
            this.originalPosition = new(this.PositionX, this.PositionY);
        }

        internal override void OnActorCollision(Actor otherActor)
        {
            if (!this.isCollected && otherActor.Type is ActorType.Player)
            {
                this.isCollected = true;
                this.ActorManager.Destroy(this);

                GameHandler.CollectBigCrystal();

                if (GameHandler.BigCrystalCollected == Constants.DEFAULT_BIG_CRYSTALS_PER_LEVEL &&
                    this.ActorManager.TryCreate(ActorIndex.MagicHeart, out Actor magicHeartActor))
                {
                    magicHeartActor.PositionX = this.PositionX;
                    magicHeartActor.PositionY = Constants.SCREEN_HEIGHT - 32;
                }
            }
        }

        internal override void Update(GameTime gameTime)
        {
            this.time += (float)gameTime.ElapsedGameTime.TotalSeconds;

            float offsetY = MathF.Sin(this.time * SPEED) * AMPLITUDE;
            float targetY = this.originalPosition.Y + offsetY;
            float deltaY = targetY - this.PositionY;

            MoveVertically(deltaY);
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(AssetDatabase.GetTexture(TextureIndex.Actors), new(this.PositionX, this.PositionY), new(128, 32, 16, 16), Color.White, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.0f);
        }
    }
}
