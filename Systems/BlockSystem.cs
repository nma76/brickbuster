using System.Collections.Generic;
using brickbuster.Config;
using brickbuster.Entities;
using brickbuster.Entities.Blocks;

namespace brickbuster.Systems;

public class BlockSystem
{
    public List<BlockBase> Blocks { get; set; }

    private readonly PowerUpSystem _powerUpSystem;
    private readonly ScoreSystem _scoreSystem;

    public BlockSystem(PowerUpSystem powerUpSystem, ScoreSystem scoreSystem)
    {
        _powerUpSystem = powerUpSystem;
        _scoreSystem = scoreSystem;
    }

    public void Update()
    {
        foreach (var block in Blocks)
        {
            if (block.IsDestroyed)
            {
                _scoreSystem.Add(block.ScoreValue);

                if (block.PowerUp != PowerUpType.None)
                {
                    _powerUpSystem.Add(new PowerUp(block.PowerUp, block.Rect.X, block.Rect.Y));
                }
            }
        }

        // Remove destroyed blocks
        Blocks.RemoveAll(b => b.IsDestroyed);

    }
}