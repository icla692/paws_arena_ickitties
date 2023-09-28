using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float speed;

    private void Update()

    {
        transform.Translate(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0).normalized * speed * Time.deltaTime);
        Camera.main.orthographicSize -= Input.mouseScrollDelta.y;
        Camera.main.orthographicSize = Mathf.Max(1, Camera.main.orthographicSize);
    }
}
