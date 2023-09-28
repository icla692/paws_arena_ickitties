using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotTurnState : IRoomState
{
    public void Init(RoomStateManager context)
    {
        context.lastPlayerRound = 1;
    }

    public void OnExit()
    {

    }
}
