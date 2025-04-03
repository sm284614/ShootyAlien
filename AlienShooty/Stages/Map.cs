using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlienShooty.Stages
{
    public class Map
    {
        private Texture2D _tileSet;
        private Dictionary<int, Tile> _tileDictionary;
        private int[,] _mapData;
        private Point _mapSize;
        private Point _tileSize;
        private Vector2 _halfTileSize;
        Vector2 _drawPosition;
        public Map(ContentLoader contentLoader, string stageFile)
        {
            _tileSize = new Point(32, 32);
            _halfTileSize = new Vector2(_tileSize.X / 2, _tileSize.Y / 2);
            _tileSet = contentLoader.LoadTexture("spacestation32");
            Point tileSetSize = new Point(8, 8);
            _tileDictionary = LoadTiles(contentLoader, _tileSet, tileSetSize, _tileSize);
            _mapData = LoadMapData(stageFile);
        }
        public Point TileSize => _tileSize;
        public Point MapSize => _mapSize;
        private Dictionary<int, Tile> LoadTiles(ContentLoader contentLoader, Texture2D tileSet, Point tileSetSize, Point tileSize)
        {
            Dictionary<int, Tile> tileDictionary = new Dictionary<int, Tile>();
            for (int y = 0; y < tileSetSize.Y; y++)
            {
                for (int x = 0; x < tileSetSize.X; x++)
                {
                    Texture2D tileTexture = contentLoader.SubTexture(tileSet, new Rectangle(x * tileSize.X, y * tileSize.Y, tileSize.X, tileSize.Y));
                    int tileIndex = y * tileSetSize.X + x;
                    bool isSolid = y > 3; //first four rows are walkable
                    Tile tile = new Tile(tileTexture, isSolid);
                    tileDictionary.Add(tileIndex, tile);
                }
            }
            return tileDictionary;
        }
        private int[,] LoadMapData(string stageFile)
        {
            _mapSize = new Point(60, 34);
            int[,] mapData = new int[_mapSize.X, _mapSize.Y];
            for (int y = 0; y < _mapSize.Y; y++)
            {
                for (int x = 0; x < _mapSize.X; x++)
                {
                    mapData[x, y] = 2;
                }
            }
            for (int y = 0; y < _mapSize.Y; y++)
            {
                mapData[0, y] = 40;
                mapData[_mapSize.X-1, y] = 40;
            }
            for (int x = 0; x < _mapSize.X; x++)
            {
                mapData[x, 0] = 57;
                mapData[x, _mapSize.Y - 1] = 57;
            }
            mapData[20, 17] = 34;
            mapData[20, 18] = 37;
            return mapData;
        }
        // TODO: maybe error-proof this? (out-of-bounds X or Y, invalid key)
        public bool SolidTile(Point tilePosition)
        {
            return _tileDictionary[_mapData[tilePosition.X, tilePosition.Y]].IsSolid;
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
