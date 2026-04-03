using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StarMage.Core.Actors.Particles;
using StarMage.Core.Actors.Projectiles;
using StarMage.Core.Audio;
using StarMage.Core.Databases;
using StarMage.Core.Enums;
using StarMage.Core.Handlers;
using StarMage.Core.InputSystem;
using StarMage.Core.Managers;
using StarMage.Core.Tiles;

using System;

namespace StarMage.Core.Actors.Players
{
    internal sealed class MagePlayer : Actor
    {
        internal byte Health { get; private set; }
        internal SpellType SpellType { get; set; }
        internal bool HasTemporarilyInvincible { get; set; }
        internal bool IsDead => this.Health == 0;

        private bool IsWalking => MathF.Abs(this.velocityX) > VELOCITY_EPSILON && this.isGrounded;
        private bool IsJumping => this.velocityY < 0.0f;
        private bool IsFalling => this.velocityY > 0.0f;

        private bool isFacingRight;
        private bool isGrounded;
        private bool isFirstAnimationFrame;

        private byte shootFrameCounter;
        private byte animationFrameCounter;
        private byte invincibilityFrameCounter;
        private byte invincibilityBlinkFrameCounter;
        private byte deathAnimationFrameIndex;
        private sbyte horizontalInputDirection;

        private float velocityX;
        private float velocityY;
        private float coyoteTimeCounter;

        private Rectangle currentSpriteRectangle;

        private static readonly Rectangle[] normalAnimationFrameRectangles =
        [
            #region RIGHT

            // Idle or Moving (right)
            new(0, 16, 16, 16),

            // Jumping or Falling (right)
            new(32, 16, 16, 16),

            #endregion

            #region LEFT

            // Idle or Moving (left)
            new(0, 32, 16, 16),

            // Jumping or Falling (left)
            new(0, 32, 16, 16),

            #endregion
        ];

        private static readonly Rectangle[] deathAnimationFrameRectangles =
        [
            new(64, 16, 16, 16),
            new(80, 16, 16, 16),
            new(80, 32, 16, 16),
            new(64, 32, 16, 16),
        ];

        internal const byte DEFAULT_HEALTH = 3;
        private const byte ANIMATION_LEFT_DIRECTION_OFFSET = 2;
        private const byte ANIMATION_FRAME_DELAY = 5;
        private const byte BULLET_DISTANCE = 8;
        private const byte SHOOT_FRAME_DELAY = 8;
        private const byte RESIDUE_COUNT = 18;

        private const byte INVINCIBILITY_DURATION_FRAMES = 192;
        private const byte INVINCIBILITY_BLINK_INTERVAL_FRAMES = 5;

        private const float GRAVITY = 650.0f;
        private const float ACCELERATION = 11.0f;
        private const float DECELERATION = 10.0f;
        private const float MAX_FALL_SPEED = 500.0f;
        private const float JUMP_SPEED = 275.0f;
        private const float WALK_SPEED = 88.0f;
        private const float JUMP_CUTOFF_MULTIPLIER = 0.5f;
        private const float RUN_MULTIPLIER = 1.5f;
        private const float VELOCITY_EPSILON = 0.1f;
        private const float COYOTE_TIME_MAX = 0.25f;
        private const float KNOCKBACK_HORIZONTAL_FORCE = 250.0f;
        private const float KNOCKBACK_VERTICAL_FORCE = 250.0f;

        internal MagePlayer(ActorIndex index, ActorType type, ActorTeam team, ActorManager actorManager, TileMap tileMap) : base(index, type, team, actorManager, tileMap)
        {
            Reset();
        }

        public override void Reset()
        {
            this.Health = DEFAULT_HEALTH;
            this.SpellType = SpellType.Bolt;
            this.isFacingRight = true;
            this.isGrounded = false;
            this.isFirstAnimationFrame = true;
            this.HasTileCollision = true;
            this.HasTemporarilyInvincible = false;
            this.animationFrameCounter = 0;
            this.invincibilityFrameCounter = 0;
            this.invincibilityBlinkFrameCounter = 0;
            this.deathAnimationFrameIndex = 0;
            this.horizontalInputDirection = 0;
            this.velocityX = 0.0f;
            this.velocityY = 0.0f;
            this.coyoteTimeCounter = 0.0f;
            this.currentSpriteRectangle = normalAnimationFrameRectangles[0];
        }

