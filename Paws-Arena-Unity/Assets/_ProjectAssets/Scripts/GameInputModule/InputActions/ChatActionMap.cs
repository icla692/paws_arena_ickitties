public class ChatActionMap 
{

    private GameInputActions.ChatActions chatActions;

    public ChatActionMap(GameInputActions.ChatActions chatActions)
    {
        this.chatActions = chatActions;
    }

    public GameInputActions.ChatActions GetChatActions()
    {
        return chatActions;
    }

    public void SetActiveChatActionMap(bool value)
    {
        if(value)
        {
            chatActions.Enable();
        }
        else
        {
            chatActions.Disable();
        }
    }

}
