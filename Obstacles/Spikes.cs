using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour, IDoDamage
{
    public string OriginTag { get; set; }

    public void ApplyDamage(ITakeDamage damagedObject)
    {
        damagedObject.TakeDamage();
    }

    public void Initialize(string originTag)
    {
        OriginTag = originTag;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.parent != null && collision.gameObject.tag != OriginTag)
        {
            ITakeDamage damageableObject = collision.transform.parent.gameObject.GetComponent<ITakeDamage>();
            if (damageableObject != null && collision.gameObject.tag != OriginTag)
            {
                ApplyDamage(damageableObject);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Initialize(gameObject.tag);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
