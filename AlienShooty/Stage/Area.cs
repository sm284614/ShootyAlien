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
        //the physics engine world speed for bodies is limited to 120 m/s, it needs scaling
        //TODO: remove physics engine and do bounding-box collision for player vs map & projectiles.
        private World _world;
        private List<Entity> _players;
        private List<Entity> _entities;
        private Map _map;
        private InputManager _input;
        private double _spawnTimer;
        private ContentLoader _contentLoader;
        public Area(ContentLoader contentLoader, string stageFile, InputManager input)
        {
            _spawnTimer = 0;
            _world = new World();
            _input = input;
            _map = new Map(contentLoader, stageFile);
            _entities = new List<Entity>();
            _players = new List<Entity>();
            _contentLoader = contentLoader;
            SpawnPlayer(Game1.Resolution.ToVector2() /2);
        }
        private void SpawnPlayer(Vector2 position)
        {
            // player is currently 32 by 32 metres in size. Need to scale pixels to physics bodies and use camera.
            Point playerBodySize = new Point(32, 32);
            EntityTemplate playerTemplate = new EntityTemplate("Player", "astronaut", _contentLoader.LoadTexture("astronaut"), 120);
            Body playerBody = _world.CreateRectangle(playerBodySize.X, playerBodySize.Y, 1, position, 0, BodyType.Dynamic);
            InputControllerPlayer playerInputController = new InputControllerPlayer(_input);
            Entity player = new Entity(playerTemplate, playerBody, playerInputController);
            _entities.Add(player);
            _players.Add(player);
        }
        private void SpawnEnemy(Vector2 position)
        {            
            Point enemyBodySize = new Point(32, 32);
            EntityTemplate enemyTemplate = new EntityTemplate("Enemy", "alien", _contentLoader.LoadTexture("robot"), 120);
            Body enemyBody = _world.CreateRectangle(enemyBodySize.X, enemyBodySize.Y, 1, position, 0, BodyType.Dynamic);
            InputController enemyInputController = new InputController();
            Entity enemy = new Entity(enemyTemplate, enemyBody, enemyInputController);
            _entities.Add(enemy);
        }

        public void Update(GameTime gameTime)
        {
            UpdateEntities(gameTime);
            SpawnEnemies(gameTime);
            _world.Step((float)gameTime.ElapsedGameTime.TotalSeconds);
        }
        public void UpdateEntities(GameTime gameTime)
        {
            foreach (Entity entity in _entities)
            {
                entity.Update(gameTime);
            }
        }     
        public void SpawnEnemies(GameTime gameTime)
        {
            _spawnTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if (_spawnTimer > 0.5)
            {
                _spawnTimer = 0;
                //Random random = new Random();
                //int x, y;
                //int edge = random.Next(0, 4);
                //if (edge % 2 == 0)
                //{
                //    x = random.Next(0, Game1.Resolution.X);
                //    y = (edge == 0) ? y = 0 : y = Game1.Resolution.Y;
                //}
                //else
                //{
                //    y = random.Next(0, Game1.Resolution.Y);
                //    x = (edge == 1) ? x = 0 : x = Game1.Resolution.X;
                //}
                Vector2 position = new Vector2(0, Random.Shared.Next(0, Game1.Resolution.Y));
                SpawnEnemy(position);
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
