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
            //shoot
            Shoot = _input.KeyPressStarted(Keys.Space);
            //get direction
            Direction = _input.MousePosition;
        }
    }
}
