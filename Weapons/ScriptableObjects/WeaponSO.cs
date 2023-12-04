using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObject/Weapon")]
public class WeaponSO : ScriptableObject
{
    public int ReloadTime;
    public int BulletSpeed;
    public int RecoilForce;
    public AudioClip SoundEffect;
}
