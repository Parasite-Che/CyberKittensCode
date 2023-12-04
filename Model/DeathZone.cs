using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.parent != null)
        {
            PlayerController pc = collision.transform.parent.GetComponent<PlayerController>();
            if (pc != null)
            {
                Events.onPlayerDeath?.Invoke();
            }
        }
    }
}
