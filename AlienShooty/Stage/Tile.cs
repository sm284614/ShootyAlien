using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlienShooty.Stage
{
    public class Tile
    {
        public Texture2D Texture;
        public bool IsSolid;
        public Tile(Texture2D texture, bool isSolid)
        {
            Texture = texture;
            IsSolid = isSolid;
        }
    }
}
