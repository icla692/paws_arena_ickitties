using System.Collections;
using UnityEngine;

public class LuckyWheelRewardDisplay : MonoBehaviour
{
    private const string IDLE_ANIMATION_KEY = "Idle";
    private const string SHAKING_ANIMATION_KEY = "Shaking";

    [SerializeField] private Vector3 shakingScale;
    [SerializeField] private GameObject shadowHolder;
    [SerializeField] private Transform cristalHolder;
    [SerializeField] private ItemType rewardType;

    public ItemType RewardType => rewardType;

    private IEnumerator shakingRoutine;

    private Vector3 defaultScale;
    private Vector3 defaultPostion;

    private void OnEnable()
    {
        defaultScale = cristalHolder.localScale;
        defaultPostion = cristalHolder.position;
    }

    public void ResetDisplay()
    {
        if (shakingRoutine != null)
        {
            StopCoroutine(shakingRoutine);
            shakingRoutine = null;
        }
        cristalHolder.localScale = defaultScale;
        cristalHolder.position = defaultPostion;
        shadowHolder.SetActive(false);
    }

    public void ShowShadow()
    {
        shadowHolder.SetActive(true);
    }

    public void Shake()
    {
        cristalHolder.localScale = shakingScale;
        shakingRoutine = ShakeInCircularShapeRoutine();
        StartCoroutine(shakingRoutine);
    }

    private IEnumerator ShakingRoutine()
    {
        Vector3 _originalPos = cristalHolder.position;
        float _shakeMagnitude = 10f;
        while (true)
        {
            cristalHolder.position = _originalPos + Random.insideUnitSphere * _shakeMagnitude * Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator ShakeInCircularShapeRoutine()
    {
        Vector3 _center = cristalHolder.position;
        float _radius = 0.1f;
        float _angle = 0.0f;
        float _moveSpeed = 0.1f;
        float _timeToWait = 0.01f;

        while (true)
        {
            _angle += Time.deltaTime * 50.0f;
            Vector3 offset = new Vector3(Mathf.Sin(_angle), Mathf.Cos(_angle), 0) * _radius;
            Vector3 _newPos = _center + offset;

            while (Vector3.Distance(cristalHolder.position, _newPos) > 0.01f)
            {
                cristalHolder.position = Vector3.MoveTowards(cristalHolder.position, _newPos, _moveSpeed * Time.deltaTime);
                yield return null;
            }

            yield return new WaitForSeconds(_timeToWait);
        }
    }
}
