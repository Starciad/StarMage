using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StarMage.Core.Actors;
using StarMage.Core.Databases;
using StarMage.Core.Enums;
using StarMage.Core.Handlers;
using StarMage.Core.Tiles;

using System;
using System.Collections.Generic;

namespace StarMage.Core.Managers
{
    internal sealed class ActorManager
    {
        private const int PROCESSING_TOLERANCE = 256;

        internal int TotalActorCount { get; private set; }

        internal bool CanUpdate { get; set; } = true;
        internal bool CanDraw { get; set; } = true;

        private readonly List<Actor> instantiatedActors = [];
        private readonly Queue<Actor> actorsToAdd = [];
        private readonly Queue<Actor> actorsToRemove = [];

        private readonly TileMap tileMap;

        internal ActorManager(TileMap tileMap)
        {
            this.tileMap = tileMap;
        }

        internal IEnumerable<Actor> GetActors()
        {
            foreach (Actor actor in this.actorsToAdd)
            {
                yield return actor;
            }

            foreach (Actor actor in this.instantiatedActors)
            {
                yield return actor;
            }

            foreach (Actor actor in this.actorsToRemove)
            {
                yield return actor;
            }
        }

        internal IEnumerable<Actor> GetActors(ActorIndex index)
        {
            foreach (Actor actor in GetActors())
            {
                if (actor.Index == index)
                {
                    yield return actor;
                }
            }
        }

        internal Actor GetActor(ActorIndex index)
        {
            foreach (Actor actor in GetActors())
            {
                if (actor.Index == index)
                {
                    return actor;
                }
            }

            return null;
        }

        internal bool TryCreate(ActorIndex index, out Actor actor)
        {
            actor = ActorDatabase.GetDescriptor(index).Dequeue();

            this.actorsToAdd.Enqueue(actor);
            this.TotalActorCount++;

            return true;
        }

        internal void Destroy(Actor actor)
        {
            if (actor.State is ActorState.Destroyed)
            {
                return;
            }

            ActorDatabase.GetDescriptor(actor.Index).Enqueue(actor);
            this.actorsToRemove.Enqueue(actor);
            this.TotalActorCount--;
        }

        private void FlushPendingChanges()
        {
            FlushAdditions();
            FlushRemovals();
        }

        private void FlushAdditions()
        {
            while (this.actorsToAdd.TryDequeue(out Actor actor))
            {
                this.instantiatedActors.Add(actor);
                actor.State = ActorState.Active;
                actor.OnCreated();
            }
        }

        private void FlushRemovals()
        {
            while (this.actorsToRemove.TryDequeue(out Actor actor))
            {
                _ = this.instantiatedActors.Remove(actor);
                actor.OnDestroyed();
            }
        }

        internal void Clear()
        {
            Clear(_ => false);
        }

        internal void Clear(Predicate<Actor> shouldRemain)
        {
            HashSet<Actor> visited = [];
            List<Actor> toRemove = [];

            foreach (Actor actor in GetActors())
            {
                if (!visited.Add(actor))
                {
                    continue;
                }

                if (!shouldRemain(actor))
                {
                    toRemove.Add(actor);
                }
            }

            if (toRemove.Count == visited.Count)
            {
                foreach (Actor actor in toRemove)
                {
                    if (actor.State is not ActorState.Destroyed)
                    {
                        ActorDatabase.GetDescriptor(actor.Index).Enqueue(actor);
                        actor.OnDestroyed();
                    }
                }

                this.actorsToAdd.Clear();
                this.actorsToRemove.Clear();
                this.instantiatedActors.Clear();

                this.TotalActorCount = 0;
            }
            else
            {
                foreach (Actor actor in toRemove)
                {
                    Destroy(actor);
                }
            }
        }

        private void UpdateActor(GameTime gameTime, Actor actor)
        {
            // Update the actor
            actor.Update(gameTime);

            // Check if the actor is outside the map's boundaries
            Rectangle tileMapBounds = this.tileMap.Bounds;
            Rectangle actorBounds = actor.Bounds;

            if (!actorBounds.Intersects(tileMapBounds))
            {
                // Determine the direction and distance the actor is outside the bounds
                Direction direction = Direction.None;
                int distance = 0;

                if (actorBounds.Right < tileMapBounds.Left)
                {
                    direction = Direction.Left;
                    distance = tileMapBounds.Left - actorBounds.Right;
                }
                else if (actorBounds.Left > tileMapBounds.Right)
                {
                    direction = Direction.Right;
                    distance = actorBounds.Left - tileMapBounds.Right;
                }
                else if (actorBounds.Bottom < tileMapBounds.Top)
                {
                    direction = Direction.Up;
                    distance = tileMapBounds.Top - actorBounds.Bottom;
                }
                else if (actorBounds.Top > tileMapBounds.Bottom)
                {
                    direction = Direction.Down;
                    distance = actorBounds.Top - tileMapBounds.Bottom;
                }

                actor.OnOutsideLevelBounds(direction, distance);
            }

            // Check if the actor is within the camera's frame
            if (!actor.Bounds.Intersects(CameraHandler.GetViewBounds()))
            {
                actor.OnOutsideCameraBounds();
            }

            // Check Collision with other actors
            foreach (Actor otherActor in GetActors())
            {
                if (otherActor == actor || otherActor.State is not ActorState.Active)
                {
                    continue;
                }

                // Check for collision
                if (actorBounds.Intersects(otherActor.Bounds))
                {
                    actor.OnActorCollision(otherActor);
                }
            }
        }

        internal void Update(GameTime gameTime)
        {
            FlushPendingChanges();

            if (!this.CanUpdate)
            {
                return;
            }

            Rectangle processingBounds = CameraHandler.GetViewBounds();
            processingBounds.Inflate(PROCESSING_TOLERANCE, PROCESSING_TOLERANCE);

            // Update each instantiated actor
            foreach (Actor actor in GetActors())
            {
                // Skip non-updatable actors, non-active actors, and actors outside the processing rectangle
                if (!actor.CanUpdate || actor.State is not ActorState.Active || !actor.Bounds.Intersects(processingBounds))
                {
                    continue;
                }

                UpdateActor(gameTime, actor);
            }
        }

        internal void Draw(SpriteBatch spriteBatch, ActorTeam team)
        {
            if (!this.CanDraw)
            {
                return;
            }

            foreach (Actor actor in GetActors())
            {
                // Skip non-drawable actors, destroyed actors, and actors outside the view rectangle
                if (!actor.CanDraw ||
                    actor.State is ActorState.Destroyed ||
                    actor.Team != team ||
                    !actor.Bounds.Intersects(CameraHandler.GetViewBounds()))
                {
                    continue;
                }

                actor.Draw(spriteBatch);
            }
        }
    }
}
