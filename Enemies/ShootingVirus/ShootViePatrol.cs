using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootViePatrol : MonoBehaviour
{
    public DstEnemyState mainObj;
    
    private void Update()
    {
        if(mainObj != null)
            transform.position = mainObj.gameObject.transform.position;
        else if(mainObj == null)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player"&&mainObj != null&&this.isActiveAndEnabled)
        {
            mainObj._target = other.gameObject;
            mainObj._enemyDetected = true;
            mainObj._combatDistance = Vector3.Distance(transform.position,  mainObj._target.transform.position);
        }
    }
}
