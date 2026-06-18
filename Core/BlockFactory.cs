using brickbuster.Entities.Blocks;
using Microsoft.Xna.Framework;

public static class BlockFactory
{
    public static BlockBase Create(char symbol, int x, int y)
    {
        return symbol switch
        {
            'G' => new StandardBlock(x, y, Color.Green, 100),
            'B' => new StandardBlock(x, y, Color.Blue, 150),
            'Y' => new StandardBlock(x, y, Color.Yellow, 200),
            'O' => new StandardBlock(x, y, Color.Orange, 250),
            'R' => new StandardBlock(x, y, Color.Red, 300),
            'H' => new HardBlock(x, y),
            'U' => new UnbreakableBlock(x, y),
            _ => null
        };
    }
}