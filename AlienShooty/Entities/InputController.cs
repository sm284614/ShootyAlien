using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlienShooty.Entities
{
    public class InputController
    {
        public bool MoveUp { get; protected set; }
        public bool MoveDown { get; protected set; }
        public bool MoveLeft { get; protected set; }
        public bool MoveRight { get; protected set; }
        public bool Shoot { get; protected set; }
        public bool ShootUp { get; protected set; }
        public bool ShootDown { get; protected set; }
        public bool ShootLeft { get; protected set; }
        public bool ShootRight { get; protected set; }
        public bool Running { get; protected set; }
        public Vector2 Direction { get; protected set; }
        public EntityBehaviour Behaviour { get; set; }
        public InputController(EntityBehaviour behaviour = EntityBehaviour.None)
        {
            MoveUp = false;
            MoveDown = false;
            MoveLeft = false;
            MoveRight = false;
            Shoot = false;
            Behaviour = behaviour;
            Direction = Vector2.Zero;
        }
        public enum EntityBehaviour
        {
            None,
            StraightLine,
            Wander,
            Chase
        }
        public virtual void Update(GameTime gameTime)
        {
            switch (Behaviour)
            {
                case EntityBehaviour.StraightLine:
                    StraightLine();
                    break;
                case EntityBehaviour.Wander:
                    Wander(gameTime);
                    break;
                case EntityBehaviour.Chase:
                    break;
                default:
                    break;
            }
        }
        public void StraightLine()
        {
            MoveRight = true;
        }
        public void Wander(GameTime gameTime)
        {
            if (Random.Shared.NextDouble() < 0.01)
            {
                Direction = new Vector2(Random.Shared.Next(-1, 2), Random.Shared.Next(-1, 2));
            }
            MoveRight = Direction.X > 0;
            MoveLeft = Direction.X < 0;
            MoveUp = Direction.Y < 0;
            MoveDown = Direction.Y > 0;
        }
    }
}
