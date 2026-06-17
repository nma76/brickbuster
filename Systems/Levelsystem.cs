using System.Collections.Generic;
using brickbuster.Entities;
using brickbuster.Entities.Blocks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace brickbuster.Systems;

public class LevelSystem
{
    public List<BlockBase> Blocks { get; private set; } = [];

    public LevelSystem()
    {
        // For testing only!! Replace this with data loader from json file or something similar later on
        Blocks.Add(new StandardBlock(100, 100));
        Blocks.Add(new StandardBlock(200, 100));
        Blocks.Add(new StandardBlock(300, 100));
        Blocks.Add(new StandardBlock(400, 100));
        Blocks.Add(new StandardBlock(500, 100));
        Blocks.Add(new StandardBlock(600, 100));
        Blocks.Add(new StandardBlock(700, 100));
        Blocks.Add(new StandardBlock(800, 100));
        Blocks.Add(new StandardBlock(900, 100));
        Blocks.Add(new StandardBlock(1000, 100));

        Blocks.Add(new HardBlock(100, 140));
        Blocks.Add(new HardBlock(200, 140));
        Blocks.Add(new HardBlock(300, 140));
        Blocks.Add(new HardBlock(400, 140));
        Blocks.Add(new HardBlock(500, 140));
        Blocks.Add(new HardBlock(600, 140));
        Blocks.Add(new HardBlock(700, 140));
        Blocks.Add(new HardBlock(800, 140));
        Blocks.Add(new HardBlock(900, 140));
        Blocks.Add(new HardBlock(1000, 140));
    }

    public void HandleBallOutOfBounds(Ball ball, Paddle paddle, Viewport viewport)
    {
        if (ball.IsOutOfBounds(viewport))
        {
            ball.AttachToPaddle(paddle.Rect);

            // Todo: Handle player losing a life, resetting the level, etc.    
        }
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