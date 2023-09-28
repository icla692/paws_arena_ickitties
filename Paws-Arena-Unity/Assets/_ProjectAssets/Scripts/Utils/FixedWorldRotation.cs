using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedWorldRotation : MonoBehaviour
{
    public Vector3 rotation;

    private void Update()
    {
        transform.rotation = Quaternion.Euler(rotation);
    }
}
