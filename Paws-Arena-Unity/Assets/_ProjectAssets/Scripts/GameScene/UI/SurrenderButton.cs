using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurrenderButton : MonoBehaviour
{
    [SerializeField]
    private ConfirmationModal modal;
    public void OnClick()
    {
        modal.ShowModal("Are you sure you want to surrender?", () =>
        {
            RoomStateManager.Instance.SendRetreatRPC();
        });
    }
}
