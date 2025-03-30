using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AlienShooty
{
    public class Sprite
    {
        protected Texture2D _texture;
        protected Vector2 _position;
        protected Color _colour;
        protected float _rotation;
        protected Vector2 _origin; //offset from texture top-left, used for rotation and scaling
        protected Vector2 _scale;
        protected SpriteEffects _facing; //can flip sprites left/right, top/bottom
        protected float _layerDepth;
        protected Rectangle _boundingBox;
        public Sprite(Texture2D texture, Vector2 position, Color? colour = null, SpriteEffects facing = SpriteEffects.None, Vector2? origin = null, Vector2? scale = null, float layerDepth = 0.01f, float rotation = 0)
        {
            _texture = texture;
            _position = position;
            _scale = scale ?? Vector2.One;
            _colour = colour ?? Color.White;
            _rotation = rotation;
            _origin = origin ?? new Vector2(texture.Width / 2, _texture.Height / 2);
            _facing = facing;
            _layerDepth = layerDepth;
            _boundingBox = new Rectangle((int)_position.X - _texture.Width / 2, (int)_position.Y - _texture.Height / 2, _texture.Width, _texture.Height);
        }
        public Vector2 Position { get => _position; }
        public Color Colour { get => _colour; set => _colour = value; }
        public float Rotation { get => _rotation; }
        public Vector2 Scale { get => _scale; }
        public Vector2 Origin { get => _origin; }
        public float LayerDepth { get => _layerDepth; set => _layerDepth = value; }
        public virtual Rectangle TextureBoundingBox { get => _boundingBox; }
        public Texture2D Texture { get => _texture; set => _texture = value; }

        public virtual void Update(GameTime gameTime, Vector2 position, float rotation)
        {
            _position = position;
            _rotation = rotation;
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, null, _colour, _rotation, _origin, _scale, _facing, _layerDepth);
        }
    }
}
