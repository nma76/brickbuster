using System;
using brickbuster.Config;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace brickbuster.Systems;

public class AudioSystem
{
    // Keeps track of what music is currently playing
    public MusicType CurrentMusicType { get; private set; }

    // Holds Hit sound fx
    private readonly SoundEffect _hit;

    // Holds the music
    private readonly Song _music1;
    private readonly Song _musicintense1;

    // Local volume
    private float _sfxVolume = GameConstants.SfxVolume;
    private float _musicVolume = GameConstants.MusicVolume;

    public AudioSystem(ContentManager content)
    {
        _hit = content.Load<SoundEffect>("Audio/blockhit");
        _music1 = content.Load<Song>("Audio/music1");
        _musicintense1 = content.Load<Song>("Audio/musicintense1");
        CurrentMusicType = MusicType.Normal;
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
    public void SwitchMusic(bool isBossLevel)
    {
        var desired = isBossLevel ? MusicType.Intense : MusicType.Normal;

        if (desired == CurrentMusicType)
            return;

        if (desired == MusicType.Intense)
            PlayMusicIntense();
        else
            PlayMusic();
    }
    public void PlayMusic()
    {
        CurrentMusicType = MusicType.Normal;

        MediaPlayer.IsRepeating = true;
        MediaPlayer.Volume = _musicVolume;
        MediaPlayer.Play(_music1);
    }
    public void PlayMusicIntense()
    {
        CurrentMusicType = MusicType.Intense;

        MediaPlayer.IsRepeating = true;
        MediaPlayer.Volume = _musicVolume;
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
        _musicVolume = Math.Clamp(volume, 0f, 1f);
    }
}