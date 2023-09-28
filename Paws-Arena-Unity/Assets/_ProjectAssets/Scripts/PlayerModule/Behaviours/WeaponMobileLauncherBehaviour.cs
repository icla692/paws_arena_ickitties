using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMobileLauncherBehaviour : MonoBehaviour
{
    //private Animator _animator;
    //public AudioClip sfx;

    private void Start()
    {
        //_animator = GetComponent<Animator>();
        PlayerThrowBehaviour.onLaunchPreparing += PrepareLaunch;
    }

    private void OnDestroy()
    {
        PlayerThrowBehaviour.onLaunchPreparing -= PrepareLaunch;
    }

    private void PrepareLaunch(WeaponEntity weapon)
    {
        //_animator.SetTrigger("Shoot");
        //if (sfx != null)
        //{
        //    SFXManager.Instance.PlayOneShot(sfx);
        //}
        //StartCoroutine(Launch());
    }
    
    //public IEnumerator Launch()
    //{
    //    //throwBehaviour.Launch();
    //}
}
