using AlienShooty.Stages;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlienShooty.Entities
{
    public class InputControllerPlayer : InputController
    {
        private InputManager _input;
        public InputControllerPlayer(InputManager input)
        {
            _input = input;
        }
        public override void Update(GameTime gameTime)
        {
            //move
            MoveUp = _input.KeyDown(Keys.W);
            MoveRight = _input.KeyDown(Keys.D);
            MoveDown = _input.KeyDown(Keys.S);
            MoveLeft = _input.KeyDown(Keys.A);
            Running = _input.KeyDown(Keys.LeftShift);
            //shoot
            Shoot = _input.KeyPressStarted(Keys.Space);
            ShootUp = _input.KeyPressStarted(Keys.Up);
            ShootDown = _input.KeyPressStarted(Keys.Down);
            ShootLeft = _input.KeyPressStarted(Keys.Left);
            ShootRight = _input.KeyPressStarted(Keys.Right);
            //get direction
            Direction = _input.MousePosition;
        }
    }
}
