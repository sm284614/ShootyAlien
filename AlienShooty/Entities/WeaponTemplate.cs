using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlienShooty.Entities
{
    public class WeaponTemplate
    {
        public readonly string Name;
        public readonly EntityTemplate Weapon;
        public readonly EntityTemplate Projectile;
        public readonly string ProjectileName;
        public readonly float Damage;
        public readonly double FiringDelay;
        public readonly int Ammo;
        public readonly float Range;
        public readonly float Accuracy;
        public readonly float ProjectileSpeed;
        
        public WeaponTemplate(string name, EntityTemplate weaponEntityTemplate, EntityTemplate projectileEntityTemplate, float projectileDamage, float projectilesPerSecond, int ammo, float projectileSpeed, float range = float.MaxValue, float accuracy = 1)
        {
            Name = name;
            Weapon = weaponEntityTemplate;
            Projectile = projectileEntityTemplate;
            Damage = projectileDamage;
            FiringDelay = 1 / projectilesPerSecond;
            Ammo = ammo;
            Range = range;
            Accuracy = accuracy;
            ProjectileSpeed = projectileSpeed;
        }
    }
}
