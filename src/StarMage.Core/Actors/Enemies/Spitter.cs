using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StarMage.Core.Actors.Players;
using StarMage.Core.Actors.Projectiles;
using StarMage.Core.Databases;
using StarMage.Core.Enums;
using StarMage.Core.Extensions;
using StarMage.Core.Managers;
using StarMage.Core.Tiles;

using System;

namespace StarMage.Core.Actors.Enemies
{
    internal sealed class Spitter : Actor
    {
        private MagePlayer magePlayer;

        private bool isFirstFrame;
        private bool isShowingDamage;
        private bool isPlayerDetected;

        private Direction gripDirection;

        private byte health;
        private byte animationFrameCounter;
        private byte hitAnimationCounter;

        private byte spitCooldownCounter;

        private Rectangle currentAnimationFrame;

        private static readonly Rectangle[] animationFrames =
        [
            // Holding onto the ceiling
            new(096, 32, 16, 16), // Frame 1
            new(112, 32, 16, 16), // Frame 2
            new(112, 80, 16, 16), // Hit

            // Holding onto the right wall
            new(096, 48, 16, 16), // Frame 1
            new(112, 48, 16, 16), // Frame 2
            new(096, 80, 16, 16), // Hit
            
            // Holding onto the floor
            new(096, 00, 16, 16), // Frame 1
            new(112, 00, 16, 16), // Frame 2
            new(096, 64, 16, 16), // Hit

            // Holding onto the left wall
            new(096, 16, 16, 16), // Frame 1
            new(112, 16, 16, 16), // Frame 2
            new(112, 64, 16, 16), // Hit
        ];

        private const byte DEFAULT_ANIMATION_FRAME_DELAY = 12;
        private const byte DETECTION_ANIMATION_FRAME_DELAY = 4;

        private const byte DEFAULT_HEALTH = 12;
        private const byte DEFAULT_SPIT_COOLDOWN = 180;
        private const byte HIT_ANIMATION_DURATION = 8;
        private const byte FRAMES_PER_DIRECTION = 3;

        private const float DETECTION_RANGE = 80.0f;

        private const float SPINY_BALL_LATERAL_SPEED = 80.0f;
        private const float SPINY_BALL_IMPULSE_FORCE = 285.0f;

        internal Spitter(ActorIndex index, ActorType type, ActorTeam team, ActorManager actorManager, TileMap tileMap) : base(index, type, team, actorManager, tileMap)
        {
            Reset();
        }

        public override void Reset()
        {
            this.isFirstFrame = true;
            this.isShowingDamage = false;

            this.gripDirection = Direction.Down;

            this.health = DEFAULT_HEALTH;
            this.animationFrameCounter = 0;
            this.hitAnimationCounter = 0;

            this.currentAnimationFrame = animationFrames[0];
        }

        internal override void OnCreated()
        {
            Point tilePosition = TileMap.WorldToTile(this.Position);

            ReadOnlySpan<(Point Offset, Direction Grip)> adjacentChecks =
            [
                (new(0, 1), Direction.Down),
                (new(0, -1), Direction.Up),
                (new(1, 0), Direction.Right),
                (new(-1, 0), Direction.Left)
            ];

            foreach ((Point Offset, Direction Grip) in adjacentChecks)
            {
                Point targetPosition = new(tilePosition.X + Offset.X, tilePosition.Y + Offset.Y);

                if (this.TileMap.IsWithinBounds(targetPosition) && TileProperty.IsSolidTile(this.TileMap[targetPosition]))
                {
                    this.gripDirection = Grip;
                    break;
                }
            }

            this.magePlayer = (MagePlayer)this.ActorManager.GetActor(ActorIndex.MagePlayer);
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
            ActorHelpers.CreateEnemyDeathResidues(this.ActorManager, this.Position, this.Size);
        }

        private void UpdatePlayerDetection()
        {
            if (this.magePlayer == null || this.magePlayer.IsDead)
            {
                return;
            }

            this.isPlayerDetected = this.Position.DistanceTo(this.magePlayer.Position) <= DETECTION_RANGE;
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
            if (++this.animationFrameCounter < (this.isPlayerDetected ? DETECTION_ANIMATION_FRAME_DELAY : DEFAULT_ANIMATION_FRAME_DELAY))
            {
                return;
            }

            this.animationFrameCounter = 0;
            this.isFirstFrame = !this.isFirstFrame;

            int baseFrameIndex = ((int)this.gripDirection - 1) * FRAMES_PER_DIRECTION;
            int frameOffset = this.isShowingDamage ? 2 : this.isFirstFrame ? 0 : 1;

            this.currentAnimationFrame = animationFrames[baseFrameIndex + frameOffset];
        }

        private void UpdateAttackBehaviour()
        {
            if (!this.isPlayerDetected)
            {
                return;
            }

            if (this.spitCooldownCounter > 0)
            {
                this.spitCooldownCounter--;
                return;
            }

            this.spitCooldownCounter = DEFAULT_SPIT_COOLDOWN;

            if (this.ActorManager.TryCreate(ActorIndex.SpinyBall, out Actor spinyBallActor))
            {
                SpinyBall spinyBall = (SpinyBall)spinyBallActor;
                spinyBall.Position = this.Position;

                sbyte playerDirectionX = (sbyte)Math.Sign(this.magePlayer.Position.X - this.Position.X);

                Vector2 initialVelocity = this.gripDirection switch
                {
                    Direction.Up => new(SPINY_BALL_LATERAL_SPEED * playerDirectionX, -SPINY_BALL_IMPULSE_FORCE),
                    Direction.Right => new(SPINY_BALL_LATERAL_SPEED, 0),
                    Direction.Down => new(SPINY_BALL_LATERAL_SPEED * playerDirectionX, SPINY_BALL_IMPULSE_FORCE),
                    Direction.Left => new(-SPINY_BALL_LATERAL_SPEED, 0),
                    _ => Vector2.Zero,
                };

                spinyBall.VelocityX = initialVelocity.X;
                spinyBall.VelocityY = initialVelocity.Y;
            }
        }

        internal override void Update(GameTime gameTime)
        {
            UpdatePlayerDetection();
            UpdateDamageAnimation();
            UpdateSpriteAnimation();
            UpdateAttackBehaviour();
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(AssetDatabase.GetTexture(TextureIndex.Actors), new(this.PositionX, this.PositionY), this.currentAnimationFrame, Color.White, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.0f);
        }
    }
}
