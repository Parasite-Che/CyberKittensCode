using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public static RespawnManager instance;
    public Checkpoint checkpoint;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        Events.onPlayerDeath += Respawn;
    }

    public void SetCurrentCheckpoint(Checkpoint checkpoint)
    {
        this.checkpoint = checkpoint;
    }

    public void Respawn()
    {
        checkpoint.Respawn();
    }

}
