using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class scr_Player_AudioControl : MonoBehaviour
{
    [SerializeField] private AudioSource AmbientAudioSource;
    [SerializeField] private AudioSource ChaseAudioSource;
    
    [SerializeField] private float zoneTransitionTime = 1f;
    [SerializeField] private float dangerTransitionTime = 2f;
    [SerializeField] private float dangerOverTransitionTime = 5f;
    
    [SerializeField] private AudioClip[] dangerOverSting;

    [SerializeField] private AudioClip[] zoneMusic;

    private int currentZone = -1; 

    public AudioMixerSnapshot ambienceSnapshot;
    public AudioMixerSnapshot quietSnapshot;
    public AudioMixerSnapshot chaseSnapshot;
    
    public bool chaseMusicOn;

    public void switchToChase(bool chasing)
    {
        chaseMusicOn = chasing;

        if (chasing)
            chaseSnapshot.TransitionTo(dangerTransitionTime);
        else
        {
            if(dangerOverSting.Length > 0)
                ChaseAudioSource.PlayOneShot(dangerOverSting[Random.Range(0, dangerOverSting.Length)]);
            
            ambienceSnapshot.TransitionTo(dangerOverTransitionTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        int zoneFrom = currentZone;
        switch (other.tag)
        {
            case "RedZone":
                currentZone = 0;
                break;
            case "GreenZone":
                currentZone = 1;
                break;
            case "BlueZone":
                currentZone = 2;
                break;
            case "WhiteZone":
                currentZone = 3;
                break;
            default:
                break;
        }

        
        if (currentZone != zoneFrom)
        {
            quietSnapshot.TransitionTo(zoneTransitionTime);
            AmbientAudioSource.clip = zoneMusic[currentZone];

            if (chaseMusicOn)
                chaseSnapshot.TransitionTo(zoneTransitionTime);
            else
                ambienceSnapshot.TransitionTo(zoneTransitionTime);
            
            AmbientAudioSource.Play();
        }
    }
}
