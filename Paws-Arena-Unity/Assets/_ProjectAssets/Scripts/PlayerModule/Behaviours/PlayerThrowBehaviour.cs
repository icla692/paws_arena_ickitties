using Anura.ConfigurationModule.Managers;
using Anura.ConfigurationModule.ScriptableObjects;
using Anura.Extensions;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThrowBehaviour : MonoBehaviour
{
    public static event Action<WeaponEntity> onLaunchPreparing;
    [SerializeField] private BasePlayerComponent playerComponent;

    [SerializeField] private PlayerIndicatorBehaviour indicator;
    [SerializeField] private Transform launchPoint;

    private Config config => ConfigurationManager.Instance.Config;

    private PhotonView photonView;
    private bool isMultiplayer;
    private bool isEnabled = false;

    private WeaponEntity currentWeapon;
    private List<GameObject> projectiles;

    private void OnEnable()
    {
        photonView = GetComponent<PhotonView>();
        isMultiplayer = ConfigurationManager.Instance.Config.GetIsMultiplayer();

        if (!isMultiplayer && photonView != null)
        {
            photonView.enabled = false;
            photonView = null;
        }

        isEnabled = true;
        if (!isMultiplayer || photonView.IsMine)
        {
            PlayerActionsBar.OnShoot += PrepareLaunch;
        }
    }

    public Transform GetLaunchPoint()
    {
        return launchPoint;
    }

    private void OnDisable()
    {
        isEnabled = false;
        if (!isMultiplayer || photonView.IsMine)
        {
            PlayerActionsBar.OnShoot -= PrepareLaunch;
        }
    }

    public void RegisterThrowCallbacks(GameInputActions.PlayerActions playerActions)
    {
        playerActions.Approve.performed += _ => PrepareLaunch();
    }
    public void RegisterThrowCallbacks(BotInputActions.PlayerActions playerActions)
    {
        playerActions.Approve.performed += _ => PrepareLaunch();
    }

    private void PrepareLaunch()
    {
        if (!isEnabled) return;
        isEnabled = false;

        int weaponIdx = playerComponent.state.weaponIdx;
        if (weaponIdx==0)
        {
            EventsManager.OnUsedRocket?.Invoke();
        }
        else if (weaponIdx==1)
        {
            EventsManager.OnUsedCannon?.Invoke();
        }
        else if (weaponIdx==2)
        {
            EventsManager.OnUsedTripleRocket?.Invoke();
        }
        else if (weaponIdx==3)
        {
            EventsManager.OnUsedAirplane?.Invoke();
        }
        else if (weaponIdx==4)
        {
            EventsManager.OnUsedMouse?.Invoke();
        }
        else if (weaponIdx==5)
        {
            EventsManager.OnUsedArrow?.Invoke();
        }
        currentWeapon = ConfigurationManager.Instance.Weapons.GetWeapon(weaponIdx);

        projectiles = new List<GameObject>();

        for (int i = 0; i < currentWeapon.numberOfProjectiles; i++) {
            GameObject obj = SingleAndMultiplayerUtils.Instantiate("Bullets/" + currentWeapon.bulletPrefab.name, launchPoint.position, Quaternion.Euler(transform.rotation.eulerAngles));
            
            projectiles.Add(obj);

            bool isMine = playerComponent.IsMine();
            obj.GetComponent<BulletComponent>().isMine = isMine;

            if(i != currentWeapon.numberOfProjectiles / 2)
            {
                obj.GetComponent<BulletComponent>().hasEnabledPositionTracking = false;
            }
        }

        //So that animation etc plays on all clients
        SingleAndMultiplayerUtils.RpcOrLocal(this, photonView, false, "OnLaunchPreparing", RpcTarget.All);
    }

    [PunRPC]
    public void OnLaunchPreparing()
    {
        onLaunchPreparing?.Invoke(currentWeapon);
        StartCoroutine(Launch());
    }

    public IEnumerator Launch()
    {
        if (photonView != null && !photonView.IsMine) yield break;

        //yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(.1f);
        int weaponIdx = playerComponent.state.weaponIdx;
        var weapon = ConfigurationManager.Instance.Weapons.GetWeapon(weaponIdx);

        float deviation = 10;
        for (int i = 0; i < projectiles.Count; i++)
        {
            float angle = deviation * (i - projectiles.Count / 2);            
            Vector3 direction = Quaternion.Euler(0, 0, angle) * launchPoint.up;

            projectiles[i].GetComponent<BulletComponent>().Launch(direction, GetBulletSpeed());
        }
        RoomStateManager.Instance.SetProjectileLaunchedState(weapon.waitBeforeTurnEnd);
    }

    private LineRenderer lr;
    private BotAIAim aiaim;
    /*private void Update()
    {
        if (lr == null)
        {
            lr = gameObject.AddComponent<LineRenderer>();
            lr.startWidth = 0.2f;

            aiaim = new BotAIAim(BotManager.Instance.Enemy.ToArray());
        }

        Vector3 dir = Quaternion.Euler(0, 0, 10 * (0 - 1 / 2)) * launchPoint.up;
        float force = GetBulletSpeed();

        var pos = aiaim.SimulateArc(dir, launchPoint.position, force, 1).ToArray();
        lr.positionCount = pos.Length;
        lr.SetPositions(pos);
    }*/

    private float GetBulletSpeed()
    {
        return config.GetBulletSpeed(indicator.currentPower);
    }
}