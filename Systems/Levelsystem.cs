using System;
using System.Linq;
using brickbuster.Entities;
using brickbuster.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace brickbuster.Systems;

public class LevelSystem
{
    // Holds all meta and design for the current level
    public LevelData CurrentLevelData { get; private set; }

    // Keeps track of the current level
    public int CurrentLevel { get; private set; } = 1;

    // Holds instances of sub-systems
    public LifeSystem LifeSystem { get; private set; }
    public ScoreSystem ScoreSystem { get; private set; }

    public event Action<LevelData> OnLevelChanged;

    // Keep track of if level is cleared from breakable blocks
    public bool IsLevelCleared => CurrentLevelData.Blocks.Where(b => b.Type != BlockType.Unbreakable).All(b => b.IsDestroyed);

    public LevelSystem(LifeSystem lifeSystem, ScoreSystem scoreSystem)
    {
        LifeSystem = lifeSystem;
        ScoreSystem = scoreSystem;
        LoadLevel(CurrentLevel.ToString("0000"));
    }

    public void LoadLevel(string level)
    {
        CurrentLevelData = LevelLoader.Load(level);
        OnLevelChanged?.Invoke(CurrentLevelData);
    }

    public void HandleLevelComplete()
    {
        CurrentLevel++;
        LoadLevel(CurrentLevel.ToString("0000"));
    }

    public bool HandleBallOutOfBounds(Ball ball, Paddle paddle, Viewport viewport)
    {
        if (ball.IsOutOfBounds(viewport))
        {
            return true;
        }
        return false;
    }

    private void HandleGameOver()
    {
        ScoreSystem.Reset();
        LifeSystem.Reset();
        CurrentLevel = 1;
        LoadLevel(CurrentLevel.ToString("0000"));
    }

    public void Update(Ball ball, Paddle paddle, Viewport viewport)
    {
        if (HandleBallOutOfBounds(ball, paddle, viewport))
        {
            if (LifeSystem.LoseLife())
            {
                HandleGameOver();
            }

            ball.AttachToPaddle(paddle.Rect);
        }

        // Check for destroyed blocks and update the score
        // TODO: Move this to HandleScore
        foreach (var block in CurrentLevelData.Blocks)
        {
            if (block.IsDestroyed)
            {
                ScoreSystem.Add(block.ScoreValue);
            }
        }

        // Remove destroyed blocks
        CurrentLevelData.Blocks.RemoveAll(b => b.IsDestroyed);

        if (IsLevelCleared)
        {
            HandleLevelComplete();
            ball.AttachToPaddle(paddle.Rect);
        }
    }

    public void Draw(SpriteBatch spriteBatch, Texture2D pixel)
    {
        // Iterate all blocks
        foreach (var block in CurrentLevelData.Blocks)
        {
            // Draw the block
            block.Draw(spriteBatch, pixel);

            // If debug is true, visualize where power-ups are located
            if (GameConstants.Debug)
            {
                if (block.PowerUp != PowerUpType.None)
                {
                    var marker = new Rectangle(
                        block.Rect.Center.X - 5,
                        block.Rect.Center.Y - 5,
                        10,
                        10
                    );
                    spriteBatch.Draw(pixel, marker, Color.Gold);
                }
            }
        }
    }
}