using Microsoft.Xna.Framework;

namespace brickbuster.Entities.Blocks;

public class UnbreakableBlock : BlockBase
{
    public UnbreakableBlock(int x, int y) : base(x, y, Color.DarkGoldenrod, 0, 0, PowerUpType.None, BlockType.Unbreakable)
    {
    }
}