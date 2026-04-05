using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StarMage.Core.Actors.Players;
using StarMage.Core.Audio;
using StarMage.Core.Databases;
using StarMage.Core.Enums;
using StarMage.Core.Tiles;

using System;

namespace StarMage.Core.Actors.Collectibles
{
    internal sealed class MagicHeart : Actor
    {
        private bool isCollected;

        private Vector2 originalPosition;
        private float time;

        private const float VERTICAL_SPEED = 25.0f;
        private const float HORIZONTAL_AMPLITUDE = 12.0f;
        private const float HORIZONTAL_FREQUENCY = 3.0f;

        internal MagicHeart(ActorIndex index, ActorType type, ActorTeam team, ActorManager actorManager, TileMap tileMap) : base(index, type, team, actorManager, tileMap)
        {
            Reset();
        }

        public override void Reset()
        {
            this.isCollected = false;
            this.time = 0.0f;
        }

        internal override void OnInstantiated()
        {
            this.originalPosition = new(this.PositionX, this.PositionY);
        }

        internal override void OnActorCollision(Actor otherActor)
        {
            if (!this.isCollected && otherActor.Index is ActorIndex.MagePlayer)
            {
                SoundEffectPlayer.Play(SoundEffectIndex.CollectedHeart);
                this.isCollected = true;
                this.ActorManager.Destroy(this);
                ((MagePlayer)otherActor).Heal(1);
            }
        }

        internal override void OnOutsideLevelBounds(Direction direction, int distance)
        {
            if (direction is Direction.Up)
            {
                this.ActorManager.Destroy(this);
            }
        }

        internal override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            this.time += deltaTime;

            MoveVertically(-VERTICAL_SPEED * deltaTime);

            float offsetX = MathF.Cos(this.time * HORIZONTAL_FREQUENCY) * HORIZONTAL_AMPLITUDE;
            float targetX = this.originalPosition.X + offsetX;

            float deltaX = targetX - this.PositionX;

            MoveHorizontally(deltaX);
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(AssetDatabase.GetTexture(TextureIndex.Actors), new Vector2(this.PositionX, this.PositionY), new Rectangle(144, 32, 16, 16), Color.White, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.0f);
        }
    }
}