        internal void PrepareForLevel()
        {
            this.CanUpdate = true;
            this.CanDraw = true;
            this.isFacingRight = true;
            this.isGrounded = false;
            this.isFirstAnimationFrame = true;
            this.HasTemporarilyInvincible = true;
            this.animationFrameCounter = 0;
            this.invincibilityFrameCounter = 0;
            this.invincibilityBlinkFrameCounter = 0;
            this.deathAnimationFrameIndex = 0;
            this.horizontalInputDirection = 0;
            this.velocityX = 0.0f;
            this.velocityY = 0.0f;
            this.coyoteTimeCounter = 0.0f;
            this.currentSpriteRectangle = normalAnimationFrameRectangles[0];
        }

        internal override void OnCreated()
        {
            this.Health = DEFAULT_HEALTH;
        }

        private void OnHorizontalCollision()
        {
            this.velocityX = 0.0f;
        }

        private void OnVerticalCollision()
        {
            // Is Grounded
            if (this.velocityY > 0.0f)
            {
                this.isGrounded = true;
            }

            this.velocityY = 0.0f;
        }

        internal override void OnTileCollision(bool isHorizontalAxis)
        {
            if (isHorizontalAxis)
            {
                OnHorizontalCollision();
            }
            else
            {
                OnVerticalCollision();
            }
        }

        private void PerformDeath(Point origin)
        {
            GameHandler.GameOver();
            SoundEffectPlayer.Play(SoundEffectIndex.PlayerDeath);

            this.HasTileCollision = false;

            this.velocityX = KNOCKBACK_HORIZONTAL_FORCE / 2.5f * (this.PositionX < origin.X ? -1.0f : 1.0f);
            this.velocityY = KNOCKBACK_VERTICAL_FORCE * -1.0f;
        }

        private void PerformKnockback(Point origin)
        {
            SoundEffectPlayer.Play(SoundEffectIndex.PlayerHit);

            this.velocityX = KNOCKBACK_HORIZONTAL_FORCE * (this.PositionX < origin.X ? -1.0f : 1.0f);
            this.velocityY = KNOCKBACK_VERTICAL_FORCE * -0.75f;
        }

        private void CreateDamageResidues()
        {
            for (int i = 0; i < RESIDUE_COUNT; i++)
            {
                if (this.ActorManager.TryCreate(ActorIndex.PlayerResidue, out Actor playerResidueActor))
                {
                    playerResidueActor.Position = new(this.PositionX + (this.Width / 2), this.PositionY + (this.Height / 2));

                    float angle = i * (MathF.PI / (RESIDUE_COUNT / 2.0f));
                    ((PlayerResidue)playerResidueActor).Direction = new(MathF.Cos(angle), MathF.Sin(angle));
                }
            }
        }

        internal void TakeDamage(byte value, Point origin)
        {
            if (this.IsDead || this.HasTemporarilyInvincible)
            {
                return;
            }

            if (value >= this.Health)
            {
                this.Health = 0;
                PerformDeath(origin);
                return;
            }

            GameHandler.Freeze(0.25f);
            this.Health -= value;
            this.HasTemporarilyInvincible = true;
            CreateDamageResidues();
            PerformKnockback(origin);
        }

        internal void Kill()
        {
            if (!this.IsDead)
            {
                this.Health = 0;
                PerformDeath(this.Position);
            }
        }

        internal void Heal(byte value)
        {
            if (this.IsDead)
            {
                return;
            }

            this.Health = (byte)Math.Min(this.Health + value, DEFAULT_HEALTH);
        }

        internal override void OnActorCollision(Actor otherActor)
        {
            if (otherActor.Type is ActorType.Enemy or ActorType.Boss || (otherActor.Type is ActorType.Projectile && otherActor.Team is ActorTeam.Bad))
            {
                TakeDamage(1, otherActor.Position);
            }
        }

        internal override void OnOutsideLevelBounds(Direction direction, int distance)
        {
            if (direction is Direction.Down)
            {
                Kill();
            }
        }

        private void AccelerateHorizontalSpeed(float targetSpeed)
        {
            if (this.velocityX < targetSpeed)
            {
                this.velocityX = MathF.Min(this.velocityX + ACCELERATION, targetSpeed);
            }
            else if (this.velocityX > targetSpeed)
            {
                this.velocityX = MathF.Max(this.velocityX - ACCELERATION, targetSpeed);
            }
        }

        private void DecelerateHorizontalSpeed()
        {
            if (this.velocityX > 0.0f)
            {
                this.velocityX = MathF.Max(this.velocityX - DECELERATION, 0.0f);
            }
            else if (this.velocityX < 0.0f)
            {
                this.velocityX = MathF.Min(this.velocityX + DECELERATION, 0.0f);
            }
        }

