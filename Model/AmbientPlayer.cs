using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AmbientPlayer : MonoBehaviour
{
    public AudioSource audioPlayer;
    public static AudioClip[] musicClipList;
    int _previousIndex;
    public bool isPaused;
    double _endTime;

    private void Start()
    {
        musicClipList = Resources.LoadAll<AudioClip>("Sounds/Ambient");
        audioPlayer = GetComponent<AudioSource>();
        audioPlayer.PlayOneShot(musicClipList[Random.Range(0,musicClipList.Length)]);
        _endTime= AudioSettings.dspTime + musicClipList[0].length+10;
        
        audioPlayer.PlayScheduled(_endTime);
        _previousIndex= 0;
        isPaused = false;
    }

    private void Update()
    {
        if (!isPaused && (AudioSettings.dspTime>_endTime))
        {
            audioPlayer.Stop();
            _previousIndex=PlayNext(_previousIndex);
            
        }
            
    }

    int PlayNext(int index)
    {
        int newIndex;
        do
        {
            newIndex = UnityEngine.Random.Range((int)0, (int)musicClipList.Length);
        } while (newIndex == index);

        audioPlayer.PlayOneShot(musicClipList[newIndex]);
        _endTime = AudioSettings.dspTime + musicClipList[newIndex].length+10;
        audioPlayer.PlayScheduled(_endTime);

        return newIndex;
    }
}
