using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEditor;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public enum MusicType
    {
        Lobby,
        Game
    }

    public MusicType TypeOfMusic = MusicType.Game;
    public AudioSource MusicAudioSource;
    [Range(0f, 1f)]
    public float MusicVolume = 1;
    public AudioSource SfxAudioSource;
    [Range(0f, 1f)]
    public float SfxVolume = 1;
    [Range(0f, 0.2f)]
    public float maxPitchOffset = 0;

    //Themes
    public AudioClip Music_MainTheme;
    public AudioClip Music_LobbyTheme;

    //Environment
    public AudioClip[] SFX_Button_Activating;
    public AudioClip[] SFX_Button_Deactivating;
    public AudioClip[] SFX_Hook_Creating;
    public AudioClip[] SFX_Hook_Removing;
    public AudioClip[] SFX_Fireworks_Firing;
    public AudioClip[] SFX_Fireworks_Exploding;
    public AudioClip[] SFX_Tank_Filling;
    public AudioClip[] SFX_Terminal_Working;
    public AudioClip[] SFX_Terminal_Finalising;
    public AudioClip[] SFX_Transferring;

    //Player
    public AudioClip[] SFX_Player_Jumping;
    public AudioClip[] SFX_Player_Stunned;
    public AudioClip[] SFX_Gun_Firing;

    private AudioClip _music;
    private float pitch
    {
        get
        {
            return 1 + Random.Range(0 - maxPitchOffset, maxPitchOffset);
        }
    }

    // Use this for initialization
    void Start ()
    {
        SetDefaultMusic();
    }

    void Update()
    {
        SfxAudioSource.volume = SfxVolume;
        MusicAudioSource.volume = MusicVolume;
    }

    void SetDefaultMusic()
    {
        switch (TypeOfMusic)
        {
            case MusicType.Game:
                _music = Music_MainTheme;
                break;
            case MusicType.Lobby:
                _music = Music_LobbyTheme;
                break;
        }

        MusicAudioSource.clip = _music;
        MusicAudioSource.Play();
    }

    private static AudioManager GetAudioManager()
    {
        return GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    public static void GunFired()
    {
        AudioManager audioManager = GetAudioManager();

        AudioClip audioClip =
            audioManager.SFX_Gun_Firing[Random.Range(0, audioManager.SFX_Gun_Firing.Length)];
        audioManager.SfxAudioSource.PlayOneShot(audioClip);
    }

    public static void PlayerStunned()
    {
        AudioManager audioManager = GetAudioManager();

        AudioClip audioClip =
            audioManager.SFX_Player_Stunned[Random.Range(0, audioManager.SFX_Player_Stunned.Length)];
        audioManager.SfxAudioSource.PlayOneShot(audioClip);
    }

    public static void PlayerJumped()
    {
        AudioManager audioManager = GetAudioManager();

        AudioClip audioClip =
            audioManager.SFX_Player_Jumping[Random.Range(0, audioManager.SFX_Player_Jumping.Length)];
        audioManager.SfxAudioSource.PlayOneShot(audioClip);
    }

    public static void PackageTransferred()
    {
        AudioManager audioManager = GetAudioManager();

        AudioClip audioClip =
            audioManager.SFX_Transferring[Random.Range(0, audioManager.SFX_Transferring.Length)];
        audioManager.SfxAudioSource.PlayOneShot(audioClip);
    }

    public static void TerminalFinished()
    {
        AudioManager audioManager = GetAudioManager();

        AudioClip audioClip =
            audioManager.SFX_Terminal_Finalising[Random.Range(0, audioManager.SFX_Terminal_Finalising.Length)];
        audioManager.SfxAudioSource.PlayOneShot(audioClip);
    }

    public static void TerminalBusy()
    {
        AudioManager audioManager = GetAudioManager();

        AudioClip audioClip =
            audioManager.SFX_Terminal_Working[Random.Range(0, audioManager.SFX_Terminal_Working.Length)];
        audioManager.SfxAudioSource.PlayOneShot(audioClip);
    }

    public static void TankFilled()
    {
        AudioManager audioManager = GetAudioManager();

        AudioClip audioClip =
            audioManager.SFX_Tank_Filling[Random.Range(0, audioManager.SFX_Tank_Filling.Length)];
        audioManager.SfxAudioSource.PlayOneShot(audioClip);
    }

    public static void FireworkExploded()
    {
        AudioManager audioManager = GetAudioManager();

        AudioClip audioClip =
            audioManager.SFX_Fireworks_Exploding[Random.Range(0, audioManager.SFX_Fireworks_Exploding.Length)];
        audioManager.SfxAudioSource.PlayOneShot(audioClip);
    }

    public static void FireworksFired()
    {
        AudioManager audioManager = GetAudioManager();

        AudioClip audioClip =
            audioManager.SFX_Fireworks_Firing[Random.Range(0, audioManager.SFX_Fireworks_Firing.Length)];
        audioManager.SfxAudioSource.PlayOneShot(audioClip);
    }

    public static void HookRemoved()
    {
        AudioManager audioManager = GetAudioManager();

        AudioClip audioClip =
            audioManager.SFX_Hook_Removing[Random.Range(0, audioManager.SFX_Hook_Removing.Length)];
        audioManager.SfxAudioSource.PlayOneShot(audioClip);
    }

    public static void HookCreated()
    {
        AudioManager audioManager = GetAudioManager();

        AudioClip audioClip =
            audioManager.SFX_Hook_Creating[Random.Range(0, audioManager.SFX_Hook_Creating.Length)];
        audioManager.SfxAudioSource.PlayOneShot(audioClip);
    }

    public static void ButttonActivate()
    {
        AudioManager audioManager = GetAudioManager();

        AudioClip audioClip =
            audioManager.SFX_Button_Activating[Random.Range(0, audioManager.SFX_Button_Activating.Length)];
        audioManager.SfxAudioSource.PlayOneShot(audioClip);
    }

    public static void ButttonDeactivate()
    {
        AudioManager audioManager = GetAudioManager();

        AudioClip audioClip =
            audioManager.SFX_Button_Deactivating[Random.Range(0, audioManager.SFX_Button_Deactivating.Length)];
        audioManager.SfxAudioSource.PlayOneShot(audioClip);
    }
}
