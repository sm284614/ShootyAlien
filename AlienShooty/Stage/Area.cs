using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using nkast.Aether.Physics2D.Dynamics;

namespace AlienShooty.Stage
{
    public class Area
    {
        private World _world;
        private List<Entity> _players;
        private List<Entity> _entities;
        private Map _map;
        private InputManager _input;
        public Area(ContentLoader contentLoader, string stageFile, InputManager input)
        {
            _world = new World();
            _input = input;
            _map = new Map(contentLoader, stageFile);
            _entities = new List<Entity>();
            _players = new List<Entity>();
            LoadEntities(contentLoader);
        }
        private void LoadEntities(ContentLoader contentLoader)
        {
            AddPlayer(contentLoader);
        }

        private void AddPlayer(ContentLoader contentLoader)
        {
            Vector2 playerPosition = new Vector2(100, 100);
            Point playerBodySize = new Point(32, 32);
            EntityTemplate playerTemplate = new EntityTemplate("Player", "astronaut", contentLoader.LoadTexture("astronaut"), 250);
            Body playerBody = _world.CreateRectangle(playerBodySize.X, playerBodySize.Y, 1, playerPosition, 0, BodyType.Dynamic);
            InputControllerPlayer playerInputController = new InputControllerPlayer(_input);
            Entity player = new Entity(playerTemplate, playerBody, playerInputController);
            _entities.Add(player);
            _players.Add(player);
        }

        public void Update(GameTime gameTime)
        {
            UpdateEntities(gameTime);
            _world.Step((float)gameTime.ElapsedGameTime.TotalSeconds);
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
