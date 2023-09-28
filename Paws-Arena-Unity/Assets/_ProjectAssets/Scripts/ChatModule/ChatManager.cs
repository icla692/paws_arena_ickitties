using Anura.ConfigurationModule.Managers;
using Anura.Templates.MonoSingleton;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ChatLinePool))]
public class ChatManager : MonoSingleton<ChatManager>
{
    [SerializeField] private ChatBehaviour chatBehaviour;
    [SerializeField] private NotificationChatBehaviour notificationChatBehaviour;
    [SerializeField] private Button openChat;

    private PhotonView chatPhotonView;
    private ChatLinePool chatLinePool;

    private void Start()
    {
        openChat.onClick.AddListener(() => SetActiveChat(true));
        chatLinePool = GetComponent<ChatLinePool>();
        chatPhotonView = GetComponent<PhotonView>();
        
    }

    public bool GetChatPanelStatus()
    {
        return chatBehaviour.gameObject.activeSelf;
    }

    public ChatLinePool GetChatLinePool()
    {
        return chatLinePool;
    }

    public PhotonView GetChatPhotonView()
    {
        return chatPhotonView;
    }

    public void SetActiveNotification(bool value)
    {
        notificationChatBehaviour.gameObject.SetActive(value);
    }

    public void SetActiveChat(bool value)
    {
        SetChatPanelBehaviour(value);
        GameInputManager.Instance.GetChatActionMap().SetActiveChatActionMap(value);
    }

    private void SetChatPanelBehaviour(bool isActive)
    {
        chatBehaviour.gameObject.SetActive(isActive);

        bool shouldBeAbleToMove = !isActive && RoomStateManager.Instance.currentState is MyTurnState;
        Debug.Log("Should be able to move " + shouldBeAbleToMove);
        GameInputManager.Instance.GetPlayerActionMap().SetActivePlayerActionMap(shouldBeAbleToMove);
    }

    [PunRPC]
    private void ChatMessage(string message, PhotonMessageInfo info)
    {
        if (info.Sender.IsLocal)
        {
           chatBehaviour.CleanInputField();
        }
        else if(!GetChatPanelStatus())
        {
            SetActiveNotification(true);
        }

        chatBehaviour.DisplayMessage(message, info.Sender.IsLocal);
    }
}