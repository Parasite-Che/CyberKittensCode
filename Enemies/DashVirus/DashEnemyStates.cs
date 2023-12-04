using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class DashEnemyStates : MonoBehaviour, ITakeDamage, IEnemy
{
    public Transform patrolPoint;
    public Transform _target;
    private GameObject _patrolZone;
    Action _currentAction;
    Vector3 _nextPoint;
    Vector3 _patrolPoint;
    float _patrolRange=2f;
    float _speed=0.1f;
    [SerializeField] private AudioClip clip;
    public bool _enemyDetected=false;

    [SerializeField] private LockedArea lockedArea;
    [SerializeField] private int health;

    void Start()
    {
        _patrolZone = Instantiate(Objects.instance.spherePatrolZone, transform.position, Quaternion.identity);
        _patrolZone.GetComponent<PatrolZone>().enabled = true;
        _patrolZone.GetComponent<PatrolZone>().mainObj = this;
        _patrolZone.GetComponent<CircleCollider2D>().enabled = true;
        _patrolPoint.x = Random.Range(patrolPoint.position.x - 3, patrolPoint.position.x + 3);
        _patrolPoint.y = Random.Range(patrolPoint.position.y - 1, patrolPoint.position.y + 1);
        _currentAction = Patrol;
    }

    void Update()
    {
        State();
    }

    void Patrol()
    {
        if (_patrolPoint.x == transform.position.x)
        {
            _patrolPoint.x = Random.Range(patrolPoint.position.x - 5, patrolPoint.position.x + 5);
            _patrolPoint.y = Random.Range(patrolPoint.position.y - 1, patrolPoint.position.y + 1);
        }
        else
            transform.position=Vector3.MoveTowards(transform.position, _patrolPoint,(_speed+30*Time.deltaTime)/20);
    }

    void Attack()
    {
        Vector2 origin = transform.position;
        Vector2 direction = _target.position;
        transform.position = Vector3.MoveTowards(transform.position, _target.position, (_speed+40*Time.deltaTime)/10);
    }

    void State()
    {
        if(_currentAction!=Patrol&&!_enemyDetected)
        {
            _currentAction = Patrol;
        }
        else if(_enemyDetected&&_currentAction!=Attack)
            _currentAction = Attack;
        _currentAction();
    }

    public void TakeDamage()
    {
        Events.onEnvironmentDoSound?.Invoke(clip);
        Debug.Log("Lower health");
        health--;
        if (health <= 0)
        {
            GetDestroyed();
        }
    }

    public void GetDestroyed()
    {
        Events.onEnvironmentDoSound?.Invoke(clip);
        RemoveFromArea(lockedArea);
        Destroy(gameObject);
    }

    public IEnumerator InvincibilityTimer(float time)
    {
        yield break;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.parent != null)
        {
            PlayerController pc = other.transform.parent.GetComponent<PlayerController>();
            if (pc != null)
            {
                ITakeDamage damageableObject = other.transform.parent.GetComponent<ITakeDamage>();
                damageableObject.TakeDamage();
                GetDestroyed();
            }
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
}
