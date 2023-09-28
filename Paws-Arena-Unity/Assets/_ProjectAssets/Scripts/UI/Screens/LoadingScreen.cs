using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    public GameObject wrapper;
    public TMPro.TextMeshProUGUI text;

    public void Activate(string displayText)
    {
        wrapper.SetActive(true);
        text.text = displayText;
    }

    public void Deactivate()
    {
        wrapper.SetActive(false);
    }

    public bool IsActivated()
    {
        return wrapper.activeSelf;
    }
}
