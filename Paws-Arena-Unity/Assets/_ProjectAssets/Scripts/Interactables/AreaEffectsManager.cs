using Anura.Templates.MonoSingleton;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEffectsManager : MonoSingleton<AreaEffectsManager>
{
    public event Action<Vector2, float, int, bool, bool, float, int> OnAreaDamage;

    public void AreaDamage(Vector2 position, float area, int maxDamage, bool doesDamageByDistance, bool hasPushForce, float pushForce, int bulletCount)
    {
        OnAreaDamage?.Invoke(position, area, maxDamage, doesDamageByDistance, hasPushForce, pushForce, bulletCount);
    }
}
