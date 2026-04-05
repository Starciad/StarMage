using StarMage.Core.Actors;
using StarMage.Core.Actors.Bosses;
using StarMage.Core.Actors.Collectibles;
using StarMage.Core.Actors.Enemies;
using StarMage.Core.Actors.Particles;
using StarMage.Core.Actors.Players;
using StarMage.Core.Actors.Projectiles;
using StarMage.Core.Enums;
using StarMage.Core.Interfaces;
using StarMage.Core.Tiles;

namespace StarMage.Core.Databases
{
    internal static class ActorDatabase
    {
        private static IActorDescriptor[] descriptors;

        internal static void LoadContent(ActorManager actorManager, TileMap tileMap)
        {
            descriptors = [

                #region BOSSES

                new ActorDescriptor<GreatDarkWizard>(ActorIndex.GreatDarkWizard, () => new(ActorIndex.GreatDarkWizard, ActorType.Boss, ActorTeam.Bad, actorManager, tileMap)
                {
                    CanDraw = true,
                    CanUpdate = true,
                    Width = 28,
                    Height = 28,
                }),

                #endregion

                #region COLLECTIBLES

                new ActorDescriptor<BigCrystal>(ActorIndex.BigCrystal, () => new(ActorIndex.BigCrystal, ActorType.Collectable, ActorTeam.Good, actorManager, tileMap)
                {
                    CanDraw = true,
                    CanUpdate = true,
                    Width = 16,
                    Height = 16,
                }),

                new ActorDescriptor<FinishingStar>(ActorIndex.FinishingStar, () => new(ActorIndex.FinishingStar, ActorType.Collectable, ActorTeam.Good, actorManager, tileMap)
                {
                    CanDraw = true,
                    CanUpdate = true,
                    Width = 16,
                    Height = 16,
                }),

                new ActorDescriptor<MagicFrame>(ActorIndex.MagicFrame, () => new(ActorIndex.MagicFrame, ActorType.Collectable, ActorTeam.Good, actorManager, tileMap)
                {
                    CanDraw = true,
                    CanUpdate = true,
                    Width = 16,
                    Height = 16,
                }),

                new ActorDescriptor<MagicHeart>(ActorIndex.MagicHeart, () => new(ActorIndex.MagicHeart, ActorType.Collectable, ActorTeam.Good, actorManager, tileMap)
                {
                    CanDraw = true,
                    CanUpdate = true,
                    Width = 16,
                    Height = 16,
                }),

                new ActorDescriptor<StartingStar>(ActorIndex.StartingStar, () => new(ActorIndex.StartingStar, ActorType.Collectable, ActorTeam.Good, actorManager, tileMap)
                {
                    CanDraw = true,
                    CanUpdate = true,
                    HasTileCollision = true,
                    Width = 16,
                    Height = 16,
                }),

                new ActorDescriptor<TreasureChest>(ActorIndex.TreasureChest, () => new(ActorIndex.TreasureChest, ActorType.Collectable, ActorTeam.Good, actorManager, tileMap)
                {
                    CanDraw = true,
                    CanUpdate = true,
                    Width = 16,
                    Height = 16,
                }),

                #endregion

                #region ENEMIES

                new ActorDescriptor<Batter>(ActorIndex.Batter, () => new(ActorIndex.Batter, ActorType.Enemy, ActorTeam.Bad, actorManager, tileMap)
                {
                    CanDraw = true,
                    CanUpdate = true,
                    HasTileCollision = true,
                    Width = 16,
                    Height = 16,
                }),

                new ActorDescriptor<FloatingBall>(ActorIndex.FloatingBall, () => new(ActorIndex.FloatingBall, ActorType.Enemy, ActorTeam.Bad, actorManager, tileMap)
                {
                    CanDraw = true,
                    CanUpdate = true,
                    HasTileCollision = true,
                    Width = 16,
                    Height = 16,
                }),

                new ActorDescriptor<HappyMask>(ActorIndex.HappyMask, () => new(ActorIndex.HappyMask, ActorType.Enemy, ActorTeam.Bad, actorManager, tileMap)
                {
                    CanDraw = true,
                    CanUpdate = true,
                    HasTileCollision = true,
                    Width = 16,
                    Height = 16,
                }),

                new ActorDescriptor<Slime>(ActorIndex.Slime, () => new(ActorIndex.Slime, ActorType.Enemy, ActorTeam.Bad, actorManager, tileMap)
                {
                    CanDraw = true,
                    CanUpdate = true,
                    HasTileCollision = true,
                    Width = 16,
                    Height = 16,
                }),

                new ActorDescriptor<Spino>(ActorIndex.Spino, () => new(ActorIndex.Spino, ActorType.Enemy, ActorTeam.Bad, actorManager, tileMap)
                {
                    CanDraw = true,
                    CanUpdate = true,
                    Width = 16,
                    Height = 16,
                }),

                new ActorDescriptor<Spitter>(ActorIndex.Spitter, () => new(ActorIndex.Spitter, ActorType.Enemy, ActorTeam.Bad, actorManager, tileMap)
                {
                    CanDraw = true,
                    CanUpdate = true,
                    Width = 16,
                    Height = 16,
                }),

                #endregion

                #region PARTICLES

                new ActorDescriptor<Explosion>(ActorIndex.BadExplosion, () => new(ActorIndex.BadExplosion, ActorType.Particle, ActorTeam.Bad, actorManager, tileMap)
                {
                    CanDraw = true,
                    CanUpdate = true,
                    Width = 16,
                    Height = 16,
                }),

                new ActorDescriptor<DeadEnemyResidue>(ActorIndex.DeadEnemyResidue, () => new(ActorIndex.DeadEnemyResidue, ActorType.Particle, ActorTeam.Bad, actorManager, tileMap)
                {
                    CanDraw = true,
                    CanUpdate = true,
                    Width = 8,
                    Height = 8,
                }),

                new ActorDescriptor<Explosion>(ActorIndex.GoodExplosion, () => new(ActorIndex.GoodExplosion, ActorType.Particle, ActorTeam.Good, actorManager, tileMap)
                {
                    CanDraw = true,
                    CanUpdate = true,
                    Width = 16,
                    Height = 16,
                }),

                new ActorDescriptor<PlayerResidue>(ActorIndex.PlayerResidue, () => new(ActorIndex.PlayerResidue, ActorType.Particle, ActorTeam.Good, actorManager, tileMap)
                {
                    CanDraw = true,
                    CanUpdate = true,
                    Width = 8,
                    Height = 8,
                }),

                #endregion

                #region PLAYERS

                new ActorDescriptor<MagePlayer>(ActorIndex.MagePlayer, () => new(ActorIndex.MagePlayer, ActorType.Player, ActorTeam.Good, actorManager, tileMap)
                {
                    CanDraw = true,
                    CanUpdate = true,
                    HasTileCollision = true,
                    Width = 8,
                    Height = 16,
                }),

                #endregion

                #region PROJECTILES

                new ActorDescriptor<BoltSpell>(ActorIndex.BoltSpell, () => new(ActorIndex.BoltSpell, ActorType.Projectile, ActorTeam.Good, actorManager, tileMap)
                {
                    CanDraw = true,
                    CanUpdate = true,
                    HasTileCollision = true,
                    Width = 15,
                    Height = 12,
                }),

                new ActorDescriptor<BounceSpell>(ActorIndex.BounceSpell, () => new(ActorIndex.BounceSpell, ActorType.Projectile, ActorTeam.Good, actorManager, tileMap)
                {
                    CanDraw = true,
                    CanUpdate = true,
                    HasTileCollision = true,
                    Width = 12,
                    Height = 12,
                }),

                new ActorDescriptor<EvilStar>(ActorIndex.EvilStar, () => new(ActorIndex.EvilStar, ActorType.Projectile, ActorTeam.Bad, actorManager, tileMap)
                {
                    CanDraw = true,
                    CanUpdate = true,
                    HasTileCollision = true,
                    Width = 12,
                    Height = 12,
                }),

                new ActorDescriptor<SpinyBall>(ActorIndex.SpinyBall, () => new(ActorIndex.SpinyBall, ActorType.Projectile, ActorTeam.Bad, actorManager, tileMap)
                {
                    CanDraw = true,
                    CanUpdate = true,
                    HasTileCollision = true,
                    Width = 16,
                    Height = 16,
                }),

                new ActorDescriptor<WaveSpell>(ActorIndex.WaveSpell, () => new(ActorIndex.WaveSpell, ActorType.Projectile, ActorTeam.Good, actorManager, tileMap)
                {
                    CanDraw = true,
                    CanUpdate = true,
                    HasTileCollision = true,
                    Width = 12,
                    Height = 12,
                }),

                new ActorDescriptor<ZigZagSpell>(ActorIndex.ZigZagSpell, () => new(ActorIndex.ZigZagSpell, ActorType.Projectile, ActorTeam.Good, actorManager, tileMap)
                {
                    CanDraw = true,
                    CanUpdate = true,
                    HasTileCollision = true,
                    Width = 12,
                    Height = 12,
                }),

                #endregion

            ];
        }

        internal static IActorDescriptor GetDescriptor(ActorIndex index)
        {
            return descriptors[(byte)index];
        }
    }
}
