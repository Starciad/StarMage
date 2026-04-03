using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StarMage.Core.Enums;
using StarMage.Core.Interfaces;
using StarMage.Core.Managers;
using StarMage.Core.Tiles;

using System;

namespace StarMage.Core.Actors
{
    internal abstract class Actor : IPoolableObject
    {
        internal Rectangle Bounds => new(this.PositionX, this.PositionY, this.Width, this.Height);

        internal Point Position
        {
            get => new(this.PositionX, this.PositionY);

            set
            {
                this.PositionX = value.X;
                this.PositionY = value.Y;
            }
        }

        internal Point Size
        {
            get => new(this.Width, this.Height);

            set
            {
                this.Width = value.X;
                this.Height = value.Y;
            }
        }

        internal ActorIndex Index { get; }
        internal ActorType Type { get; }
        internal ActorTeam Team { get; }

        internal ActorState State { get; set; }

        internal bool CanUpdate { get; set; }
        internal bool CanDraw { get; set; }

        internal int PositionX { get; set; }
        internal int PositionY { get; set; }

        internal int Width { get; set; }
        internal int Height { get; set; }

        internal bool HasTileCollision { get; set; }

        protected ActorManager ActorManager => this.actorManager;
        protected TileMap TileMap => this.tileMap;

        private float remainderX;
        private float remainderY;

        private readonly ActorManager actorManager;
        private readonly TileMap tileMap;

        internal Actor(ActorIndex index, ActorType type, ActorTeam team, ActorManager actorManager, TileMap tileMap)
        {
            this.Index = index;
            this.Team = team;
            this.Type = type;
            this.actorManager = actorManager;
            this.tileMap = tileMap;
        }

        internal abstract void Update(GameTime gameTime);
        internal abstract void Draw(SpriteBatch spriteBatch);
        public abstract void Reset();

        internal virtual void OnCreated() { return; }
        internal virtual void OnDestroyed() { return; }

        internal virtual void OnTileCollision(bool isHorizontalAxis) { return; }
        internal virtual void OnActorCollision(Actor otherActor) { return; }
        internal virtual void OnOutsideLevelBounds(Direction direction, int distance) { return; }
        internal virtual void OnOutsideCameraBounds() { return; }

        internal void MoveHorizontally(float amount)
        {
            this.remainderX += amount;
            int motion = (int)MathF.Round(this.remainderX);

            if (motion == 0)
            {
                return;
            }

            this.remainderX -= motion;
            sbyte sign = (sbyte)MathF.Sign(motion);

            Rectangle nextRect = this.Bounds;

            while (motion != 0)
            {
                nextRect.X = this.PositionX + sign;
                nextRect.Y = this.PositionY;

                if (this.HasTileCollision && this.tileMap.IsCollideAt(nextRect, sign, 0))
                {
                    OnTileCollision(true);
                    break;
                }

                this.PositionX += sign;
                motion -= sign;
            }
        }

        internal void MoveVertically(float amount)
        {
            this.remainderY += amount;
            int motion = (int)MathF.Round(this.remainderY);

            if (motion == 0)
            {
                return;
            }

            this.remainderY -= motion;
            sbyte sign = (sbyte)MathF.Sign(motion);

            Rectangle nextRect = this.Bounds;

            while (motion != 0)
            {
                nextRect.X = this.PositionX;
                nextRect.Y = this.PositionY + sign;

                if (this.HasTileCollision && this.tileMap.IsCollideAt(nextRect, 0, sign))
                {
                    OnTileCollision(false);
                    break;
                }

                this.PositionY += sign;
                motion -= sign;
            }
        }
    }
}
