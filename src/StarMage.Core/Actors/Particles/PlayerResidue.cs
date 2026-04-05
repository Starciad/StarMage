using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StarMage.Core.Databases;
using StarMage.Core.Enums;
using StarMage.Core.Tiles;

namespace StarMage.Core.Actors.Particles
{
    internal sealed class PlayerResidue : Actor
    {
        internal Vector2 Direction { get; set; }

        private const float SPEED = 180.0f;

        internal PlayerResidue(ActorIndex index, ActorType type, ActorTeam team, ActorManager actorManager, TileMap tileMap) : base(index, type, team, actorManager, tileMap)
        {
            Reset();
        }

        public override void Reset()
        {
            this.Direction = Vector2.Zero;
        }

        internal override void OnOutsideCameraBounds()
        {
            this.ActorManager.Destroy(this);
        }

        internal override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            MoveHorizontally(this.Direction.X * SPEED * deltaTime);
            MoveVertically(this.Direction.Y * SPEED * deltaTime);
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(AssetDatabase.GetTexture(TextureIndex.Actors), new(this.PositionX, this.PositionY), new(8, 136, 8, 8), Color.White, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.0f);
        }
    }
}
