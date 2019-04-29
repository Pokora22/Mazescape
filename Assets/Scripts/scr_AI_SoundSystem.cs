using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_AI_SoundSystem : MonoBehaviour
{
    public AudioClip[] clips;

    private AudioSource m_AudioSource;

    [SerializeField] private float minVol = .8f;
    [SerializeField] private float maxVol = 1f;

    // Start is called before the first frame update
    private void Awake()
    {
        m_AudioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {

    }

    private void playClip(int clipNo)
    {
        m_AudioSource.PlayOneShot(clips[clipNo], Random.Range(minVol, maxVol));
    }

    private void playStep()
    {
        int stepNo = Random.Range(1, 4);
        playClip(stepNo);
    }
}
