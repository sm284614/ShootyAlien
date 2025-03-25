using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace AlienShooty.Interface
{
    public class UserInterface
    {
        public Menu MainMenu;
        public Menu PauseMenu;
        private InputManager _input;
        public UserInterface(ContentLoader contentLoader, InputManager input)
        {
            _input = input;
            MainMenu = new MainMenu(contentLoader, input);
            PauseMenu = new PauseMenu(contentLoader, input);
        }
        public void Update(GameTime gameTime)
        {
            switch (Game1.State)
            {
                case Game1.GameState.InStage:
                    if (_input.KeyReleased(Keys.Escape))
                    {
                        Game1.ChangeState(Game1.GameState.Paused);
                    }
                    break;
            }
        }   
    }
}
