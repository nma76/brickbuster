using brickbuster.Config;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace brickbuster.Entities.Blocks;

public class InvisibleBlock : BlockBase
{
    private bool _revealed = false;
    public InvisibleBlock(int x, int y) : base(x, y, Color.Transparent, 2, 750, PowerUpType.None, BlockType.Invisible)
    {
    }

    public override void Draw(SpriteBatch spriteBatch, Texture2D pixel)
    {
        if(_revealed && !IsDestroyed)
        {
            spriteBatch.Draw(pixel, Rect, Color.DarkViolet);
        }
    }

    public override void Hit()
    {
        _revealed = true;
        base.Hit();
    }
}