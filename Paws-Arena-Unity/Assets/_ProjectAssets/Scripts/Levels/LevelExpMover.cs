using UnityEngine;

public class LevelExpMover : MonoBehaviour
{
    private Transform target;
    private Vector3 offsetRange = new Vector3(1f, 1f, 1f);

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("MyExpBar").transform;
    }

    private void Start()
    {
        float offsetX = Random.Range(-offsetRange.x, offsetRange.x);
        float offsetY = Random.Range(-offsetRange.y, offsetRange.y);
        float offsetZ = Random.Range(-offsetRange.z, offsetRange.z);
        transform.position += new Vector3(offsetX, offsetY, offsetZ);
        
        LeanTween.move( gameObject,
                        Camera.main.ScreenToWorldPoint(target.position), 
                        Random.Range(3,5))
            .setEase(LeanTweenType.linear)
            .setOnComplete(() => { Destroy(gameObject); })
            .setDelay(1f);
    }
}
