using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace brickbuster;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    // Create a pixel texture for drawing rectangles
    Texture2D _pixel;

    // Boundary of the game world
    Rectangle leftWall, rightWall, topWall;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // Set the window size
        _graphics.PreferredBackBufferWidth = 1200;
        _graphics.PreferredBackBufferHeight = 720;
        _graphics.ApplyChanges();

        // Define the walls of the game world
        int thickness = 10;
        int width = GraphicsDevice.Viewport.Width;
        int height = GraphicsDevice.Viewport.Height;

        leftWall = new Rectangle(0, 0, thickness, height);
        rightWall = new Rectangle(width - thickness, 0, thickness, height);
        topWall = new Rectangle(0, 0, width, thickness);

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
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        // Clear the screen with a black background
        GraphicsDevice.Clear(Color.Black);

        // Begin drawing
        _spriteBatch.Begin();

        // Draw the walls
        _spriteBatch.Draw(_pixel, leftWall, Color.Red);
        _spriteBatch.Draw(_pixel, rightWall, Color.Red);
        _spriteBatch.Draw(_pixel, topWall, Color.Red);


        _spriteBatch.End();
        base.Draw(gameTime);
    }
}
