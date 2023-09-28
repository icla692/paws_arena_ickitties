using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRoomState
{
    public void Init(RoomStateManager context);
    public void OnExit();
}
