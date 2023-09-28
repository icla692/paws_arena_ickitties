using Anura.ConfigurationModule.Managers;
using Anura.Templates.MonoSingleton;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CratesManager : MonoSingleton<CratesManager>
{
    [SerializeField]
    private Vector2 minPos;
    [SerializeField]
    private Vector2 maxPos;

    private CratesConfig cratesConfig;

    // Start is called before the first frame update
    private void Start()
    {
        cratesConfig = ConfigurationManager.Instance.Crates;

        RoomStateManager.OnStateUpdated += OnStateUpdated;
    }

    private void OnDestroy()
    {
        RoomStateManager.OnStateUpdated -= OnStateUpdated;
    }

    private void OnStateUpdated(IRoomState state)
    {
        if (!PhotonNetwork.LocalPlayer.IsMasterClient) return;

        int roundNumber = RoomStateManager.Instance.roundNumber;
        if (roundNumber == 6)
        {
            if (state is MyTurnState)
            {
                SpawnCrate(cratesConfig.GetCrate());
            }
        }else if(roundNumber == 12)
        {
            if(state is OtherPlayerTurnState)
            {
                SpawnCrate(cratesConfig.GetCrate());
            }
        }                
    }

    public void SpawnCrate(Vector3 pos, int healingValue)
    {
        GameObject go = SingleAndMultiplayerUtils.Instantiate(cratesConfig.GetCrate().prefab.name, pos, Quaternion.identity);
        go.GetComponent<CrateHealthBehaviour>().healValue = healingValue;
    }
    private void SpawnCrate(CrateConfig crateConfig)
    {
        Vector2 randomPos = new Vector2(UnityEngine.Random.Range(minPos.x, maxPos.x), UnityEngine.Random.Range(minPos.y, maxPos.y));
        SingleAndMultiplayerUtils.Instantiate(crateConfig.prefab.name, randomPos, Quaternion.identity);
    }
}
