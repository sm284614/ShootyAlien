﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlienShooty.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using AlienShooty.Utilities;


namespace AlienShooty.Stages
{
    public class Stage
    {
        private World _world;
        private Camera _camera;
        private Vector2 _mouseWorldPosition;
        private EntityManager _entityManager;
        private InputManager _input;
        private Map _map;
        private ContentLoader _contentLoader;
        public Stage(string stageFile, ContentLoader contentLoader, InputManager input, GraphicsDevice graphics)
        {
            _input = input;
            _camera = new Camera(graphics);
            _contentLoader = contentLoader;
            _map = new Map(contentLoader, stageFile);
            Reset();
        }
        public enum EntityType
        {
            Player,
            Enemy,
            Bullet,
            Turret
        }
        private void Reset()
        {
            _camera.Reset();
            _camera.ZoomToScale(2);
            _world = new World(_map);
            _entityManager = new EntityManager(_contentLoader, _world, _input);
            Entity player =  _entityManager.AddEntity("player", new Vector2(96,96), Vector2.Zero, 0, EntityType.Player);
            _camera.TrackingBody = player.Body;
        }

        public void Update(GameTime gameTime)
        {
            UpdateDebugCommands();
            _mouseWorldPosition = _camera.ConvertScreenToWorld(_input.MousePosition);
            _camera.Update(gameTime);
            _entityManager.Update(gameTime);
            _world.Update(gameTime);
            Debugging.DebugText += $"MouseWorld@{_camera.ConvertScreenToWorld(_input.MousePosition)}";
        }
        private void UpdateDebugCommands()
        {
            if (_input.KeyPressStarted(Keys.R))
            {
                Reset();
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
            _entityManager.Draw(spriteBatch);
            if (Game1.DebugMode)
            {
                _world.DrawDebug(spriteBatch);
                _entityManager.DrawDebug(spriteBatch);
            }
            _world.DrawDebug(spriteBatch);
            spriteBatch.End();
        }
    }
}
