using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StarMage.Core.Actors.Players;
using StarMage.Core.Audio;
using StarMage.Core.Databases;
using StarMage.Core.Enums;
using StarMage.Core.Managers;
using StarMage.Core.Tiles;

namespace StarMage.Core.Actors.Enemies
{
    internal sealed class FloatingBall : Actor
    {
        private MagePlayer magePlayer;

        private byte health;
        private byte hitAnimationCounter;
        private Vector2 velocity;
        private Vector2 lastDirection;
        private float noiseDelayCounter;
        private bool isShowingDamage;

        private static readonly Rectangle defaultSpriteRectangle = new(16, 0, 16, 16);
        private static readonly Rectangle damagedSpriteRectangle = new(32, 0, 16, 16);

        private const byte DEFAULT_HEALTH = 5;
        private const byte HIT_ANIMATION_DURATION = 8;
        private const float MAX_SPEED = 1.25f;
        private const float ACCELERATION = 0.08f;
        private const float DECELERATION = 0.12f;
        private const float SKID_FACTOR = 0.85f;
        private const float NOISE_DELAY = 1.0f;
        private const float KNOCKBACK_STRENGTH = 16_384.0f;
        private const float RANGE_OF_VISION = 196.0f;

        internal FloatingBall(ActorIndex index, ActorType type, ActorTeam team, ActorManager actorManager, TileMap tileMap) : base(index, type, team, actorManager, tileMap)
        {
            Reset();
        }

        public override void Reset()
        {
            this.health = DEFAULT_HEALTH;
            this.hitAnimationCounter = 0;
            this.velocity = Vector2.Zero;
            this.lastDirection = Vector2.Zero;
            this.noiseDelayCounter = 0.0f;
            this.isShowingDamage = false;
        }

        internal override void OnCreated()
        {
            this.magePlayer = this.ActorManager.GetActor(ActorIndex.MagePlayer) as MagePlayer;
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

                // Knockback
                Vector2 knockbackDirection = new(this.PositionX - otherActor.PositionX, this.PositionY - otherActor.PositionY);

                if (knockbackDirection != Vector2.Zero)
                {
                    knockbackDirection.Normalize();
                    this.velocity += knockbackDirection * KNOCKBACK_STRENGTH; // Knockback strength
                }
            }
        }

        internal override void OnTileCollision(bool isHorizontalAxis)
        {
            if (isHorizontalAxis)
            {
                this.velocity.X *= -1.0f * SKID_FACTOR;
            }
            else
            {
                this.velocity.Y *= -1.0f * SKID_FACTOR;
            }
        }

        internal override void OnOutsideLevelBounds(Direction direction, int distance)
        {
            if (direction is Direction.Left or Direction.Right)
            {
                this.velocity.X *= -1.0f * SKID_FACTOR;
            }
            else
            {
                this.velocity.Y *= -1.0f * SKID_FACTOR;
            }
        }

        internal override void OnDestroyed()
        {
            ActorHelpers.CreateEnemyDeathResidues(this.ActorManager, this.Position, this.Size);
        }

        private void MoveTowardsPlayer()
        {
            if (this.magePlayer == null || this.magePlayer.IsDead)
            {
                if (this.velocity.LengthSquared() > 0.01f)
                {
                    this.velocity *= (float)(1.0 - DECELERATION);
                }
                else
                {
                    this.velocity = Vector2.Zero;
                }

                MoveHorizontally(this.velocity.X);
                MoveVertically(this.velocity.Y);
                return;
            }

            Vector2 targetDirection = new(this.magePlayer.PositionX - this.PositionX, this.magePlayer.PositionY - this.PositionY);
            if (targetDirection != Vector2.Zero)
            {
                targetDirection.Normalize();
            }

            // Detects abrupt direction change for skidding
            if (this.lastDirection != Vector2.Zero && Vector2.Dot(targetDirection, this.lastDirection) < 0.7f)
            {
                this.velocity *= SKID_FACTOR;
            }

            // Accelerates towards the target
            this.velocity += targetDirection * ACCELERATION;

            // Limits the maximum speed
            if (this.velocity.Length() > MAX_SPEED)
            {
                this.velocity.Normalize();
                this.velocity *= MAX_SPEED;
            }

            // Decelerates if not moving towards the target
            if (targetDirection == Vector2.Zero && this.velocity.LengthSquared() > 0.01f)
            {
                this.velocity *= (float)(1.0 - DECELERATION);
            }

            MoveHorizontally(this.velocity.X);
            MoveVertically(this.velocity.Y);

            this.lastDirection = targetDirection;
        }

        private void UpdateDamageAnimation()
        {
            if (this.isShowingDamage)
            {
                if (++this.hitAnimationCounter >= HIT_ANIMATION_DURATION)
                {
                    this.hitAnimationCounter = 0;
                    this.isShowingDamage = false;
                }
            }
        }

        internal override void Update(GameTime gameTime)
        {
            if (this.magePlayer != null && !this.magePlayer.IsDead && Vector2.DistanceSquared(new Vector2(this.PositionX, this.PositionY), new Vector2(this.magePlayer.PositionX, this.magePlayer.PositionY)) < RANGE_OF_VISION * RANGE_OF_VISION)
            {
                MoveTowardsPlayer();
            }

            this.noiseDelayCounter += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (this.magePlayer != null && this.noiseDelayCounter > NOISE_DELAY)
            {
                this.noiseDelayCounter = 0.0f;
            }

            UpdateDamageAnimation();
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(AssetDatabase.GetTexture(TextureIndex.Actors), new Vector2(this.PositionX, this.PositionY), this.isShowingDamage ? damagedSpriteRectangle : defaultSpriteRectangle, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
        }
    }
}
