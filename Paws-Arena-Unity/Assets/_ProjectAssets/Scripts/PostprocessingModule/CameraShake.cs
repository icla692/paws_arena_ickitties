using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public Camera cam;
    public float amplitude;
    public float frequency;

    public bool isEnabled = false;
    private Vector3 startingPosition;

    private void Start()
    {
        startingPosition = cam.transform.localPosition;
    }

    private void Update()
    {
        if (!isEnabled) return;
        cam.transform.position += new Vector3(Mathf.Sin(Time.time * frequency)*amplitude, Mathf.Sin(Time.time * frequency) * amplitude, 0);
    }

    public void setIsEnabled(bool val)
    {
        isEnabled = val;
        
        if (isEnabled)
        {
            startingPosition = cam.transform.localPosition;
        }
        if (!isEnabled) 
        {
            cam.transform.localPosition = startingPosition;
        }
    }
}
