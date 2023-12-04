using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Katana : MonoBehaviour, IWeapon
{
    public GameObject katanaGO;

    [SerializeField] private WeaponSO weaponSO;

    [SerializeField] private int reloadTime;
    [SerializeField] private int recoilForce;
    [SerializeField] private float attackTime;
    [SerializeField] private float katanaAttackDegree;
    [SerializeField] private AudioClip clip;

    private string ownerTag;

    public float reloadProgress = 100;

    // Start is called before the first frame update
    void Start()
    {
        clip = weaponSO.SoundEffect;
        reloadTime = weaponSO.ReloadTime;
        recoilForce = weaponSO.RecoilForce;
    }


    public void Attack(Transform weapon)
    {
        if ((int)reloadProgress == 100)
        {
            Events.onDoSound?.Invoke(clip);
            Debug.Log("Katana attack");
            StartCoroutine(KatanaAttack());
            StartCoroutine(Reload(reloadTime));
        }
    }

    private IEnumerator KatanaAttack()
    {
        Transform katana = katanaGO.transform;
        for (float i = 0; i < attackTime / 2; i += Time.deltaTime)
        {
            Vector3 rotation = Vector3.Slerp(katana.rotation.eulerAngles, katana.rotation.eulerAngles - new Vector3(0, 0, katanaAttackDegree), i / (attackTime / 2));
            katana.rotation = Quaternion.Euler(rotation);
            yield return null;
        }
        //for (float i = 0; i < attackTime / 2; i += Time.deltaTime)
        //{
        //    Vector3 rotation = Vector3.Slerp(katana.rotation.eulerAngles, katana.rotation.eulerAngles - new Vector3(0, 0, katanaAttackDegree), i / (attackTime / 2));
        //    katana.rotation = Quaternion.Euler(rotation);
        //    yield return null;
        //}
    }

    public void ProjectileLaunch(GameObject projectile, Vector2 direction, float speed)
    {
        ;
    }

    public void Recoil(GameObject holder, Transform weaponPos)
    {
        if ((int)reloadProgress == 100)
        {
            Vector2 direct = Camera.main.ScreenToWorldPoint(Input.mousePosition) - weaponPos.position;
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
