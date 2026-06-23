using brickbuster.Config;
using brickbuster.Core;
using brickbuster.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace brickbuster.uI;

public class EditorOverlay
{
    public bool IsVisible { get; set; }

    private EditorSystem _editorSystem;

    public EditorOverlay(EditorSystem editorSystem)
    {
        _editorSystem = editorSystem;
    }

    public void Update(GameTime gameTime)
    {

    }

    public void Draw(SpriteBatch spriteBatch, Texture2D pixel)
    {
        for (int row = 0; row < 14; row++)
        {
            for (int column = 0; column < 14; column++)
            {
                Rectangle rect = GridHelper.GetGridCellRectangle(column, row);
                DrawRectangleOutline(spriteBatch, pixel, rect, Color.Yellow);
            }
        }
    }

    private void DrawRectangleOutline(SpriteBatch spriteBatch, Texture2D pixel, Rectangle rect, Color color)
    {
        const int thickness = GameConstants.EditorGridThickness;

        // Top
        spriteBatch.Draw(pixel, new Rectangle(rect.X, rect.Y, rect.Width, thickness), color);

        // Bottom
        spriteBatch.Draw(pixel, new Rectangle(rect.X, rect.Bottom - thickness, rect.Width, thickness), color);

        // Left
        spriteBatch.Draw(pixel, new Rectangle(rect.X, rect.Y, thickness, rect.Height), color);

        // Right
        spriteBatch.Draw(pixel, new Rectangle(rect.Right - thickness, rect.Y, thickness, rect.Height), color);
    }
}