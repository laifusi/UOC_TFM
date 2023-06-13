using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [SerializeField] AudioClip music1, music2, music3;
    [SerializeField] float FXvolume1, FXvolume2, FXvolume3;
    [SerializeField] AudioSource additionalSoundFX;

    private AudioSource audioSource;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartMusic();
    }

    public void StartMusic()
    {
        InvokeRepeating(nameof(UpdateMusic), 0.1f, music1.length);
    }

    private void UpdateMusic()
    {
        if (GameManager.Instance == null)
        {
            audioSource.clip = music1;
            additionalSoundFX.volume = FXvolume1;
        }
        else
        {
            switch (GameManager.Instance.MusicZone)
            {
                case MusicZone.Light:
                    audioSource.clip = music1;
                    additionalSoundFX.volume = FXvolume1;
                    break;
                case MusicZone.Mid:
                    audioSource.clip = music2;
                    additionalSoundFX.volume = FXvolume2;
                    break;
                case MusicZone.Dark:
                    audioSource.clip = music3;
                    additionalSoundFX.volume = FXvolume3;
                    break;
            }
        }

        audioSource.Play();
    }

    public void StopMusic()
    {
        audioSource.Stop();
        CancelInvoke();
    }

    private void OnDestroy()
    {
        CancelInvoke();
    }
}

public enum MusicZone
{
    Light, Mid, Dark
}
