using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public static class AddressablesExtensions
{
    public static Task<GameObject> InstantiateAsyncAwait(this AssetReference assetRef)
    {
        var tcs = new TaskCompletionSource<GameObject>();
        var handle = assetRef.InstantiateAsync();

        handle.Completed += operation =>
        {
            if (operation.Status == AsyncOperationStatus.Succeeded)
                tcs.SetResult(operation.Result);
            else
                tcs.SetException(new System.Exception("Failed to instantiate Addressable: " + assetRef.RuntimeKey));
        };

        return tcs.Task;
    }
}

public class Initializer 
{
    [Header("Permament")]
    [SerializeField] private AssetReference managersPrefab;
    [SerializeField] private AssetReference ui;

    [Header("CurrentScene")]
    [SerializeField] private AssetReference cameraPrefab;
    [SerializeField] private AssetReference environmentPrefab;

    private async void Awake()
    {
        await CurrentAsync();
        await PermamentAsync();
    }

    private async Task PermamentAsync()
    {
        
    }

    private async Task CurrentAsync()
    {
        if (!string.IsNullOrEmpty(cameraPrefab?.AssetGUID))
        {
            await cameraPrefab.InstantiateAsyncAwait();
        }

        if (!string.IsNullOrEmpty(environmentPrefab?.AssetGUID))
        {
            await environmentPrefab.InstantiateAsyncAwait();
        }
    }
}
