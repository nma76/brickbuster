using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using brickbuster.Config;
using brickbuster.Entities;
using brickbuster.Systems;
using brickbuster.Models;

namespace brickbuster;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    // Create a pixel texture for drawing rectangles
    private Texture2D _pixel;

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

    // The player's paddle
    private Paddle _paddle;

    // The ball that will bounce around the screen
    private Ball _ball;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
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

        // Initialize background system
        _backgroundSystem = new BackgroundSystem(GraphicsDevice);

        // Initialize audio system
        _audioSystem = new AudioSystem(Content);
        _audioSystem.PlayMusic();

        // Initialize life system to handle player lifes
        _lifeSystem = new LifeSystem();

        // Initalize score system to keep track of score
        _scoreSystem = new ScoreSystem();

        // Initialize the level system and create some blocks for testing
        _levelSystem = new LevelSystem(_lifeSystem, _scoreSystem, _audioSystem);
        _levelSystem.OnLevelChanged += HandleLevelChanged;
        _levelSystem.OnGameCompleted += HandleGameCompleted;

        // Initialize the player's paddle
        _paddle = new Paddle(GraphicsDevice.Viewport);

        // Initialize the ball
        _ball = new Ball(GraphicsDevice.Viewport, _audioSystem);
        _ball.AttachToPaddle(_paddle.Rect);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        // Create a 1x1 white texture for drawing
        _pixel = new Texture2D(GraphicsDevice, 1, 1);
        _pixel.SetData([Color.White]);

        // Initialize sprite batch
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // Load background for level 1
        var backgroundTexture = Content.Load<Texture2D>("Backgrounds/0001");
        _backgroundSystem.LoadBackground(backgroundTexture);
    }

    protected override void Update(GameTime gameTime)
    {
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
            _ball.Update(gameTime);
            // Check for collisions with the walls and bounce the ball if necessary
            _ball.HandleWallCollision(GraphicsDevice.Viewport, 40);
            // Check for collision with the paddle and bounce the ball if necessary
            _ball.HandlePaddleCollision(_paddle.Rect);
            // Check for collision with the blocks and bounce the ball if necessary
            _ball.HandleBlockCollision(_levelSystem.CurrentLevelData.Blocks);
        }

        // Update the level system (remove destroyed blocks etc.)
        _levelSystem.Update(_ball, _paddle, GraphicsDevice.Viewport);

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

        // show current score
        _spriteBatch.DrawString(Content.Load<SpriteFont>("DefaultFont"), $"Score: {_scoreSystem.Score}", new Vector2(150, 5), Color.White);

        // Show current lifes
        _spriteBatch.DrawString(Content.Load<SpriteFont>("DefaultFont"), $"Lifes: {_lifeSystem.Lifes}", new Vector2(50, 5), Color.White);

        _spriteBatch.End();
        base.Draw(gameTime);
    }
}
