using brickbuster.Entities.Blocks;
using Microsoft.Xna.Framework;

namespace brickbuster.Core;

public static class BlockFactory
{
    public static BlockBase Create(char symbol, int x, int y)
    {
        return symbol switch
        {
            'G' => new StandardBlock(x, y, Color.Green, 100),
            'L' => new StandardBlock(x, y, Color.LimeGreen, 125),
            'B' => new StandardBlock(x, y, Color.Blue, 150),
            'Y' => new StandardBlock(x, y, Color.Yellow, 175),
            'O' => new StandardBlock(x, y, Color.Orange, 200),
            'R' => new StandardBlock(x, y, Color.Red, 225),
            'A' => new StandardBlock(x, y, Color.Black, 250),
            'H' => new HardBlock(x, y),
            'I' => new InvisibleBlock(x,y),
            'X' => new GhostBlock(x,y),
            'U' => new UnbreakableBlock(x, y),
            _ => null
        };
    }
}