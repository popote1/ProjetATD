using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSoundLauncherComponent : MonoBehaviour
{
    public AudioClip AudioClip;
    [Range(0, 1)] public float volume;

    public void PlaySound()
    {
        AudioManager.PlaySfx(AudioClip ,volume);
    }
}
