using StarMage.Core.Collections;
using StarMage.Core.Enums;
using StarMage.Core.Interfaces;

using System;

namespace StarMage.Core.Actors
{
    internal sealed class ActorDescriptor<T> : IActorDescriptor where T : Actor
    {
        public ActorIndex Index { get; }

        private readonly Func<T> factory;
        private readonly ObjectPool pool;

        internal ActorDescriptor(ActorIndex index, Func<T> factory)
        {
            this.Index = index;
            this.factory = factory;
            this.pool = new();
        }

        public Actor Dequeue()
        {
            Actor actor = this.pool.TryDequeue(out IPoolableObject value) ? (T)value : this.factory();
            actor.State = ActorState.Pending;
            return actor;
        }

        public void Enqueue(Actor actor)
        {
            actor.State = ActorState.Destroyed;
            this.pool.Enqueue(actor);
        }
    }
}
