using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterSound : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;
    private AudioSource audioSource;
    private float volumeMultiplier = 1.0f;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
        audioSource.volume = SoundManager.Instance.GetVolume() * volumeMultiplier;
        SoundManager.Instance.OnSoundEffectVolumeChange += SoundManager_OnSoundEffectVolumeChange;
    }

    private void SoundManager_OnSoundEffectVolumeChange(object sender, System.EventArgs e)
    {
        audioSource.volume = SoundManager.Instance.GetVolume() * volumeMultiplier;
    }

    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventHandler e)
    {
        if (e.isOn)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Pause();
        }
    }
}
