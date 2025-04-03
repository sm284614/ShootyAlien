using Microsoft.Xna.Framework;
using AlienShooty.Stages;

namespace AlienShooty.Entities
{
    public class PhysicsData
    {
        public readonly Body Body;
        private float _speed;
        public PhysicsData(Body body, float speed)
        {
            Body = body;
            _speed = speed;
        }
        public void Update(GameTime gameTime, InputController inputController)
        {
            Body.Direction = inputController.Direction - Body.Position;
            if (inputController.MoveUp)
            {
                Body.Velocity.Y = -_speed;
            }
            else if (inputController.MoveDown)
            {
                Body.Velocity.Y = _speed;
            }
            else
            {
                Body.Velocity.Y = 0;
            }
            if (inputController.MoveLeft)
            {
                Body.Velocity.X = -_speed;
            }
            else if (inputController.MoveRight)
            {
                Body.Velocity.X = _speed;
            }
            else
            {
                Body.Velocity.X = 0;
            }
        }
    }
}
