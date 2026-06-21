using System;
using System.Linq;
using brickbuster.Config;
using brickbuster.Core;
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
    public int CurrentLevel { get; private set; } = 10;

    // Holds instances of sub-systems
    public LifeSystem LifeSystem { get; private set; }
    public ScoreSystem ScoreSystem { get; private set; }
    public AudioSystem AudioSystem { get; private set; }
    public DifficultySystem DifficultySysem { get; private set; }
    public PowerUpSystem PowerUpSystem { get; private set;}
    public BlockSystem BlockSystem { get; private set; }

    //Events
    public event Action<LevelData> OnLevelChanged;
    public event Action<bool> OnGameCompleted;

    // Keep track of if level is cleared from breakable blocks
    public bool IsLevelCleared => CurrentLevelData.Blocks.Where(b => b.Type != BlockType.Unbreakable).All(b => b.IsDestroyed);

    // Keep track on the paddle hit count
    private int _paddleHits = 0;

    public LevelSystem(LifeSystem lifeSystem, ScoreSystem scoreSystem, AudioSystem audioSystem, DifficultySystem difficultySystem, PowerUpSystem powerUpSystem, BlockSystem blockSystem)
    {
        LifeSystem = lifeSystem;
        ScoreSystem = scoreSystem;
        AudioSystem = audioSystem;
        DifficultySysem = difficultySystem;
        PowerUpSystem = powerUpSystem;
        BlockSystem = blockSystem;
    }

    public void LoadLevel(string level)
    {
        CurrentLevelData = LevelLoader.Load(level);
        OnLevelChanged?.Invoke(CurrentLevelData);
    }

    public void HandleBallOutOfBounds(Ball ball, Paddle paddle, Viewport viewport)
    {
        if (!ball.IsOutOfBounds(viewport))
        {
            return;
        }

        // Decrease life
        if (LifeSystem.LoseLife())
        {
            HandleGameOver(ball, paddle);
        }

        // Attach ball to paddle
        paddle.Restore();
        ball.AttachToPaddle(paddle.Rect);

        // Reset ball speed
        DifficultySysem.ResetBallSpeed(ball);
    }
    public void RegisterPaddleHit()
    {
        _paddleHits++;
    }
    public int GetPaddleHits()
    {
        return _paddleHits;
    }

    private void HandleGameOver(Ball ball, Paddle paddle)
    {
        ScoreSystem.Reset();
        LifeSystem.Reset();
        CurrentLevel = 1;
        LoadLevel(CurrentLevel.ToString("0000"));

        // Remove any PowerUp still onscreen
        PowerUpSystem.ClearActive();

        // Restore paddle width and attach ball to paddle
        paddle.Restore();
        ball.AttachToPaddle(paddle.Rect);

        // Reset ball speed
        DifficultySysem.ResetBallSpeed(ball);
    }

    private void HandleGameCompleted()
    {
        OnGameCompleted?.Invoke(true);
    }

    private void HandleBlocks()
    {
        BlockSystem.Blocks = CurrentLevelData.Blocks;
        BlockSystem.Update();
    }
    private void HandlePowerUps(GameTime gameTime, Ball ball, Paddle paddle)
    {
        // Update power-ups
        PowerUpSystem.Update(gameTime, paddle);
    }
    private void HandleLevelCompletion(Ball ball, Paddle paddle)
    {
        // If level i snot comleted, return early
        if (!IsLevelCleared)
        {
            return;
        }

        // Add bonus based on paddle hits (fewer is better)
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

        // Remove any PowerUp still onscreen
        PowerUpSystem.ClearActive();

        // Restore paddle width and attach ball to paddle
        paddle.Restore();
        ball.AttachToPaddle(paddle.Rect);

        // Reset ball speed
        DifficultySysem.ResetBallSpeed(ball);
    }

    private void HandleIncreaeDifficulty(Ball ball)
    {
        // Increase ball speed gradually
        DifficultySysem.UpdateBallspeed(ball, _paddleHits);
    }

    public void Update(GameTime gameTime, Ball ball, Paddle paddle, Viewport viewport)
    {
        HandleBallOutOfBounds(ball, paddle, viewport);
        HandleBlocks();
        HandlePowerUps(gameTime, ball, paddle);
        HandleLevelCompletion(ball, paddle);
        HandleIncreaeDifficulty(ball);
    }

    public void Draw(SpriteBatch spriteBatch, Texture2D pixel)
    {
        // Iterate all blocks
        foreach (var block in CurrentLevelData.Blocks)
        {
            // Draw the block
            block.Draw(spriteBatch, pixel);
        }

        PowerUpSystem.Draw(spriteBatch, pixel);
    }
}