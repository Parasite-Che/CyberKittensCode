using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Events : MonoBehaviour
{
    public static Events Instance { get; set; }

    private void Awake()
    {
        Instance = this;
    }

    public delegate void OnPlayerDeath();
    public static OnPlayerDeath onPlayerDeath;

    public delegate void OnSoundEffectProduced(AudioClip effect);
    public static OnSoundEffectProduced onDoSound;
    public static OnSoundEffectProduced onEnvironmentDoSound;
}
