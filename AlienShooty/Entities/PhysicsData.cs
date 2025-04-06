using Microsoft.Xna.Framework;
using AlienShooty.Stages;

namespace AlienShooty.Entities
{
    public class PhysicsData
    {
        public readonly Body Body;
        private float _walkingSpeed;
        private float _runningSpeed;
        public PhysicsData(Body body, float speed)
        {
            Body = body;
            _walkingSpeed = speed;
            _runningSpeed = speed * 2;
        }
        public void Update(GameTime gameTime, InputController inputController)
        {
            Body.Direction = inputController.Direction - Body.Position;
            float currentSpeed = inputController.Running ? _runningSpeed : _walkingSpeed;
            if (inputController.MoveUp)
            {
                Body.Velocity.Y = -currentSpeed;
            }
            else if (inputController.MoveDown)
            {
                Body.Velocity.Y = currentSpeed;
            }
            else
            {
                Body.Velocity.Y = 0;
            }
            if (inputController.MoveLeft)
            {
                Body.Velocity.X = -currentSpeed;
            }
            else if (inputController.MoveRight)
            {
                Body.Velocity.X = currentSpeed;
            }
            else
            {
                Body.Velocity.X = 0;
            }
        }
    }
}
