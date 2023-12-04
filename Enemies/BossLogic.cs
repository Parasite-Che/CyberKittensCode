using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class BossLogic : MonoBehaviour, ITakeDamage
{
    [SerializeField] private GameObject[] points;
    public EnemyBulletPool enemyBulletPool;
    [SerializeField] public float health = 20f;
    [SerializeField] public GameObject player;
    [SerializeField] private AudioClip clip;
    private int previousIndex = 0;

    private void Start()
    {
        enemyBulletPool = EnemyBulletPool.instance;
        //StartCoroutine(Fight());
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public IEnumerator Fight()
    {
        Debug.Log("BOSSSSSSSSSSSS");
        while (health>0)
        {
            gameObject.transform.position = GetRandomPoint().transform.position;
            StartCoroutine(Fire());
            Debug.Log("Fire eeeeeend");
            yield return new WaitForSeconds(2f+health/5f);
        }
    }

    GameObject GetRandomPoint()
    {
        int newInd;
        do
        {
            newInd = Random.Range(0, points.Length);
        } while (newInd == previousIndex);

        previousIndex = newInd;

        return points[newInd];
    }

    IEnumerator Fire()
    {
        for (int i = 0; i < 20;i++)
        {
            Events.onEnvironmentDoSound?.Invoke(clip);
            EnemyBullet newBullet = enemyBulletPool.pool.Get().GetComponent<EnemyBullet>();
            newBullet.Initialize(gameObject.tag);
            newBullet.transform.position = transform.position;
            StartCoroutine(newBullet.Launch(player.transform));
            yield return new WaitForSeconds(0.5f);
        }
    }


    public void TakeDamage()
    {
        health--;
    }

    public void GetDestroyed()
    {
        
    }

    public IEnumerator InvincibilityTimer(float time)
    {
        return null;
    }
}
