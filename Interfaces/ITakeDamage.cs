using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITakeDamage
{
    public void TakeDamage();
    public void GetDestroyed();
    IEnumerator InvincibilityTimer(float time);
}
