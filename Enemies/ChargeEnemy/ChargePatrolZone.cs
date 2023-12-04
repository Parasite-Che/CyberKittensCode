using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargePatrolZone : MonoBehaviour
{
    public ChargeEnemy mainObj;

    private void Update()
    {
        if(mainObj != null)
            transform.position = mainObj.gameObject.transform.position;
        else if(mainObj == null)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController pc = other.gameObject.GetComponentInParent<PlayerController>();
        if (pc != null && this.isActiveAndEnabled)
        {
            mainObj._target = other.gameObject.transform;
            mainObj._enemyDetected = true;
        }
    }
}
