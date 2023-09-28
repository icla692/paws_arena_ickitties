using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class MyCollisionEvent : UnityEvent<Collision2D>
{

}

[System.Serializable]
public class MyTriggerEvent : UnityEvent<Collider2D>
{

}

public class CollisionEventRaiser : MonoBehaviour
{
    public MyCollisionEvent OnCollision2DEnter;
    public MyTriggerEvent OnTrigger2DEnter;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnCollision2DEnter?.Invoke(collision);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        OnTrigger2DEnter?.Invoke(collider);
    }
}
