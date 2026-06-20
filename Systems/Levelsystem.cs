using System;
using System.Collections.Generic;
using System.Linq;
using brickbuster.Config;
using brickbuster.Core;
using brickbuster.Entities;
using brickbuster.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
    public AudioSystem AudioSystem { get; private set; }

    //Events
    public event Action<LevelData> OnLevelChanged;
    public event Action<bool> OnGameCompleted;

    // Keep track of if level is cleared from breakable blocks
    public bool IsLevelCleared => CurrentLevelData.Blocks.Where(b => b.Type != BlockType.Unbreakable).All(b => b.IsDestroyed);

    // Keep track on the paddle hit count
    private int _paddleHits = 0;

    private List<PowerUp> _activePowerUps = [];
    private List<PowerUp> _removePowerUps = [];

    public LevelSystem(LifeSystem lifeSystem, ScoreSystem scoreSystem, AudioSystem audioSystem)
    {
        LifeSystem = lifeSystem;
        ScoreSystem = scoreSystem;
        AudioSystem = audioSystem;

        LoadLevel(CurrentLevel.ToString("0000"));
    }

    public void LoadLevel(string level)
    {
        CurrentLevelData = LevelLoader.Load(level);
        OnLevelChanged?.Invoke(CurrentLevelData);
    }

    public void HandleLevelComplete()
    {
        // Add bonus based on padle hits (fewer is better)
        ScoreSystem.AddBonus(_paddleHits);
        _paddleHits = 0;

        // If this was the last level, handle game completed
        if (CurrentLevelData.IsFinal)
        {
            HandleGameCompleted();
            return;
        }

        // Load the next level
        CurrentLevel++;
        LoadLevel(CurrentLevel.ToString("0000"));
    }

    public bool HandleBallOutOfBounds(Ball ball, Viewport viewport)
    {
        if (ball.IsOutOfBounds(viewport))
        {
            return true;
        }
        return false;
    }

    public void RegisterPaddleHit()
    {
        _paddleHits++;
    }

    private void HandleGameOver()
    {
        ScoreSystem.Reset();
        LifeSystem.Reset();
        CurrentLevel = 1;
        LoadLevel(CurrentLevel.ToString("0000"));
    }

    private void HandleGameCompleted()
    {
        OnGameCompleted?.Invoke(true);
    }

    public void Update(GameTime gameTime, Ball ball, Paddle paddle, Viewport viewport)
    {
        if (HandleBallOutOfBounds(ball, viewport))
        {
            if (LifeSystem.LoseLife())
            {
                HandleGameOver();
            }

            ball.AttachToPaddle(paddle.Rect);
        }

        // Check for destroyed blocks and update the score
        // TODO: Refactor this!!
        foreach (var block in CurrentLevelData.Blocks)
        {
            if (block.IsDestroyed)
            {
                ScoreSystem.Add(block.ScoreValue);

                if (block.PowerUp != PowerUpType.None)
                {
                    _activePowerUps.Add(new PowerUp(block.PowerUp, block.Rect.X, block.Rect.Y));
                }
            }
        }

        // Update power-ups
        foreach (var powerUp in _activePowerUps)
        {
            powerUp.Update(gameTime);

            if (powerUp.Rect.Intersects(paddle.Rect))
            {
                switch (powerUp.Type)
                {
                    case PowerUpType.ExpandPaddle:
                        paddle.Expand();
                        break;
                    case PowerUpType.ShrinkPaddle:
                        paddle.Shrink();
                        break;
                    case PowerUpType.ExtraLife:
                        LifeSystem.AddLife();
                        break;
                }

                AudioSystem.PlayPowerUp();
                _removePowerUps.Add(powerUp);
            }
        }
        _activePowerUps.RemoveAll(_removePowerUps.Contains);


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

        foreach (var powerUp in _activePowerUps)
        {
            powerUp.Draw(spriteBatch, pixel);
        }
    }
}