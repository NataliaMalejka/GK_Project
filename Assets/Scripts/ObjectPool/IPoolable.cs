using UnityEngine;

public interface IPoolable 
{
    void SetPool<T>(ObjectPool<T> pool) where T : MonoBehaviour, IPoolable;
    void OnGetFromPool();
    void OnGetFromPool(Vector3 dir);
    void OnRetunToPool();
}
