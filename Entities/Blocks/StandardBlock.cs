using Microsoft.Xna.Framework;

namespace brickbuster.Entities.Blocks;

public class StandardBlock : BlockBase
{
    public StandardBlock(int x, int y, Color color, int scoreValue) : base(x, y, color, 1, scoreValue)
    {
    }
}