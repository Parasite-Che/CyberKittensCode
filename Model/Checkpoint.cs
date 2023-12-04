using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public RespawnManager respawnManager;
    public PlayerController pc;
    public bool isActivated = false;

    void Start()
    {
        respawnManager = RespawnManager.instance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isActivated) return;
        if (collision.transform.parent != null)
        {
            pc = collision.transform.parent.GetComponent<PlayerController>();
            if (pc != null)
            {
                respawnManager.SetCurrentCheckpoint(this);
                isActivated = true;
            }
        }
    }

    public void Respawn()
    {
        pc.transform.position = transform.position;
        pc.Respawn();
    }
}
