using System;
using brickbuster.Config;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace brickbuster.Systems;

public class AudioSystem
{
    private readonly SoundEffect _hit;
    private readonly SoundEffect _paddleHit;
    private readonly SoundEffect _wallHit;
    private float _sfxVolume = GameConstants.SfxVolume;

    public AudioSystem(ContentManager content)
    {
        _hit = content.Load<SoundEffect>("Audio/blockhit");
        //_paddleHit = content.Load<SoundEffect>("Audio/paddlehit");
        //_wallHit = content.Load<SoundEffect>("Audio/wallhit");
    }

    public void PlayBlockHit()
    {
        _hit?.Play(_sfxVolume, 0f, 0f);
    }
    public void PlayPaddleHit()
    {
        // _paddleHit?.Play(_sfxVolume, 0f, 0f);
    }
    public void PlayWallHit()
    {
        _hit?.Play(_sfxVolume, -0.8f, 0f);
    }

    public void SetVolume(float volume)
    {
        _sfxVolume = Math.Clamp(volume, 0f, 1f);
    }
}