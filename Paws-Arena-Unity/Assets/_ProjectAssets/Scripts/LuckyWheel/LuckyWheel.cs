using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuckyWheel : MonoBehaviour
{
    [SerializeField] private AnimationCurve spinCurve;
    [SerializeField] private AnimationCurve endSpinCurve;
    [SerializeField] private float spinSpeed;
    [SerializeField] private RectTransform pointerHolder;
    [SerializeField] private List<LuckyWheelRewardDisplay> rewardDisplays;

    private Action callback;
    private LuckyWheelRewardSO choosenReward;
    private float speed;

    public void Spin(Action _callback, LuckyWheelRewardSO _choosenReward)
    {
        foreach (var _rewardDisplay in rewardDisplays)
        {
            _rewardDisplay.ResetDisplay();
        }
        callback = _callback;
        speed = spinSpeed;
        choosenReward = _choosenReward;
        StartCoroutine(SpinRoutine());
    }

    private IEnumerator SpinRoutine()
    {
        pointerHolder.eulerAngles = Vector3.zero;
        float _spinDuration = UnityEngine.Random.Range(3, 5f);
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
        float _targetedZ = UnityEngine.Random.Range(choosenReward.MinRotation, choosenReward.MaxRotation);
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

        // Show shadows and start shaking
        foreach (var _rewardDisplay in rewardDisplays)
        {
            if (_rewardDisplay.RewardType == choosenReward.Type)
            {
                _rewardDisplay.Shake();
            }
            else
            {
                _rewardDisplay.ShowShadow();
            }
        }

        callback?.Invoke();
    }
}