using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AsynchronousLoading : MonoBehaviour
{
        private string _Address = "Assets/Raw/Prefabs/Cubes/GreenCube.prefab";

        private AsyncOperationHandle _LoadHandle;

        IEnumerator _LoadAssetCoroutine()
        {
                _LoadHandle = Addressables.LoadAssetAsync<GameObject>(_Address);
                yield return _LoadHandle;
        }

        void _LoadAssetCallBack()
        {
                _LoadHandle = Addressables.LoadAssetAsync<GameObject>(_Address);
                _LoadHandle.Completed += handle =>
                {
                        if (handle.Status == AsyncOperationStatus.Succeeded)
                        {
                                
                        }
                };
        }

        async void _LoadAssetWait()
        {
                _LoadHandle = Addressables.LoadAssetAsync<GameObject>(_Address);
                await _LoadHandle.Task;
        }

        private void OnDestroy()
        {
                _LoadHandle.Release();
        }
}