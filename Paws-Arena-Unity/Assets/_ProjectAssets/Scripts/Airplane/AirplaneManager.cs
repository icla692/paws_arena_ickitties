using Anura.Templates.MonoSingleton;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirplaneManager : MonoSingleton<AirplaneManager>
{
    [Header("Internals")]
    public GameObject airplaneParent;
    public Animator airplaneAnimator;
    public AudioSource audioSource;

    [Header("Params")]
    public float travelTime = 2f;
    public float distanceToTargetThreshold = 5f;
    public Transform startPos;
    public Transform endPos;
    public GameObject bombPrefab;
    public Transform bombStartPos;
    public Transform targetToHit;
    public AudioClip flySound;

    private float _speed;
    private Vector3 _direction;
    private bool routineActive = false;
    private bool shouldDropBomb = false;

    private void Start()
    {
        ApplySettings();
    }

    private void ApplySettings()
    {
        audioSource.volume = GameState.gameSettings.soundFXVolume * GameState.gameSettings.masterVolume;
    }

    private void FixedUpdate()
    {
        if (!routineActive) return;

        airplaneParent.transform.position += _direction.normalized * _speed * Time.deltaTime;


        if (shouldDropBomb && Math.Abs(bombStartPos.transform.position.x - targetToHit.transform.position.x) <= distanceToTargetThreshold)
        {
            DropBomb();
            shouldDropBomb = false;
        }

        if (Vector3.Distance(airplaneParent.transform.position, endPos.transform.position) <= 5)
        {
            EndRoutine();
        }
    }

    public void StartRoutine(Vector2 position, bool isMine)
    {
        targetToHit.transform.position = position;

        shouldDropBomb = isMine;
        routineActive = true;

        airplaneParent.transform.position = startPos.transform.position;
        _speed = Vector3.Distance(startPos.transform.position, endPos.transform.position) / travelTime;
        _direction = endPos.transform.position - startPos.transform.position;

        airplaneAnimator.SetTrigger("start");
        audioSource.PlayOneShot(flySound);
    }

    private void EndRoutine()
    {
        routineActive = false;
    }

    public void DropBomb()
    {
        StartCoroutine(DropBombCoroutine());
    }
    private IEnumerator DropBombCoroutine()
    {
        var go = SingleAndMultiplayerUtils.Instantiate("Bullets/" + bombPrefab.name, bombStartPos.transform.position, Quaternion.identity);
        go.GetComponent<BulletComponent>().hasEnabledPositionTracking = false;
        yield return new WaitForEndOfFrame();
        go.GetComponent<BulletComponent>().Launch(Vector3.zero, 0);

        yield return new WaitForSeconds(0.15f);
        go = SingleAndMultiplayerUtils.Instantiate("Bullets/" + bombPrefab.name, bombStartPos.transform.position, Quaternion.identity);
        go.GetComponent<BulletComponent>().hasEnabledPositionTracking = false;
        yield return new WaitForEndOfFrame();
        go.GetComponent<BulletComponent>().Launch(Vector3.zero, 0);

        yield return new WaitForSeconds(0.15f);
        go = SingleAndMultiplayerUtils.Instantiate("Bullets/" + bombPrefab.name, bombStartPos.transform.position, Quaternion.identity);
        go.GetComponent<BulletComponent>().hasEnabledPositionTracking = false;
        yield return new WaitForEndOfFrame();
        go.GetComponent<BulletComponent>().Launch(Vector3.zero, 0);
    }

    [ContextMenu("Test Routine")]
    public void TestRoutine()
    {
        StartRoutine(targetToHit.transform.position, true);
    }
}