        private void HandleMovementOrDeceleration(float targetSpeed)
        {
            if (this.horizontalInputDirection != 0)
            {
                AccelerateHorizontalSpeed(targetSpeed);
            }
            else
            {
                DecelerateHorizontalSpeed();
            }
        }

        private void UpdateHorizontalInput()
        {
            bool isLeftPressed = Controller.IsMoveLeftPressed();
            bool isRightPressed = Controller.IsMoveRightPressed();
            bool isRunPressed = Controller.IsRunPressed();

            this.horizontalInputDirection = (sbyte)((isLeftPressed ? -1 : 0) + (isRightPressed ? 1 : 0));

            if (this.horizontalInputDirection > 0)
            {
                this.isFacingRight = true;
            }
            else if (this.horizontalInputDirection < 0)
            {
                this.isFacingRight = false;
            }

            HandleMovementOrDeceleration(WALK_SPEED * (isRunPressed ? RUN_MULTIPLIER : 1.0f) * this.horizontalInputDirection);
        }

        private void Jump(float force)
        {
            SoundEffectPlayer.Play(SoundEffectIndex.PlayerJump);

            this.isGrounded = false;
            this.coyoteTimeCounter = 0.0f;

            this.velocityY = force * -1;
        }

        private void HandleJumping()
        {
            // Jump if jump is started and player is grounded or within coyote time
            if (Controller.IsJumpStarted() && (this.isGrounded || this.coyoteTimeCounter > 0.0f))
            {
                Jump(JUMP_SPEED);
            }
        }

        private void HandleJumpCutoff()
        {
            if (Controller.IsJumpCanceled() && this.velocityY < 0.0f)
            {
                this.velocityY *= JUMP_CUTOFF_MULTIPLIER;
            }
        }

        private void UpdateVerticalInput()
        {
            HandleJumping();
            HandleJumpCutoff();
        }

        private void UpdateShootingInput()
        {
            if (++this.shootFrameCounter < SHOOT_FRAME_DELAY || !Controller.IsShootStarted())
            {
                return;
            }

            this.shootFrameCounter = 0;
            SoundEffectPlayer.Play(SoundEffectIndex.PlayerShooting);

            switch (this.SpellType)
            {
                case SpellType.Bolt:
                    if (this.ActorManager.TryCreate(ActorIndex.BoltSpell, out Actor magicBallActor))
                    {
                        BoltSpell magicBall = (BoltSpell)magicBallActor;

                        magicBall.PositionX = this.PositionX + (this.isFacingRight ? BULLET_DISTANCE : -BULLET_DISTANCE);
                        magicBall.PositionY = this.PositionY;
                        magicBall.HorizontalDirection = (sbyte)(this.isFacingRight ? 1 : -1);
                    }

                    break;

                case SpellType.Bounce:
                    if (this.ActorManager.TryCreate(ActorIndex.BounceSpell, out Actor bounceSpellActor))
                    {
                        BounceSpell bounceSpell = (BounceSpell)bounceSpellActor;

                        bounceSpell.PositionX = this.PositionX + (this.isFacingRight ? BULLET_DISTANCE : -BULLET_DISTANCE);
                        bounceSpell.PositionY = this.PositionY;
                        bounceSpell.HorizontalDirection = (sbyte)(this.isFacingRight ? 1 : -1);
                    }

                    break;

                case SpellType.ZigZag:
                    if (this.ActorManager.TryCreate(ActorIndex.ZigZagSpell, out Actor zigZagSpellActor))
                    {
                        ZigZagSpell zigZagSpell = (ZigZagSpell)zigZagSpellActor;

                        zigZagSpell.PositionX = this.PositionX + (this.isFacingRight ? BULLET_DISTANCE : -BULLET_DISTANCE);
                        zigZagSpell.PositionY = this.PositionY;

                        zigZagSpell.Direction = new(this.isFacingRight ? 1 : -1, 1);
                    }

                    break;

                case SpellType.Wave:
                    for (int i = 0; i < 6; i++)
                    {
                        if (this.ActorManager.TryCreate(ActorIndex.WaveSpell, out Actor waveSpellActor))
                        {
                            WaveSpell waveSpell = (WaveSpell)waveSpellActor;

                            waveSpell.PositionX = this.PositionX + (this.isFacingRight ? BULLET_DISTANCE : -BULLET_DISTANCE);
                            waveSpell.PositionY = this.PositionY;

                            float angle = i * (MathF.PI / (6.0f / 2.0f));
                            waveSpell.Direction = new(MathF.Cos(angle), MathF.Sin(angle));
                        }
                    }

                    break;

                default:
                    break;
            }
        }

