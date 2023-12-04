using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectPlayer : MonoBehaviour
{
    private AudioSource _player;

    private void Start()
    {
        _player = GetComponent<AudioSource>();
        Events.onDoSound = PlayEffect;
    }

    void PlayEffect(AudioClip source)
    {
        _player.PlayOneShot(source);
    }
    
    private void OnApplicationQuit()
    {
        Events.onDoSound = null;
    }
}
