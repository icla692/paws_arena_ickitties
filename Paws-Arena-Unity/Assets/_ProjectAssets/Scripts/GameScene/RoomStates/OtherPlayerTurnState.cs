using Photon.Pun;

public class OtherPlayerTurnState : IRoomState
{
    public void Init(RoomStateManager context)
    {
        context.lastPlayerRound = PhotonNetwork.LocalPlayer.IsMasterClient ? 1 : 0;

    }

    public void OnExit()
    {
    }
}