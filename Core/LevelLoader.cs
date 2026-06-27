using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using brickbuster.Config;
using brickbuster.Entities.Blocks;
using brickbuster.Models;
using brickbuster.Models.Json;

namespace brickbuster.Core;

public static class LevelLoader
{
    public static LevelData Load(string levelId)
    {
        var blocks = new List<BlockBase>();

        // Read level data from file
        var json = File.ReadAllText($"GameData/Levels/level{levelId}.json");
        var data = JsonSerializer.Deserialize<LevelDefinition>(json, JsonConfig.JsonOptions);

        for (int row = 0; row < data.Grid.Length; row++)
        {
            for (int col = 0; col < data.Grid[row].Length; col++)
            {
                // Get symbol of current position in JSON
                char symbol = data.Grid[row][col];

                // If symbol is . then do nothing
                if (symbol == '.')
                {
                    continue;
                }

                // Get position of the block
                int x = GameConstants.GridStartX + col * (GameConstants.BlockWidth + GameConstants.BlockSpacingX);
                int y = GameConstants.GridStartY + row * (GameConstants.BlockHeight + GameConstants.BlockSpacingY);

                // check data for block type
                var block = BlockFactory.Create(symbol, x, y);

                if (block != null)
                {
                    // If block isn't unbreakable, it can hold a power-up
                    if (block.Type != BlockType.Unbreakable && block.Type != BlockType.Ghost)
                    {
                        block.PowerUp = PowerUpRandomizer.Roll();
                    }

                    // Add block
                    blocks.Add(block);
                }
            }
        }
        
        return new LevelData
        {
            Name = data.Name,
            Background = data.Background,
            IsBossLevel = data.IsBossLevel,
            IsFinal = data.IsFinal,
            Blocks = blocks,
            Source = data
        };
    }
}