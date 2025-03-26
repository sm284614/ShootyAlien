using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlienShooty.Stage
{
    public class EntityTemplate
    {
        public string Name;
        public string TextureName;
        public Texture2D Texture;
        public float Speed;        
        public EntityTemplate(string name, string textureName, Texture2D texture, float speed) 
        {
            Name = name;
            TextureName = textureName;
            Texture = texture;
            Speed = speed;
        }

    }
}
