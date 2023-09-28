using Anura.ConfigurationModule.Managers;
using Anura.Templates.MonoSingleton;
using Photon.Pun;
using UnityEngine;

public class VFXManager : MonoSingleton<VFXManager>
{
    public void PUN_InstantiateExplosion(Vector3 position, GameObject explosion)
    {
        SingleAndMultiplayerUtils.Instantiate("Explosions/" + explosion.name, position, Quaternion.identity);
    }
}
