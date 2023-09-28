using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsActionMap : MonoBehaviour
{
    private GameInputActions.WeaponsActions weaponsActions;

    public WeaponsActionMap(GameInputActions.WeaponsActions weaponsActions)
    {
        this.weaponsActions = weaponsActions;
    }

    public GameInputActions.WeaponsActions GetWeaponsInputActions()
    {
        return weaponsActions;
    }
}
