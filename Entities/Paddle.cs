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
    private bool _isReversed = false;
    private int _lastMouseX;

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
        // Never shrink below min
        _width = Math.Max(_width - 20, GameConstants.PaddleMinWidth);
        UpdateRect();
    }
    public void ReverseControls()
    {
        _isReversed = !_isReversed;
    }
    public void RestoreControls()
    {
        _isReversed = false;
    }
    public void Restore()
    {
        _width = GameConstants.PaddleWidth;
        RestoreControls();
        UpdateRect();
    }

    public void MoveTo(int mouseX)
    {
        // Calculate movement
        int delta = mouseX - _lastMouseX;

        // If reversed, invert movement
        if(_isReversed)
        {
            delta = -delta;
        }

        // Use delta for new position
        int newX = Rect.X + delta;

        // Prevent moving past the boundary (assuming a 10 pixel boundary on the left and right)
        if (newX < GameConstants.BorderThickness) newX = GameConstants.BorderThickness;
        if (newX + Rect.Width > _screenWidth - GameConstants.BorderThickness) newX = _screenWidth - GameConstants.BorderThickness - Rect.Width;

        // Update paddle
        Rect = new Rectangle(
            newX,
            Rect.Y,
            Rect.Width,
            Rect.Height
        );

        // Save last position
        _lastMouseX = mouseX;
    }

    public void Draw(SpriteBatch spriteBatch, Texture2D pixel)
    {
        spriteBatch.Draw(pixel, Rect, Color.White);
    }
}
