using Anura.ConfigurationModule.Managers;
using Anura.Extensions;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerGraphicsBehaviour : MonoBehaviour
{
    public event Action onCatFlipped;
    [SerializeField]
    private AudioClip jumpStartSound;
    [SerializeField]
    private AudioClip jumpEndSound;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private PlayerCustomization playerCustomization;
    [SerializeField]
    private Transform weaponParent;
    [SerializeField]
    private Transform damageDisplay;

    private bool isMultiplayer;
    private PhotonView _photonView;
    private PlayerState playerState;

    [HideInInspector]
    public bool isFacingRight = true;

    private Vector3 initialWeaponRotationOffset;

    private void Start()
    {
        isMultiplayer = ConfigurationManager.Instance.Config.GetIsMultiplayer();

        _photonView = GetComponent<PhotonView>();
        if (_photonView != null && !isMultiplayer)
        {
            _photonView.enabled = false;
            _photonView = null;
        }

        //StringBuilder ids = new StringBuilder();
        //foreach (string id in GameState.selectedNFT.ids)
        //{
        //    ids.Append(id);
        //    ids.Append(",");
        //}

        if ((!isMultiplayer || _photonView.IsMine) && transform.parent.GetComponent<BotPlayerComponent>() == null)
        {
            string serializedConfig = JsonUtility.ToJson(KittiesCustomizationService.GetCustomization(GameState.selectedNFT.imageUrl).GetSerializableObject());
            SingleAndMultiplayerUtils.RpcOrLocal(this, _photonView, true, "SetCatNFT", RpcTarget.All, GameState.selectedNFT.imageUrl, serializedConfig);
        }
    }

    [PunRPC]
    public void SetCatNFT(string url, string serializedConfig)
    {
        KittyCustomization customization = JsonUtility.FromJson<KittyCustomization.KittyCustomizationSerializable>(serializedConfig).GetNonSerializable();
        playerCustomization.SetTransientCat(url, customization);
    }

    public async void SetCustomCatNFT(string url)
    {
        NFT nft = new NFT()
        {
            imageUrl = url
        };

        await nft.GrabImage();
        playerCustomization.SetTransientCat(url, nft.ids);
    }

    public void RegisterPlayerState(PlayerState playerState)
    {
        RegisterBasePlayerState(playerState);
        PlayerManager.Instance.onHealthUpdated += OnHealthUpdated;
    }

    public void RegisterBasePlayerState(PlayerState playerState)
    {
        this.playerState = playerState;
        this.playerState.onJumpStateChanged += OnJumpStateChanged;
        this.playerState.onMovementDirectionChanged += OnMovementDirectionChanged;
        this.playerState.onMidJumpChanged += OnMidJumpStateChanged;
    }

    public void PreJumpAnimEnded()
    {
        if (!isMultiplayer || _photonView.IsMine)
        {
            this.playerState.SetQueueJumpImpulse(true);
            SFXManager.Instance.PlayOneShot(jumpStartSound);
        }
    }

    public void SetIsMidJump()
    {
        if (!isMultiplayer || _photonView.IsMine)
        {
            this.playerState.SetIsMidJump(true);
        }
    }

    public void AfterJump()
    {
        if (!isMultiplayer || _photonView.IsMine)
        {
            this.playerState.SetHasJump(false);
        }
    }

    private void OnJumpStateChanged(bool val)
    {
        if (val)
        {
            animator.SetBool("isJumping", true);
        }
    }

    private void OnMidJumpStateChanged(bool midJumpState)
    {
        if (!midJumpState)
        {
            animator.SetBool("isJumping", false);
            SFXManager.Instance.PlayOneShot(jumpEndSound);
        }
    }

    private void OnMovementDirectionChanged(float dir)
    {
        if (dir > 0 && !isFacingRight)
        {
            SingleAndMultiplayerUtils.RpcOrLocal(this, _photonView, true, "Flip", RpcTarget.All);
        }
        else if (dir < 0 && isFacingRight)
        {
            SingleAndMultiplayerUtils.RpcOrLocal(this, _photonView, true, "Flip", RpcTarget.All);
        }

        animator.SetBool("isMoving", dir != 0);
    }

    [PunRPC]
    public void Flip()
    {
        // Switch the way the player is labelled as facing.
        isFacingRight = !isFacingRight;

        // Multiply the player's x local scale by -1.
        var theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;

        //Corrections
        //Weapons
        if (!isFacingRight)
        {
            initialWeaponRotationOffset = weaponParent.GetComponent<RotationConstraint>().rotationOffset;
            weaponParent.GetComponent<RotationConstraint>().rotationOffset += Vector3.zero.WithZ(180);
        }
        else if(initialWeaponRotationOffset != Vector3.zero)
        {
            weaponParent.GetComponent<RotationConstraint>().rotationOffset = initialWeaponRotationOffset;
        }

        //DamageDisplay
        damageDisplay.localScale = new Vector3(isFacingRight ? 1 : -1, 1, 1);

        onCatFlipped?.Invoke();
    }


    public void OnHealthUpdated(int health)
    {
        if (health <= 0)
        {
            animator.SetBool("isDead", true);
        }
    }

    public void SetShootingPhase(bool val)
    {
        animator.SetBool("isAiming", val);
    }
}
