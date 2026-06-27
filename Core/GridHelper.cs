using System.Collections.ObjectModel;
using System.Reflection.Metadata.Ecma335;
using brickbuster.Config;
using Microsoft.Xna.Framework;

namespace brickbuster.Core;

public static class GridHelper
{
    public static Rectangle GetGridCellRectangle(int column, int row)
    {
        return new Rectangle(
            GameConstants.GridStartX + column * (GameConstants.BlockWidth + GameConstants.BlockSpacingX),
            GameConstants.GridStartY + row * (GameConstants.BlockHeight + GameConstants.BlockSpacingY),
            GameConstants.BlockWidth,
            GameConstants.BlockHeight);
    }

    public static Point? GetCellAtPosition(Point mousePosition)
    {
        for (int row = 0; row < 14; row++)
        {
            for (int col = 0; col < 14; col++)
            {
                if (GetGridCellRectangle(col, row).Contains(mousePosition))
                {
                    return new Point(col, row);
                }
            }
        }
        return null;
    }
}