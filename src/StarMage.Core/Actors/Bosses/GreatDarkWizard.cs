using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StarMage.Core.Actors.Players;
using StarMage.Core.Actors.Projectiles;
using StarMage.Core.Databases;
using StarMage.Core.Enums;
using StarMage.Core.Extensions;
using StarMage.Core.Handlers;
using StarMage.Core.Managers;
using StarMage.Core.Tiles;

using System;

namespace StarMage.Core.Actors.Bosses
{
    /*
     * A particular characteristic of this enemy is its rendering.
     * 
     * Basically, each part of its body (HEAD, BODY, and HAND) acts semi-independently, following a common point for the entity.
     * 
     * This creates an interesting and dynamic effect for the boss, avoiding the need for complex animations and making development a little easier.
    */
    internal sealed class GreatDarkWizard : Actor
    {
        private MagePlayer magePlayer;

        private sbyte facingDirection;
        private Vector2 movementDirection;

        private int health;
        private float speed;

        private byte attackFrameCounter;

        private byte hitAnimationCounter;
        private byte attackAnimationCounter;

        private byte handAttackAnimationIndex;

        private bool isShowingDamage;
        private bool isAttacking;
        private bool isLoading;

        // Positions of the body parts relative to the entity's position.
        // These will be used to calculate the actual positions of the body parts when drawing.
        private float targetHeadPositionX, targetHeadPositionY, targetBodyPositionX, targetBodyPositionY, targetHandPositionX, targetHandPositionY;
        private float currentHeadPositionX, currentHeadPositionY, currentBodyPositionX, currentBodyPositionY, currentHandPositionX, currentHandPositionY;

        private static readonly Rectangle[] headSpriteRectangles =
        [
            // Normal
            new(160, 0, 32, 32), // Looking to the Left
            new(192, 0, 32, 32), // Looking to the Center
            new(224, 0, 32, 32), // Looking to the Right

            // Damaged
            new(160, 32, 32, 32), // Looking to the Left
            new(192, 32, 32, 32), // Looking to the Center
            new(224, 32, 32, 32), // Looking to the Right
        ];

        // The body has a single neutral sprite that can be used regardless of direction.
        private static readonly Rectangle[] bodySpriteRectangles =
        [
            // Normal
            new(160, 64, 32, 32),

            // Damaged
            new(192, 64, 32, 32),
        ];

        private static readonly Rectangle[] handSpriteRectangles =
        [
            // Normal
            new(160, 96, 32, 32), // Looking to the Left
            new(192, 96, 32, 32), // Looking to the Center
            new(224, 96, 32, 32), // Looking to the Right

            // Damaged
            new(160, 128, 32, 32), // Looking to the Left
            new(192, 128, 32, 32), // Looking to the Center
            new(224, 128, 32, 32), // Looking to the Right
        ];

        private static readonly Rectangle[] handAttackSpriteRectangles =
        [
            #region COUNTERCLOCKWISE DIRECTION

            // Normal
            new(48, 160, 48, 48),
            new(0, 208, 48, 48),
            new(48, 208, 48, 48),

            // Damaged
            new(144, 160, 48, 48),
            new(96, 208, 48, 48),
            new(144, 208, 48, 48),

            #endregion

            #region CLOCKWISE DIRECTION

            // Normal
            new(48, 256, 48, 48),
            new(0, 304, 48, 48),
            new(48, 304, 48, 48),

            // Damaged
            new(144, 256, 48, 48),
            new(96, 304, 48, 48),
            new(144, 304, 48, 48),

            #endregion
        ];

        private const byte DEFAULT_HEALTH = 30;
        private const byte ATTACK_FRAME_DELAY = 160;
        private const byte HIT_ANIMATION_DURATION = 8;
        private const byte ATTACK_ANIMATION_DURATION = 4;
        private const byte PROJECTILE_COUNT = 3;

        private const float PROJECTILE_ANGLE_OFFSET = 15.0f;
        private const float DEFAULT_SPEED = 32.0f;
        private const float LOADING_AREA = 128.0f;

        internal GreatDarkWizard(ActorIndex index, ActorType type, ActorTeam team, ActorManager actorManager, TileMap tileMap) : base(index, type, team, actorManager, tileMap)
        {
            Reset();
        }

