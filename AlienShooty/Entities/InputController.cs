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
        public bool MoveUp {get; protected set;}
        public bool MoveDown {get; protected set;}
        public bool MoveLeft {get; protected set;}
        public bool MoveRight {get; protected set;}
        public bool Shoot {get; protected set;}
        public Vector2 Direction { get; protected set;}
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
            Chase
        }
        public virtual void Update(GameTime gameTime)
        {
            switch (Behaviour)
            {
                case EntityBehaviour.StraightLine:
                    StraightLine();
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
    }
}
