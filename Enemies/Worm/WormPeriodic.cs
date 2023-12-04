using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormPeriodic : MonoBehaviour, ITakeDamage, IDoDamage
{
    [SerializeField] private float digDistance;
    [SerializeField] private float switchStateTime;
    [SerializeField] private bool isVertical;
    [SerializeField] private AudioClip clip;
    [SerializeField] private bool isUnderground;

    [SerializeField] private int health;

    public string OriginTag { get; set; }

    void Start()
    {
        Initialize(gameObject.tag);
        StartCoroutine(SwitchStatePeriodically(switchStateTime));
    }

    IEnumerator SwitchStatePeriodically(float timer)
    {
        while (true)
        {
            if (!isUnderground) StartCoroutine(Dig(digDistance));
            else StartCoroutine(Dig(-digDistance));
            isUnderground = !isUnderground;
            yield return new WaitForSeconds(timer);
        }
    }

    IEnumerator Dig(float digDistance)
    {
        Events.onEnvironmentDoSound.Invoke(clip);
        Vector3 moveVector;
        if (isVertical) moveVector = new Vector3(0, digDistance);
        else moveVector = new Vector3(digDistance, 0);

        Vector3 newPos = transform.position + moveVector;

        for (float i = 0; i < 1f; i += Time.deltaTime)
        {
            transform.position = Vector3.Lerp(transform.position, newPos, i / 1f);
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isUnderground && collision.transform.parent != null && collision.tag != OriginTag)
        {
            ITakeDamage damageableObject = collision.transform.parent.GetComponent<ITakeDamage>();
            if (damageableObject != null )
            {
                ApplyDamage(damageableObject);
            }
        }
    }

    public void TakeDamage()
    {
        if (!isUnderground)
        {
            Events.onEnvironmentDoSound.Invoke(clip);
            health--;
            if (health <= 0)
            {
                GetDestroyed();
            }
        }
        ;
    }

    public void GetDestroyed()
    {
        Destroy(gameObject);
    }

    public IEnumerator InvincibilityTimer(float time)
    {
        yield break;
    }

    public void Initialize(string originTag)
    {
        OriginTag = originTag;
    }

    public void ApplyDamage(ITakeDamage damagedObject)
    {
        damagedObject.TakeDamage();
    }
}
