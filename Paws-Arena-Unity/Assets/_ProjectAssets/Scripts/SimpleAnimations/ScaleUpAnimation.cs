using UnityEngine;

public class ScaleUpAnimation : MonoBehaviour
{
    [SerializeField] private float animationLength = 0.2f;

    private void OnEnable()
    {
        transform.localScale = Vector3.zero;
        gameObject.LeanScale(Vector3.one, animationLength);
    }
}
