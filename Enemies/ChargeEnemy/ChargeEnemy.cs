using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class ChargeEnemy : MonoBehaviour, ITakeDamage, IEnemy
{
    public Transform _target;
    private GameObject _patrolZone;
    float _speed=0.01f;
    float _range = 1f;
    public bool _enemyDetected=false;
    [SerializeField] private Vector2 _borders;
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private LockedArea lockedArea;
    [SerializeField] private int health;
    [SerializeField] private AudioClip clip;

    private void Start()
    {
        //clip = Resources.Load<AudioClip>("Sounds/ChargerHit");
        _patrolZone = Instantiate(Objects.instance.spherePatrolZone, transform.position, Quaternion.identity);
        _patrolZone.GetComponent<ChargePatrolZone>().enabled = true;
        _patrolZone.GetComponent<ChargePatrolZone>().mainObj = this;
        _patrolZone.GetComponent<BoxCollider2D>().enabled = true;
        _borders.x = transform.position.x - _range;
        _borders.y = transform.position.x + _range;
        if (!_enemyDetected)
            StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        while (!_enemyDetected)
        {
            if (transform.position.x == _borders.x)
                transform.position = Vector3.MoveTowards(transform.position,
                    new Vector3(_borders.y, transform.position.y),
                    (_borders.y - _borders.x));
            else
            {
                transform.position = Vector3.MoveTowards(transform.position,
                    new Vector3(_borders.x, transform.position.y),
                    (_borders.y - _borders.x));
            }

            rigidBody.AddForce(0.5f * Vector2.up, ForceMode2D.Impulse);
            yield return new WaitForSeconds(1f);
            rigidBody.AddForce(0.5f * Vector2.down, ForceMode2D.Impulse);
            yield return new WaitForSeconds(0.1f);
            rigidBody.AddForce(0.5f * Vector2.down, ForceMode2D.Impulse);
            yield return new WaitForSeconds(1f);
            rigidBody.AddForce(0.5f * Vector2.up, ForceMode2D.Impulse);
            Debug.Log("Coroutine started");
            yield return new WaitForSeconds(0.3f);
        }

        StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        Vector3 targetPos;
        targetPos = _target.position;
        while (_enemyDetected)
        {
            Vector3 attackWay;
            if (targetPos.x < transform.position.x)
                attackWay = new Vector3(targetPos.x - 1, targetPos.y);
            else
                attackWay = new Vector3(targetPos.x + 1, targetPos.y);
            transform.position = Vector3.MoveTowards(transform.position, attackWay,
                Vector3.Distance(attackWay, transform.position));
            targetPos = _target.position;
            yield return new WaitForSeconds(1f);
        }
    }
    

    public void AddToArea(LockedArea area)
    {
        lockedArea = area;
    }

    public void RemoveFromArea(LockedArea area)
    {
        area?.RemoveEnemy(this);
    }

    public void TakeDamage()
    {
        health--;
        Events.onEnvironmentDoSound?.Invoke(clip);
        if (health <= 0)
        {
            GetDestroyed();
        }
    }

    public void GetDestroyed()
    {
        RemoveFromArea(lockedArea);
        Destroy(gameObject);
    }

    public IEnumerator InvincibilityTimer(float time)
    {
        yield break;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.gameObject.name);
        if (other.transform.parent != null)
        {
            PlayerController pc = other.transform.parent.GetComponent<PlayerController>();
            if (pc != null)
            {
                Events.onEnvironmentDoSound?.Invoke(clip);
                Debug.Log("SoundPlayed");
                ITakeDamage damageableObject = other.transform.parent.GetComponent<ITakeDamage>();
                damageableObject.TakeDamage();
            }
        }
    }
}
