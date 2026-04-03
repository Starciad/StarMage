using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StarMage.Core.Audio;
using StarMage.Core.Databases;
using StarMage.Core.Enums;
using StarMage.Core.Managers;
using StarMage.Core.Tiles;

namespace StarMage.Core.Actors.Enemies
{
    internal sealed class Batter : Actor
    {
        private byte health;
        private byte hitAnimationCounter;
        private byte currentSpriteIndex;

        private sbyte direction;
        private bool isShowingDamage;

        private static readonly Rectangle[] spriteRectangles =
        [
            // Right
            new(96, 96, 16, 16), // Default
            new(96, 112, 16, 16), // Hit

            // Left
            new(112, 96, 16, 16), // Default
            new(112, 112, 16, 16), // Hit
        ];

        private const byte DEFAULT_HEALTH = 3;
        private const byte HIT_ANIMATION_DURATION = 8;
        private const float DEFAULT_SPEED = 32.0f;

        internal Batter(ActorIndex index, ActorType type, ActorTeam team, ActorManager actorManager, TileMap tileMap) : base(index, type, team, actorManager, tileMap)
        {
            Reset();
        }

        public override void Reset()
        {
            this.health = DEFAULT_HEALTH;
            this.direction = -1;
            this.hitAnimationCounter = 0;
            this.isShowingDamage = false;
        }

        internal override void OnTileCollision(bool isHorizontalAxis)
        {
            if (isHorizontalAxis)
            {
                this.direction *= -1;
            }
        }

        internal override void OnActorCollision(Actor otherActor)
        {
            if (otherActor.Type is ActorType.Projectile && otherActor.Team is ActorTeam.Good)
            {
                this.ActorManager.Destroy(otherActor);
                this.health--;

                if (this.health <= 0)
                {
                    SoundEffectPlayer.Play(SoundEffectIndex.EnemyExploded);
                    this.ActorManager.Destroy(this);
                    return;
                }

                SoundEffectPlayer.Play(SoundEffectIndex.EnemyHit);
                this.isShowingDamage = true;
            }
        }

        internal override void OnDestroyed()
        {
            ActorHelpers.CreateEnemyDeathResidues(this.ActorManager, this.Position, this.Size);
        }

        private void UpdateDamageAnimation()
        {
            if (!this.isShowingDamage)
            {
                return;
            }

            if ((++this.hitAnimationCounter) >= HIT_ANIMATION_DURATION)
            {
                this.hitAnimationCounter = 0;
                this.isShowingDamage = false;
            }
        }

        private void UpdateCurrentSpriteIndex()
        {
            if (this.direction > 0)
            {
                this.currentSpriteIndex = (byte)(this.isShowingDamage ? 1 : 0);
            }
            else
            {
                this.currentSpriteIndex = (byte)(this.isShowingDamage ? 3 : 2);
            }
        }

        internal override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            UpdateDamageAnimation();
            UpdateCurrentSpriteIndex();

            MoveHorizontally(direction * DEFAULT_SPEED * deltaTime);
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(AssetDatabase.GetTexture(TextureIndex.Actors), new Vector2(this.PositionX, this.PositionY), spriteRectangles[this.currentSpriteIndex], Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
        }
    }
}
