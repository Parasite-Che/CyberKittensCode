using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedArea : MonoBehaviour
{
    public List<IEnemy> activeEnemies;
    public BoxCollider2D areaCollider;
    [SerializeField] private Door lockedObject;
    public int numOfEnemies;
    
    void Start()
    {
        areaCollider.enabled = false;
        activeEnemies = new List<IEnemy>();
        LockArea();
    }

    private void Update()
    {
        numOfEnemies = activeEnemies.Count;
    }

    private void LockArea()
    {
        areaCollider.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IEnemy enemy = collision.gameObject.GetComponent<IEnemy>();
        if (collision.transform.parent != null && enemy == null) enemy = collision.transform.parent.GetComponent<IEnemy>();
        if (enemy != null)
        {
            activeEnemies.Add(enemy);
            Debug.Log("enemy added");
            enemy.AddToArea(this);
        }

    }

    public void RemoveEnemy(IEnemy enemy)
    {
        Debug.Log("enemy removed");
        activeEnemies.Remove(enemy);
        if (activeEnemies.Count < 1)
        {
            lockedObject?.Unlock();
        }
    }
    
}
