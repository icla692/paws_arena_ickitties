using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFlareComponent : BulletComponent
{
    public GameObject bulletGraphics;
    public ParticleSystem particles;
    protected override void HandleCollision(Vector2 hitPose)
    {
        rb.isKinematic = true;
        rb.velocity = Vector2.zero;
        rb.Sleep();

        SingleAndMultiplayerUtils.RpcOrLocal(this, photonView, false, "CallAirplane", RpcTarget.All, hitPose);
    }

    [PunRPC]
    public void CallAirplane(Vector2 hitPose)
    {
        StartCoroutine(CallAirplaneCoroutine(hitPose));
    }

    private IEnumerator CallAirplaneCoroutine(Vector2 hitPose)
    {
        bulletGraphics.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        AirplaneManager.Instance.StartRoutine(hitPose, photonView == null ? true : photonView.IsMine);

        yield return new WaitForSeconds(1f);
        var emission = particles.emission;
        emission.rateOverTime = 0;

        yield return new WaitForSeconds(3.5f);

        SingleAndMultiplayerUtils.Destroy(photonView, gameObject);
    }
}
