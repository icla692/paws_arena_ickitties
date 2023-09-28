
using Anura.ConfigurationModule.Managers;
using Photon.Pun;
using System.Collections;
using UnityEngine;

public class ProjectileLaunchedState : IRoomState
{
    private float waitBeforeNextRound = 2f;

    public ProjectileLaunchedState()
    {

    }

    public ProjectileLaunchedState(float waitBeforeNextRound)
    {
        this.waitBeforeNextRound = waitBeforeNextRound;
    }

    public void Init(RoomStateManager context)
    {
        context.StartCoroutine(HandleProjectileLaunched(context));
    }

    public void OnExit()
    {
    }
    private IEnumerator HandleProjectileLaunched(RoomStateManager context)
    {
        if (context.WasMyRound())
        {
            context.trajectory.StartRecording();
        }
        yield return new WaitForSeconds(waitBeforeNextRound);


        if (context.WasMyRound())
        {
            context.trajectory.StopRecording();
        }
        if (!ConfigurationManager.Instance.Config.GetIsMultiplayer() || PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            context.TryStartNextRound();
        }
    }
}