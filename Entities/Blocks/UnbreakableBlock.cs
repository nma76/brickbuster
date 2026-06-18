using Microsoft.Xna.Framework;

namespace brickbuster.Entities.Blocks;

public class UnbreakableBlock : BlockBase
{
    public UnbreakableBlock(int x, int y) : base(x, y, Color.DarkGray, 0, 0, true, PowerUpType.None, BlockType.Unbreakable)
    {
    }
}