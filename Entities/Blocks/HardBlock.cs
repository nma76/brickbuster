using Microsoft.Xna.Framework;

namespace brickbuster.Entities.Blocks;

public class HardBlock : BlockBase
{
    public HardBlock(int x, int y) : base(x, y, Color.Silver, 2, 500, PowerUpType.None, BlockType.Hard)
    {
    }
}