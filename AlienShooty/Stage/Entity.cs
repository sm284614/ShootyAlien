using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlienShooty;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AlienShooty.Stage
{
    public class Entity
    {
        private Sprite _sprite;
        private InputController _inputController;
        private PhysicsData _physicsData;
        public Entity(EntityTemplate template, Vector2 position)
        {
            _sprite = new Sprite(template.Texture, position);
            _inputController = new InputController();
            _physicsData = new PhysicsData(position);
        }
        public void Update(GameTime gameTime)
        {
            _inputController.Update(gameTime);
            _physicsData.Update(gameTime);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            _sprite.Draw(spriteBatch);
        }
    }
}
