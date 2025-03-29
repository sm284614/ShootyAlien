using Microsoft.Xna.Framework;
using AlienShooty.Stage;

namespace AlienShooty.Entities
{
    public class PhysicsData
    {
        public readonly Body Body;
        private Rectangle _boundingBox;
        private float _mass;
        private Vector2 _position;
        private Vector2 _velocity;
        private float _speed;
        public PhysicsData(Body body, float speed)
        {
            Body = body;
            _velocity = Vector2.Zero;
            _speed = speed;
        }
        public void Update(GameTime gameTime, InputController inputController)
        {
            if (inputController.MoveUp)
            {
                _velocity.Y = -_speed;
            }
            else if (inputController.MoveDown)
            {
                _velocity.Y = _speed;
            }
            else
            {
                _velocity.Y = 0;
            }
            if (inputController.MoveLeft)
            {
                _velocity.X = -_speed;
            }
            else if (inputController.MoveRight)
            {
                _velocity.X = _speed;
            }
            else
            {
                _velocity.X = 0;
            }
            Body.Velocity = _velocity;
        }
    }
}
