using Microsoft.Xna.Framework;

namespace brickbuster.Entities.Blocks;

public class StandardBlock : BlockBase
{
    public StandardBlock(int x, int y) : base(x, y, Color.Green, 1)
    {
    }
}