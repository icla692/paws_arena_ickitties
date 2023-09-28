using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseBallSpawn : MonoBehaviour
{
    [SerializeField]
    private GameObject ball = null;

    private void Update()
    {
        CreateBall();
    }

    private void CreateBall()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            Vector3 mPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mPos.z = 0;
            Instantiate(ball, mPos, new Quaternion(0, 0, 0, 0));
        }
    }
}
