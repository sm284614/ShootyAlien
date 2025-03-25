using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AlienShooty.Stage
{
    public class Area
    {
        private List<Entity> _entities;
        private Map _map;
        private InputManager _input;
        public Area(ContentLoader contentLoader, string stageFile, InputManager _input)
        {
            _map = new Map(contentLoader, stageFile);
            _entities = new List<Entity>();
        }
        public void Update(GameTime gameTime)
        {
            UpdateEntities(gameTime);
        }
        public void UpdateEntities(GameTime gameTime)
        {
            foreach (Entity entity in _entities)
            {
                entity.Update(gameTime);
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            _map.Draw(spriteBatch);
            foreach (Entity entity in _entities)
            {
                entity.Draw(spriteBatch);
            }
        }
    }
}
