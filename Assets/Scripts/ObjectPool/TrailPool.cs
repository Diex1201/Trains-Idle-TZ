using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class TrailPool : MonoBehaviour
{
    [SerializeField] private AssetReference trailPrefabReference;
    [SerializeField] private int poolSize = 10;
    private Queue<GameObject> pool = new();
    private GameObject trailPrefab;
    private AsyncOperationHandle<GameObject> trailHandle;
    private bool isPrefabLoaded = false;
    public event Action OnPrefabLoaded;
    private void Awake()
    {
        LoadPrefabAsync().Forget();
    }
    private void OnDestroy()
    {
        Addressables.Release(trailHandle);
    }
    private async UniTaskVoid LoadPrefabAsync()
	{
        trailHandle = Addressables.LoadAssetAsync<GameObject>(trailPrefabReference);
        await trailHandle.Task;
        if (trailHandle.Status == AsyncOperationStatus.Succeeded)
		{
            trailPrefab = trailHandle.Result;
            isPrefabLoaded = true;
            OnPrefabLoaded?.Invoke();
            for (int i = 0; i < poolSize; i++)
            {
                GameObject trail = Instantiate(trailPrefab, transform);
                trail.SetActive(false);
                pool.Enqueue(trail);
            }
        }
        else
        {
            Debug.LogError("Failed to load trail prefab from addressable");
        }
    }
    public async UniTask<GameObject> GetTrail()
    {
        if (!isPrefabLoaded)
        {
            var taskCompletionSource = new UniTaskCompletionSource();
            OnPrefabLoaded += () =>
            {
                taskCompletionSource.TrySetResult();
            };
            await taskCompletionSource.Task;
        }
        if (trailPrefab == null) return null;
        if (pool.Count > 0)
        {
            GameObject trail = pool.Dequeue();
            trail.SetActive(true);
            return trail;
        }
        else
        {
            GameObject trail = Instantiate(trailPrefab, transform);
            return trail;
        }
    }
    public void ReturnTrail(GameObject trail)
    {
        if (trail == null) return;
        trail.SetActive(false);
        pool.Enqueue(trail);
    }
}
