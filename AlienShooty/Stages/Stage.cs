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
using AlienShooty.Utilities;


namespace AlienShooty.Stages
{
    public class Stage
    {
        private World _world;
        private Camera _camera;
        private Vector2 _mouseWorldPosition;
        private Dictionary<string, EntityTemplate> _entityTemplates;
        private Dictionary<string, WeaponTemplate> _weaponTemplates;
        private List<EntitySpawnData> _spawningEntities;
        private List<Entity> _players;
        private List<Entity> _entities;
        private List<Weapon> _weapons;
        private Map _map;
        private InputManager _input;
        private double _spawnTimer;
        private ContentLoader _contentLoader;
        public Stage(string stageFile, ContentLoader contentLoader, InputManager input, GraphicsDevice graphics)
        {
            _input = input;
            _map = new Map(contentLoader, stageFile);
            _contentLoader = contentLoader;
            _camera = new Camera(graphics);
            ResetWorld();
        }
        public enum StageAction
        {
            None,
            FireWeapon,
            PlaceTurret,
            Repair
        }
        public enum EntityType
        {
            Player,
            Enemy,
            Bullet,
            Turret
        }
        private void ResetWorld()
        {
            _camera.Reset();
            _spawnTimer = 0;
            _world = new World(_map);
            _spawningEntities = new List<EntitySpawnData>();
            _entities = new List<Entity>();
            _players = new List<Entity>();
            LoadEntityTemplates();
            LoadWeaponTemplates();
            EntitySpawnData playerSpawn = new EntitySpawnData(_entityTemplates["player"], Game1.Resolution.ToVector2() / 2, Vector2.Zero, 0, EntityType.Player);
            AddEntity(playerSpawn);
        }
        private void LoadEntityTemplates()
        {
            _entityTemplates = new Dictionary<string, EntityTemplate>();
            _entityTemplates.Add("player", new EntityTemplate("Player", "player", _contentLoader.LoadTexture("astronaut"), 8, new Vector2(32, 32), 1));
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
            _mouseWorldPosition = _camera.ConvertScreenToWorld(_input.MousePosition);
            Debugging.DebugText = $"Mouse: {_input.MousePosition} World: {_mouseWorldPosition}\n";
            _camera.Update(gameTime);
            UpdateDebugCommands();
            UpdateEntities(gameTime);
            //SpawnEnemies(gameTime);
            _world.Update(gameTime);
        }
        public void UpdateEntities(GameTime gameTime)
        {
            foreach (Entity entity in _entities)
            {
                entity.Update(gameTime);
                if (entity.StageAction == StageAction.FireWeapon)
                {
                    FireWeapon(entity);
                }
            }
            foreach (EntitySpawnData entitySpawnData in _spawningEntities)
            {
                AddEntity(entitySpawnData);
            }
            _spawningEntities.Clear();
        }

        private void FireWeapon(Entity entity)
        {
            EntityTemplate bulletTemplate = entity.Weapon.Template.ProjectileEntityTemplate;
            Vector2 direction = _mouseWorldPosition - entity.PhysicsData.Body.Position;
            direction.Normalize();
            Vector2 initialBulletPosition = entity.PhysicsData.Body.Position + direction * 20;
            Vector2 bulletVelocity = direction * entity.Weapon.Template.ProjectileSpeed;
            float bulletRotation = MathF.Atan2(direction.Y, direction.X);
            _spawningEntities.Add(new EntitySpawnData(bulletTemplate, initialBulletPosition, bulletVelocity, bulletRotation, EntityType.Bullet));
            entity.StageAction = StageAction.None;
        }

        private void AddEntity(EntitySpawnData spawnData)
        {
            Body body = _world.CreateBody(spawnData.Position, spawnData.Template.Size, spawnData.Template.Density, spawnData.Rotation, Body.BodyType.Dynamic);
            body.Velocity = spawnData.Velocity;
            switch (spawnData.EntityType)
            {
                case EntityType.Player:
                    Entity player = new Entity(spawnData.Template, body, new InputControllerPlayer(_input));
                    player.AddWeapon(new Weapon(_weaponTemplates["gun"], player));
                    _players.Add(player);
                    _camera.TrackingBody = player.PhysicsData.Body;
                    _entities.Add(player);
                    break;
                case EntityType.Bullet:
                    Entity bullet = new Entity(spawnData.Template, body, null);
                    _entities.Add(bullet);
                    break;
                case EntityType.Enemy:
                    Entity enemy = new Entity(spawnData.Template, body, new InputController(spawnData.Template.DefaultBehaviour));
                    _entities.Add(enemy);
                    break;
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
        private void UpdateDebugCommands()
        {
            if (_input.KeyPressStarted(Keys.R))
            {
                ResetWorld();
            }
            if (_input.KeyDown(Keys.OemPlus))
            {
                _camera.Zoom(0.05f);
            }
            if (_input.KeyDown(Keys.OemMinus))
            {
                _camera.Zoom(-0.05f);
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, RasterizerState.CullNone, transformMatrix: _camera.View);
            _map.Draw(spriteBatch);
            foreach (Entity entity in _entities)
            {
                entity.Draw(spriteBatch);
            }
            spriteBatch.End();
        }
    }
}
