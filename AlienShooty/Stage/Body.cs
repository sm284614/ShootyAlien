using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlienShooty.Stage
{
    public class Body
    {
        public Vector2 Position;
        public Vector2 Velocity;
        public float Rotation;
        public readonly float Mass;
        public readonly float Density;
        public bool Colliding;
        public Rectangle BoundingBox { get; private set; }
        private BodyType _bodyType;          
        public Body(Vector2 position, Vector2 size, float density, float rotation, Body.BodyType bodyType)
        {
            Position = position;
            Rotation = rotation;
            Density = density;
            Mass = size.X * size.Y * density;
            Colliding = false;
            BoundingBox = new Rectangle(position.ToPoint(), size.ToPoint());
            _bodyType = bodyType;

        }
        public void Update()
        {
            BoundingBox = new Rectangle(Position.ToPoint(), BoundingBox.Size);
        }
        public enum BodyType
        {
            Static,
            Dynamic
        }
    }
}
