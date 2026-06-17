using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace brickbuster.Entities
{
    public class Paddle
    {
        public Rectangle Rect { get; private set; }
        private readonly int _screenWidth;

        public Paddle(Viewport viewport)
        {
            // Store the screen width for later use in constraining the paddle's movement
            _screenWidth = viewport.Width;

            // Size of the paddle
            int width = 120;
            int height = 16;

            // Start the paddle centered at the bottom of the screen
            Rect = new Rectangle(
                viewport.Width / 2 - width / 2,
                viewport.Height - 60,
                width,
                height
            );
        }

        public void MoveTo(int mouseX)
        {
            // Prevent moving past the boundary (assuming a 10 pixel boundary on the left and right)
            int newX = mouseX - Rect.Width / 2;
            if (newX < 10) newX = 10; 
            if (newX + Rect.Width > _screenWidth - 10) newX = _screenWidth - 10 - Rect.Width;

            Rect = new Rectangle(
                newX,
                Rect.Y,
                Rect.Width,
                Rect.Height
            );
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D pixel)
        {
            spriteBatch.Draw(pixel, Rect, Color.White);
        }
    }
}