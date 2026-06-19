using brickbuster.Config;

namespace brickbuster.Systems;

public class LifeSystem
{
    public int Lifes { get; private set; } = GameConstants.PlayerLifes;

    public bool LoseLife()
    {
        Lifes--;
        return Lifes <= 0;
    }

    public void Reset()
    {
        Lifes = GameConstants.PlayerLifes;
    }
}