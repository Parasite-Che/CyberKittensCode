using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class DstEnemyState : MonoBehaviour, ITakeDamage, IEnemy
{
    public Transform patrolTarget;
    public EnemyBulletPool enemyBulletPool;
    public GameObject _target;
    private GameObject _patrolZone;
    float _patrolRange=2f;
    float _speed = 0.2f;
    public float _combatDistance;
    Vector2 _borders;
    Vector3 _patrolPoint;
    int _counter = 600;
    [SerializeField] private AudioClip clip;
    [SerializeField] private AudioClip deathClip;
    public bool _enemyDetected=false;



    [SerializeField] private LockedArea lockedArea;
    [SerializeField] private bool canAttack;
    [SerializeField] private float reloadTime;
    [SerializeField] private int health;

    private void Start()
    {
        _patrolZone = Instantiate(Objects.instance.spherePatrolZone, transform.position, Quaternion.identity);
        _patrolZone.GetComponent<ShootViePatrol>().enabled = true;
        _patrolZone.GetComponent<ShootViePatrol>().mainObj = this;
        _patrolZone.GetComponent<BoxCollider2D>().enabled = true;
        canAttack = true;
        enemyBulletPool = EnemyBulletPool.instance;
        _borders.x = patrolTarget.position.x - _patrolRange;
        _borders.y = patrolTarget.position.x + _patrolRange;
        _patrolPoint=new Vector3(transform.position.x,transform.position.y,transform.position.z);
    }

    private void Update()
    {
        Patrol();
        if(_enemyDetected && canAttack)
            Attack();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController pc = other.gameObject.GetComponent<PlayerController>();
        if (pc != null)
        {
            _target = other.gameObject;
            _enemyDetected = true;
            _combatDistance = Vector3.Distance(transform.position, _target.transform.position);
        }
    }
    
    void Patrol()
    {
        if(_patrolPoint.x == transform.position.x)
            _patrolPoint.x = Random.Range(_borders.x, _borders.y);
        else
            transform.position=Vector3.MoveTowards(transform.position, _patrolPoint,(_speed+20*Time.deltaTime)/10);
        if (_enemyDetected)
        {
            float angle = _target.transform.position.x<transform.position.x? Random.Range(0, Mathf.PI/6):Random.Range((5*Mathf.PI)/6, Mathf.PI);
            if (_target.transform.position.x > transform.position.x&&transform.localScale.x>0)
            {
                transform.localScale = new Vector3(transform.localScale.x*(-1),transform.localScale.y);
            }
            else if (_target.transform.position.x < transform.position.x && transform.localScale.x < 0)
            {
                transform.localScale = new Vector3(transform.localScale.x*(-1),transform.localScale.y);
            }
            Vector3 newPos = new Vector3(_target.transform.position.x + _combatDistance/2f * Mathf.Cos(angle),
                _target.transform.position.y + _combatDistance/2f * Mathf.Sin(angle), transform.position.z);
            patrolTarget.transform.position = newPos;
            
            _patrolPoint.y = patrolTarget.position.y;
            _patrolPoint.z = patrolTarget.position.z;
            
            _borders.x = patrolTarget.position.x - _patrolRange;
            _borders.y = patrolTarget.position.x + _patrolRange;
            _patrolPoint.x = Random.Range(_borders.x, _borders.y);
        }
    }

    IEnumerator Reload()
    {
        if (!canAttack) yield break;
        canAttack = false;
        yield return new WaitForSeconds(reloadTime);
        canAttack = true;
    }

    void Attack()
    {
        Events.onEnvironmentDoSound?.Invoke(clip);
        EnemyBullet newBullet = enemyBulletPool.pool.Get().GetComponent<EnemyBullet>();
        newBullet.Initialize(gameObject.tag);
        newBullet.transform.position = transform.position;
        //EnemyBullet newBullet = Instantiate(_enemyBullet, _selfTransform.position, Quaternion.identity).GetComponent<EnemyBullet>();
        StartCoroutine(newBullet.Launch(_target.transform));
        StartCoroutine(Reload());
    }

    public void TakeDamage()
    {
        health--;
        Events.onEnvironmentDoSound.Invoke(deathClip);
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

    public void AddToArea(LockedArea area)
    {
        lockedArea = area;
    }

    public void RemoveFromArea(LockedArea area)
    {
        area?.RemoveEnemy(this);
    }
}
