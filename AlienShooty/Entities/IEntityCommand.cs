using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace AlienShooty.Entities
{
    public interface IEntityCommand { }

    public class ShootCommand : IEntityCommand
    {
        public Entity Source { get; }
        public Vector2 Direction { get; }
        public Weapon Weapon { get; }

        public ShootCommand(Entity source, Vector2 direction, Weapon weapon)
        {
            Source = source;
            Direction = direction;
            Weapon = weapon;
        }
    }

}
