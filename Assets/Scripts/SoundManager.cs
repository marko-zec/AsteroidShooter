using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    // Effects
    static AudioSource audio_source;
    public AudioClip laser1;
    public AudioClip laser2;
    public AudioClip laser3;
    public AudioClip small_impact;
    public AudioClip medium_impact;
    public AudioClip big_impact;
    public AudioClip buttons;
    public AudioClip ship_upgraded;
    public AudioClip asteroid_break;
    public AudioClip asteroid_collision;
    public AudioClip damage_warning;
    public AudioClip critical_damage_warning;
    public AudioClip ship_explosion;

    // Music
    public AudioClip menu;
    public AudioClip start_music;
    public AudioClip level1_2;
    public AudioClip level3_4;
    public AudioClip level5_6;
    public AudioClip level7_8;
    public AudioClip level9_10;
    public AudioClip level11_inf;
    public AudioClip game_over;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    void Start()
    {
        audio_source = GetComponent<AudioSource>();

        // Play Menu BGM at start
        audio_source.clip = menu;
        audio_source.Play();
    }

    public void PlaySound(string clip)
    {
        switch (clip)
        {
            case "laser1":
                audio_source.PlayOneShot(laser1);
                break;
            case "laser2":
                audio_source.PlayOneShot(laser2);
                break;
            case "laser3":
                audio_source.PlayOneShot(laser3);
                break;
            case "small-impact":
                audio_source.PlayOneShot(small_impact);
                break;
            case "medium-impact":
                audio_source.PlayOneShot(medium_impact);
                break;
            case "big-impact":
                audio_source.PlayOneShot(big_impact);
                break;
            case "button-press":
                audio_source.PlayOneShot(buttons);
                break;
            case "ship-upgraded":
                audio_source.PlayOneShot(ship_upgraded);
                break;
            case "asteroid-break":
                audio_source.PlayOneShot(asteroid_break);
                break;
            case "asteroid-collision":
                audio_source.PlayOneShot(asteroid_collision);
                break;
            case "damage-warning":
                audio_source.PlayOneShot(damage_warning);
                break;
            case "critical-warning":
                audio_source.PlayOneShot(critical_damage_warning);
                break;
            case "explosion":
                audio_source.PlayOneShot(ship_explosion);
                break;
            default:
                break;
        }
    }

    public void ChangeBGM(string music)
    {
        switch (music)
        {
            case "level1_2":
                audio_source.clip = level1_2;
                audio_source.PlayOneShot(start_music);
                audio_source.PlayDelayed(3.0f);
                break;
            case "level3_4":
                audio_source.clip = level3_4;
                audio_source.PlayDelayed(0.5f);
                break;
            case "level5_6":
                audio_source.clip = level5_6;
                audio_source.PlayDelayed(0.5f);
                break;
            case "level7_8":
                audio_source.clip = level7_8;
                audio_source.PlayDelayed(0.5f);
                break;
            case "level9_10":
                audio_source.clip = level9_10;
                audio_source.PlayDelayed(0.5f);
                break;
            case "level11_inf":
                audio_source.clip = level11_inf;
                audio_source.PlayDelayed(0.5f);
                break;
            case "game_over":
                audio_source.clip = game_over;
                audio_source.PlayDelayed(0.5f);
                break;
            default:
                break;
        }
    }
}
