using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LuckyWheelWhoPlaysFirst : MonoBehaviour
{
    public static bool DoIPlayFirst;
    
    [SerializeField] private AnimationCurve spinCurve;
    [SerializeField] private AnimationCurve endSpinCurve;
    [SerializeField] private float spinSpeed;
    [SerializeField] private RectTransform pointerHolder;
    [SerializeField] private Button leaveButton;
    
    private LuckyWheelRewardSO choosenPlayer;
    private float speed;
    private PhotonView photonView;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    private void OnEnable()
    {
        leaveButton.gameObject.SetActive(false);
    }

    private void Start()
    {
        StartCoroutine(ChooseStartingPlayer());
    }

    private IEnumerator ChooseStartingPlayer()
    {
        yield return new WaitForSeconds(2);
        
        if (!PhotonNetwork.IsMasterClient)
        {
            yield break;
        }
        
        DoIPlayFirst = Random.Range(0,2)==0;

        if (PhotonNetwork.CurrentRoom==null|| PhotonNetwork.CurrentRoom.PlayerCount==1)
        {
            DoIPlayFirst = true;
        }
        
        float _targetZ = DoIPlayFirst ? Random.Range(20, 170) : Random.Range(190, 350);

        if (PhotonNetwork.CurrentRoom!=null&&PhotonNetwork.CurrentRoom.PlayerCount==2)
        {
            photonView.RPC(nameof(Spin),RpcTarget.Others,_targetZ,!DoIPlayFirst);
        }

        Spin(_targetZ,DoIPlayFirst);
    }

    private IEnumerator SpinRoutine(float _targetRotationZ)
    {
        pointerHolder.eulerAngles = Vector3.zero;
        float _spinDuration = 2;
        float _timePassed = 0;

        while (_timePassed < _spinDuration)
        {
            _timePassed += Time.deltaTime;
            float _pointOnCurve = _timePassed / _spinDuration;
            float _zIncrement = Time.deltaTime * speed * spinCurve.Evaluate(_pointOnCurve);

            Vector3 _rotaiton = pointerHolder.eulerAngles;
            _rotaiton.z += _zIncrement;
            pointerHolder.eulerAngles = _rotaiton;

            yield return null;
        }

        // End spin
        float _targetedZ = _targetRotationZ;
        float _currentZRotation = pointerHolder.eulerAngles.z;
        int _additionalFullSPins = 1;
        _targetedZ -= (360 * _additionalFullSPins);
        float _distanceTraveled = 0;
        float _distanceToTravel = _targetedZ - _currentZRotation;
        float _pointAtCurve = 0;

        do
        {
            if (_distanceTraveled == 0)
            {
                _pointAtCurve = 0;
            }
            else
            {
                _pointAtCurve = _distanceTraveled / _distanceToTravel;
            }

            float _speedModifier = endSpinCurve.Evaluate(_pointAtCurve);
            float _movingDistanceThisFrame = Mathf.MoveTowards(_currentZRotation, _targetedZ, Time.deltaTime * _speedModifier * speed);
            _movingDistanceThisFrame = _currentZRotation - _movingDistanceThisFrame;
            _distanceTraveled += _movingDistanceThisFrame;
            _currentZRotation += _movingDistanceThisFrame;
            pointerHolder.eulerAngles = new Vector3(pointerHolder.eulerAngles.x,
                                                    pointerHolder.eulerAngles.y,
                                                    _currentZRotation);
            yield return null;

        } while (_pointAtCurve < 1);

        // Snap position just in case
        pointerHolder.eulerAngles = new Vector3(pointerHolder.eulerAngles.x, pointerHolder.eulerAngles.y, _targetedZ);

        StartCoroutine(EndSpin());
    }

    private IEnumerator EndSpin()
    {
        yield return new WaitForSeconds(1);
        // foreach (var _playerPlatform in playerPlatforms)
        // {
        //     _playerPlatform.gameObject.SetActive(true);
        // }
        gameObject.SetActive(false);
    }

    [PunRPC]
    private void Spin(float _targetZ, bool _doIPlayFirst)
    {
        DoIPlayFirst = _doIPlayFirst;
        speed = spinSpeed;
        StartCoroutine(SpinRoutine(_targetZ));
    }
}
