using System;
using brickbuster.Config;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace brickbuster.Systems;

public class AudioSystem
{
    private readonly SoundEffect _hit;
    private readonly Song _music1;
    private readonly Song _musicintense1;
    private float _sfxVolume = GameConstants.SfxVolume;
    private float _songVolume = GameConstants.MusicVolume;

    public AudioSystem(ContentManager content)
    {
        _hit = content.Load<SoundEffect>("Audio/blockhit");
        _music1 = content.Load<Song>("Audio/music1");
        _musicintense1 = content.Load<Song>("Audio/musicintense1");
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
    public void PlayMusic()
    {
        MediaPlayer.IsRepeating = true;
        MediaPlayer.Volume = _songVolume;
        MediaPlayer.Play(_music1);
    }
    public void PlayMusicIntense()
    {
        MediaPlayer.IsRepeating = true;
        MediaPlayer.Volume = _songVolume;
        MediaPlayer.Play(_musicintense1);
    }
    public void StopMusic()
    {
        MediaPlayer.Stop();
    }

    public void SetSfxVolume(float volume)
    {
        _sfxVolume = Math.Clamp(volume, 0f, 1f);
    }
    public void SetMusicVolume(float volume)
    {
        _sfxVolume = Math.Clamp(volume, 0f, 1f);
    }
}