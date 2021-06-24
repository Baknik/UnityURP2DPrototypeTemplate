using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SFXPlayer : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        this.audioSource = this.GetComponent<AudioSource>();
    }

    public void Play()
    {
        this.audioSource.Stop();
        this.audioSource.Play();
    }
}
