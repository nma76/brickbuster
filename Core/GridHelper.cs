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
}