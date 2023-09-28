using Anura.ConfigurationModule.Managers;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateHealthBehaviour : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip healSFX;

    [HideInInspector]
    public int healValue = -1;

    private PhotonView photonView;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();

        var cratesConfig = ConfigurationManager.Instance.Crates;
        HealthCrate config = cratesConfig.GetCrate<HealthCrate>();
        if (healValue == -1)
        {
            healValue = UnityEngine.Random.Range(config.minHP, config.maxHP);
        }
    }
    public void OnChildCollisionEnter2D(Collision2D collision)
    {
        var playerComponent = collision.gameObject.GetComponent<BasePlayerComponent>();
        if(playerComponent != null && playerComponent.IsMine())
        {
            PlayerManager.Instance.Heal(healValue);
            SingleAndMultiplayerUtils.RpcOrLocal(this, photonView, false, "DestroyCrate", RpcTarget.MasterClient);
        }
    }

    //Called only if master
    [PunRPC]
    public void DestroyCrate()
    {
        audioSource.PlayOneShot(healSFX);
        SingleAndMultiplayerUtils.Destroy(photonView, gameObject);
    }
}
