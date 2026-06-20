using brickbuster.Config;

namespace brickbuster.Systems;

public class ScoreSystem
{
    public float Multiplier { get; set; } = GameConstants.NormalMultiplier;
    public int Score { get; private set; }

    public void SetMultiplier(float multiplier)
    {
        Multiplier = multiplier;
    }
    public void Add(int value)
    {
        Score += (int)(value * Multiplier);
    }
    public void Reset()
    {
        Score = 0;
    }
}