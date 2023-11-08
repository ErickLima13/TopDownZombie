using GamePlay;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static GamePlay.Sound;
using static Unity.VisualScripting.Member;

public class AudioManager : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioSource sfxSource;

    public AudioClip[] steps;
    public AudioClip[] fires;
    public AudioClip[] reloads;
    public AudioClip noAmmo;

    [SerializeField] private float soundRange = 25f;

    [SerializeField] private Sound.SoundType soundType = Sound.SoundType.Dangerous;

    public static AudioManager Instance
    {
        get; private set;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    public void PlaySfx(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
}
