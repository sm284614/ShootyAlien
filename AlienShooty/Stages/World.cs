using AlienShooty.Utilities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlienShooty.Stages
{
    public class World
    {
        //private float _islandSize;
        private List<Body> _bodies;
        private Map _map;
        public World(Map map)
        {
            _bodies = new List<Body>();
            _map = map;
        }
        public void Update(GameTime gameTime)
        {
            foreach (Body body in _bodies)
            {
                Debugging.DebugText = $"Player@ {body.Position} : [{body.BoundingBox.Left / _map.TileSize.X},{body.BoundingBox.Top / _map.TileSize.Y}] to [{body.BoundingBox.Right / _map.TileSize.X},{body.BoundingBox.Bottom / _map.TileSize.Y}]\n";
                if (body.Velocity != Vector2.Zero)
                {
                    Vector2 intendedPosition = body.Position + body.Velocity;
                    Rectangle intendedBounds = new Rectangle(
                        (int)(intendedPosition.X - body.HalfSize.X),
                        (int)(intendedPosition.Y - body.HalfSize.Y),
                        body.BoundingBox.Width,
                        body.BoundingBox.Height);
                    CheckMapCollisions(body, intendedBounds);
                }
                body.Update();
            }
        }
        private void CheckMapCollisions(Body body, Rectangle intendedBounds)
        {
            // Get tile coordinates that the bounding box overlaps with
            int startX = Math.Max(0, intendedBounds.Left / _map.TileSize.X);
            int startY = Math.Max(0, intendedBounds.Top / _map.TileSize.Y);
            int endX = Math.Min(_map.MapSize.X - 1, intendedBounds.Right / _map.TileSize.X);
            int endY = Math.Min(_map.MapSize.Y - 1, intendedBounds.Bottom / _map.TileSize.Y);


            bool collision = false;
            Vector2 correctedPosition = body.Position + body.Velocity; // Start with intended position

            //check if attepmted moving LEFT into solid tiles
            if (body.Velocity.X < 0)
            {
                for (int y = startY; y <= endY; y++)
                {
                    if (_map.SolidTile(new Point(startX, y)))
                    {
                        correctedPosition.X = startX * _map.TileSize.X + _map.TileSize.X;
                        collision = true;
                        break;
                    }
                }
            }
            //check if attepmted moving RIGHT into solid tiles
            else if (body.Velocity.X > 0)
            {
                for (int y = startY; y <= endY; y++)
                {
                    if (_map.SolidTile(new Point(endX, y)))
                    {
                        correctedPosition.X = endX * _map.TileSize.X - body.BoundingBox.Width;
                        collision = true;
                        break;
                    }
                }
            }
            //check if attepmted moving UP into solid tiles
            if (body.Velocity.Y < 0)
            {
                for (int x = startX; x <= endX; x++)
                {
                    if (_map.SolidTile(new Point(x, startY)))
                    {
                        correctedPosition.Y = startY * _map.TileSize.Y + _map.TileSize.Y;
                        collision = true;
                        break;
                    }
                }
            }
            //check if attepmted moving DOWN into solid tiles
            else if (body.Velocity.Y > 0)
            {
                for (int x = startX; x <= endX; x++)
                {
                    if (_map.SolidTile(new Point(x, endY)))
                    {
                        correctedPosition.Y = endY * _map.TileSize.Y - body.BoundingBox.Height;
                        collision = true;
                        break;
                    }
                }
            }

            // Update body's position and collision state
            if (collision)
            {
                body.Position = correctedPosition;
                body.Colliding = true;
            }
            else
            {
                body.Position += body.Velocity;
                body.Colliding = false;
            }
        }
        public Body CreateBody(Vector2 position, Vector2 size, float density, float rotation, Body.BodyType bodyType)
        {
            Body body = new Body(position, size, density, rotation, bodyType);
            _bodies.Add(body);
            return body;
        }
    }
}
