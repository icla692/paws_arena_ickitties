using Anura.ConfigurationModule.Managers;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUIBehaviour : MonoBehaviour
{
    public Transform healthBarParent;
    public RectTransform healthBar;
    public RectTransform healthBarFilling;

    public TMPro.TextMeshProUGUI nickname;

    private float healthBarTotalWidth = -1;
    private int totalhealth;
    private int currentHealth;

    private bool isInit = false;

    private void Start()
    {
        currentHealth = totalhealth;
    }

    public void OnHealthUpdated(int val)
    {
        currentHealth = val;

        if (!isInit) return;

        float startingX = healthBar.sizeDelta.x;
        LeanTween.value(startingX, healthBarTotalWidth * (currentHealth * 1.0f / totalhealth), 1f).setEaseInOutCirc()
            .setOnUpdate(val => { healthBar.sizeDelta = new Vector2(val, healthBar.sizeDelta.y); });
    }

    public void Init()
    {
        isInit = true;
        healthBarTotalWidth = healthBar.sizeDelta.x;
        totalhealth = ConfigurationManager.Instance.Config.GetPlayerTotalHealth();

        OnHealthUpdated(currentHealth);
    }

    public void SetOrientationRight()
    {
        var parentAnchor = healthBarParent.GetComponent<RectTransform>();
        parentAnchor.anchorMin = parentAnchor.anchorMax = parentAnchor.pivot = new Vector2(1, 1);
        parentAnchor.transform.localScale = new Vector3(-1, 1, 1);
        parentAnchor.anchoredPosition = new Vector2(-470, -58);

        nickname.GetComponent<RectTransform>().anchoredPosition = new Vector2(81, -4);

    }

    public void SetTag()
    {
        healthBarFilling.gameObject.tag = "MyExpBar";
    }

    public void OverrideColor(Color _color)
    {
        healthBarFilling.GetComponent<Image>().color = _color;
    }

}
