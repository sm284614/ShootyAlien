using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlienShooty
{
    public class ContentLoader
    {
        // using two content managers: one for stage (can be disposed at stage end), one for interface (stays loaded)
        private ContentManager _stageContent;
        private ContentManager _nterfaceContent;
        private GraphicsDeviceManager _graphics;
        private Texture2D _defaultTexture;
        public ContentLoader(IServiceProvider services, GraphicsDeviceManager graphics)
        {
            _stageContent = new ContentManager(services, "Content");
            _nterfaceContent = new ContentManager(services, "Content");
            _graphics = graphics;
            _defaultTexture = CreateRectangleTexture(32, 32, Color.Magenta);
        }
        // load a texture from the content pipeline
        public Texture2D LoadTexture(string path, bool stageContent = true)
        {
            try
            {
                if (stageContent)
                {
                    return _stageContent.Load<Texture2D>(path);
                }
                else
                {
                    return _nterfaceContent.Load<Texture2D>(path);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return _defaultTexture;
            }
        }
        public void DisposeStageTextures()
        {
            _stageContent.Unload();
        }
        //useful utilities for debugging
        public Texture2D CreateRectangleTexture(int width, int height, Color colour)
        {
            Texture2D texture = new Texture2D(_graphics.GraphicsDevice, width, height);

            Color[] data = new Color[width * height];
            for (int i = 0; i < data.Length; ++i)
            {
                data[i] = colour;
            }

            texture.SetData(data);
            return texture;
        }
        public Texture2D SubTexture(Texture2D texture, Rectangle sourceRectangle)
        {
            // Calculate the intersection between the source rectangle and the texture bounds
            Rectangle textureBounds = new Rectangle(0, 0, texture.Width, texture.Height);
            Rectangle intersection = Rectangle.Intersect(sourceRectangle, textureBounds);

            // If there is no intersection, return null
            if (intersection.Width == 0 || intersection.Height == 0)
            {
                return null;
            }

            // Create the subTexture with the size of the intersection
            Texture2D subTexture = new Texture2D(texture.GraphicsDevice, intersection.Width, intersection.Height);
            Color[] data = new Color[intersection.Width * intersection.Height];

            // Get the data from the texture
            texture.GetData(0, intersection, data, 0, data.Length);
            // Set the data to the subTexture
            subTexture.SetData(data);

            return subTexture;
        }
    }
}
