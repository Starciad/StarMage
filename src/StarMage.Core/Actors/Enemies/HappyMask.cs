using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StarMage.Core.Audio;
using StarMage.Core.Databases;
using StarMage.Core.Enums;
using StarMage.Core.Tiles;

namespace StarMage.Core.Actors.Enemies
{
    internal sealed class HappyMask : Actor
    {
        private byte health;
        private byte hitAnimationCounter;
        private bool isShowingDamage;
        private Vector2 direction;

        private static readonly Rectangle[] spriteRectangles =
        [
            new(48, 0, 16, 16), // Normal
            new(64, 0, 16, 16) // Hit
        ];

        private const byte DEFAULT_HEALTH = 4;
        private const byte HIT_ANIMATION_DURATION = 8;
        private const float SPEED = 32.0f;

        internal HappyMask(ActorIndex index, ActorType type, ActorTeam team, ActorManager actorManager, TileMap tileMap) : base(index, type, team, actorManager, tileMap)
        {
            Reset();
        }

        public override void Reset()
        {
            this.health = DEFAULT_HEALTH;
            this.hitAnimationCounter = 0;
            this.isShowingDamage = false;
            this.direction = new(
                Randomness.Random.GetBool() ? 1.0f : -1.0f,
                Randomness.Random.GetBool() ? 1.0f : -1.0f
            );
        }

        private void PerformDeath()
        {
            SoundEffectPlayer.Play(SoundEffectIndex.EnemyExploded);
            ActorHelpers.CreateEnemyDeathResidues(this.ActorManager, this.Position, this.Size);
        }

        internal override void OnActorCollision(Actor otherActor)
        {
            if (otherActor.Type is ActorType.Projectile && otherActor.Team is ActorTeam.Good)
            {
                this.ActorManager.Destroy(otherActor);
                this.health--;

                if (this.health <= 0)
                {
                    PerformDeath();
                    this.ActorManager.Destroy(this);
                    return;
                }

                SoundEffectPlayer.Play(SoundEffectIndex.EnemyHit);
                this.isShowingDamage = true;
            }
        }

        internal override void OnTileCollision(bool isHorizontalAxis)
        {
            if (isHorizontalAxis)
            {
                this.direction.X *= -1.0f;
            }
            else
            {
                this.direction.Y *= -1.0f;
            }
        }

        internal override void OnOutsideLevelBounds(Direction direction, int distance)
        {
            if (direction is Direction.Left or Direction.Right)
            {
                this.direction.X *= -1.0f;
            }
            else
            {
                this.direction.Y *= -1.0f;
            }
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

        internal override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            UpdateDamageAnimation();

            MoveHorizontally(this.direction.X * SPEED * deltaTime);
            MoveVertically(this.direction.Y * SPEED * deltaTime);
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(AssetDatabase.GetTexture(TextureIndex.Actors), new Vector2(this.PositionX, this.PositionY), this.isShowingDamage ? spriteRectangles[1] : spriteRectangles[0], Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
        }
    }
}
