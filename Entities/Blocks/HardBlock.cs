using Microsoft.Xna.Framework;

namespace brickbuster.Entities.Blocks;

public class HardBlock : BlockBase
{
    public HardBlock(int x, int y) : base(x, y, 80, 30, Color.Blue, 2)
    {
    }
}