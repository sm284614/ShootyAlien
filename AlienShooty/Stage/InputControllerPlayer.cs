using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlienShooty.Stage
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
            MoveUp = _input.KeyDown(Keys.W);
            MoveRight = _input.KeyDown(Keys.D);
            MoveDown = _input.KeyDown(Keys.S);
            MoveLeft = _input.KeyDown(Keys.A);
            Shoot = _input.KeyPressStarted(Keys.Space);
        }
    }
}
