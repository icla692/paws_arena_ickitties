
using Anura.ConfigurationModule.Managers;
using Photon.Pun;
using System;
using System.Collections;
using UnityEngine;

public class StartingGameState : IRoomState
{
    public void Init(RoomStateManager context)
    {
        context.StartCoroutine(HandleStartingSceneCoroutine(context));
    }

    public void OnExit()
    {
    }

    private IEnumerator HandleStartingSceneCoroutine(RoomStateManager context)
    {
        yield return new WaitForSeconds(1.5f);

        InstantiatePlayer(context);
        TryInstantiateBot(context);

        yield return new WaitForSeconds(3f);

        //If tutorial don't auto-start game
        if (ConfigurationManager.Instance.Config.GetGameType() == Anura.ConfigurationModule.ScriptableObjects.GameType.TUTORIAL)
        {
            context.SetState(new GamePausedState());
        }
        else
        {
            context.SetFirstPlayerTurn();
        }
    }

    private void TryInstantiateBot(RoomStateManager context)
    {
        if (ConfigurationManager.Instance.Config.GetGameType() != Anura.ConfigurationModule.ScriptableObjects.GameType.SINGLEPLAYER)
            return;

        Vector2 spawnPos = PlayerManager.Instance.GetPlayer2SpawnPos();
        var go = SingleAndMultiplayerUtils.Instantiate(context.botPlayerPrefab.name, spawnPos, Quaternion.identity);
        go.GetComponent<BasePlayerComponent>().playerSeat = 1;

        var playerUI = SingleAndMultiplayerUtils.Instantiate(context.playerUIPrefab.name, Vector3.zero, Quaternion.identity);
        playerUI.GetComponent<PlayerDataCustomView>().isForNPC = true;

        BotManager.Instance.botUI = playerUI.GetComponent<PlayerDataCustomView>();
    }

    private void InstantiatePlayer(RoomStateManager context)
    {
        int seat = 0;
        if (ConfigurationManager.Instance.Config.GetIsMultiplayer())
        {
            seat = context.photonManager.GetMySeat();
        }
        Vector2 spawnPos = seat == 0 ? PlayerManager.Instance.GetPlayer1SpawnPos() : PlayerManager.Instance.GetPlayer2SpawnPos();

        var go = SingleAndMultiplayerUtils.Instantiate(context.playerPrefab.name, spawnPos, Quaternion.identity);

        if (!ConfigurationManager.Instance.Config.GetIsMultiplayer())
        {
            GameObject.Destroy(go.GetComponent<PhotonView>());
        }

        go.GetComponent<BasePlayerComponent>().playerSeat = seat;
        SingleAndMultiplayerUtils.Instantiate(context.playerUIPrefab.name, Vector3.zero, Quaternion.identity);
    }
}