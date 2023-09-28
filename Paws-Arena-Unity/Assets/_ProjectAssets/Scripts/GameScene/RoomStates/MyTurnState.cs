using Anura.ConfigurationModule.Managers;
using Photon.Pun;

public class MyTurnState : IRoomState
{
    public void Init(RoomStateManager context)
    {
        if (!context.isMultiplayer)
        {
            context.lastPlayerRound = 0;
        }
        else
        {
            context.lastPlayerRound = PhotonNetwork.LocalPlayer.IsMasterClient ? 0 : 1;
        }
    }

    public void OnExit()
    {
    }
}
