using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StarMage.Core.Audio;
using StarMage.Core.Databases;
using StarMage.Core.Enums;
using StarMage.Core.Tiles;

namespace StarMage.Core.Actors.Collectibles
{
    internal sealed class TreasureChest : Actor
    {
        private byte health;
        private byte hitAnimationCounter;
        private bool isShowingDamage;

        private static readonly Rectangle defaultSpriteRectangle = new(16, 128, 16, 16);
        private static readonly Rectangle damagedSpriteRectangle = new(32, 128, 16, 16);

        private const byte DEFAULT_HEALTH = 8;
        private const byte HIT_ANIMATION_DURATION = 8;

        internal TreasureChest(ActorIndex index, ActorType type, ActorTeam team, ActorManager actorManager, TileMap tileMap) : base(index, type, team, actorManager, tileMap)
        {
            Reset();
        }

        public override void Reset()
        {
            this.health = DEFAULT_HEALTH;
            this.hitAnimationCounter = 0;
            this.isShowingDamage = false;
        }

        private void PerformDeath()
        {
            SoundEffectPlayer.Play(SoundEffectIndex.EnemyExploded);

            if (this.ActorManager.TryCreate(ActorIndex.MagicFrame, out Actor actor))
            {
                actor.Position = this.Position;
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
                    PerformDeath();
                    this.ActorManager.Destroy(this);
                    return;
                }

                SoundEffectPlayer.Play(SoundEffectIndex.EnemyHit);
                this.isShowingDamage = true;
            }
        }

        internal override void Update(GameTime gameTime)
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

        internal override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(AssetDatabase.GetTexture(TextureIndex.Actors), new Vector2(this.PositionX, this.PositionY), this.isShowingDamage ? damagedSpriteRectangle : defaultSpriteRectangle, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
        }
    }
}
