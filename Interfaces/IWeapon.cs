using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    public void Attack(Transform weapon);
    public void Recoil(GameObject holder, Transform weaponPos);
    IEnumerator Reload(int time);
    void ProjectileLaunch(GameObject projectile, Vector2 direction, float speed);
    public void Initialize(string ownerTag);
}
