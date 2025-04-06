using AlienShooty.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlienShooty.Stages
{
    public struct TileBounds
    {
        int StartX;
        int StartY;
        int EndX;
        int EndY;
        public TileBounds(int startX, int startY, int endX, int endY)
        {
            this.StartX = startX;
            this.StartY = startY;
            this.EndX = endX;
            this.EndY = endY;
        }
    }
    public class World
    {
        //private float _islandSize;
        private List<Body> _bodies;
        private Map _map;
        private List<Vector2> _collisionCheckingTiles;
        public World(Map map)
        {
            _bodies = new List<Body>();
            _map = map;
            _collisionCheckingTiles = new List<Vector2>();
        }
        public void Update(GameTime gameTime)
        {
            foreach (Body body in _bodies)
            {
                Debugging.DebugText = $"Player@ {body.Position} : [{body.BoundingBox.Left / _map.TileSize.X},{body.BoundingBox.Top / _map.TileSize.Y}] to [{body.BoundingBox.Right / _map.TileSize.X},{body.BoundingBox.Bottom / _map.TileSize.Y}]\n";
                CheckMapCollisions(body);
                body.Update();
            }
        }
        private void CheckMapCollisions(Body body)
        {
            if (body.Velocity != Vector2.Zero)
            {
                body.Colliding = false;
                _collisionCheckingTiles.Clear();
                CheckCollisionXAxis(body);
                CheckCollisionYAxis(body);
            }
        }
        private void CheckCollisionXAxis(Body body)
        {
            //check if attepmted moving LEFT/RIGHT into solid tiles
            if (body.Velocity.X != 0)
            {
                Vector2 intendedPosition = body.Position + new Vector2(body.Velocity.X, 0);
                Rectangle intendedBounds = new Rectangle(
                    (int)(intendedPosition.X - body.HalfSize.X),
                    (int)(intendedPosition.Y - body.HalfSize.Y),
                    body.BoundingBox.Width,
                    body.BoundingBox.Height);

                // Get tile coordinates that the bounding box overlaps with
                int startX = Math.Max(0, intendedBounds.Left / _map.TileSize.X);
                int startY = Math.Max(0, intendedBounds.Top / _map.TileSize.Y);
                int endX = Math.Min(_map.MapSize.X - 1, intendedBounds.Right / _map.TileSize.X);
                int endY = Math.Min(_map.MapSize.Y - 1, intendedBounds.Bottom / _map.TileSize.Y);

                if (body.Velocity.X < 0)
                {
                    for (int y = startY; y <= endY; y++)
                    {
                        _collisionCheckingTiles.Add(new Vector2(startX, y));
                        if (_map.SolidTile(new Point(startX, y)))
                        {
                            float entityLeftEdge = body.BoundingBox.Left;
                            float tileRightEdge = startX * _map.TileSize.X + _map.TileSize.X;
                            body.Velocity.X = entityLeftEdge - tileRightEdge;
                            body.Colliding = true;
                            break;
                        }
                    }
                }
                else if (body.Velocity.X > 0)
                {
                    for (int y = startY; y <= endY; y++)
                    {
                        _collisionCheckingTiles.Add(new Vector2(endX, y));
                        if (_map.SolidTile(new Point(endX, y)))
                        {
                            float entityRightEdge = body.BoundingBox.Right;
                            float tileLeftEdge = endX * _map.TileSize.X - 1; // -1 is for 1-pixel offset
                            body.Velocity.X = entityRightEdge - tileLeftEdge;
                            body.Colliding = true;
                            break;
                        }
                    }
                }
                body.Position.X += body.Velocity.X;
            }
        }
        private void CheckCollisionYAxis(Body body)
        {
            if (body.Velocity.Y != 0)
            {
                Vector2 intendedPosition = body.Position + new Vector2(0, body.Velocity.Y);
                Rectangle intendedBounds = new Rectangle(
                    (int)(intendedPosition.X - body.HalfSize.X),
                    (int)(intendedPosition.Y - body.HalfSize.Y),
                    body.BoundingBox.Width,
                    body.BoundingBox.Height);

                // Get tile coordinates that the bounding box overlaps with
                int startX = Math.Max(0, intendedBounds.Left / _map.TileSize.X);
                int startY = Math.Max(0, intendedBounds.Top / _map.TileSize.Y);
                int endX = Math.Min(_map.MapSize.X - 1, intendedBounds.Right / _map.TileSize.X);
                int endY = Math.Min(_map.MapSize.Y - 1, intendedBounds.Bottom / _map.TileSize.Y);
                if (body.Velocity.Y < 0)
                {
                    for (int x = startX; x <= endX; x++)
                    {
                        _collisionCheckingTiles.Add(new Vector2(x, startY));
                        if (_map.SolidTile(new Point(x, startY)))
                        {
                            float entityTopEdge = body.BoundingBox.Top;
                            float tileBottomEdge = startY * _map.TileSize.Y + _map.TileSize.Y;
                            body.Velocity.Y = entityTopEdge - tileBottomEdge;
                            body.Colliding = true;
                            break;
                        }
                    }
                }
                // Check if moving DOWN into solid tiles
                else if (body.Velocity.Y > 0)
                {
                    for (int x = startX; x <= endX; x++)
                    {
                        _collisionCheckingTiles.Add(new Vector2(x, endY));
                        if (_map.SolidTile(new Point(x, endY)))
                        {
                            float entityBottomEdge = body.BoundingBox.Bottom;
                            float tileTopEdge = endY * _map.TileSize.Y - 1;
                            body.Velocity.Y = entityBottomEdge - tileTopEdge;
                            body.Colliding = true;
                            break;
                        }
                    }
                }
                body.Position.Y += body.Velocity.Y;
            }
        }
        public Body CreateBody(Vector2 position, Vector2 size, float density, float rotation, Body.BodyType bodyType)
        {
            Body body = new Body(position, size, density, rotation, bodyType);
            _bodies.Add(body);
            return body;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (Game1.DebugMode)
            {
                foreach (Vector2 tilePosition in _collisionCheckingTiles)
                {
                    Rectangle tileBounds = new Rectangle(
                        (int)(tilePosition.X * _map.TileSize.X),
                        (int)(tilePosition.Y * _map.TileSize.Y),
                        _map.TileSize.X,
                        _map.TileSize.Y);
                    Debugging.DrawRectangle(spriteBatch, Debugging.RedTexture, tileBounds, Color.White, 1f, 0.5f);
                }
            }
        }
    }
}
