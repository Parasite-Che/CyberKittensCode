using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour, IDoDamage
{
    [SerializeField] private float timeActive;
    [SerializeField] private float timePassive;
    [SerializeField] private bool isActive;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private BoxCollider2D boxCollider;

    public string OriginTag { get; set; }

    IEnumerator SwitchStatePeriodically()
    {
        while (true)
        {
            if (!isActive)
            {
                Activate();
                yield return new WaitForSeconds(timeActive);
            }
            else
            {
                Deactivate();
                yield return new WaitForSeconds(timePassive);
            }
        }
    }

    public void Activate()
    {
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1f);
        isActive = true;
        boxCollider.enabled = true;
    }

    public void Deactivate()
    {
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0f);
        isActive = false;
        boxCollider.enabled = false;
    }

    public void ApplyDamage(ITakeDamage damagedObject)
    {
        damagedObject.TakeDamage();
    }

    public void Initialize(string originTag)
    {
        OriginTag = originTag;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isActive) return;
        if (other.transform.parent != null && other.gameObject.tag != OriginTag)
        {
            ITakeDamage damageableObject = other.transform.parent.gameObject.GetComponent<ITakeDamage>();
            if (damageableObject != null && other.gameObject.tag != OriginTag)
            {
                ApplyDamage(damageableObject);
            }
        }
    }


    void Start()
    {
        Initialize(gameObject.tag);
        isActive = !isActive;
        boxCollider = GetComponent<BoxCollider2D>();
        StartCoroutine(SwitchStatePeriodically());
    }

}
