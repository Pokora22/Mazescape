using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_Portal_AudioControl : MonoBehaviour
{
    [SerializeField] private AudioClip portalUse;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void idleSFX(bool play)
    {
        if(play)
            audioSource.Play();
        else
            audioSource.Stop();
    }

    public void teleportSFX()
    {
        audioSource.PlayOneShot(portalUse);
    }
}
