using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace brickbuster.Entities.Blocks;

public class BlockBase
{
    public Rectangle Rect { get; private set; }
    public int HitPoints { get; private set; }
    public Color Color { get; private set; }
    public int ScoreValue { get; private set; }
    public bool IsDestroyed => !IsUnbreakable && HitPoints <= 0;
    public bool IsUnbreakable { get; private set; }
    public PowerUpType PowerUp { get; set; }

    public BlockBase(int x, int y, Color color, int hitPoints = 1, int scoreValue = 100, bool isUnbreakable = false, PowerUpType powerUp = PowerUpType.None)
    {
        Rect = new Rectangle(x, y, GameConstants.BlockWidth, GameConstants.BlockHeight);
        Color = color;
        HitPoints = hitPoints;
        ScoreValue = scoreValue;
        IsUnbreakable = isUnbreakable;
        PowerUp = powerUp;
    }

    public void Hit()
    {
        if (!IsUnbreakable)
        {
            HitPoints--;
        }
    }

    public void Draw(SpriteBatch spriteBatch, Texture2D pixel)
    {
        if (!IsDestroyed)
        {
            spriteBatch.Draw(pixel, Rect, Color);
        }
    }
}