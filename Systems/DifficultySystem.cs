using brickbuster.Config;
using brickbuster.Entities;

namespace brickbuster.Systems;

public class DifficultySystem
{
    private int _lastSpeedIncreaseHit = 0;
    private bool _endGameSpeedIncreased = false;

    private readonly BlockSystem _blockSystem;

    public DifficultySystem(BlockSystem blockSystem)
    {
        _blockSystem = blockSystem;
    }
    public void ResetBallSpeed(Ball ball)
    {
        _lastSpeedIncreaseHit = 0;
        _endGameSpeedIncreased = false;
        ball.ResetSpeed();
    }
    public bool IsEndgameSpeed => _endGameSpeedIncreased;
    public void UpdateBallspeed(Ball ball, int paddleHits)
    {
        var remaining = _blockSystem.GetRemaining();

        if(remaining == 0)
        {
            return;
        }

        if (paddleHits - _lastSpeedIncreaseHit >= 5)
        {
            ball.IncreaseSpeed(GameConstants.IncreseBallSpeedFactor);
            _lastSpeedIncreaseHit = paddleHits;
        }

        if ((!_endGameSpeedIncreased) && remaining <= 10)
        {
            ball.IncreaseSpeed(GameConstants.IncreseBallSpeedEndGameFactor);
            _endGameSpeedIncreased = true;
        }
    }
}