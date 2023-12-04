using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BulletPool : MonoBehaviour
{
    public static BulletPool instance { get; set; }

    public ObjectPool<Bullet> pool;
    public int defaultCapacity;
    public int maxCapacity;
    public int activeObjectsInPool;
    public int inactiveObjectsInPool;

    [SerializeField] private Transform bulletParent;
    [SerializeField] private GameObject bulletPrefab;

    private void Awake()
    {
        instance = this;
        bulletPrefab = Objects.instance.Bullet;
        pool = new ObjectPool<Bullet>(CreateBullet, GetBulletFromPool, ReturnBulletToPool, DestroyBullet, true, defaultCapacity, maxCapacity);
    }

    public void Initialize()
    {
        //bulletPrefab = Objects.instance.Bullet;
        //pool = new ObjectPool<Bullet>(CreateBullet, GetBulletFromPool, ReturnBulletToPool, DestroyBullet, true, defaultCapacity, maxCapacity);
    }


    // Update is called once per frame
    void Update()
    {
        activeObjectsInPool = pool.CountActive;
        inactiveObjectsInPool = pool.CountInactive;
    }

    public Bullet CreateBullet()
    {
        Bullet bullet = Instantiate(bulletPrefab, bulletParent).GetComponent<Bullet>();
        bullet.SetPool(pool);
        bullet.released = false;
        return bullet;
    }

    public void GetBulletFromPool(Bullet bullet)
    {
        bullet.gameObject.SetActive(true);
        bullet.released = false;
    }

    public void ReturnBulletToPool(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
        bullet.released = true;
    }

    public void DestroyBullet(Bullet bullet)
    {
        bullet.released = true;
        Destroy(bullet.gameObject);
    }
}
