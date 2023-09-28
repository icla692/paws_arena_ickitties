using Anura.ConfigurationModule.Managers;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct StringListWrapper
{
    [SerializeField]
    public string[] strings;
}
public class SingleAndMultiplayerUtils
{
    public static GameObject Instantiate(string path, Vector3 position, Quaternion rotation)
    {
        if (ConfigurationManager.Instance.Config.GetIsMultiplayer())
        {
            return PhotonNetwork.Instantiate(path, position, rotation);
        }
        else
        {
            GameObject myPrefab = Resources.Load(path) as GameObject;
            return GameObject.Instantiate(myPrefab, position, rotation);
        }
    }

    public static void Destroy(PhotonView photonView, GameObject go)
    {
        if (ConfigurationManager.Instance.Config.GetIsMultiplayer())
        {
            if (photonView.IsMine)
            {
                PhotonNetwork.Destroy(go);
            }
        }
        else
        {
            GameObject.Destroy(go);
        }
    }

    public static void RpcOrLocal(object source, PhotonView pv, bool isMineRequired, string methodName, RpcTarget target, params object[] args)
    {
        if (ConfigurationManager.Instance.Config.GetIsMultiplayer())
        {
            if (!isMineRequired || (isMineRequired && pv.IsMine))
            {
                pv.RPC(methodName, target, args);
            }
        }
        else
        {
            source.GetType().GetMethod(methodName).Invoke(source, args);
        }
    }
}
