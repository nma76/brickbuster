using brickbuster.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace brickbuster.Systems;

public class EditorSystem
{
    public Point? HoveredCell { get; set; }

    private char _selectedBlock = 'G';

    public LevelSystem LevelSystem { get; set; }

    public EditorSystem(LevelSystem levelSystem)
    {
        LevelSystem = levelSystem;
    }

    public void Update(GameTime gameTime)
    {
        // Get mouse position
        var mouse = Mouse.GetState();
        Point mousePos = new(mouse.X, mouse.Y);

        // Get hovered cell, if any.
        HoveredCell = GridHelper.GetCellAtPosition(mousePos);

        if (mouse.LeftButton == ButtonState.Pressed && HoveredCell.HasValue)
        {
            SetBlock(HoveredCell.Value.X, HoveredCell.Value.Y, 'G');
        }
    }

    public void SetBlock(int column, int row, char blockType)
    {
        char[] chars = LevelSystem.CurrentLevelData.Source.Grid[row].ToCharArray();

        chars[column] = blockType;

        LevelSystem.CurrentLevelData.Source.Grid[row] = new string(chars);
    }
}