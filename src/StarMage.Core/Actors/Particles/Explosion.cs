using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StarMage.Core.Databases;
using StarMage.Core.Enums;
using StarMage.Core.Tiles;

namespace StarMage.Core.Actors.Particles
{
    internal sealed class Explosion : Actor
    {
        private byte animationFrameCounter;
        private byte animationCurrentFrameIndex;

        private static readonly Rectangle[] animationFrameRectangles =
        [
            new(00, 144, 16, 16),
            new(16, 144, 16, 16),
            new(32, 144, 16, 16),
            new(48, 144, 16, 16),
            new(64, 144, 16, 16),
        ];

        private const byte EXPLOSION_ANIMATION_FRAME_DELAY = 5;

        internal Explosion(ActorIndex index, ActorType type, ActorTeam team, ActorManager actorManager, TileMap tileMap) : base(index, type, team, actorManager, tileMap)
        {
            Reset();
        }

        public override void Reset()
        {
            this.animationFrameCounter = 0;
            this.animationCurrentFrameIndex = 0;
        }

        internal override void OnOutsideCameraBounds()
        {
            this.ActorManager.Destroy(this);
        }

        internal override void Update(GameTime gameTime)
        {
            // Update Frame Counter
            if (++this.animationFrameCounter < EXPLOSION_ANIMATION_FRAME_DELAY)
            {
                return;
            }

            this.animationFrameCounter = 0;

            // Update Current Frame
            if (++this.animationCurrentFrameIndex >= animationFrameRectangles.Length)
            {
                this.ActorManager.Destroy(this);
                return;
            }
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(AssetDatabase.GetTexture(TextureIndex.Actors), new(this.PositionX, this.PositionY), animationFrameRectangles[this.animationCurrentFrameIndex], Color.White, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.0f);
        }
    }
}