        public override void Reset()
        {
            // Defining the boss's challenge based on the current world
            switch (GameHandler.CurrentLevelIndex.World)
            {
                case 1:
                    this.health = (int)(DEFAULT_HEALTH * 0.75f); // 25% less health in World 1
                    this.speed = DEFAULT_SPEED * 0.75f; // 25% slower speed in World 1
                    break;

                case 2:
                    this.health = (int)(DEFAULT_HEALTH * 1.25f); // 25% more health in World 2
                    this.speed = DEFAULT_SPEED * 1.25f; // 25% faster speed in World 2
                    break;

                case 3:
                    this.health = (int)(DEFAULT_HEALTH * 1.5f); // 50% more health in World 3
                    this.speed = DEFAULT_SPEED * 1.5f; // 50% faster speed in World 3
                    break;

                case 4:
                    this.health = (int)(DEFAULT_HEALTH * 2.0f); // 100% more health in World 4
                    this.speed = DEFAULT_SPEED * 2.0f; // 100% faster speed in World 4
                    break;

                default:
                    this.health = DEFAULT_HEALTH; // Default health for any other world (shouldn't happen)
                    this.speed = DEFAULT_SPEED; // Default speed for any other world (shouldn't happen)
                    break;
            }

            // The boss starts facing to the right and moving diagonally downwards.
            this.facingDirection = 0;
            this.movementDirection = new Vector2(1, 1);
            this.isShowingDamage = false;
            this.isLoading = false;

            // Reset body part positions to the entity's position.
            this.currentHeadPositionX = this.PositionX;
            this.currentHeadPositionY = this.PositionY - 16; // Head is above the body
            this.currentBodyPositionX = this.PositionX;
            this.currentBodyPositionY = this.PositionY;
            this.currentHandPositionX = this.PositionX; // Hand starts at the same position as the body, but will be offset based on facing direction
            this.currentHandPositionY = this.PositionY + 8; // Hand is below the body
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
                    this.ActorManager.Destroy(this);
                    return;
                }

