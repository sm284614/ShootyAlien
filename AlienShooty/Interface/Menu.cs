using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlienShooty.Interface
{
    public abstract class Menu
    {
        protected readonly ContentLoader _contentLoader;
        protected readonly InputManager _input;
        protected List<Sprite> _graphics;
        public Menu(ContentLoader content, InputManager input)
        {
            _graphics = new List<Sprite>();
            _contentLoader = content;
            _input = input;
        }
        public virtual void Update(GameTime gametTime)
        {
         
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            foreach (Sprite graphic in _graphics)
            {
                graphic.Draw(spriteBatch);
            }
            spriteBatch.End();
        }
    }
}
