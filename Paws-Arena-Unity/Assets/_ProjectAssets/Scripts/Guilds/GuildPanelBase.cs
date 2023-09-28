using System;
using UnityEngine;

public class GuildPanelBase : MonoBehaviour
{
    public virtual void Setup()
    {
        throw new Exception("Setup must be implemented");
    }

    public virtual void Close()
    {
        throw new Exception("Close must be implemented");
    }
}
