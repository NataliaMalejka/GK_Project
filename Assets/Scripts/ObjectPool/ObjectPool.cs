using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T: MonoBehaviour, IPoolable
{
    private readonly Queue<T> pool;
    private readonly T prefab;

    public ObjectPool(int amound, T prefab)
    {
        this.prefab = prefab;
        pool = new Queue<T>(amound);

        Init(amound);
    }

    private void Init(int amound)
    {
        for(int i=0; i<amound; i++)
        {
            T obj =CreateNewObject();
            pool.Enqueue(obj);
        }
    }

    private T CreateNewObject()
    {
        T obj = Object.Instantiate(prefab);
        obj.gameObject.SetActive(false);
        return obj;
    }

    public T GetObjectFromPool(Vector3? spawnPosition = null, Vector3? dir = null)
    {
        T obj = (pool.Count > 0) ? pool.Dequeue() : CreateNewObject();
        obj.gameObject.SetActive(true);

        if (spawnPosition.HasValue)
        {
            obj.transform.position = spawnPosition.Value;
        }

        if (dir.HasValue)
        {
            obj.OnGetFromPool(dir.Value);
        }
        else
        {
            obj.OnGetFromPool();
        }
        
        return obj;
    }

    public void ReturnToPool(T obj)
    {
        obj.OnRetunToPool();
        obj.gameObject.SetActive(false);
        pool.Enqueue(obj);
    }
}
