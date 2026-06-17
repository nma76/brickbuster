using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace brickbuster.Entities
{
    public class Ball
    {
        public Vector2 Position;
        public Vector2 Velocity;
        public float Radius;

        public Ball(Viewport viewport)
        {
            // Set the ball's radius
            Radius = 8f;

            // Start the ball in the center of the screen
            Position = new Vector2(viewport.Width / 2f, viewport.Height / 2f);

            // Give the ball an initial velocity
            Velocity = new Vector2(200f, -200f);
        }

        public void HandleWallCollision(Viewport viewport, int wallThickness = 10)
        {
            // Check for collision with the left wall
            if (Position.X - Radius < wallThickness)
            {
                Position.X = wallThickness + Radius;
                Velocity.X *= -1;
            }

            // Check for collision with the right wall
            if (Position.X + Radius > viewport.Width - wallThickness)
            {
                Position.X = viewport.Width - wallThickness - Radius;
                Velocity.X *= -1;
            }

            // Check for collision with the top wall
            if (Position.Y - Radius < wallThickness)
            {
                Position.Y = wallThickness + Radius;
                Velocity.Y *= -1;
            }
        }

        public void Update(GameTime gameTime)
        {
            // Move the ball based on its velocity and the elapsed time since the last update
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Position += Velocity * deltaTime;
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D pixel)
        {
            // Draw the ball as a filled circle using the pixel texture
            // We can draw a circle by drawing a rectangle and using the radius to create a circular mask
            Rectangle rect = new Rectangle(
                (int)(Position.X - Radius),
                (int)(Position.Y - Radius),
                (int)(Radius * 2),
                (int)(Radius * 2)
            );

            spriteBatch.Draw(pixel, rect, Color.Yellow);
        }
    }
}