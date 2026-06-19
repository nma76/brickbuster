namespace brickbuster.Models.Json;

public class LevelDefinition
{
    public string Name { get; set; }
    public string Music { get; set; }
    public string Background { get; set; }
    public bool IsFinal { get; set; }
    public string[] Grid { get; set; }
}