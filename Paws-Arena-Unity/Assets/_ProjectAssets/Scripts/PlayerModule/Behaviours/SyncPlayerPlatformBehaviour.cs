using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using UnityEngine;

public class SyncPlayerPlatformBehaviour : MonoBehaviour
{

    [SerializeField]
    private PlayerCustomization playerCustomization;

    [HideInInspector]
    public bool isBot = false;
    [HideInInspector]
    public PUNRoomUtils punRoomUtils;

    private PhotonView photonView;

    private void Awake()
    {
        transform.position = new Vector3(-100, 0, 0);
    }

    private async void Start()
    {
        photonView = GetComponent<PhotonView>();

        if (photonView.IsMine && !isBot)
        {
            var config = playerCustomization.SetCat(GameState.selectedNFT.imageUrl, GameState.selectedNFT.ids);
            string serializedConfig = JsonUtility.ToJson(config.GetSerializableObject());

            photonView.RPC("SetCatStyle", RpcTarget.Others, GameState.selectedNFT.imageUrl, serializedConfig);

            PUNRoomUtils.onPlayerJoined += OnPlayerJoined;
            SyncPlatformsBehaviour.onPlayersChanged += Reposition;
        }

        if (isBot)
        {
            playerCustomization.wrapper.SetActive(false);
        }

        Reposition();

        if (isBot)
        {
            NFT nft = new NFT()
            {
                imageUrl = GameState.botInfo.kittyUrl
            };

            await nft.GrabImage();
            playerCustomization.wrapper.SetActive(true);
            playerCustomization.SetTransientCat(nft.imageUrl, nft.ids);
        }
    }

    private void OnDestroy()
    {
        PUNRoomUtils.onPlayerJoined -= OnPlayerJoined;
        SyncPlatformsBehaviour.onPlayersChanged -= Reposition;
    }

    private void Reposition()
    {
        PlatformPose pose = SyncPlatformsBehaviour.Instance.GetMySeatPosition(photonView, isBot);
        transform.position = pose.pos;
        transform.localScale = pose.scale;

        //Set my seat on room props
        if ((photonView == null && !isBot) || photonView.IsMine)
        {
            punRoomUtils.AddPlayerCustomProperty("seat", "" + pose.seatIdx);
        }
    }

    private void OnPlayerJoined(string nickname, string userId)
    {
        Player player = PhotonNetwork.PlayerList.First(player => player.UserId == userId);
        string serializedConfig = JsonUtility.ToJson(KittiesCustomizationService.GetCustomization(GameState.selectedNFT.imageUrl).GetSerializableObject());
        photonView.RPC("SetCatStyle", player, GameState.selectedNFT.imageUrl, serializedConfig);
    }

    [PunRPC]
    public void SetCatStyle(string url, string configJson)
    {
        KittyCustomization customization = JsonUtility.FromJson<KittyCustomization.KittyCustomizationSerializable>(configJson).GetNonSerializable();
        playerCustomization.SetTransientCat(url, customization);
    }



}
