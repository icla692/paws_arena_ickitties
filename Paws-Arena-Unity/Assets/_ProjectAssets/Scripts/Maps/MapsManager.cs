using Anura.ConfigurationModule.Managers;
using Anura.Templates.MonoSingleton;
using DTerrain;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapsManager : MonoSingleton<MapsManager> 
{
    public PUNRoomUtils punRoomUtils;
    public List<GameObject> mapPrefabs;
    private bool isMultiplayer;

    public void CreateMap()
    {
        isMultiplayer = ConfigurationManager.Instance.Config.GetIsMultiplayer();
        int mapIdx = isMultiplayer ? (int)punRoomUtils.GetRoomCustomProperty("mapIdx") : Random.Range(0, mapPrefabs.Count);

        var go = GameObject.Instantiate(mapPrefabs[mapIdx], Vector3.zero, Quaternion.identity, transform);
        List<BasicPaintableLayer> paintables =  go.GetComponentsInChildren<BasicPaintableLayer>().ToList();
        if(paintables.Count != 2)
        {
            Debug.LogWarning("There are not just 2 BasicPaintableLayer's in the map!");
        }
        PaintingManager.Instance.primaryLayer = paintables[0];
        PaintingManager.Instance.secondaryLayer = paintables[1];
    }
}
