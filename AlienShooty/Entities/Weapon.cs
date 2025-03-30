using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlienShooty.Entities
{
    public class Weapon
    {
        public bool Firing;
        public WeaponTemplate Template;
        public Entity Owner;
        private float _lastFired;
        private float _ammo;
        public Weapon(WeaponTemplate template, Entity owner, float remainingAmmoPercent = 1)
        {
            Template = template;
            Owner = owner;
            Firing = false;
            _lastFired = 0;
            _ammo = (int)(template.Ammo * remainingAmmoPercent);
        }
        public void Update(InputController inputController, GameTime gameTime)
        {
            if (inputController.Shoot)
            {
                if (gameTime.TotalGameTime.TotalSeconds - _lastFired > Template.FiringDelay)
                {
                    Firing = true;
                    _lastFired = (float)gameTime.TotalGameTime.TotalSeconds;
                }
            }
        }   
    }
}
