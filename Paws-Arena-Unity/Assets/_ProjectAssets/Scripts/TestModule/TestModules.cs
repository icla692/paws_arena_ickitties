using NaughtyAttributes;
using UnityEngine;

public class TestModules : MonoBehaviour
{
    
    [Button("Enable Player Map", EButtonEnableMode.Playmode)]
    private void EnablePlayerMap()
    {
        GameInputManager.Instance.GetPlayerActionMap()
            .SetActivePlayerActionMap(true);
    }

    [Button("Disable Player Map", EButtonEnableMode.Playmode)]
    private void DisablePlayerMap()
    {
        GameInputManager.Instance.GetPlayerActionMap()
            .SetActivePlayerActionMap(false);
    }
}
