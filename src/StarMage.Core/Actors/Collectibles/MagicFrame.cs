using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StarMage.Core.Actors.Players;
using StarMage.Core.Audio;
using StarMage.Core.Databases;
using StarMage.Core.Enums;
using StarMage.Core.Extensions;
using StarMage.Core.Tiles;

using System.Collections.Generic;

namespace StarMage.Core.Actors.Collectibles
{
    internal sealed class MagicFrame : Actor
    {
        private bool isCollected;
        private bool isFirstFrame;

        private byte animationFrameCounter;

        private SpellType targetSpellType;

        private Rectangle currentAnimationFrame;

        private static readonly HashSet<SpellType> recentlyDistributedSpells = [];

        private static readonly Rectangle[] animationFrames =
        [
            // Spell Type 00 (Bolt)
            new(96, 128, 16, 16),
            new(128, 128, 16, 16),

            // Spell Type 01 (Bounce)
            new(96, 144, 16, 16),
            new(128, 144, 16, 16),

            // Spell Type 02 (ZigZag)
            new(112, 128, 16, 16),
            new(144, 128, 16, 16),

            // Spell Type 03 (Wave)
            new(112, 144, 16, 16),
            new(144, 144, 16, 16),
        ];

        private const byte ANIMATION_FRAME_DELAY = 12;

        internal MagicFrame(ActorIndex index, ActorType type, ActorTeam team, ActorManager actorManager, TileMap tileMap) : base(index, type, team, actorManager, tileMap)
        {
            Reset();
        }

        public override void Reset()
        {
            this.isCollected = false;
            this.isFirstFrame = true;
            this.animationFrameCounter = 0;
            this.currentAnimationFrame = animationFrames[0];
        }

        internal override void OnInstantiated()
        {
            MagePlayer magePlayer = (MagePlayer)this.ActorManager.GetActor(ActorIndex.MagePlayer);

            List<SpellType> availableSpells =
            [
                SpellType.Bolt,
                SpellType.Bounce,
                SpellType.ZigZag,
                SpellType.Wave
            ];

            _ = availableSpells.Remove(magePlayer.SpellType);

            foreach (SpellType spell in recentlyDistributedSpells)
            {
                _ = availableSpells.Remove(spell);
            }

            if (availableSpells.Count == 0)
            {
                recentlyDistributedSpells.Clear();
                
                availableSpells =
                [
                    SpellType.Bolt,
                    SpellType.Bounce,
                    SpellType.ZigZag,
                    SpellType.Wave
                ];

                _ = availableSpells.Remove(magePlayer.SpellType);
            }

            this.targetSpellType = availableSpells.GetRandomItem();
            _ = recentlyDistributedSpells.Add(this.targetSpellType);

            this.currentAnimationFrame = animationFrames[(int)this.targetSpellType * 2];
        }

        internal override void OnActorCollision(Actor otherActor)
        {
            if (!this.isCollected && otherActor.Type is ActorType.Player)
            {
                SoundEffectPlayer.Play(SoundEffectIndex.ItemCollected);
                this.isCollected = true;
                ((MagePlayer)otherActor).SpellType = this.targetSpellType;
                this.ActorManager.Destroy(this);
            }
        }

        internal override void Update(GameTime gameTime)
        {
            if (++this.animationFrameCounter > ANIMATION_FRAME_DELAY)
            {
                this.animationFrameCounter = 0;
                this.isFirstFrame = !this.isFirstFrame;

                int spellTypeOffset = (int)this.targetSpellType * 2;
                int frameOffset = this.isFirstFrame ? 0 : 1;

                this.currentAnimationFrame = animationFrames[spellTypeOffset + frameOffset];
            }
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(AssetDatabase.GetTexture(TextureIndex.Actors), new(this.PositionX, this.PositionY), this.currentAnimationFrame, Color.White, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.0f);
        }
    }
}
