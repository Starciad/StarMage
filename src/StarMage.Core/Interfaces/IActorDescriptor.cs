using StarMage.Core.Actors;
using StarMage.Core.Enums;

namespace StarMage.Core.Interfaces
{
    internal interface IActorDescriptor
    {
        ActorIndex Index { get; }

        Actor Dequeue();
        void Enqueue(Actor actor);
    }
}
