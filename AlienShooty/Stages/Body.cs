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
        public Vector2 HalfSize;
        public Vector2 Direction;
        public float Rotation;
        public readonly float Mass;
        public readonly float Density;
        public bool Colliding;
        public bool IsBullet;
        public Func<Body, bool> OnTileCollision;
        public Func<Body, bool> OnEntityCollision;
        public Rectangle BoundingBox { get; private set; }
        private BodyType _bodyType;          
        public Body(Vector2 position, Vector2 size, float density, float rotation, Body.BodyType bodyType, bool isBullet = false)
        {
            Position = position;
            Size = size;
            HalfSize = size / 2;
            Rotation = rotation;
            Density = density;
            Mass = size.X * size.Y * density;
            Colliding = false;
            Velocity = Vector2.Zero;
            _bodyType = bodyType;
            SetBoundingBox();
            IsBullet = isBullet;
        }
        private void SetBoundingBox()
        {
            BoundingBox = new Rectangle((Position - HalfSize).ToPoint(), Size.ToPoint());
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
