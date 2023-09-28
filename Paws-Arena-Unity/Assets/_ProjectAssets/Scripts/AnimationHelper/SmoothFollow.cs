using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    public PlayerGraphicsBehaviour playerGraphicsBehaviour;
    public Transform target;
    public float speed;

    private Vector3 offset;

    private void Start()
    {
        transform.parent = null;
        offset = transform.position - target.position;
    }

    private void OnEnable()
    {
        playerGraphicsBehaviour.onCatFlipped += Flip;
    }

    private void OnDisable()
    {
        playerGraphicsBehaviour.onCatFlipped -= Flip;
    }

    private void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, target.position + offset, Time.deltaTime * speed);
    }

    public void Flip()
    {
        transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y, transform.localScale.z);
        offset = new Vector3(-1 * offset.x, offset.y, offset.z);
    }
}
