using System.Collections;
using UnityEngine;

public class CannonComponent : BulletComponent
{
    [SerializeField] private float delayExplosion;

    protected override void HandleCollision(Vector2 hitPose)
    {
        StartCoroutine(DelayExplosion(delayExplosion, hitPose));
    }


    private IEnumerator DelayExplosion(float seconds, Vector2 hitPose)
    {
        yield return new WaitForSeconds(seconds);
        base.HandleCollision(transform.position);
    }
}
