using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using brickbuster.Config;
using brickbuster.Entities;
using brickbuster.Systems;
using brickbuster.Models;
using brickbuster.uI;

namespace brickbuster;

public class Game1 : Game
{
    // Editor Mode variables
    private bool _editorMode = false;
    private EditorSystem _editorSystem;
    private EditorOverlay _editorOverlay;

    private GraphicsDeviceManager _graphics;

    private SpriteBatch _spriteBatch;

    // Create a pixel texture for drawing rectangles
    private Texture2D _pixel;

    // Holds the default font
    private SpriteFont _defaultFont;

    // Handles all blocks
    private BlockSystem _blockSystem;

    // Handles the backgrounds
    private BackgroundSystem _backgroundSystem;

    // Handles music and sfx
    private AudioSystem _audioSystem;

    // The level system that manages the blocks in the game
    private LevelSystem _levelSystem;

    // LifeSystem handles lifes
    private LifeSystem _lifeSystem;

    // Handles the score
    private ScoreSystem _scoreSystem;

    // Handels difficulty
    private DifficultySystem _difficultySystem;

    // Handles power-ups
    private PowerUpSystem _powerUpSystem;

    // The player's paddle
    private Paddle _paddle;

    // The ball that will bounce around the screen
    private Ball _ball;

    public Game1(bool editorMode)
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        _editorMode = editorMode;
    }

    private void HandleLevelChanged(LevelData levelData)
    {
        // load background
        var backgroundTexture = Content.Load<Texture2D>($"Backgrounds/{levelData.Background}");
        _backgroundSystem.LoadBackground(backgroundTexture);

        // Set background music depending on level type
        _audioSystem.SwitchMusic(levelData.IsBossLevel);

        // Set score multiplier depending on level type
        var scoremultiplier = levelData.IsBossLevel ? GameConstants.BossMultiplier : GameConstants.NormalMultiplier;
        _scoreSystem.SetMultiplier(scoremultiplier);
    }

    private void HandleGameCompleted(bool completed)
    {
        Exit();
    }

    protected override void Initialize()
    {
        // Set the window size
        _graphics.PreferredBackBufferWidth = 1200;
        _graphics.PreferredBackBufferHeight = 720;
        _graphics.ApplyChanges();

        // Editor Mode overlay
        _editorSystem = new EditorSystem();
        _editorOverlay = new EditorOverlay(_editorSystem);

        // Initialize background system
        _backgroundSystem = new BackgroundSystem(GraphicsDevice);

        // Initialize audio system
        _audioSystem = new AudioSystem(Content);
        _audioSystem.PlayMusic();

        // Initialize life system to handle player lifes
        _lifeSystem = new LifeSystem();

        // Initalize score system to keep track of score
        _scoreSystem = new ScoreSystem();

        // Initialize Power-Up System
        _powerUpSystem = new PowerUpSystem(_audioSystem, _lifeSystem);

        // Initialize Block system
        _blockSystem = new BlockSystem(_powerUpSystem, _scoreSystem);

        // Initialize difficulty system
        _difficultySystem = new DifficultySystem(_blockSystem);

        // Initialize the level system and create some blocks for testing
        _levelSystem = new LevelSystem(_lifeSystem, _scoreSystem, _audioSystem, _difficultySystem, _powerUpSystem, _blockSystem);
        _levelSystem.OnLevelChanged += HandleLevelChanged;
        _levelSystem.OnGameCompleted += HandleGameCompleted;
        _levelSystem.LoadLevel(_levelSystem.CurrentLevel.ToString("0000"));

        // Initialize the player's paddle
        _paddle = new Paddle(GraphicsDevice.Viewport);

        // Initialize the ball
        _ball = new Ball(GraphicsDevice.Viewport, _audioSystem);
        _ball.AttachToPaddle(_paddle.Rect);
        _ball.OnPaddleHit += _levelSystem.RegisterPaddleHit;

        base.Initialize();
    }

    protected override void LoadContent()
    {
        // Create a 1x1 white texture for drawing
        _pixel = new Texture2D(GraphicsDevice, 1, 1);
        _pixel.SetData([Color.White]);

        // Initialize sprite batch
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // Load the default font
        _defaultFont = Content.Load<SpriteFont>("DefaultFont");
    }

    protected override void Update(GameTime gameTime)
    {
        // Editor mode
        if(_editorMode)
        {
            _editorSystem.Update(gameTime);
            return;
        }

        // Move the paddle based on the mouse's X position
        var mouse = Mouse.GetState();
        _paddle.MoveTo(mouse.X);

        if (!_ball.IsLaunched)
        {
            _ball.AttachToPaddle(_paddle.Rect);

            if (mouse.LeftButton == ButtonState.Pressed)
            {
                _ball.Launch();
            }
        }
        else
        {
            // Update the ball's position
            _ball.UpdateDeltaTime(gameTime);
            // Check for collisions with the walls and bounce the ball if necessary
            _ball.HandleWallCollision(GraphicsDevice.Viewport);
            // Check for collision with the paddle and bounce the ball if necessary
            _ball.HandlePaddleCollision(_paddle.Rect);
            // Check for collision with the blocks and bounce the ball if necessary
            _ball.HandleBlockCollision(_levelSystem.CurrentLevelData.Blocks);
        }

        // Update the level system (remove destroyed blocks etc.)
        _levelSystem.Update(gameTime, _ball, _paddle, GraphicsDevice.Viewport);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        // Clear the screen with a black background
        GraphicsDevice.Clear(Color.Black);

        // Begin drawing
        _spriteBatch.Begin();

        // Draw background
        _backgroundSystem.Draw(_spriteBatch);

        // Draw the paddle
        _paddle.Draw(_spriteBatch, _pixel);

        // Draw the ball
        _ball.Draw(_spriteBatch, _pixel);

        // Draw the blocks
        _levelSystem.Draw(_spriteBatch, _pixel);

        // Show current lifes
        _spriteBatch.DrawString(_defaultFont, $"Lifes: {_lifeSystem.Lifes}", new Vector2(50, 5), Color.White);

        // show current score
        _spriteBatch.DrawString(_defaultFont, $"Score: {_scoreSystem.Score}", new Vector2(150, 5), Color.White);

        // Editor Mode
        if(_editorMode)
        {
            _editorOverlay.Draw(_spriteBatch, _pixel);
        }

        // Print debug information 
        if (GameConstants.Debug)
        {
            // Show current ball speed
            _spriteBatch.DrawString(_defaultFont, $"Ball Speed: {_ball.Velocity.Length():F0}", new Vector2(50, 50), Color.White);

            // Show paddle hit count
            _spriteBatch.DrawString(_defaultFont, $"Paddle Hits: {_levelSystem.GetPaddleHits()}", new Vector2(50, 70), Color.White);

            // Show remaining blocks
            _spriteBatch.DrawString(_defaultFont, $"Block remaining: {_blockSystem.GetRemaining()}", new Vector2(50, 90), Color.White);

            // Show end game speed boost
            _spriteBatch.DrawString(_defaultFont, $"End-game speed: {_difficultySystem.IsEndgameSpeed}", new Vector2(50, 110), Color.White);
        }

        _spriteBatch.End();
        base.Draw(gameTime);
    }
}
