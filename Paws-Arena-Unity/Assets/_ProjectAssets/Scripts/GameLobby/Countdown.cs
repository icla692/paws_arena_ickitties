using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Countdown : MonoBehaviour
{
    public AudioClip sfx;

    private TextMeshProUGUI _text;

    private bool isCanceled = false;
    private void OnEnable()
    {
        PUNRoomUtils.onPlayerLeft += CancelCountdown;
    }

    private void OnDisable()
    {
        PUNRoomUtils.onPlayerLeft -= CancelCountdown;
    }

    private void CancelCountdown()
    {
        isCanceled = true;
    }

    public void StartCountDown(Action callback)
    {
        _text = GetComponent<TextMeshProUGUI>();
        StartCoroutine(CountdownAnimation(8, callback));   
    }

    private IEnumerator CountdownAnimation(int seconds, Action callback)
    {
        while(seconds > 0 && !isCanceled)
        {
            _text.text = "" + seconds;
            SFXManager.Instance.PlayOneShot(sfx);
            yield return new WaitForSeconds(1f);
            seconds--;
        }

        if (isCanceled)
        {
            isCanceled = false;
            _text.text = "";
        }
        else
        {
            callback?.Invoke();
        }

    }
}
