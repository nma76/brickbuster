using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace brickbuster.Systems;

public class BackgroundSystem
{
    private Texture2D _background;
    private readonly GraphicsDevice _graphicsDevice;

    public BackgroundSystem(GraphicsDevice graphicsDevice)
    {
        _graphicsDevice = graphicsDevice;
    }

    public void LoadBackground(Texture2D texture)
    {
        _background = texture;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (_background == null)
        {
            return;
        }

        spriteBatch.Draw(_background, new Rectangle(0, 0, 1200, 720), Color.White * 0.6f);
    }
}