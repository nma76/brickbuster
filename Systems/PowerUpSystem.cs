using System.Collections.Generic;
using System.Linq;
using brickbuster.Config;
using brickbuster.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace brickbuster.Systems;

public class PowerUpSystem
{
    private readonly AudioSystem _audioSystem;
    private readonly LifeSystem _lifeSystem;


    private readonly List<PowerUp> _activePowerUps = [];
    private readonly List<PowerUp> _removePowerUps = [];

    public PowerUpSystem(AudioSystem audioSystem, LifeSystem lifeSystem)
    {
        _audioSystem = audioSystem;
        _lifeSystem = lifeSystem;
    }

    public void Add(PowerUp powerUp)
    {
        _activePowerUps.Add(powerUp);
    }
    public void ClearActive()
    {
        _activePowerUps.Clear();
    }

    public void Update(GameTime gameTime, Paddle paddle)
    {
        // Update power-ups
        foreach (var powerUp in _activePowerUps.ToList())
        {
            powerUp.Update(gameTime);

            if (powerUp.Rect.Intersects(paddle.Rect))
            {
                switch (powerUp.Type)
                {
                    case PowerUpType.Death:
                        // Decrease life
                        _lifeSystem.LoseLife();
                        break;
                    case PowerUpType.ReverseControls:
                        paddle.ReverseControls();
                        break;
                    case PowerUpType.ExpandPaddle:
                        paddle.Expand();
                        break;
                    case PowerUpType.ShrinkPaddle:
                        paddle.Shrink();
                        break;
                    case PowerUpType.ExtraLife:
                        _lifeSystem.AddLife();
                        break;
                }

                _audioSystem.PlayPowerUp();
                _removePowerUps.Add(powerUp);
            }
        }
        _activePowerUps.RemoveAll(_removePowerUps.Contains);
    }

    public void Draw(SpriteBatch spriteBatch, Texture2D pixel)
    {
        foreach (var powerUp in _activePowerUps)
        {
            powerUp.Draw(spriteBatch, pixel);
        }
    }
}