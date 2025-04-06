using AlienShooty.Entities;
using AlienShooty.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AlienShooty.Stages.Stage;

namespace AlienShooty.Stages
{
    public class EntityManager
    {
        private Dictionary<string, EntityTemplate> _entityTemplates;
        private Dictionary<string, WeaponTemplate> _weaponTemplates;
        private List<Weapon> _weapons;
        private List<Entity> _players;
        private List<Entity> _entities;
        private Queue<EntitySpawnData> _spawnQueue;
        private double _spawnTimer;
        private ContentLoader _contentLoader;
        private World _world;
        private InputManager _inputManager;
        public EntityManager(ContentLoader contentLoader, World world, InputManager inputManager)
        {
            _contentLoader = contentLoader;
            _world = world;
            _inputManager = inputManager;
            Reset();
        }
        public void Reset()
        {
            _spawnTimer = 0;
            _spawnQueue = new Queue<EntitySpawnData>();
            _entities = new List<Entity>();
            _players = new List<Entity>();
            LoadEntityTemplates();
            LoadWeaponTemplates();
        }
        private void LoadEntityTemplates()
        {
            _entityTemplates = new Dictionary<string, EntityTemplate>();
            _entityTemplates.Add("player", new EntityTemplate("Player", "player", _contentLoader.LoadTexture("astronaut"), 1, new Vector2(16, 16), 1));
            _entityTemplates.Add("enemy", new EntityTemplate("Enemy", "enemy", _contentLoader.LoadTexture("robot"), 8, new Vector2(19, 27), 1, InputController.EntityBehaviour.StraightLine));
            _entityTemplates.Add("gun", new EntityTemplate("Gun", "gun", _contentLoader.LoadTexture("gun"), 8, new Vector2(27, 11), 1));
            _entityTemplates.Add("bullet", new EntityTemplate("bullet", "bullet", _contentLoader.LoadTexture("bullet"), 8, new Vector2(3, 5), 1));
        }
        private void LoadWeaponTemplates()
        {
            _weaponTemplates = new Dictionary<string, WeaponTemplate>();
            _weaponTemplates.Add("gun", new WeaponTemplate("Gun", _entityTemplates["gun"], _entityTemplates["bullet"], 5, 4, 100, 30));
        }
        public void Update(GameTime gameTime)
        {
            foreach (Entity entity in _entities)
            {
                entity.Update(gameTime);
                while (entity.PendingCommands.Count > 0)
                {
                    var command = entity.PendingCommands.Dequeue();
                    if (command is ShootCommand fire)
                    {
                        FireWeapon(fire);
                    }
                }
            }
            while (_spawnQueue.Count > 0)
            {
                EntitySpawnData spawnData = _spawnQueue.Dequeue();
                AddEntity(spawnData);
            }
        }
        private void FireWeapon(ShootCommand shot)
        {
            EntityTemplate bulletTemplate = shot.Weapon.Template.Projectile;
            Vector2 direction = shot.Direction;
            direction.Normalize();
            Vector2 initialBulletPosition = shot.Source.Body.Position + direction * 20;
            Vector2 bulletVelocity = direction * shot.Weapon.Template.ProjectileSpeed;
            float bulletRotation = MathF.Atan2(direction.Y, direction.X);
            _spawnQueue.Enqueue(new EntitySpawnData(bulletTemplate, initialBulletPosition, bulletVelocity, bulletRotation, EntityType.Bullet));
        }
        public Entity AddEntity(string key, Vector2 position, Vector2 velocity, float rotation, EntityType entityType)
        {
            EntitySpawnData data = new EntitySpawnData(_entityTemplates[key], position,Vector2.Zero, 0, entityType);
            return AddEntity(data);
        }
        public Entity AddEntity(EntitySpawnData spawnData)
        {
            Body body = _world.CreateBody(spawnData.Position, spawnData.Template.Size, spawnData.Template.Density, spawnData.Rotation, Body.BodyType.Dynamic);
            body.Velocity = spawnData.Velocity;
            switch (spawnData.EntityType)
            {
                case EntityType.Player:                    
                    Entity player = new Entity(spawnData.Template, body, new InputControllerPlayer(_inputManager));
                    player.AddWeapon(new Weapon(_weaponTemplates["gun"], player));
                    _players.Add(player);
                    _entities.Add(player);
                    return player;
                case EntityType.Bullet:
                    Entity bullet = new Entity(spawnData.Template, body, null);
                    _entities.Add(bullet);
                    return bullet;
                case EntityType.Enemy:
                    Entity enemy = new Entity(spawnData.Template, body, new InputController(spawnData.Template.DefaultBehaviour));
                    _entities.Add(enemy);
                    return enemy;
                default:
                    throw new NotImplementedException($"Entity type {spawnData.EntityType} not supported");
            }
        }
        public void SpawnEnemies(GameTime gameTime)
        {
            _spawnTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if (_spawnTimer > 0.5)
            {
                _spawnTimer = 0;
                Vector2 position = new Vector2(0, Random.Shared.Next(0, Game1.Resolution.Y));
                EntitySpawnData spawnData = new EntitySpawnData(_entityTemplates["enemy"], position, Vector2.Zero, 0, EntityType.Enemy);
                AddEntity(spawnData);
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Entity entity in _entities)
            {
                entity.Draw(spriteBatch);
            }
        }
        public void DrawDebug(SpriteBatch spriteBatch)
        {
            foreach (Entity entity in _entities)
            {
                Debugging.DrawRectangle(spriteBatch, Debugging.WhiteTexture, entity.Body.BoundingBox, Color.Blue);
            }
        }
    }
}
