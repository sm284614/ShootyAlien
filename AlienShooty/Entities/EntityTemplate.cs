using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlienShooty.Entities
{
    public class EntityTemplate
    {
        public string DisplayName;
        public string InternalName;
        public Texture2D Texture;
        public float Speed;
        public Vector2 Size;
        public float Density;
        public InputController.EntityBehaviour DefaultBehaviour;
        public EntityTemplate(string displayName, string internalName, Texture2D texture, float speed, Vector2 size, float density, InputController.EntityBehaviour defaultBehaviour = InputController.EntityBehaviour.None)
        {
            DisplayName = displayName;
            InternalName = internalName;
            Texture = texture;
            Speed = speed;
            Size = size;
            Density = density;
            DefaultBehaviour = defaultBehaviour;
        }

    }
}
