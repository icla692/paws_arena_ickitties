using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotActionMap : MonoBehaviour
{
    private BotInputActions.PlayerActions botActions;
    public BotActionMap(BotInputActions.PlayerActions botActions)
    {
        this.botActions = botActions;
    }

    public BotInputActions.PlayerActions GetPlayerActions()
    {
        return botActions;
    }

    public void SetActivePlayerActionMap(bool value)
    {
        if (value)
        {
            botActions.Enable();
        }
        else
        {
            botActions.Disable();
        }
    }

}
