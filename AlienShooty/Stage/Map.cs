using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlienShooty.Stage
{
    public class Map
    {
        private Texture2D _tileSet;
        private Dictionary<int, Tile> _tileDictionary;
        private int[,] _mapData;
        private Point _mapSize;
        private Point _tileSize;
        Vector2 _drawPosition;
        public Map(ContentLoader contentLoader, string stageFile)
        {
            _tileSize = new Point(32, 32);
            _tileSet = contentLoader.LoadTexture("spacestationtiles32");
            Point tileSetSize = new Point(8, 8);
            _tileDictionary = LoadTiles(contentLoader, _tileSet, tileSetSize, _tileSize);
            _mapData = LoadMapData(stageFile);
        }
        private Dictionary<int, Tile> LoadTiles(ContentLoader contentLoader, Texture2D tileSet, Point tileSetSize, Point tileSize)
        {
            Dictionary<int, Tile> tileDictionary = new Dictionary<int, Tile>();
            for (int y = 0; y < tileSetSize.Y; y++)
            {
                for (int x = 0; x < tileSetSize.X; x++)
                {
                    Texture2D tileTexture = contentLoader.SubTexture(tileSet, new Rectangle(x * tileSize.X, y * tileSize.Y, tileSize.X, tileSize.Y));
                    int tileIndex = y * tileSetSize.X + x;
                    Tile tile = new Tile(tileTexture, false);
                    tileDictionary.Add(tileIndex, tile);
                }
            }
            return tileDictionary;
        }
        private int[,] LoadMapData(string stageFile)
        {
            _mapSize = new Point(40, 25);
            int[,] mapData = new int[_mapSize.X, _mapSize.Y];
            for (int y = 0; y < _mapSize.Y; y++)
            {
                for (int x = 0; x < _mapSize.X; x++)
                {
                    mapData[x, y] = 2;
                }
            }
            return mapData;
        }
        public void Draw(SpriteBatch spriteBatch)
        {

            for (int y = 0; y < _mapSize.Y; y++)
            {
                for (int x = 0; x < _mapSize.X; x++)
                {
                    Texture2D texture = _tileDictionary[_mapData[x, y]].Texture;
                    _drawPosition = new Vector2(x * _tileSize.X, y * _tileSize.Y);
                    spriteBatch.Draw(texture, _drawPosition, Color.White);
                }
            }
        }
    }
}
