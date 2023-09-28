using UnityEngine;

public class PlayerActionMap
{
    private GameInputActions.PlayerActions playerActions;
    
    public PlayerActionMap(GameInputActions.PlayerActions playerActions)
    {
        this.playerActions = playerActions;
    }

    public GameInputActions.PlayerActions GetPlayerActions()
    {
        return playerActions;
    }

    public void SetActivePlayerActionMap(bool value)
    {
        if (value)
        {
            playerActions.Enable();
        }
        else
        {
            playerActions.Disable();
        }
    }
}
