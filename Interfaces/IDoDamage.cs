using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDoDamage
{
    public string OriginTag { get; set; }
    public void Initialize(string originTag);
    public void ApplyDamage(ITakeDamage damagedObject);
}
