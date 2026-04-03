using Microsoft.Xna.Framework;

using StarMage.Core.Actors.Particles;
using StarMage.Core.Enums;
using StarMage.Core.Managers;

using System;

namespace StarMage.Core.Actors
{
    internal static class ActorHelpers
    {
        private const byte RESIDUE_COUNT = 12;

        internal static void CreateEnemyDeathResidues(ActorManager actorManager, Point position, Point size)
        {
            Point center = new(position.X + (size.X / 2), position.Y + (size.Y / 2));

            for (int i = 0; i < RESIDUE_COUNT; i++)
            {
                if (actorManager.TryCreate(ActorIndex.DeadEnemyResidue, out Actor deadEnemyResidueActor))
                {
                    deadEnemyResidueActor.Position = center;

                    float angle = i * (MathF.PI / (RESIDUE_COUNT / 2.0f));
                    ((DeadEnemyResidue)deadEnemyResidueActor).Direction = new(MathF.Cos(angle), MathF.Sin(angle));
                }
            }
        }
    }
}
