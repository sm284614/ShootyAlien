using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AlienShooty.Stages;
using AlienShooty.Utilities;

namespace AlienShooty.Entities
{
    public class Entity
    {
        public Stage.StageAction StageAction { get; set; }
        public PhysicsData PhysicsData { get; protected set; }
        public Weapon Weapon { get; protected set; }
        private Sprite _sprite;
        private InputController _inputController;
        public Entity(EntityTemplate template, Body body, InputController inputController, Weapon weapon = null)
        {
            _sprite = new Sprite(template.Texture, body.Position, rotation: body.Rotation);
            _inputController = inputController;
            PhysicsData = new PhysicsData(body, template.Speed);
            Weapon = weapon;
            StageAction = Stage.StageAction.None;
        }
        public void Update(GameTime gameTime)
        {
            if (_inputController != null)
            {
                _inputController?.Update(gameTime);
                PhysicsData.Update(gameTime, _inputController);
            }
            _sprite.Update(gameTime, PhysicsData.Body.Position, PhysicsData.Body.Rotation);
            Weapon?.Update(_inputController, gameTime);
            if (Weapon?.Firing ?? false)
            {
                Weapon.Firing = false;
                StageAction = Stage.StageAction.FireWeapon;
            }
        }
        public void AddWeapon(Weapon weapon)
        {
            Weapon = weapon;
        }
        public void Draw(SpriteBatch spriteBatch, bool debug = false)
        {
            _sprite.Draw(spriteBatch);
            Debugging.DrawRectangle(spriteBatch, Debugging.WhiteTexture, PhysicsData.Body.BoundingBox, Color.White);
        }
    }
}
