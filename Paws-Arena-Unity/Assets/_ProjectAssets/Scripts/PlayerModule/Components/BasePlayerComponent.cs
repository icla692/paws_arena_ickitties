using Anura.ConfigurationModule.Managers;
using Anura.ConfigurationModule.ScriptableObjects;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePlayerComponent : MonoBehaviour
{
    public event Action<int> onDamageTaken;
    [HideInInspector]
    public PlayerState state;
    [HideInInspector]
    public int playerSeat = 0;
    [SerializeField]
    private GameObject weaponWrapper;

    private PlayerMotionBehaviour playerMotionBehaviour;
    private bool isMultiplayer;
    private PhotonView photonView;

    private void OnEnable()
    {
        AreaEffectsManager.Instance.OnAreaDamage += AreaDamage;
    }

    private void OnDisable()
    {
        AreaEffectsManager.Instance.OnAreaDamage -= AreaDamage;
    }

    private void AreaDamage(Vector2 position, float area, int maxDamage, bool damageByDistance, bool hasPushForce, float pushForce, int bulletCount)
    {
        //In Tutorial, players cannot take bullet damage
        if(ConfigurationManager.Instance.Config.GetGameType() == GameType.TUTORIAL)
        {
            return;
        }

        Vector3 playerPos = transform.position;

        var closestPoint = GetComponent<CapsuleCollider2D>().ClosestPoint(position);
        float dmgDistance = Vector3.Distance(closestPoint, position);

        if (dmgDistance > area) return;

        float damagePercentage = (area - dmgDistance) / area;
        int dmgToBeDone = damageByDistance ? (int)Math.Floor(damagePercentage * maxDamage) : maxDamage;

        onDamageTaken?.Invoke(dmgToBeDone);

        if (hasPushForce)
        {
            Vector2 direction = new Vector2(playerPos.x, playerPos.y) - position;
            PushPlayer(damagePercentage * pushForce, direction);
        }
    }

    public void GiveDamage(int dmg)
    {
        onDamageTaken?.Invoke(dmg);
    }

    private void PushPlayer(float force, Vector2 direction)
    {
        GetComponent<Rigidbody2D>().AddForce(direction.normalized * force, ForceMode2D.Impulse);
    }

    private void Awake()
    {
        isMultiplayer = ConfigurationManager.Instance.Config.GetIsMultiplayer();
        photonView = GetComponent<PhotonView>();
        playerMotionBehaviour = GetComponent<PlayerMotionBehaviour>();
    }

    private void OnDestroy()
    {
        if (state != null)
        {
            state.onWeaponIdxChanged -= OnWeaponOutChanged;
            PlayerActionsBar.WeaponIndexUpdated -= ChangeWeaponState;
        }
    }

    public void PreSetup()
    {
        state = new PlayerState();
        state.onWeaponIdxChanged += OnWeaponOutChanged;
    }
    public void PostSetup()
    {
        PlayerActionsBar.WeaponIndexUpdated += ChangeWeaponState;
        ChangeWeaponState(-1);
    }

    private void ChangeWeaponState(int idx)
    {
        if (state.weaponIdx == idx)
        {
            idx = -1;
            state.SetHasWeaponOut(idx);
        }
        if (playerSeat == RoomStateManager.Instance.lastPlayerRound)
        {
            state.SetHasWeaponOut(idx);
        }
    }

    private void OnWeaponOutChanged(int val)
    {
        playerMotionBehaviour.SetIsPaused(val >= 0);

        SingleAndMultiplayerUtils.RpcOrLocal(this, photonView, false, "NetworkedChangeWeaponState", RpcTarget.All, val);
    }

    public bool IsMine()
    {
        if (isMultiplayer)
        {
            return photonView.IsMine;
        }
        else
        {
            if (GetComponent<PlayerComponent>() != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    [PunRPC]
    public void NetworkedChangeWeaponState(int val)
    {
        weaponWrapper.SetActive(val >= 0);
        weaponWrapper.GetComponent<WeaponBehaviour>().Init(val);
    }
}
