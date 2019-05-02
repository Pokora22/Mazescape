using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class scr_Player_AudioControl : MonoBehaviour
{
    [SerializeField] private AudioSource AmbientAudioSource;
    [SerializeField] private AudioSource ChaseAudioSource;
    [SerializeField] private float musicTransitionTime = 1f;

    public AudioMixerSnapshot ambienceSnapshot;
    public AudioMixerSnapshot chaseSnapshot;

    
    
    public bool chaseMusicOn;

    public void switchToChase(bool chasing)
    {
        chaseMusicOn = chasing;

        if (chasing)
            chaseSnapshot.TransitionTo(musicTransitionTime);
        else
            ambienceSnapshot.TransitionTo(musicTransitionTime);
    }
    
}
