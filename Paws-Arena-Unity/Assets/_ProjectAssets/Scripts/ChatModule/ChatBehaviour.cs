using Anura.ConfigurationModule.Managers;
using Anura.ConfigurationModule.ScriptableObjects;
using Anura.Extensions;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatBehaviour : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private ChatLine chatLine;


    [SerializeField] private Button sendButton;
    [SerializeField] private Button chatBackground;
    [SerializeField] private Button xChatButton;

    [SerializeField] private RectTransform content;

    private void OnEnable()
    {
        ChatManager.Instance.SetActiveNotification(false);
        FocusInputField();
    }

    private void Start()
    {   
        chatBackground.onClick.AddListener(() => ChatManager.Instance.SetActiveChat(false));
        xChatButton.onClick.AddListener(() => ChatManager.Instance.SetActiveChat(false));
        sendButton.onClick.AddListener(SendMessage);

        InitializeChat(ConfigurationManager.Instance.Config);
        RegisterSendMessage(GameInputManager.Instance.GetChatActionMap());
        RegisterCloseChat(GameInputManager.Instance.GetChatActionMap());
    }

    public void DisplayMessage(string message, bool isLocal)
    {
        var line = ChatManager.Instance.GetChatLinePool().GetObjectFromPool();
        ChatManager.Instance.GetChatLinePool().AddObjectToPool(line);
        line.SetText(message, isLocal);

        content.anchoredPosition = content.anchoredPosition.WithY(ConfigurationManager.Instance.Config.GetHeightRefreshingChat());
    }

    public void CleanInputField()
    {
        inputField.text = string.Empty;
    }

    private void InitializeChat(Config config)
    {
        for (int i = 0; i < config.GetNumberOfLines(); i++)
        {
            var line = Instantiate(chatLine, content.transform);
            ChatManager.Instance.GetChatLinePool().AddObjectToPool(line);
        }

        content.sizeDelta = content.sizeDelta.WithY(config.GetNumberOfLines() * config.GetChatLineHeight());
    }

    private void RegisterCloseChat(ChatActionMap chatActionMap)
    {
        chatActionMap.GetChatActions().Close.started += _ =>
        {
            ChatManager.Instance.SetActiveChat(false);
        };
    }

    private void RegisterSendMessage(ChatActionMap chatActionMap)
    {
        chatActionMap.GetChatActions().Send.started += _ =>
        {
            SendMessage();
            FocusInputField();
        };
    }

    private void FocusInputField()
    {
        inputField.ActivateInputField();
    }

    private void SendMessage()
    {
        if (inputField.text.IsEmptyOrWhiteSpace())
            return;

        ChatManager.Instance.GetChatPhotonView().RPC("ChatMessage", RpcTarget.All, inputField.text);
    }


}
