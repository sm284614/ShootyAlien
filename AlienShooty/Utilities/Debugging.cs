using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlienShooty.Utilities
{
    public static class Debugging
    {
        public static string DebugText { get; set; } = "";
        public static  Texture2D WhiteTexture { get; private set; }
        public static  Texture2D RedTexture { get; private set; }
        public static  Texture2D GreenTexture { get; private set; }
        public static  Texture2D BlueTexture { get; private set; }
        public static Texture2D YellowTexture { get; private set; }
        public static Texture2D MagentaTexture { get; private set; }
        public static  Texture2D CyanTexture { get; private set; }
        public static  Texture2D BlueTextureTransparent { get; private set; }
        public static  Texture2D RedTextureTransparent { get; private set; }
        private static GraphicsDevice _graphics;
        public static void Initialise(GraphicsDevice graphicsDevice)
        {
            _graphics = graphicsDevice;
            WhiteTexture = CreateRectangleTexture(1, 1, Color.White);
            RedTexture = CreateRectangleTexture(1, 1, Color.Red);
            GreenTexture = CreateRectangleTexture(1, 1, Color.LightGreen);
            BlueTexture = CreateRectangleTexture(1, 1, Color.Blue);
            BlueTextureTransparent = CreateRectangleTexture(1, 1, new Color(32, 64, 96, 64));
            RedTextureTransparent = CreateRectangleTexture(1, 1, new Color(255, 0, 0, 64));
            YellowTexture = CreateRectangleTexture(1, 1, Color.Yellow);
            MagentaTexture = CreateRectangleTexture(1, 1, Color.Magenta);
            CyanTexture = CreateRectangleTexture(1, 1, Color.Cyan);
        }
        public static Texture2D CreateRectangleTexture(int width, int height, Color colour)
        {
            Texture2D texture = new Texture2D(_graphics, width, height);

            Color[] data = new Color[width * height];
            for (int i = 0; i < data.Length; ++i)
            {
                data[i] = colour;
            }

            texture.SetData(data);
            return texture;
        }
        public static void DrawRectangle(SpriteBatch spriteBatch, Texture2D texture, Rectangle rectangle, Color colour, float thickness = 1f, float layerDepth = 1f)
        {
            DrawLine(spriteBatch, texture, new Vector2(rectangle.Left, rectangle.Top),new Vector2(rectangle.Right, rectangle.Top), colour, thickness, layerDepth);
            DrawLine(spriteBatch, texture, new Vector2(rectangle.Right, rectangle.Top),new Vector2(rectangle.Right, rectangle.Bottom), colour, thickness, layerDepth);
            DrawLine(spriteBatch, texture, new Vector2(rectangle.Right, rectangle.Bottom),new Vector2(rectangle.Left, rectangle.Bottom), colour, thickness, layerDepth);
            DrawLine(spriteBatch, texture, new Vector2(rectangle.Left, rectangle.Bottom),new Vector2(rectangle.Left, rectangle.Top), colour, thickness, layerDepth);
        }
        public static void DrawLine(SpriteBatch spriteBatch, Texture2D texture, Vector2 startPosition, Vector2 endPosition, Color colour, float thickness = 1f, float layerDepth = 1f)
        {
            var distance = Vector2.Distance(startPosition, endPosition);
            var angle = (float)Math.Atan2(endPosition.Y - startPosition.Y, endPosition.X - startPosition.X);
            DrawLine(spriteBatch, texture, startPosition, distance, angle, colour, thickness, layerDepth);
        }
        public static void DrawLine(SpriteBatch spriteBatch, Texture2D texture, Vector2 position, float length, float angle, Color colour, float thickness = 1f, float layerDepth = 1f)
        {
            var origin = new Vector2(0f, thickness / 2);
            var scale = new Vector2(length, thickness);
            spriteBatch.Draw(texture, position, null, colour, angle, origin, scale, SpriteEffects.None, layerDepth);
        }
    }
}
