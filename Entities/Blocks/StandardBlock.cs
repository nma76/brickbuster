using Microsoft.Xna.Framework;

namespace brickbuster.Entities.Blocks;

public class StandardBlock : BlockBase
{
    public StandardBlock(int x, int y) : base(x, y, 80, 30, Color.Blue, 1)
    {
    }
}