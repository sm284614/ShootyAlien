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
            //pew pew
            //Shoot = _input.KeyPressStarted(Keys.Space);
            ShootUp = _input.KeyDown(Keys.Up);
            ShootDown = _input.KeyDown(Keys.Down);
            ShootLeft = _input.KeyDown(Keys.Left);
            ShootRight = _input.KeyDown(Keys.Right);
            //get direction (sort of, just store the mouse position to use later, else we need the camera here to get world position from mouse relative to camera)
            Direction = _input.MousePosition;
        }
    }
}
