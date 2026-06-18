using System.Collections.Generic;
using brickbuster.Entities.Blocks;

namespace brickbuster.Models;

public class LevelData
{
    public string Name { get; set; }
    public string Music { get; set; }
    public string Background { get; set; }
    public List<BlockBase> Blocks { get; set; }
}