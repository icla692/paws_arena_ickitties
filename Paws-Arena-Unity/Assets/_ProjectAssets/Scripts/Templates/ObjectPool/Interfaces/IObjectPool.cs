namespace Anura.Templates.ObjectPool.Interfaces
{
    internal interface IObjectPool<T>
    {
        T GetObjectFromPool(bool isActive);
        void AddObjectToPool(T component, bool isActive = false);
    }
}
