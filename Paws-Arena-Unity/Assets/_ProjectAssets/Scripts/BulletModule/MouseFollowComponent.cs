using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollowComponent : BulletComponent
{
    [SerializeField] private float followTime = 10f;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float rotationCorrectionSpeed = 10f;
    [SerializeField] private AudioSource followingSound;


    private bool hasHitGround = false;
    private Transform playerToFollow;

    protected override void Start()
    {
        base.Start();

        //rb.freezeRotation = true;


        playerToFollow = isMine ? PlayerManager.Instance.otherPlayerTransform : PlayerManager.Instance.myPlayer.transform;

    }

    private void Update()
    {
        if (Mathf.Abs(transform.localRotation.eulerAngles.z) > 0)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(0, 0, 0)), rotationCorrectionSpeed * Time.deltaTime);
        }
        
        //var clampedRot = Mathf.Clamp(transform.localRotation.z, -30, 30);
        //transform.localRotation = Quaternion.Euler(new Vector3(0, 0, clampedRot));

        if (!hasHitGround) return;

        float dir = Mathf.Sign(playerToFollow.position.x - transform.position.x);
        float force = dir * Time.deltaTime * speed;
        rb.AddForce(new Vector2(force, 0), ForceMode2D.Force);     
        
        if(dir < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = Vector3.one;
        }

    }

    protected override void HandleCollision(Vector2 hitPose)
    {
        StartCoroutine(FollowEnemy(followTime, hitPose));
    }


    private IEnumerator FollowEnemy(float seconds, Vector2 hitPose)
    {
        ApplySettings();
        hasHitGround = true;
        followingSound.Play();

        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;

        yield return new WaitForSeconds(seconds);

        followingSound.Stop();

        base.HandleCollision(transform.position);
    }

    private void ApplySettings()
    {
        followingSound.volume = GameState.gameSettings.soundFXVolume * GameState.gameSettings.masterVolume;
    }
}
