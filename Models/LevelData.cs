using System.Collections.Generic;
using brickbuster.Entities.Blocks;
using brickbuster.Models.Json;

namespace brickbuster.Models;

public class LevelData
{
    public string Name { get; set; }
    public string Background { get; set; }
    public bool IsBossLevel { get; set; }
    public bool IsFinal { get; set; }
    public List<BlockBase> Blocks { get; set; }
    public LevelDefinition Source { get; set; }
}