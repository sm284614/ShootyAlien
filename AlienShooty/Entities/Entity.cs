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
        public Queue<IEntityCommand> PendingCommands { get; } = new();
        public Weapon Weapon { get; protected set; }
        public readonly Body Body;
        private EntityTemplate _template;
        private Sprite _sprite;
        private InputController _inputController;
        public Entity(EntityTemplate template, Body body, InputController inputController, Weapon weapon = null)
        {
            Body = body;
            _template = template;
            _sprite = new Sprite(template.Texture, body.Position, rotation: body.Rotation);
            _inputController = inputController;
            Weapon = weapon;
        }
        public void Update(GameTime gameTime)
        {
            if (_inputController != null)
            {
                _inputController?.Update(gameTime);
                Move();
                Shoot(gameTime);
            }
            _sprite.Update(gameTime, Body.Position, Body.Rotation);
        }
        public void Move()
        {
            Body.Direction = _inputController.Direction - Body.Position;
            float currentSpeed = _inputController.Running ? _template.Speed * 2 : _template.Speed;
            if (_inputController.MoveUp)
            {
                Body.Velocity.Y = -currentSpeed;
            }
            else if (_inputController.MoveDown)
            {
                Body.Velocity.Y = currentSpeed;
            }
            else
            {
                Body.Velocity.Y = 0;
            }
            if (_inputController.MoveLeft)
            {
                Body.Velocity.X = -currentSpeed;
            }
            else if (_inputController.MoveRight)
            {
                Body.Velocity.X = currentSpeed;
            }
            else
            {
                Body.Velocity.X = 0;
            }
        }
        public void Shoot(GameTime gameTime)
        {
            if (_inputController.Shoot)
            {
                if (Weapon?.Fire(gameTime) ?? false)
                {
                    PendingCommands.Enqueue(new ShootCommand(this, Body.Direction, Weapon));
                }
            }
            if (_inputController.ShootUp)
            {
                if (Weapon?.Fire(gameTime) ?? false)
                {
                    PendingCommands.Enqueue(new ShootCommand(this, -Vector2.UnitY, Weapon));
                }
            }
            if (_inputController.ShootDown)
            {
                if (Weapon?.Fire(gameTime) ?? false)
                {
                    PendingCommands.Enqueue(new ShootCommand(this, Vector2.UnitY, Weapon));
                }
            }
            if (_inputController.ShootLeft)
            {
                if (Weapon?.Fire(gameTime) ?? false)
                {
                    PendingCommands.Enqueue(new ShootCommand(this, -Vector2.UnitX, Weapon));
                }
            }
            if (_inputController.ShootRight)
            {
                if (Weapon?.Fire(gameTime) ?? false)
                {
                    PendingCommands.Enqueue(new ShootCommand(this, Vector2.UnitX, Weapon));
                }
            }
        }
        public void AddWeapon(Weapon weapon)
        {
            Weapon = weapon;
        }
        public void Draw(SpriteBatch spriteBatch, bool debug = false)
        {
            _sprite.Draw(spriteBatch);
        }
    }
}
