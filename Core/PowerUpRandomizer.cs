using System;
using brickbuster.Config;

namespace brickbuster.Core;

public static class PowerUpRandomizer
{
    private static readonly PowerUpType[] possiblePowerUps =
    [
        PowerUpType.ExpandPaddle,
        PowerUpType.ExtraLife,
        PowerUpType.ShrinkPaddle
    ];

    public static PowerUpType Roll()
    {
        if(Random.Shared.NextDouble() < GameConstants.PowerUpchance)
        {
            return possiblePowerUps[Random.Shared.Next(possiblePowerUps.Length)];
        }
        return PowerUpType.None;
    }
}