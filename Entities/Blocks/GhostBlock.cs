using brickbuster.Config;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace brickbuster.Entities.Blocks;

public class GhostBlock : BlockBase
{
    public GhostBlock(int x, int y) : base(x, y, Color.White, 3, 1000, PowerUpType.None, BlockType.Ghost)
    {
    }

    public override void Draw(SpriteBatch spriteBatch, Texture2D pixel)
    {
        if (!IsDestroyed)
        {
            switch (HitPoints)
            {
                case 3:
                    spriteBatch.Draw(pixel, Rect, Color.White * 1f);
                    break;
                case 2:
                    spriteBatch.Draw(pixel, Rect, Color.White * 0.15f);
                    break;
                case 1:
                    spriteBatch.Draw(pixel, Rect, Color.White * 0.03f);
                    break;
            }
        }
    }
}
