using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Objects : MonoBehaviour
{
    public static Objects instance { get; set; }
    
    public GameInput gameInput;

    public GameObject Bullet;
    public GameObject enemyBullet;
    public GameObject LightRifle;
    public GameObject HeavyRifle;
    public GameObject Claws;
    public GameObject Field;
    public GameObject spherePatrolZone;
    public GameObject PausePanel;
    public GameObject[] HealthPanels;
    


    private void Awake()
    {
        instance = this;
    }
    
}
