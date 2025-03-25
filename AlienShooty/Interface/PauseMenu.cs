using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlienShooty.Interface
{
    class PauseMenu :Menu
    {
        public PauseMenu(ContentLoader content, InputManager input) : base(content, input)
        {
            Vector2 midScreen = new Vector2(Game1.Resolution.X, Game1.Resolution.Y) / 2;
            _graphics.Add(new Sprite(content.LoadTexture(@"menu\pause"), midScreen));
        }
        public override void Update(GameTime gameTime)
        {
            if (_input.KeyReleased(Keys.Escape))
            {
                Game1.ChangeState(Game1.GameState.InStage);
            }
            if (_input.KeyReleased(Keys.X))
            {
                Game1.ChangeState(Game1.GameState.MainMenu);
            }
        }
    }
}
