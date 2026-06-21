using brickbuster.Config;
using brickbuster.Entities;

namespace brickbuster.Systems;

public class DifficultySystem
{
    private int _lastSpeedIncreaseHit = 0;

    public void ResetBallSpeed(Ball ball)
    {
        _lastSpeedIncreaseHit = 0;
        ball.ResetSpeed();
    }
    public void IncreaseBallspeed(Ball ball, int paddleHits)
    {
        if (paddleHits - _lastSpeedIncreaseHit >= 5)
        {
            ball.IncreaseSpeed(GameConstants.IncreseBallSpeedFactor);
            _lastSpeedIncreaseHit = paddleHits;
        }
    }
}