using Microsoft.Xna.Framework;
using AlienShooty.Entities;

namespace AlienShooty.Stages
{
    public class EntitySpawnData
    {
        public readonly EntityTemplate Template;
        public readonly Vector2 Position;
        public readonly Vector2 Velocity;
        public readonly float Rotation;
        public readonly Stage.EntityType EntityType;
        public EntitySpawnData(EntityTemplate template, Vector2 position, Vector2 velocity, float rotation, Stage.EntityType entityType)
        {
            Template = template;
            Position = position;
            Velocity = velocity;
            Rotation = rotation;
            EntityType = entityType;
        }
    }
}
