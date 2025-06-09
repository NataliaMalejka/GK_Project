using UnityEngine;

public interface IPoolable 
{
    void OnGetFromPool();
    void OnGetFromPool(Vector3 dir);
    void OnRetunToPool();
}
