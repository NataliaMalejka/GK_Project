using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public static class SceneLoader 
{
    private static AsyncOperationHandle<SceneInstance>? _currentSceneHandle;
 
    public static async Task LoadSceneSingle(string sceneKey)
    {
        Debug.Log(sceneKey);

        if (_currentSceneHandle.HasValue && _currentSceneHandle.Value.IsValid())
        {
            await Addressables.UnloadSceneAsync(_currentSceneHandle.Value).Task;
        }

        var newSceneHandle = Addressables.LoadSceneAsync(sceneKey, LoadSceneMode.Single);
        await newSceneHandle.Task;
        _currentSceneHandle = newSceneHandle;

        Player.Instance.transform.position = Vector3.zero;
    }

    public static async Task LoadSceneAdditive(string sceneKey)
    {
        var handle = Addressables.LoadSceneAsync(sceneKey, LoadSceneMode.Additive);
        await handle.Task;
    }

    public static async Task UnloadScene(string sceneName)
    {
        UnityEngine.SceneManagement.Scene sceneToUnload = SceneManager.GetSceneByName(sceneName);

        if (sceneToUnload.IsValid() && sceneToUnload.isLoaded)
        {
            AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(sceneToUnload.buildIndex);
            while (!unloadOp.isDone)
                await Task.Yield();
        }
    }
}
