using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathArea : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 8)
        {
            PhotonView pv = collision.gameObject.GetComponent<PhotonView>();
            if(pv == null || pv.IsMine)
            {
                //PlayerManager.Instance.DirectDamage(1000);
                collision.gameObject.GetComponent<BasePlayerComponent>().GiveDamage(1000);
                RoomStateManager.Instance.TryStartNextRound();
            }
        }
    }
}
