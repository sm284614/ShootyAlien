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
        protected Entity _parent;
        public bool MoveUp {get; protected set;}
        public bool MoveDown {get; protected set;}
        public bool MoveLeft {get; protected set;}
        public bool MoveRight {get; protected set;}
        public bool Shoot {get; protected set;}
        public virtual void Update(GameTime gameTime)
        {
            MoveRight = true;
        }
    }
}
