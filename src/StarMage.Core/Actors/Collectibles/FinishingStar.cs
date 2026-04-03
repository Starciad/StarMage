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
    internal sealed class FinishingStar : Actor
    {
        private enum CollectedState : byte
        {
            ReturningToOrigin = 0,
            Waiting = 1,
            Ascending = 2,
            Completed = 3
        }

        private Point originalPosition;
        private float angle;

        private byte animationFrameCounter;
        private byte animationFrameIndex;

        private bool isCollected;
        private bool isFinished;

        private CollectedState collectedState;
        private float waitTimer;
        private float nextLevelTimer;

        private static readonly Rectangle[] animationFrameRectangles =
        [
            new(128, 00, 16, 16),
            new(144, 00, 16, 16),
            new(128, 16, 16, 16),
            new(144, 16, 16, 16)
        ];

        private const byte ANIMATION_FRAME_DELAY = 6;
        private const float SPEED = 25.0f;
        private const float RADIUS = 32.0f;
        private const float RETURN_SPEED = 80.0f;
        private const float WAIT_DELAY = 1.5f;
        private const float ASCEND_SPEED = 250.0f;
        private const float NEXT_LEVEL_DELAY = 4.5f;

        internal FinishingStar(ActorIndex index, ActorType type, ActorTeam team, ActorManager actorManager, TileMap tileMap) : base(index, type, team, actorManager, tileMap)
        {
            Reset();
        }

        public override void Reset()
        {
            this.originalPosition = Point.Zero;
            this.angle = 0.0f;
            this.animationFrameCounter = 0;
            this.animationFrameIndex = 0;
            this.isCollected = false;
            this.isFinished = false;
            this.collectedState = CollectedState.ReturningToOrigin;
            this.waitTimer = 0.0f;
            this.nextLevelTimer = 0.0f;
        }

        internal override void OnCreated()
        {
            this.originalPosition = this.Position;
        }

        internal override void OnActorCollision(Actor otherActor)
        {
            if (!this.isCollected && otherActor.Type is ActorType.Player)
            {
                otherActor.CanUpdate = false;
                otherActor.CanDraw = false;
                otherActor.Position = this.originalPosition;
                this.isCollected = true;
            }
        }

        internal override void OnOutsideLevelBounds(Direction direction, int distance)
        {
            if (direction is Direction.Up)
            {
                this.collectedState = CollectedState.Completed;
            }
        }

        private void UpdateAnimation()
        {
            if (++this.animationFrameCounter >= ANIMATION_FRAME_DELAY)
            {
                this.animationFrameCounter = 0;
                this.animationFrameIndex = (byte)((this.animationFrameIndex + 1) % animationFrameRectangles.Length);
            }
        }

        private void UpdateCollectedAnimation(float deltaTime)
        {
            switch (this.collectedState)
            {
                case CollectedState.ReturningToOrigin:
                {
                    float dx = this.originalPosition.X - this.PositionX;
                    float dy = this.originalPosition.Y - this.PositionY;

                    float moveStep = RETURN_SPEED * deltaTime;

                    if (MathF.Abs(dx) <= moveStep)
                    {
                        MoveHorizontally(dx);
                    }
                    else
                    {
                        MoveHorizontally(MathF.Sign(dx) * moveStep);
                    }

                    if (MathF.Abs(dy) <= moveStep)
                    {
                        MoveVertically(dy);
                    }
                    else
                    {
                        MoveVertically(MathF.Sign(dy) * moveStep);
                    }

                    dx = this.originalPosition.X - this.PositionX;
                    dy = this.originalPosition.Y - this.PositionY;

                    if (MathF.Abs(dx) < 0.5f && MathF.Abs(dy) < 0.5f)
                    {
                        MoveHorizontally(dx);
                        MoveVertically(dy);

                        this.collectedState = CollectedState.Waiting;
                        this.waitTimer = 0.0f;
                    }

                    break;
                }

                case CollectedState.Waiting:
                {
                    this.waitTimer += deltaTime;

                    if (this.waitTimer >= WAIT_DELAY)
                    {
                        this.collectedState = CollectedState.Ascending;
                        this.waitTimer = 0.0f;
                    }

                    break;
                }

                case CollectedState.Ascending:
                {
                    MoveVertically(ASCEND_SPEED * deltaTime * -1.0f);
                    break;
                }

                case CollectedState.Completed:
                default:
                    break;
            }
        }

        private void UpdateCircularMovement(float deltaTime)
        {
            this.angle += SPEED / RADIUS * deltaTime;

            if (this.angle > MathF.Tau)
            {
                this.angle -= MathF.Tau;
            }

            float targetX = this.originalPosition.X + (MathF.Cos(this.angle) * RADIUS);
            float targetY = this.originalPosition.Y + (MathF.Sin(this.angle) * RADIUS);

            float deltaX = targetX - this.PositionX;
            float deltaY = targetY - this.PositionY;

            MoveHorizontally(deltaX);
            MoveVertically(deltaY);
        }

        internal override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            UpdateAnimation();

            if (this.isCollected)
            {
                UpdateCollectedAnimation(deltaTime);

                this.nextLevelTimer += deltaTime;

                if (!this.isFinished && this.nextLevelTimer >= NEXT_LEVEL_DELAY)
                {
                    this.isFinished = true;
                    GameHandler.NextLevel();
                }
            }
            else
            {
                UpdateCircularMovement(deltaTime);
            }
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(AssetDatabase.GetTexture(TextureIndex.Actors), new(this.PositionX, this.PositionY), animationFrameRectangles[this.animationFrameIndex], Color.White, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.0f);
        }
    }
}
