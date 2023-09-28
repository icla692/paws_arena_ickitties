using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTestSceneSetup : MonoBehaviour
{
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(2f);
        var playerActions = GameInputManager.Instance.GetPlayerActionMap().GetPlayerActions();
        playerActions.Enable();
    }
}
