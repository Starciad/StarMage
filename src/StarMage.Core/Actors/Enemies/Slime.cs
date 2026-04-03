using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StarMage.Core.Audio;
using StarMage.Core.Databases;
using StarMage.Core.Enums;
using StarMage.Core.Managers;
using StarMage.Core.Tiles;

using System;

namespace StarMage.Core.Actors.Enemies
{
    internal sealed class Slime : Actor
    {
        private float jumpTimer;
        private float jumpInterval;

        private sbyte direction;
        private byte health;
        private byte animationFrameCounter;
        private byte hitAnimationCounter;

        private int currentAnimationFrameIndex;

        private float velocityX;
        private float velocityY;

        private bool isGrounded;
        private bool isShowingDamage;

        private static readonly Rectangle[] animationFrames =
        [
            #region RIGHT

            // Idle
            new(48, 48, 16, 16),
            new(64, 48, 16, 16),

            // Jumping
            new(80, 48, 16, 16),

            #endregion

            #region LEFT

            // Idle
            new(48, 64, 16, 16),
            new(64, 64, 16, 16),

            // Jumping
            new(80, 64, 16, 16),

            #endregion

            // Damaged
            new(80, 0, 16, 16),
        ];

        private const byte ANIMATION_FRAME_DELAY = 4;
        private const float GRAVITY = 650.0f;
        private const float MAX_FALL_SPEED = 500.0f;
        private const float JUMP_SPEED = 285.0f;
        private const float HORIZONTAL_SPEED = 64.0f;
        private const byte DEFAULT_HEALTH = 4;
        private const byte HIT_ANIMATION_DURATION = 8;

        internal Slime(ActorIndex index, ActorType type, ActorTeam team, ActorManager actorManager, TileMap tileMap) : base(index, type, team, actorManager, tileMap)
        {
            Reset();
        }

        public override void Reset()
        {
            this.jumpTimer = 0.0f;
            this.jumpInterval = Randomness.Random.Range(2.5f, 4.5f);
            this.direction = -1;
            this.health = DEFAULT_HEALTH;
            this.animationFrameCounter = 0;
            this.hitAnimationCounter = 0;
            this.velocityX = 0.0f;
            this.velocityY = 0.0f;
            this.isGrounded = false;
            this.isShowingDamage = false;
            this.currentAnimationFrameIndex = this.direction == 1 ? 0 : 3;
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

        internal override void OnTileCollision(bool isHorizontalAxis)
        {
            if (isHorizontalAxis && !this.isGrounded)
            {
                this.direction *= -1;
                this.velocityX *= -1.0f;
            }
            else
            {
                if (this.velocityY > 0)
                {
                    this.isGrounded = true;
                    this.velocityX = 0.0f;
                }

                this.velocityY = 0.0f;
            }
        }

        internal override void OnOutsideLevelBounds(Direction direction, int distance)
        {
            if (direction is Direction.Down)
            {
                this.ActorManager.Destroy(this);
            }
        }

        internal override void OnDestroyed()
        {
            ActorHelpers.CreateEnemyDeathResidues(this.ActorManager, this.Position, this.Size);
        }

        private void Jump()
        {
            this.velocityX = this.direction * HORIZONTAL_SPEED;
            this.velocityY = -JUMP_SPEED;
            this.isGrounded = false;
            this.jumpInterval = Randomness.Random.Range(1.5f, 2.5f);
        }

        private void UpdateJumpTimer(float deltaTime)
        {
            if (this.isGrounded)
            {
                this.jumpTimer += deltaTime;

                if (this.jumpTimer >= this.jumpInterval)
                {
                    Jump();
                    this.jumpTimer = 0.0f;
                }
            }
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

        private void UpdateSpriteAnimation()
        {
            if (++this.animationFrameCounter < ANIMATION_FRAME_DELAY)
            {
                return;
            }

            this.animationFrameCounter = 0;

            if (this.isShowingDamage)
            {
                this.currentAnimationFrameIndex = 6;
            }
            else if (!this.isGrounded)
            {
                this.currentAnimationFrameIndex = this.direction == 1 ? 2 : 5;
            }
            else
            {
                int idleFrameIndex = (this.direction == 1 ? 0 : 3) + (this.currentAnimationFrameIndex == (this.direction == 1 ? 0 : 3) ? 1 : 0);
                this.currentAnimationFrameIndex = idleFrameIndex;
            }
        }

        private void UpdateGravity(float deltaTime)
        {
            if (!this.isGrounded)
            {
                this.velocityY = MathF.Min(this.velocityY + (GRAVITY * deltaTime), MAX_FALL_SPEED);
            }
        }

        internal override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            UpdateJumpTimer(deltaTime);
            UpdateDamageAnimation();
            UpdateSpriteAnimation();
            UpdateGravity(deltaTime);

            MoveHorizontally(this.velocityX * deltaTime);
            MoveVertically(this.velocityY * deltaTime);
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(AssetDatabase.GetTexture(TextureIndex.Actors), new(this.PositionX, this.PositionY), animationFrames[this.currentAnimationFrameIndex], Color.White, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.0f);
        }
    }
}
