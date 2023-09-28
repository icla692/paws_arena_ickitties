using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class EmojisHandler : MonoBehaviour
{
    [SerializeField] private Button closeButton; //available when emojis are shown
    [SerializeField] private Button showEmojisButton;
    [SerializeField] private Transform emojisHolder;
    [SerializeField] private EmojiPreviewDisplay emojiPrefab;
    [SerializeField] private EmojiInGame emojiInGamePrefab;

    private PhotonView photonView;
    private List<EmojiPreviewDisplay> shownEmojis = new List<EmojiPreviewDisplay>();
    
    private void Awake()
    {
        AddEmojis();
        if (PhotonNetwork.CurrentRoom!=null)
        {
            photonView = GetComponent<PhotonView>();
        }
    }

    private void AddEmojis()
    {
        foreach (var _emojiId in DataManager.Instance.PlayerData.OwnedEmojis)
        {
            EmojiSO _emojiSo = EmojiSO.Get(_emojiId);
            EmojiPreviewDisplay _emoji = Instantiate(emojiPrefab, emojisHolder);
            shownEmojis.Add(_emoji);
            _emoji.Setup(_emojiSo);
        }
    }

    private void OnEnable()
    {
        closeButton.onClick.AddListener(CloseEmojis);
        showEmojisButton.onClick.AddListener(ShowEmojis);
        EmojiPreviewDisplay.OnEmojiClicked += ShowEmoji;
    }

    private void OnDisable()
    {
        closeButton.onClick.RemoveListener(CloseEmojis);
        showEmojisButton.onClick.RemoveListener(ShowEmojis);  
        EmojiPreviewDisplay.OnEmojiClicked -= ShowEmoji;
    }

    private void ShowEmojis()
    {
        float _scaleDuration = 0.2f;
        LeanTween.scale(showEmojisButton.gameObject, new Vector3(1.2f,1.2f,1.2f), _scaleDuration/2f)
            .setEase(LeanTweenType.easeOutCubic).
            setOnComplete(() =>
            {
                LeanTween.scale(showEmojisButton.gameObject, new Vector3(0.5f, 0.5f, 0.5f), _scaleDuration)
                    .setEase(LeanTweenType.easeOutCubic)
                    .setOnComplete(() =>
                    {
                        float _emojiAnimationDuration = 0.1f;
        
                        LeanTween.scale(emojisHolder.gameObject, Vector3.one, _emojiAnimationDuration*DataManager.Instance.PlayerData.OwnedEmojis.Count)
                            .setEase(LeanTweenType.easeOutCubic);

                        for (int _i = 0; _i < shownEmojis.Count; _i++)
                        {
                            shownEmojis[_i].Show(_emojiAnimationDuration,_emojiAnimationDuration*_i);
                        }
        
                        Invoke(nameof(EnableCloseButton), 0.2f);
                    });
            });
    }

    private void EnableCloseButton()
    {
        closeButton.gameObject.SetActive(true);
    }

    private void CloseEmojis()
    {
        closeButton.gameObject.SetActive(false);
        float _emojiAnimationDuration = 0.1f;
        Vector3 _targetScale = new Vector3(1, 1, 1);
        float _scaleDuration = 0.1f;
        LeanTween.scale(showEmojisButton.gameObject, _targetScale, _scaleDuration)
            .setEase(LeanTweenType.easeOutCubic)
            .setDelay(_emojiAnimationDuration*shownEmojis.Count);


        LeanTween.scale(emojisHolder.gameObject, Vector3.zero, _emojiAnimationDuration*DataManager.Instance.PlayerData.OwnedEmojis.Count)
            .setEase(LeanTweenType.easeOutCubic)
            .setDelay((_emojiAnimationDuration*shownEmojis.Count)/2);

        for (int _i = shownEmojis.Count-1; _i >= 0; _i--)
        {
            shownEmojis[_i].Hide(_emojiAnimationDuration,_emojiAnimationDuration*(shownEmojis.Count-_i));
        }    
    }

    private void ShowEmoji(int _id)
    {
        ShowEmoji(_id,true);
        CloseEmojis();
        if (photonView!=null)
        {
            photonView.RPC(nameof(ShowEmoji),RpcTarget.Others,_id,false);
        }
    }


    [PunRPC]
    private void ShowEmoji(int _emojiId, bool _showAboveMyHead)
    {
        Transform _emojiHolder = _showAboveMyHead ? PlayerManager.Instance.myPlayer.EmojiHolder : PlayerManager.Instance.OtherPlayerComponent.EmojiHolder;
        EmojiSO _emojiSo = EmojiSO.Get(_emojiId);

        EmojiInGame _emojiVisual = Instantiate(emojiInGamePrefab, _emojiHolder);
        _emojiVisual.Setup(_emojiSo);
    }
}
