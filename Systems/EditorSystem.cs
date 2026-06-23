using brickbuster.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace brickbuster.Systems;

public class EditorSystem
{
    public Point? HoveredCell { get; set; }
    public void Update(GameTime gameTime)
    {
        var mouse = Mouse.GetState();
        Point mousePos = new(mouse.X, mouse.Y);
        HoveredCell = GridHelper.GetCellAtPosition(mousePos);
    }
}