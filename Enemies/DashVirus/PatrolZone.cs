using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolZone : MonoBehaviour
{
    public DashEnemyStates mainObj;
    
    private void Update()
    {
        if(mainObj != null)
            transform.position = mainObj.gameObject.transform.position;
        else if(mainObj == null)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player"&&mainObj._target == null&&this.isActiveAndEnabled)
        {
            mainObj._target = other.gameObject.transform;
            mainObj._enemyDetected = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player"&&mainObj != null&&this.isActiveAndEnabled)
            mainObj._enemyDetected = false;
    }
}
