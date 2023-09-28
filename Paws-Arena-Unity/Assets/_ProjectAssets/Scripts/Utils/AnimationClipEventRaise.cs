using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationClipEventRaise : MonoBehaviour
{
    public UnityEvent onEventRaised;
    public void RaiseEvent()
    {
        onEventRaised?.Invoke();
    }
}
