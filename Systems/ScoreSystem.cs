namespace brickbuster.Systems;

public class ScoreSystem
{
    public int Score { get; private set; }
    public void Add(int value) => Score += value;
    public void Reset() => Score = 0;
}