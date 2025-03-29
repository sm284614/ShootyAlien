using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlienShooty.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace AlienShooty.Stage
{
    public class Area
    {
        //the physics engine world speed for bodies is limited to 120 m/s, it needs scaling
        //TODO: remove physics engine and do bounding-box collision for player vs map & projectiles.
        private World _world;
        private Dictionary<string, EntityTemplate> _entityTemplates;
        private List<Entity> _players;
        private List<Entity> _entities;
        private Map _map;
        private InputManager _input;
        private double _spawnTimer;
        private ContentLoader _contentLoader;
        public Area(ContentLoader contentLoader, string stageFile, InputManager input)
        {
            _input = input;
            _map = new Map(contentLoader, stageFile);
            _contentLoader = contentLoader;
            ResetWorld();
        }
        private void ResetWorld()
        {
            _spawnTimer = 0;
            _world = new World();
            _entities = new List<Entity>();
            _players = new List<Entity>();
            LoadEntityTemplates();
            SpawnEntity("player", Game1.Resolution.ToVector2() / 2, true);
        }
        private void LoadEntityTemplates()
        {
            _entityTemplates = new Dictionary<string, EntityTemplate>();
            _entityTemplates.Add("player", new EntityTemplate("Player", "astronaut", _contentLoader.LoadTexture("astronaut"), 16, new Vector2(32, 32), 1));
            _entityTemplates.Add("enemy", new EntityTemplate("Enemy", "robot", _contentLoader.LoadTexture("robot"), 8, new Vector2(32, 32), 1));
        }
        public void Update(GameTime gameTime)
        {
            UpdateDebugCommands();
            UpdateEntities(gameTime);
            SpawnEnemies(gameTime);
            _world.Update(gameTime);
        }
        public void UpdateEntities(GameTime gameTime)
        {
            foreach (Entity entity in _entities)
            {
                entity.Update(gameTime);
            }
        }
        private void SpawnEntity(string templateName, Vector2 position, bool isPlayer = false)
        {
            EntityTemplate template = _entityTemplates[templateName];
            Body body = _world.CreateBody(position, template.Size, template.Density, 0, Body.BodyType.Dynamic);
            Entity entity;
            if (isPlayer)
            {
                entity = new Entity(template, body, new InputControllerPlayer(_input));
                _players.Add(entity);
            }
            else
            {
                entity = new Entity(template, body, new InputController());
            }
            _entities.Add(entity);
        }
        public void SpawnEnemies(GameTime gameTime)
        {
            _spawnTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if (_spawnTimer > 0.5)
            {
                _spawnTimer = 0;
                Vector2 position = new Vector2(0, Random.Shared.Next(0, Game1.Resolution.Y));
                SpawnEntity("enemy", position);
            }
        }
        private void UpdateDebugCommands()
        {
            if (_input.KeyPressStarted(Keys.R))
            {
                ResetWorld();
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
