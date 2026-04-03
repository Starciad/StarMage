using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StarMage.Core.Databases;
using StarMage.Core.Enums;
using StarMage.Core.Managers;
using StarMage.Core.Tiles;

namespace StarMage.Core.Actors.Projectiles
{
    internal sealed class EvilStar : Actor
    {
        internal Vector2 Direction { get; set; }

        private byte currentSpriteFrameIndex;
        private byte animationFrameCounter;

        private static readonly Rectangle[] spriteRectangle =
        [
            new(128, 64, 16, 16),
            new(144, 64, 16, 16),
            new(128, 80, 16, 16),
            new(144, 80, 16, 16),
        ];

        private const byte ANIMATION_FRAME_DELAY = 5;
        private const float SPEED = 64.0f;

        internal EvilStar(ActorIndex index, ActorType type, ActorTeam team, ActorManager actorManager, TileMap tileMap) : base(index, type, team, actorManager, tileMap)
        {
            Reset();
        }

        public override void Reset()
        {
            this.Direction = Vector2.Zero;
            this.currentSpriteFrameIndex = 0;
            this.animationFrameCounter = 0;
        }

        internal override void OnTileCollision(bool isHorizontalAxis)
        {
            this.ActorManager.Destroy(this);
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
            if (this.ActorManager.TryCreate(ActorIndex.BadExplosion, out Actor explosionActor))
            {
                explosionActor.Position = this.Position;
            }
        }

        private void UpdateAnimation()
        {
            if (++this.animationFrameCounter >= ANIMATION_FRAME_DELAY)
            {
                this.animationFrameCounter = 0;
                this.currentSpriteFrameIndex = (byte)((this.currentSpriteFrameIndex + 1) % spriteRectangle.Length);
            }
        }

        internal override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            UpdateAnimation();

            MoveHorizontally(this.Direction.X * SPEED * deltaTime);
            MoveVertically(this.Direction.Y * SPEED * deltaTime);
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(AssetDatabase.GetTexture(TextureIndex.Actors), new Vector2(this.PositionX, this.PositionY), spriteRectangle[this.currentSpriteFrameIndex], Color.White, 0f, new(4), Vector2.One, SpriteEffects.None, 0f);
        }
    }
}
