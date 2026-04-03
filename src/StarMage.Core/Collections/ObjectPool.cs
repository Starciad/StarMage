using StarMage.Core.Interfaces;

using System.Collections.Generic;

namespace StarMage.Core.Collections
{
    internal sealed class ObjectPool
    {
        internal int Count => this.pool.Count;

        private readonly Queue<IPoolableObject> pool = [];

        internal IPoolableObject Dequeue()
        {
            _ = TryDequeue(out IPoolableObject value);
            return value;
        }

        internal bool TryDequeue(out IPoolableObject value)
        {
            value = null;

            if (this.pool.TryDequeue(out IPoolableObject result))
            {
                result.Reset();
                value = result;

                return true;
            }

            return false;
        }

        internal void Enqueue(IPoolableObject value)
        {
            this.pool.Enqueue(value);
        }
    }
}