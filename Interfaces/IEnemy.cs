using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{
    public void AddToArea(LockedArea area);
    public void RemoveFromArea(LockedArea area);
}
