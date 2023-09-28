using Anura.ConfigurationModule.Managers;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBehaviour : MonoBehaviour
{
    public PlayerThrowBehaviour playerThrowBehaviour;

    public GameObject weaponWrapper;
    public Transform weaponDirectParent;

    private GameObject instantiatedWeapon;

    private void OnEnable()
    {
        PlayerThrowBehaviour.onLaunchPreparing += OnLaunchPrepared;
    }

    private void OnDisable()
    {
        PlayerThrowBehaviour.onLaunchPreparing -= OnLaunchPrepared;
        weaponWrapper.SetActive(false);
    }

    private void OnLaunchPrepared(WeaponEntity obj)
    {
        var animator = instantiatedWeapon.transform.GetChild(0).GetComponent<Animator>();
        if(animator != null)
        {
            animator.SetTrigger("isShot");
        }
    }

    public void Init(int weaponIdx)
    {
        if (weaponIdx < 0)
        {
            return;
        }


        weaponWrapper.SetActive(true);

        var weapon = ConfigurationManager.Instance.Weapons.GetWeapon(weaponIdx);

        if(instantiatedWeapon != null)
        {
            Destroy(instantiatedWeapon);
        }

        instantiatedWeapon = GameObject.Instantiate(weapon.launcherPrefab, weaponDirectParent);
    }

}
