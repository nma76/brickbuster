using System;
using brickbuster.Config;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace brickbuster.Entities;

public class Paddle
{
    public Rectangle Rect { get; private set; }
    private readonly int _screenWidth;
    private int _width;
    private readonly int _height;

    public Paddle(Viewport viewport)
    {
        // Paddle size
        _width = GameConstants.PaddleWidth;
        _height = GameConstants.PaddleHeight;

        // Store the screen width for later use in constraining the paddle's movement
        _screenWidth = viewport.Width;

        // Start the paddle centered at the bottom of the screen
        Rect = new Rectangle(
            viewport.Width / 2 - _width / 2,
            viewport.Height - 60,
            _width,
            _height
        );
    }

    private void UpdateRect()
    {
        int x = Rect.X;

        // ADjust so paddle never grows outside the walls
        if (x + _width > _screenWidth - GameConstants.BorderThickness)
            x = _screenWidth - GameConstants.BorderThickness - _width;

        Rect = new Rectangle(x, Rect.Y, _width, _height);
    }

    public void Expand()
    {
        // Never grow above max
        _width = Math.Min(_width + 20, GameConstants.PaddleMaxWidth);
        UpdateRect();
    }
    public void Shrink()
    {
        // Never grow below min
        _width = Math.Max(_width - 20, GameConstants.PaddleMinWidth);
        UpdateRect();
    }
    public void Restore()
    {
        _width = GameConstants.PaddleWidth;
        UpdateRect();
    }

    public void MoveTo(int mouseX)
    {
        // Prevent moving past the boundary (assuming a 10 pixel boundary on the left and right)
        int newX = mouseX - Rect.Width / 2;
        if (newX < GameConstants.BorderThickness) newX = GameConstants.BorderThickness;
        if (newX + Rect.Width > _screenWidth - GameConstants.BorderThickness) newX = _screenWidth - GameConstants.BorderThickness - Rect.Width;

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
