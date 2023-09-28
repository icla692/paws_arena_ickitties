using Anura.ConfigurationModule.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class IndicatorInputCircleBehaviour : MonoBehaviour, IPointerUpHandler
{
    public event Action<float, float> onIndicatorPlaced;
    public LayerMask indicatorLayer;
    public float startOffset = 0.2f;

    private float radius;

    private void Start()
    {
        radius = ConfigurationManager.Instance.Config.GetCircleShootRadius();
        transform.localScale = Vector3.one * 2.0f * radius;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log($"Click position: {eventData.position}; CirclePosition: {Camera.main.WorldToScreenPoint(transform.position)}");
    }

    public void CheckPointerClick(Vector2 pointerPos)
    {
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(pointerPos);
        Vector2 myPos = transform.position;
        float angle = Vector2.SignedAngle(new Vector2(1, 0), mouseWorldPos - myPos);
        float power = ((mouseWorldPos - myPos).magnitude / radius);
        power = Math.Clamp(power, 0, 1);
        power = power * (1 + startOffset) - startOffset;
        CheckPointerClick(angle, power);
    }

    public void CheckPointerClick(float angle, float power)
    {
        onIndicatorPlaced?.Invoke(angle, power);
    }

    public bool IsSelected(Vector2 pointerPos)
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(pointerPos), Vector2.zero, 1000, indicatorLayer);
        return (hit.collider != null && hit.collider.gameObject == gameObject);
    }

}
