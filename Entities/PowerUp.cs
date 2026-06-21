using brickbuster.Config;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace brickbuster.Entities;

public class PowerUp
{
    public PowerUpType Type { get; }
    public Rectangle Rect;
    public float Speed = GameConstants.PowerUpSpeed;

    public PowerUp(PowerUpType type, int x, int y)
    {
        Type = type;
        var x2 = x + GameConstants.BlockWidth / 2 - (GameConstants.PowerUpWidth / 2);
        Rect = new Rectangle(x2, y, GameConstants.PowerUpWidth, GameConstants.PowerUpHeight);
    }

    public void Update(GameTime gameTime)
    {
        Rect.Y += (int)(Speed * gameTime.ElapsedGameTime.TotalSeconds);
    }

    public void Draw(SpriteBatch spriteBatch, Texture2D pixel)
    {
        Color color = Type switch 
        {
            PowerUpType.Death => Color.Black,
            PowerUpType.ReverseControls => Color.Red,
            PowerUpType.ExpandPaddle => Color.Blue,
            PowerUpType.ShrinkPaddle => Color.BlueViolet,
            PowerUpType.ExtraLife => Color.Gold,
            _ => Color.White
        };

        spriteBatch.Draw(pixel, Rect, color);
    }
}