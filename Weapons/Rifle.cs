using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : MonoBehaviour, IWeapon
{
    public GameObject rifleGO;

    [SerializeField] private BulletPool bulletPool;
    [SerializeField] private WeaponSO weaponSO;
    [SerializeField] private AudioClip clip;

    [SerializeField] private int reloadTime;
    [SerializeField] private int bulletSpeed;
    [SerializeField] private int recoilForce;

    private string ownerTag;
    
    public float reloadProgress = 100;
    

    // Start is called before the first frame update
    void Start()
    {
        clip = weaponSO.SoundEffect;
        
        bulletPool = BulletPool.instance;

        reloadTime = weaponSO.ReloadTime;
        bulletSpeed = weaponSO.BulletSpeed;
        recoilForce = weaponSO.RecoilForce;
    }


    public void Attack(Transform weapon)
    {
        if ((int)reloadProgress == 100)
        {
            Bullet bullet = bulletPool.pool.Get();
            bullet.Initialize(ownerTag);
            bullet.transform.position = weapon.GetChild(0).position;
            Vector2 direction = weapon.GetChild(0).position - weapon.position;
            ProjectileLaunch(bullet.gameObject, direction, bulletSpeed);
            StartCoroutine(Reload(reloadTime));
            Events.onDoSound?.Invoke(clip);
            Debug.Log("Shoot");
        }
    }

    public void ProjectileLaunch(GameObject projectile, Vector2 direction, float speed)
    {
        projectile.GetComponent<Rigidbody2D>().velocity = direction * speed;
    }

    public void Recoil(GameObject holder, Transform weaponPos)
    {
        if ((int)reloadProgress == 100)
        {
            Vector2 direct = Camera.main.ScreenToWorldPoint(Input.mousePosition) - weaponPos.position;
            holder.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            holder.GetComponent<Rigidbody2D>().AddForce(-direct.normalized * recoilForce);
        }
    }

    public IEnumerator Reload(int time)
    {
        reloadProgress = 0;
        GameObject Field = Objects.instance.Field;
        GameObject Bar = Objects.instance.Field.transform.GetChild(0).gameObject;
        
        Field.gameObject.SetActive(true);
        Bar.transform.localScale = new Vector3(0, Bar.transform.localScale.y);
        
        for (int i = 0; i <= time; i++)
        {
            Bar.transform.localScale = new Vector3(Mathf.Lerp(0, 1, (float)i / time), Bar.transform.localScale.y);
            Field.transform.localScale = Vector3.Lerp(new Vector3(0.3f, 0.3f, 1), new Vector3(0.7f, 0.7f, 1), (float)i / time);
            reloadProgress = Mathf.Lerp(0, 100, (float)i / time);
            yield return new WaitForFixedUpdate();
        }
        Objects.instance.Field.gameObject.SetActive(false);
        Debug.Log("Reloaded");
    }

    public void Initialize(string ownerTag)
    {
        this.ownerTag = ownerTag;
    }
}