                this.isShowingDamage = true;
            }
        }

        internal override void OnDestroyed()
        {
            if (this.magePlayer.IsDead)
            {
                return; // Don't trigger the boss defeat sequence if the player is already dead
            }

            ActorHelpers.CreateEnemyDeathResidues(this.ActorManager, this.Position, this.Size);
            GameHandler.BossDefeated();

            if (this.ActorManager.TryCreate(ActorIndex.FinishingStar, out Actor finishingStarActor))
            {
                finishingStarActor.PositionX = CameraHandler.Position.X + (Constants.SCREEN_WIDTH / 2) - (finishingStarActor.Width / 2);
                finishingStarActor.PositionY = CameraHandler.Position.Y + (Constants.SCREEN_HEIGHT / 2) - (finishingStarActor.Height / 2);
            }
        }

        private void UpdateLoadingState()
        {
            if (!this.isLoading && this.Position.DistanceTo(this.magePlayer.Position) <= LOADING_AREA)
            {
                this.isLoading = true;
                GameHandler.BossAppear();
            }
        }

        private void UpdateFacingDirection()
        {
            if (this.magePlayer.Position.X < this.Position.X - 32)
            {
                this.facingDirection = -1; // Looking to the Left
            }
            else if (this.magePlayer.Position.X > this.Position.X + 32)
            {
                this.facingDirection = 1; // Looking to the Right
            }
            else
            {
                this.facingDirection = 0; // Looking to the Center
            }
        }

        private void UpdateBodyPartTargets()
        {
            // Update Targets
            // Head
            this.targetHeadPositionX = this.PositionX;
            this.targetHeadPositionY = this.PositionY - 16;

            // Body
            this.targetBodyPositionX = this.PositionX;
            this.targetBodyPositionY = this.PositionY;

            // Hand
            this.targetHandPositionX = this.PositionX - (this.facingDirection * 18);
            this.targetHandPositionY = this.PositionY - 8;
        }

        private void InterpolateBodyPartPositions()
        {
            // Smoothly move the body parts towards their target positions.
            // This creates a nice animation effect without needing complex sprite animations.
            // Head
            this.currentHeadPositionX += (this.targetHeadPositionX - this.currentHeadPositionX) * 0.1f;
            this.currentHeadPositionY += (this.targetHeadPositionY - this.currentHeadPositionY) * 0.1f;

            // Body
            this.currentBodyPositionX += (this.targetBodyPositionX - this.currentBodyPositionX) * 0.1f;
            this.currentBodyPositionY += (this.targetBodyPositionY - this.currentBodyPositionY) * 0.1f;

            // Hand
            this.currentHandPositionX += (this.targetHandPositionX - this.currentHandPositionX) * 0.1f;
            this.currentHandPositionY += (this.targetHandPositionY - this.currentHandPositionY) * 0.1f;
        }

        private void UpdateDamageAnimation()
        {
            if (!this.isShowingDamage)
            {
                return;
            }

            if (++this.hitAnimationCounter >= HIT_ANIMATION_DURATION)
            {
                this.hitAnimationCounter = 0;
                this.isShowingDamage = false;
            }
        }

        private void UpdateAttackAnimation()
        {
            if (!this.isAttacking)
            {
                return;
            }

            // Change attack hand index every few frames to create a simple attack animation.
            if (++this.attackAnimationCounter >= ATTACK_ANIMATION_DURATION)
            {
                this.attackAnimationCounter = 0;
                this.handAttackAnimationIndex++;

                if (this.handAttackAnimationIndex >= 3)
                {
                    this.handAttackAnimationIndex = 0;
                    this.isAttacking = false;
                }
            }
        }

        private void UpdateMovement()
        {
            int minX = CameraHandler.Position.X + this.Width - (this.Width / 2);
            int minY = this.Height - (this.Height / 2);
            int maxX = CameraHandler.Position.X + Constants.SCREEN_WIDTH - this.Width - Constants.TILE_SIZE;
            int maxY = CameraHandler.Position.Y + Constants.SCREEN_HEIGHT - this.Height - (Constants.TILE_SIZE * 4);

            if (this.PositionX < minX || this.PositionX >= maxX)
            {
                this.movementDirection.X *= -1; // Reverse horizontal direction
            }

            if (this.PositionY < minY || this.PositionY >= maxY)
            {
                this.movementDirection.Y *= -1; // Reverse vertical direction
            }
        }

        private void UpdateAttack()
        {
            if (++this.attackFrameCounter < ATTACK_FRAME_DELAY)
            {
                return;
            }

            this.attackFrameCounter = 0;
            this.isAttacking = true;

            // Spawn projectiles
            Point offset = new(this.facingDirection * 12, -4);

            // Calculate base angle towards the player
            float baseAngle = MathF.Atan2(this.magePlayer.PositionY - this.PositionY, this.magePlayer.PositionX - this.PositionX);

            for (byte i = 0; i < PROJECTILE_COUNT; i++)
            {
                if (this.ActorManager.TryCreate(ActorIndex.EvilStar, out Actor projectileActor))
                {
                    // Spread the projectiles evenly within the angle offset
                    float spreadAngle = -PROJECTILE_ANGLE_OFFSET + (i * (PROJECTILE_ANGLE_OFFSET * 2 / (PROJECTILE_COUNT - 1)));
                    float finalAngle = baseAngle + MathHelper.ToRadians(spreadAngle);

                    Vector2 direction = new(MathF.Cos(finalAngle), MathF.Sin(finalAngle));

                    projectileActor.PositionX = this.PositionX + offset.X;
                    projectileActor.PositionY = this.PositionY + offset.Y;

                    ((EvilStar)projectileActor).Direction = direction;
                }
            }
        }

        internal override void Update(GameTime gameTime)
        {
            if (!this.isLoading)
            {
                UpdateLoadingState();
                return;
            }

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            UpdateFacingDirection();
            UpdateBodyPartTargets();
            InterpolateBodyPartPositions();
            UpdateDamageAnimation();
            UpdateAttackAnimation();
            UpdateMovement();
            UpdateAttack();

            MoveHorizontally(this.movementDirection.X * this.speed * deltaTime);
            MoveVertically(this.movementDirection.Y * this.speed * deltaTime);
        }

        private Rectangle GetHeadSpriteRectangle()
        {
            byte directionIndex = (byte)(this.facingDirection + 1); // Convert from -1, 0, 1 to 0, 1, 2
            byte damageIndex = (byte)(this.isShowingDamage ? 3 : 0); // Normal sprites are at index 0-2, damaged sprites are at index 3-5
            return headSpriteRectangles[directionIndex + damageIndex];
        }

        private Rectangle GetBodySpriteRectangle()
        {
            byte damageIndex = (byte)(this.isShowingDamage ? 1 : 0); // Normal sprite is at index 0, damaged sprite is at index 1
            return bodySpriteRectangles[damageIndex];
        }

        private Rectangle GetHandSpriteRectangle()
        {
            if (this.isAttacking)
            {
                byte directionIndex = (byte)(this.facingDirection + 1); // Convert from -1, 0, 1 to 0, 1, 2
                byte damageIndex = (byte)(this.isShowingDamage ? 3 : 0); // Normal attack sprites are at index 0-3, damaged attack sprites are at index 4-7
                byte attackIndex = this.handAttackAnimationIndex; // Attack animation frames are at index 0-3 for normal and 4-7 for damaged

                return handAttackSpriteRectangles[(directionIndex * 3) + attackIndex + damageIndex];
            }
            else
            {
                byte directionIndex = (byte)(-this.facingDirection + 1); // The hand faces the opposite direction of the head, so we invert the direction index
                byte damageIndex = (byte)(this.isShowingDamage ? 3 : 0); // Normal sprites are at index 0-2, damaged sprites are at index 3-5

                return handSpriteRectangles[directionIndex + damageIndex];
            }
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            // Body
            spriteBatch.Draw(AssetDatabase.GetTexture(TextureIndex.Actors), new Vector2(this.currentBodyPositionX, this.currentBodyPositionY), GetBodySpriteRectangle(), Color.White, 0f, new(4), Vector2.One, SpriteEffects.None, 0f);

            // Head
            spriteBatch.Draw(AssetDatabase.GetTexture(TextureIndex.Actors), new Vector2(this.currentHeadPositionX, this.currentHeadPositionY), GetHeadSpriteRectangle(), Color.White, 0f, new(4), Vector2.One, SpriteEffects.None, 0f);

            // Hand
            spriteBatch.Draw(AssetDatabase.GetTexture(TextureIndex.Actors), new Vector2(this.currentHandPositionX, this.currentHandPositionY), GetHandSpriteRectangle(), Color.White, 0f, new(4), Vector2.One, SpriteEffects.None, 0f);
        }
    }
}
