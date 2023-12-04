using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyBullet : MonoBehaviour, IDoDamage
{
    private ObjectPool<EnemyBullet> pool;
    private Vector3 _target;
    private float _time = 10f;

    public bool released;
    
    public string OriginTag { get; set; }


    public void ApplyDamage(ITakeDamage damagedObject)
    {
        damagedObject.TakeDamage();
    }

    public void Initialize(string originTag)
    {
        OriginTag = originTag;
        gameObject.tag = OriginTag;
    }
    
    public IEnumerator Launch(Transform target)
    {
        _target = target.position;
        GetComponent<Rigidbody2D>().AddForce((_target-transform.position).normalized*0.1f,ForceMode2D.Force);
        yield return new WaitForSeconds(_time);
        if (!released) pool.Release(this);
    }

    public void SetPool(ObjectPool<EnemyBullet> pool)
    {
        this.pool = pool;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.parent != null)
        {
            ITakeDamage damageableObject = other.transform.parent.gameObject.GetComponent<ITakeDamage>();
            if (damageableObject != null && other.gameObject.tag != OriginTag)
            {
                ApplyDamage(damageableObject);
                if (!released) pool.Release(this);
            }
        }
        if(other.gameObject.layer == LayerMask.NameToLayer("Ground"))
            if (!released) pool.Release(this);
    }
}
