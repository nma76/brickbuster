using System.Collections.Generic;
using brickbuster.Entities.Blocks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace brickbuster.Systems;

public class LevelSystem
{
    public List<BlockBase> Blocks { get; private set; } = [];

    public LevelSystem()
    {
        // For testing only!!
        Blocks.Add(new StandardBlock(100, 100));
        Blocks.Add(new StandardBlock(200, 100));
        Blocks.Add(new StandardBlock(300, 100));
    }

    public void Update()
    {
        // Remove destroyed blocks
        Blocks.RemoveAll(b => b.IsDestroyed);
    }

    public void Draw(SpriteBatch spriteBatch, Texture2D pixel)
    {
        foreach (var block in Blocks)
        {
            block.Draw(spriteBatch, pixel);
        }
    }
}