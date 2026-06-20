using brickbuster.Config;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace brickbuster.Entities.Blocks;

public class BlockBase
{
    public Rectangle Rect { get; private set; }
    public int HitPoints { get; protected set; }
    public Color Color { get; private set; }
    public int ScoreValue { get; private set; }
    public bool IsDestroyed => Type != BlockType.Unbreakable && HitPoints <= 0;
    public PowerUpType PowerUp { get; set; }
    public BlockType Type { get; private set; }

    public BlockBase(int x, int y, Color color, int hitPoints, int scoreValue, PowerUpType powerUp, BlockType type)
    {
        Rect = new Rectangle(x, y, GameConstants.BlockWidth, GameConstants.BlockHeight);
        Color = color;
        HitPoints = hitPoints;
        ScoreValue = scoreValue;
        PowerUp = powerUp;
        Type = type;
    }

    public virtual void Hit()
    {
        if (Type != BlockType.Unbreakable)
        {
            HitPoints--;
        }
    }

    public virtual void Draw(SpriteBatch spriteBatch, Texture2D pixel)
    {
        if (!IsDestroyed)
        {
            spriteBatch.Draw(pixel, Rect, Color);
        }

        // If debug is true, visualize where power-ups are located
        if (GameConstants.Debug)
        {
            if (PowerUp != PowerUpType.None)
            {
                var marker = new Rectangle(
                    Rect.Center.X - 5,
                    Rect.Center.Y - 5,
                    10,
                    10
                );
                spriteBatch.Draw(pixel, marker, Color.Gold);
            }
        }
    }
}