using Anura.Templates.ObjectPool.BaseScripts;

public class ChatLinePool : BaseObjectPool<ChatLine>
{
    public override ChatLine GetObjectFromPool(bool isActive = true)
    {
        var line = pool.Dequeue();
        line.transform.SetAsLastSibling();
        return line;
    }

    public override void AddObjectToPool(ChatLine component, bool isActive = false)
    {
        pool.Enqueue(component);
    }

    protected override ChatLine GetComponent()
    {
        return null;
    }
}
