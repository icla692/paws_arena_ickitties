using Anura.ConfigurationModule.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyBehaviour : MonoBehaviour
{
    public event Action<int> onDummyHit;
    public event Action<int> onDummyMiss;
    public event Action onDummyDead;
    [SerializeField]
    private GameObject dummyWrapper;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private List<Transform> possibleLocations;

    private PlayerDataCustomView npcHPBar;
    private int myHP;

    private void Start()
    {
        StartCoroutine(Init());
    }


    private IEnumerator Init()
    {
        myHP = ConfigurationManager.Instance.Dummy.dummyHP;
        while (PlayerDataCustomView.npcBar == null)
        {
            yield return null;
        }

        npcHPBar = PlayerDataCustomView.npcBar;
        npcHPBar.SetHealth(myHP);

        dummyWrapper.SetActive(false);
    }

    public void ReInit()
    {
        myHP = ConfigurationManager.Instance.Dummy.dummyHP;
        npcHPBar.SetHealth(myHP);
    }
    public void EnableDummy()
    {
        dummyWrapper.SetActive(true);
        Debug.Log("Setting Nickname");
        npcHPBar.SetNickname("Dummy");
        Debug.Log("Setting HP");
        myHP = ConfigurationManager.Instance.Dummy.dummyHP;
        npcHPBar.SetHealth(myHP);
        Debug.Log("Setting Events");
        AreaEffectsManager.Instance.OnAreaDamage += OnAreaDamage;

        Debug.Log("Done");
    }

    private void OnAreaDamage(Vector2 position, float area, int maxDamage, bool damageByDistance, bool hasPushForce, float pushForce, int bulletCount)
    {
        Vector3 dummyPos = dummyWrapper.transform.position;

        var closestPoint = dummyWrapper.GetComponent<BoxCollider2D>().ClosestPoint(position);
        float dmgDistance = Vector3.Distance(closestPoint, position);
        if (dmgDistance > area)
        {
            onDummyMiss?.Invoke(bulletCount);
            return;
        };

        float damagePercentage = (area - dmgDistance) / area;
        int dmgToBeDone = damageByDistance ? (int)Math.Floor(damagePercentage * maxDamage) : maxDamage;

        myHP -= dmgToBeDone;
        myHP = Math.Max(0, myHP);

        npcHPBar.SetHealth(myHP);

        animator.SetTrigger("IsHit");

        onDummyHit?.Invoke(bulletCount);

        if(myHP == 0)
        {
            onDummyDead?.Invoke();
        }
    }

    public void Relocate()
    {
        LeanTween.scale(dummyWrapper, Vector3.zero, 0.5f).setDelay(1f).setOnComplete(() =>
        {
            ReInit();
            dummyWrapper.transform.parent = possibleLocations[UnityEngine.Random.Range(0, possibleLocations.Count)];
            dummyWrapper.transform.localPosition = Vector3.zero;

            LeanTween.scale(dummyWrapper, Vector3.one, 0.5f).setDelay(0.5f);
        });
    }
}
