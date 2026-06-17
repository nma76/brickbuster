using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using brickbuster.Entities;
using brickbuster.Systems;

namespace brickbuster;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    // Create a pixel texture for drawing rectangles
    private Texture2D _pixel;

    // The level system that manages the blocks in the game
    private LevelSystem _levelSystem;

    // Boundary of the game world
    private Boundary _boundary;

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

    protected override void Initialize()
    {
        // Initialize the level system and create some blocks for testing
        _levelSystem = new LevelSystem();

        // Set the window size
        _graphics.PreferredBackBufferWidth = 1200;
        _graphics.PreferredBackBufferHeight = 720;
        _graphics.ApplyChanges();

        // Initialize the boundary with a thickness of 10 pixels
        int thickness = 10;
        _boundary = new Boundary(GraphicsDevice.Viewport, thickness);

        // Initialize the player's paddle
        _paddle = new Paddle(GraphicsDevice.Viewport);

        // Initialize the ball
        _ball = new Ball(GraphicsDevice.Viewport);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        // Create a 1x1 white texture for drawing rectangles
        _pixel = new Texture2D(GraphicsDevice, 1, 1);
        _pixel.SetData([Color.White]);

        _spriteBatch = new SpriteBatch(GraphicsDevice);
        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        // Move the paddle based on the mouse's X position
        var mouse = Mouse.GetState();
        _paddle.MoveTo(mouse.X);

        // Update the ball's position
        _ball.Update(gameTime);
        // Check for collisions with the walls and bounce the ball if necessary
        _ball.HandleWallCollision(GraphicsDevice.Viewport);
        // Check for collision with the paddle and bounce the ball if necessary
        _ball.HandlePaddleCollision(_paddle.Rect);
        // Check for collision with the blocks and bounce the ball if necessary
        _ball.HandleBlockCollision(_levelSystem.Blocks);

        // Update the level system (remove destroyed blocks etc.)
        _levelSystem.handleBallOutOfBounds(_ball, GraphicsDevice.Viewport);
        _levelSystem.Update();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        // Clear the screen with a black background
        GraphicsDevice.Clear(Color.Black);

        // Begin drawing
        _spriteBatch.Begin();

        // Draw the walls
        _boundary.Draw(_spriteBatch, _pixel);

        // Draw the paddle
        _paddle.Draw(_spriteBatch, _pixel);

        // Draw the ball
        _ball.Draw(_spriteBatch, _pixel);

        // Draw the blocks
        _levelSystem.Draw(_spriteBatch, _pixel);

        _spriteBatch.End();
        base.Draw(gameTime);
    }
}
