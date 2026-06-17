using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using brickbuster.Entities;
using brickbuster.Entities.Blocks;
using brickbuster.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace brickbuster.Systems;

public class LevelSystem
{
    // List of blocks in the current level
    public List<BlockBase> Blocks { get; private set; } = [];

    // Player's current score
    public int Score { get; private set; } = 0;

    public LevelSystem()
    {
        LoadLevel("level0001");
    }

    public void LoadLevel(string levelName)
    {
        // Read level data from file
        var json = File.ReadAllText($"GameData/Levels/{levelName}.json");
        var data = JsonSerializer.Deserialize<LevelData>(json, JsonConfig.JsonOptions);

        // Clear blocks
        Blocks.Clear();

        for (int row = 0; row < data.Grid.Length; row++)
        {
            for (int col = 0; col < data.Grid[row].Length; col++)
            {
                // Get symbol of current position in JSON
                char symbol = data.Grid[row][col];

                // If symbol is . then do nothing
                if (symbol == '.') continue;

                int x = GameConstants.GridStartX + col * (GameConstants.BlockWidth + GameConstants.BlockSpacingX);
                int y = GameConstants.GridStartY + row * (GameConstants.BlockHeight + GameConstants.BlockSpacingY);

                switch (symbol)
                {
                    case 'S':
                        Blocks.Add(new StandardBlock(x, y));
                        break;
                    case 'H':
                        Blocks.Add(new HardBlock(x, y));
                        break;
                    case 'U':
                        Blocks.Add(new UnbreakableBlock(x, y));
                        break;
                }
            }
        }
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
        // Check for destroyed blocks and update the score
        foreach (var block in Blocks)
        {
            if (block.IsDestroyed)
            {
                Score += block.ScoreValue;
            }
        }

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