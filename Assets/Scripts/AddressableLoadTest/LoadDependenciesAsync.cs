using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Test
{
    public class LoadDependenciesAsync : MonoBehaviour
    {
        void Start()
        {
            StartCoroutine(_PreLoadDependencies());
        }

        IEnumerator _PreLoadDependencies()
        {
            string key = "cube";

            // yield return Addressables.DownloadDependenciesAsync(key, true);
            
            AsyncOperationHandle<long> getDownloadSize = Addressables.GetDownloadSizeAsync(key);
            yield return getDownloadSize;

            if (getDownloadSize.Result > 0)
            {
                AsyncOperationHandle downloadDependencies = Addressables.DownloadDependenciesAsync(key);
                yield return downloadDependencies;
                if (downloadDependencies.Status == AsyncOperationStatus.Failed)
                {
                    Debug.LogError($"Failed to download dependencies for key {key}");
                }

                downloadDependencies.Release();
            }
            
         
            //清除AssetBundles缓存
            // Addressables.ClearDependencyCacheAsync(key, true);
        }
    }
}