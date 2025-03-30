using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlienShooty.Stages
{
    public class Body
    {
        public Vector2 Position;
        public Vector2 Velocity;
        public Vector2 Size;
        public float Rotation;
        public readonly float Mass;
        public readonly float Density;
        public bool Colliding;
        public Rectangle BoundingBox { get; private set; }
        private BodyType _bodyType;          
        public Body(Vector2 position, Vector2 size, float density, float rotation, Body.BodyType bodyType)
        {
            Position = position;
            Size = size;
            Rotation = rotation;
            Density = density;
            Mass = size.X * size.Y * density;
            Colliding = false;
            Velocity = Vector2.Zero;
            _bodyType = bodyType;
            SetBoundingBox();

        }
        private void SetBoundingBox()
        {
            BoundingBox = new Rectangle((Position - Size / 2).ToPoint(), Size.ToPoint());
        }
        public void Update()
        {
            SetBoundingBox();
        }
        public enum BodyType
        {
            Static,
            Dynamic
        }
    }
}
