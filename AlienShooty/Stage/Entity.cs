using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlienShooty;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using nkast.Aether.Physics2D.Dynamics;

namespace AlienShooty.Stage
{
    public class Entity
    {
        private Sprite _sprite;
        private InputController _inputController;
        private PhysicsData _physicsData;
        public Entity(EntityTemplate template, Body body, InputController inputController)
        {
            _sprite = new Sprite(template.Texture, body.Position);
            _inputController = inputController;
            _physicsData = new PhysicsData(body, template.Speed);
        }
        public void Update(GameTime gameTime)
        {            
            _inputController.Update(gameTime);
            _physicsData.Update(gameTime, _inputController);
            _sprite.Update(gameTime, _physicsData.Body.Position, _physicsData.Body.Rotation);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            _sprite.Draw(spriteBatch);
        }
    }
}
