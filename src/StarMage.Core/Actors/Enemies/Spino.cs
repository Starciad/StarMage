using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StarMage.Core.Databases;
using StarMage.Core.Enums;
using StarMage.Core.Managers;
using StarMage.Core.Tiles;

namespace StarMage.Core.Actors.Enemies
{
    internal sealed class Spino : Actor
    {
        private bool isFirstFrame;
        private bool isShowingDamage;

        private Direction gripDirection;
        private sbyte movementDirection;

        private byte health;
        private byte animationFrameCounter;
        private byte hitAnimationCounter;

        private float velocityX;
        private float velocityY;

        private Rectangle currentAnimationFrame;

        private static readonly Rectangle[] animationFrames =
        [
            // Holding onto the ceiling
            new(00, 96, 16, 16), // Frame 1
            new(16, 96, 16, 16), // Frame 2
            new(80, 96, 16, 16), // Hit

            // Holding onto the right wall
            new(32, 80, 16, 16), // Frame 1
            new(32, 96, 16, 16), // Frame 2
            new(64, 96, 16, 16), // Hit
            
            // Holding onto the floor
            new(00, 80, 16, 16), // Frame 1
            new(16, 80, 16, 16), // Frame 2
            new(64, 80, 16, 16), // Hit

            // Holding onto the left wall
            new(48, 80, 16, 16), // Frame 1
            new(48, 96, 16, 16), // Frame 2
            new(80, 80, 16, 16), // Hit
        ];

        private const byte ANIMATION_FRAME_DELAY = 3;
        private const byte DEFAULT_HEALTH = 6;
        private const byte HIT_ANIMATION_DURATION = 8;
        private const byte FRAMES_PER_DIRECTION = 3;
        private const float SPEED = 28.0f;

        internal Spino(ActorIndex index, ActorType type, ActorTeam team, ActorManager actorManager, TileMap tileMap) : base(index, type, team, actorManager, tileMap)
        {
            Reset();
        }

        public override void Reset()
        {
            this.isFirstFrame = true;
            this.isShowingDamage = false;

            this.gripDirection = Direction.Down;
            this.movementDirection = 0;

            this.health = DEFAULT_HEALTH;
            this.animationFrameCounter = 0;
            this.hitAnimationCounter = 0;

            this.velocityX = 0.0f;
            this.velocityY = 0.0f;

            this.currentAnimationFrame = animationFrames[0];
        }

        internal override void OnCreated()
        {
            Point tilePosition = TileMap.WorldToTile(this.Position);
            this.movementDirection = Randomness.Random.GetBool() ? (sbyte)1 : (sbyte)-1;

            bool HasSolidTile(Point position)
            {
                return this.TileMap.IsWithinBounds(position) && TileProperty.IsSolidTile(this.TileMap[position]);
            }

            if (HasSolidTile(new(tilePosition.X, tilePosition.Y + 1)))
            {
                this.gripDirection = Direction.Down;
            }
            else if (HasSolidTile(new(tilePosition.X, tilePosition.Y - 1)))
            {
                this.gripDirection = Direction.Up;
            }
            else if (HasSolidTile(new(tilePosition.X + 1, tilePosition.Y)))
            {
                this.gripDirection = Direction.Right;
            }
            else if (HasSolidTile(new(tilePosition.X - 1, tilePosition.Y)))
            {
                this.gripDirection = Direction.Left;
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
            this.isFirstFrame = !this.isFirstFrame;

            int baseFrameIndex = ((int)this.gripDirection - 1) * FRAMES_PER_DIRECTION;
            int frameOffset = this.isShowingDamage ? 2 : this.isFirstFrame ? 0 : 1;

            this.currentAnimationFrame = animationFrames[baseFrameIndex + frameOffset];
        }

        private void UpdateMovementDirection()
        {
            Point tilePosition = TileMap.WorldToTile(new(this.PositionX + (this.Width / 2), this.PositionY + (this.Height / 2)));

            bool HasSolidTile(int dX, int dY)
            {
                Point position = new(tilePosition.X + dX, tilePosition.Y + dY);
                return this.TileMap.IsWithinBounds(position) && TileProperty.IsSolidTile(this.TileMap[position]);
            }

            bool shouldFlipDirection = this.gripDirection switch
            {
                Direction.Up => this.movementDirection == 1
                    ? !HasSolidTile(1, -1) || HasSolidTile(1, 0)
                    : !HasSolidTile(-1, -1) || HasSolidTile(-1, 0),

                Direction.Down => this.movementDirection == 1
                    ? !HasSolidTile(1, 1) || HasSolidTile(1, 0)
                    : !HasSolidTile(-1, 1) || HasSolidTile(-1, 0),

                Direction.Right => this.movementDirection == 1
                    ? !HasSolidTile(1, 1) || HasSolidTile(0, 1)
                    : !HasSolidTile(1, -1) || HasSolidTile(0, -1),

                Direction.Left => this.movementDirection == 1
                    ? !HasSolidTile(-1, 1) || HasSolidTile(0, 1)
                    : !HasSolidTile(-1, -1) || HasSolidTile(0, -1),

                _ => false
            };

            if (shouldFlipDirection)
            {
                this.movementDirection *= -1;
            }
        }

        private void UpdateVelocity()
        {
            switch (this.gripDirection)
            {
                case Direction.Up:
                case Direction.Down:
                    this.velocityX = this.movementDirection * SPEED;
                    this.velocityY = 0.0f;
                    break;

                case Direction.Left:
                case Direction.Right:
                    this.velocityX = 0.0f;
                    this.velocityY = this.movementDirection * SPEED;
                    break;

                case Direction.None:
                default:
                    break;
            }
        }

        internal override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            UpdateDamageAnimation();
            UpdateSpriteAnimation();
            UpdateMovementDirection();
            UpdateVelocity();

            MoveHorizontally(this.velocityX * deltaTime);
            MoveVertically(this.velocityY * deltaTime);
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(AssetDatabase.GetTexture(TextureIndex.Actors), new(this.PositionX, this.PositionY), this.currentAnimationFrame, Color.White, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.0f);
        }
    }
}
