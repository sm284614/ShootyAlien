using AlienShooty.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
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
        private string _name;
        private string _fileName;
        private Texture2D _tileSet;
        private Dictionary<int, Tile> _tileDictionary;
        private int[,] _tileData;
        private Point _mapSize;
        private Point _tileSetSize;
        private Point _tileSize;
        private Vector2 _tileSizeV;
        private Vector2 _halfTileSize;
        Vector2 _drawPosition;
        public Map(ContentLoader contentLoader, string fileName)
        {
            _fileName = fileName;
            _tileData = LoadMapData(fileName, contentLoader);
        }
        public Point TileSize => _tileSize;
        public Vector2 TileSizeV => _tileSizeV;
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
        private int[,] LoadMapData(string fileName, ContentLoader contentLoader)
        {
            int[,] tileData;
            try
            {
                int lineNumber = 0;
                string[] fileLines = System.IO.File.ReadAllLines(fileName);
                while (lineNumber < fileLines.Length && fileLines[lineNumber].ToLower() != "layout:")
                {
                    string[] split = fileLines[lineNumber].ToLower().Split('=');
                    switch (split[0])
                    {
                        case "name":
                            _name = split[1];
                            break;
                        case "tileset":
                            _tileSet = contentLoader.LoadTexture(split[1]);
                            break;
                        case "tilesetsize":
                            _tileSetSize = PointFromString(split[1]);
                            break;
                        case "tilesize":
                            _tileSize = PointFromString(split[1]);
                            _tileSizeV = _tileSize.ToVector2();
                            _halfTileSize = new Vector2(_tileSize.X / 2, _tileSize.Y / 2);
                            break;
                        case "mapsize":
                            _mapSize = PointFromString(split[1]);
                            break;
                    }
                    lineNumber++;
                }
                lineNumber++;
                _tileDictionary = LoadTiles(contentLoader, _tileSet, _tileSetSize, _tileSize);
                tileData = new int[_mapSize.X, _mapSize.Y];
                int mapRow = 0;
                while (lineNumber < fileLines.Length)
                {
                    string[] tiles = fileLines[lineNumber].Split(',');
                    for (int x = 0; x < tiles.Length; x++)
                    {
                        if (int.TryParse(tiles[x], out int tileIndex))
                        {
                            tileData[x, mapRow] = tileIndex;
                        }
                    }
                    mapRow++;
                    lineNumber++;
                }
                return tileData;
            }
            catch (Exception ex)
            {
                _name = $"Error map: {ex.Message}";
                return HardCodedMapData(contentLoader);
            }
        }
        private int[,] HardCodedMapData(ContentLoader contentLoader)
        {
            _tileSize = new Point(32, 32);
            _tileSizeV = _tileSize.ToVector2();
            _halfTileSize = new Vector2(_tileSize.X / 2, _tileSize.Y / 2);
            _tileSet = contentLoader.LoadTexture("spacestation32");
            Point tileSetSize = new Point(8, 8);
            _tileDictionary = LoadTiles(contentLoader, _tileSet, tileSetSize, _tileSize);
            _mapSize = new Point(15, 11);
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
                mapData[_mapSize.X - 1, y] = 40;
            }
            for (int x = 0; x < _mapSize.X; x++)
            {
                mapData[x, 0] = 57;
                mapData[x, _mapSize.Y - 1] = 57;
            }
            mapData[5, 5] = 34;
            mapData[5, 6] = 37;
            return mapData;
        }
        public Point PointFromString(string commaSeparatedPointString)
        {
            string[] xyValues = commaSeparatedPointString.Split(',');
            int x = int.Parse(xyValues[0]);
            int y = int.Parse(xyValues[1]);
            return new Point(x, y);
        }
        // TODO: maybe error-proof this? (out-of-bounds X or Y, invalid key)
        public bool SolidTile(Point tilePosition)
        {
            if (tilePosition.X < 0 || tilePosition.X >= _mapSize.X || tilePosition.Y < 0 || tilePosition.Y >= _mapSize.Y)
            {
                return true;
            }
            return _tileDictionary[_tileData[tilePosition.X, tilePosition.Y]].IsSolid;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int y = 0; y < _mapSize.Y; y++)
            {
                for (int x = 0; x < _mapSize.X; x++)
                {
                    Tile tile = _tileDictionary[_tileData[x, y]];
                    Texture2D texture = tile.Texture;
                    _drawPosition = new Vector2(x * _tileSize.X, y * _tileSize.Y);
                    spriteBatch.Draw(texture, _drawPosition, Color.White);
                    if (Game1.DebugMode && tile.IsSolid)
                    {
                        Rectangle rectangle = new Rectangle(_drawPosition.ToPoint(), _tileSize);
                        Debugging.DrawRectangle(spriteBatch, Debugging.BlueTexture, rectangle, Color.White);
                    }
                }
            }
        }
    }
}
