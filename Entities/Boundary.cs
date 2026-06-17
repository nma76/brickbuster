using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace brickbuster.Entities;

public class Boundary
{
    private readonly Rectangle _left, _right, _top;
    public Boundary(Viewport viewport, int thickness = 10)
    {
        int width = viewport.Width;
        int height = viewport.Height;

        _left = new Rectangle(0, 0, thickness, height);
        _right = new Rectangle(width - thickness, 0, thickness, height);
        _top = new Rectangle(0, 0, width, thickness);
    }

    public void Draw(SpriteBatch spriteBatch, Texture2D pixel)
    {
        spriteBatch.Draw(pixel, _left, Color.Red);
        spriteBatch.Draw(pixel, _right, Color.Red);
        spriteBatch.Draw(pixel, _top, Color.Red);
    }
}
