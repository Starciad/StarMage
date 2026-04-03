using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StarMage.Core.Audio;
using StarMage.Core.Databases;
using StarMage.Core.Enums;
using StarMage.Core.Managers;
using StarMage.Core.Tiles;

namespace StarMage.Core.Actors.Projectiles
{
    internal sealed class BoltSpell : Actor
    {
        internal sbyte HorizontalDirection { get; set; }

        private byte animationFrameCounter;
        private byte animationCurrentFrameIndex;

        private static readonly Rectangle[] animationFrameRectangles =
        [
            #region RIGHT

            new(00, 48, 16, 16),
            new(16, 48, 16, 16),
            new(32, 48, 16, 16),
            new(16, 48, 16, 16),

            #endregion

            #region LEFT
            
            new(00, 64, 16, 16),
            new(16, 64, 16, 16),
            new(32, 64, 16, 16),
            new(16, 64, 16, 16),
            
            #endregion
        ];

        private const byte ANIMATION_MAX_FRAME = 3;
        private const byte ANIMATION_FRAME_DELAY = 5;
        private const byte ANIMATION_LEFT_DIRECTION_OFFSET = 4;

        private const float MOVEMENT_SPEED = 250.0f;

        internal BoltSpell(ActorIndex index, ActorType type, ActorTeam team, ActorManager actorManager, TileMap tileMap) : base(index, type, team, actorManager, tileMap)
        {
            Reset();
        }

        public override void Reset()
        {
            this.HorizontalDirection = 0;
            this.animationFrameCounter = 0;
            this.animationCurrentFrameIndex = 0;
        }

        internal override void OnTileCollision(bool isHorizontalAxis)
        {
            SoundEffectPlayer.Play(SoundEffectIndex.ShotDestroyed);
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
            if (this.ActorManager.TryCreate(ActorIndex.GoodExplosion, out Actor explosionActor))
            {
                explosionActor.Position = this.Position;
            }
        }

        private void UpdateAnimation()
        {
            // Update Frame Counter
            if (++this.animationFrameCounter < ANIMATION_FRAME_DELAY)
            {
                return;
            }

            this.animationFrameCounter = 0;

            // Update Current Frame
            if (++this.animationCurrentFrameIndex > ANIMATION_MAX_FRAME)
            {
                this.animationCurrentFrameIndex = 0;
            }
        }

        internal override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            UpdateAnimation();
            MoveHorizontally(MOVEMENT_SPEED * this.HorizontalDirection * deltaTime);
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(AssetDatabase.GetTexture(TextureIndex.Actors), new(this.PositionX, this.PositionY), animationFrameRectangles[((this.HorizontalDirection > 0 ? (byte)0 : (byte)1) * ANIMATION_LEFT_DIRECTION_OFFSET) + this.animationCurrentFrameIndex], Color.White, 0.0f, new Vector2(1, 2), Vector2.One, SpriteEffects.None, 0.0f);
        }
    }
}
