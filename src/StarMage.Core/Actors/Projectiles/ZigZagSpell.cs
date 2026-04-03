using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StarMage.Core.Databases;
using StarMage.Core.Enums;
using StarMage.Core.Managers;
using StarMage.Core.Tiles;

namespace StarMage.Core.Actors.Projectiles
{
    internal sealed class ZigZagSpell : Actor
    {
        internal Vector2 Direction { get => this.direction; set => this.direction = Vector2.Normalize(value); }

        private Vector2 direction;
        private Point previousPosition;

        private byte durability;

        private const byte DEFAULT_DURABILITY = 8;
        private const float DEFAULT_SPEED = 250.0f;

        internal ZigZagSpell(ActorIndex index, ActorType type, ActorTeam team, ActorManager actorManager, TileMap tileMap) : base(index, type, team, actorManager, tileMap)
        {
            Reset();
        }

        public override void Reset()
        {
            this.direction = Vector2.Zero;
            this.previousPosition = Point.Zero;
            this.durability = DEFAULT_DURABILITY;
        }

        internal override void OnTileCollision(bool isHorizontalAxis)
        {
            this.durability--;

            if (this.durability == 0)
            {
                this.ActorManager.Destroy(this);
                return;
            }

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
            this.ActorManager.Destroy(this);
        }

        internal override void OnOutsideCameraBounds()
        {
            this.ActorManager.Destroy(this);
        }

        internal override void OnDestroyed()
        {
            if (this.ActorManager.TryCreate(ActorIndex.GoodExplosion, out Actor explosionActor))
            {
                explosionActor.Position = this.Position;
            }
        }

        internal override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            this.previousPosition = this.Position;

            MoveHorizontally(this.Direction.X * DEFAULT_SPEED * deltaTime);
            MoveVertically(this.Direction.Y * DEFAULT_SPEED * deltaTime);

            if (this.previousPosition == this.Position)
            {
                this.ActorManager.Destroy(this);
            }
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(AssetDatabase.GetTexture(TextureIndex.Actors), new(this.PositionX, this.PositionY), new(144, 48, 16, 16), Color.White, 0.0f, new(3, 3), Vector2.One, SpriteEffects.None, 0.0f);
        }
    }
}
