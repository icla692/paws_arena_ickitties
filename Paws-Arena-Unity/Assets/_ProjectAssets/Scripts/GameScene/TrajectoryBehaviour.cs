using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryBehaviour : MonoBehaviour
{
    public float step = 2f;
    private LineRenderer lineRenderer;
    private Vector2 lastPosition = Vector2.zero;
    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }
    public void StartRecording()
    {
        lineRenderer.positionCount = 0;
        BulletComponent.onBulletMoved += AddBulletPoint;
    }

    public void StopRecording()
    {
        BulletComponent.onBulletMoved -= AddBulletPoint;
    }

    private void AddBulletPoint(bool isMine, Vector2 newPos)
    {
        if (!isMine) return;

        if(Vector2.Distance(newPos, lastPosition) > step)
        {
            lastPosition = newPos;
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, newPos);
        }
    }
}
