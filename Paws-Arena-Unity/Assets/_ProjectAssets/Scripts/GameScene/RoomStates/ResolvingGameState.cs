using Anura.ConfigurationModule.Managers;
using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolvingGameState : IRoomState
{
    public GameResolveState state;
    private RoomStateManager context;

    public ResolvingGameState(GameResolveState state)
    {
        this.state = state;
    }
    public void Init(RoomStateManager context)
    {
        this.context = context;
        EndingCoroutine();
    }

    private async void EndingCoroutine()
    {
        GameState.gameResolveState = this.state;

        if (ConfigurationManager.Instance.Config.GetGameType() != Anura.ConfigurationModule.ScriptableObjects.GameType.TUTORIAL)
        {
            try
            {
                await context.httpCommunication.RegisterEndOfTheMatch(PlayerManager.Instance.myPlayerHealth, state);
            }catch(UnityWebRequestException ex)
            {
                Debug.LogWarning($"Failed registering match {ex.ResponseCode}:{ex.Text}");
                context.LoadAfterGameScene(state);
            }
        }
        await UniTask.Delay(TimeSpan.FromSeconds(3));
        context.LoadAfterGameScene(state);
    }

    public void OnExit()
    {
    }
}
