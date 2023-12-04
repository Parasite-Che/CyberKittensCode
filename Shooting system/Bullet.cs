using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour, IDoDamage
{
    private ObjectPool<Bullet> pool;
    public float time;

    public bool released;

    public string OriginTag { get; set; }

    private void OnEnable()
    {
        StartCoroutine(TimerForDeletion(time));
    }

    public void Initialize(string originTag)
    {
        OriginTag = originTag;
    }


    IEnumerator TimerForDeletion(float time)
    {
        yield return new WaitForSeconds(time);
        GetDeleted();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name);
        ITakeDamage damageableObject = collision.GetComponent<ITakeDamage>();
        if (damageableObject != null && OriginTag != collision.tag)
        {
            ApplyDamage(damageableObject);
        }
        if (damageableObject != null || collision.gameObject.layer == LayerMask.NameToLayer("Ground")) GetDeleted();
        // Events.Instance.bulletCollision?.Invoke(this.gameObject);
    }

    public void SetPool(ObjectPool<Bullet> pool)
    {
        this.pool = pool;
    }

    public void GetDeleted()
    {
        StopAllCoroutines();
        if (!released) pool.Release(this);
    }

    public void ApplyDamage(ITakeDamage damageableObject)
    {
        damageableObject.TakeDamage();
    }
}
