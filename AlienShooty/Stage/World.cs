using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlienShooty.Stage
{
    public class World
    {
        //private float _islandSize;
        private List<Body> _bodies;
        public World()
        {
            _bodies = new List<Body>();
        }
        public void Update(GameTime gameTime)
        {
            // Process each body with non-zero velocity
            foreach (Body body in _bodies)
            {
                if (body.Velocity != Vector2.Zero)
                {
                    // Calculate intended new position
                    Vector2 intendedPosition = body.Position + body.Velocity;

                    // Create a rectangle representing where the body would be after movement
                    Rectangle intendedBounds = new Rectangle(
                        (int)intendedPosition.X,
                        (int)intendedPosition.Y,
                        body.BoundingBox.Width,
                        body.BoundingBox.Height);

                    bool willCollide = false;
                    float closestCollisionFactor = 1.0f; // Will be reduced if collisions are found

                    // Check against all other bodies for potential collisions
                    foreach (Body otherBody in _bodies)
                    {
                        if (otherBody == body) continue; // Skip self

                        // If intended movement would cause collision
                        if (intendedBounds.Intersects(otherBody.BoundingBox))
                        {
                            willCollide = true;

                            // Calculate how far along the velocity vector the collision occurs
                            float collisionFactor = CalculateCollisionFactor(
                                body.Position, body.Velocity, body.BoundingBox,
                                otherBody.Position, otherBody.BoundingBox);

                            // Keep track of the earliest collision
                            closestCollisionFactor = Math.Min(closestCollisionFactor, collisionFactor);
                        }
                    }

                    // Update position based on collision results
                    if (willCollide)
                    {
                        // Move only up to the point of collision, minus a small buffer
                        // buffer prevents rounding errors and ensures bodies don't overlap
                        float buffer = 0.001f;
                        closestCollisionFactor = Math.Max(0, closestCollisionFactor - buffer);
                        body.Position += body.Velocity * closestCollisionFactor;
                        body.Colliding = true;

                        // Optional: Stop velocity in this direction
                        body.Velocity = Vector2.Zero;
                    }
                    else
                    {
                        // No collision, move the full amount
                        body.Position += body.Velocity;
                        body.Colliding = false;
                    }
                }
                body.Update();
            }
        }

        // Helper method to calculate how far along the velocity vector a collision occurs
        private float CalculateCollisionFactor(Vector2 pos1, Vector2 vel, Rectangle bounds1,
                                              Vector2 pos2, Rectangle bounds2)
        {
            // Start with assumption we can move the full distance
            float factor = 1.0f;

            // Create rectangles for testing positions along the velocity vector
            Rectangle currentBounds = bounds1;
            Rectangle targetBounds = bounds2;

            // Binary search to find collision point
            // (A more sophisticated ray-rectangle algorithm could be used instead)
            float min = 0.0f;
            float max = 1.0f;

            for (int i = 0; i < 10; i++) // 10 iterations gives good precision
            {
                float mid = (min + max) / 2;

                // Test position at this point along velocity vector
                Rectangle testBounds = new Rectangle(
                    (int)(pos1.X + vel.X * mid),
                    (int)(pos1.Y + vel.Y * mid),
                    bounds1.Width,
                    bounds1.Height);

                if (testBounds.Intersects(targetBounds))
                {
                    // Collision at this point or earlier
                    max = mid;
                }
                else
                {
                    // No collision yet at this point
                    min = mid;
                }
            }

            return min; // Return the last safe position factor
        }
        public Body CreateBody(Vector2 position, Vector2 size, float density, float rotation, Body.BodyType bodyType)
        {
            Body body = new Body(position, size, density, rotation, bodyType);
            _bodies.Add(body);
            return body;
        }
    }
}
