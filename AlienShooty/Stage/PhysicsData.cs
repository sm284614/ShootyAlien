using Microsoft.Xna.Framework;

namespace AlienShooty.Stage
{
    public class PhysicsData
    {
        public Vector2 Position;
        public Vector2 Velocity;
        public Rectangle BoundingBox;
        public float Rotation;
        public PhysicsData()
        {
            Position = Vector2.Zero;
            Velocity = Vector2.Zero;
            BoundingBox = new Rectangle(0, 0, 0, 0);
            Rotation = 0;
        }
        public void Update(GameTime gameTime)
        {
            Position += Velocity;
            BoundingBox = new Rectangle((int)Position.X, (int)Position.Y, BoundingBox.Width, BoundingBox.Height);
        }
    }
}