        private void UpdateGroundedState()
        {
            Rectangle belowRect = this.Bounds;
            belowRect.Offset(0, 1);

            bool somethingBelow = this.TileMap.IsCollideAt(belowRect, 0, 1);

            if (this.isGrounded != somethingBelow)
            {
                this.isGrounded = somethingBelow;
            }
        }

        private void UpdateAnimation()
        {
            if (++this.animationFrameCounter < ANIMATION_FRAME_DELAY)
            {
                return;
            }

            this.animationFrameCounter = 0;

            if (this.IsDead)
            {
                // ========================= //
                // Death Animation Routine   //
                // ========================= //

                if (++this.deathAnimationFrameIndex > deathAnimationFrameRectangles.Length - 1)
                {
                    this.deathAnimationFrameIndex = 0;
                }

                this.currentSpriteRectangle = deathAnimationFrameRectangles[this.deathAnimationFrameIndex];
            }
            else
            {
                // ========================= //
                // Normal Animation Routine  //
                // ========================= //

                // Update Current Frame
                this.isFirstAnimationFrame = !this.isFirstAnimationFrame;

                // Determine Frame Rectangle
                byte directionIndex = Convert.ToByte(!this.isFacingRight);
                bool hasAnimation = false;

                if (this.IsJumping || this.IsFalling)
                {
                    this.currentSpriteRectangle = normalAnimationFrameRectangles[(directionIndex * ANIMATION_LEFT_DIRECTION_OFFSET) + 1];
                    hasAnimation = true;
                }
                else if (this.IsWalking)
                {
                    this.currentSpriteRectangle = normalAnimationFrameRectangles[directionIndex * ANIMATION_LEFT_DIRECTION_OFFSET];
                    hasAnimation = true;
                }
                else
                {
                    this.currentSpriteRectangle = normalAnimationFrameRectangles[directionIndex * ANIMATION_LEFT_DIRECTION_OFFSET];
                }

                if (hasAnimation && !this.isFirstAnimationFrame)
                {
                    this.currentSpriteRectangle.Offset(16, 0);
                }
            }
        }

        private void UpdateTemporaryInvincibility()
        {
            // If not currently invincible, no need to update
            if (!this.HasTemporarilyInvincible)
            {
                return;
            }

            // Update Invincibility Frame Counter
            if (++this.invincibilityFrameCounter >= INVINCIBILITY_DURATION_FRAMES)
            {
                this.HasTemporarilyInvincible = false;
                this.invincibilityFrameCounter = 0;
                this.invincibilityBlinkFrameCounter = 0;
                this.CanDraw = true;
                return;
            }

            // Update Blink State
            if (++this.invincibilityBlinkFrameCounter >= INVINCIBILITY_BLINK_INTERVAL_FRAMES)
            {
                this.invincibilityBlinkFrameCounter = 0;
                this.CanDraw = !this.CanDraw;
            }
        }

        private void EnforceHorizontalCameraBounds()
        {
            int minX = CameraHandler.Position.X;
            int maxX = CameraHandler.Position.X + Constants.SCREEN_WIDTH - this.Width;

            if (this.PositionX < minX)
            {
                this.PositionX = minX;
                this.velocityX = 0.0f;
            }
            else if (this.PositionX > maxX)
            {
                this.PositionX = maxX;
                this.velocityX = 0.0f;
            }
        }

        internal override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            UpdateGroundedState();
            UpdateAnimation();
            UpdateTemporaryInvincibility();

            #region INPUT

            if (!this.IsDead)
            {
                // Update Coyote Time Counter
                this.coyoteTimeCounter = this.isGrounded
                    ? COYOTE_TIME_MAX
                    : MathF.Max(0.0f, this.coyoteTimeCounter - deltaTime);

                UpdateHorizontalInput();
                UpdateVerticalInput();
                UpdateShootingInput();
            }

            #endregion

            #region PHYSICS

            // Apply Gravity
            if (!this.isGrounded)
            {
                this.velocityY = MathF.Min(this.velocityY + (GRAVITY * deltaTime), MAX_FALL_SPEED);
            }

            // Move Player
            MoveHorizontally(this.velocityX * deltaTime);
            MoveVertically(this.velocityY * deltaTime);

            if (!this.IsDead)
            {
                EnforceHorizontalCameraBounds();
            }

            #endregion
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(AssetDatabase.GetTexture(TextureIndex.Actors), new(this.PositionX, this.PositionY), this.currentSpriteRectangle, Color.White, 0.0f, new Vector2(4, 0), Vector2.One, SpriteEffects.None, 0.0f);
        }
    }
}
