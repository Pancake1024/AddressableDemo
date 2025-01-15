using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace Party
{
    public class AssetWrapper : IAssetWrapper
    {
        private string _Path;
        private object _Asset;
        private List<Action<object>> _PendingCallbacks = new List<Action<object>>();
        private LoaderStatus _LoaderStatus = LoaderStatus.None;
        private AsyncOperationHandle _AsyncOperationHandle;

        public void LoadAssetAsync<T>(string path, Action<T> callback) where T : Object
        {
            _Path = path;
            if (_AsyncOperationHandle.IsValid())
            {
                if (_AsyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
                {
                    callback?.Invoke(_Asset as T);
                }else if (_AsyncOperationHandle.Status == AsyncOperationStatus.Failed)
                {
                    callback?.Invoke(null);
                }
                else
                {
                    _PendingCallbacks.Add(obj =>
                    {
                        callback?.Invoke(obj as T);
                    });    
                }

                return;
            }
            
            _LoaderStatus = LoaderStatus.Loading;
            _AsyncOperationHandle = Addressables.LoadAssetAsync<T>(path);
            _AsyncOperationHandle.Completed += handle =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    _Asset = handle.Result;
                    _LoaderStatus = LoaderStatus.Loaded;
                    callback?.Invoke(handle.Result as T);
                    foreach (var pendingCallback in _PendingCallbacks)
                    {
                        pendingCallback?.Invoke(handle.Result);
                    }
                    _PendingCallbacks.Clear();
                }
                else
                {
                    _LoaderStatus = LoaderStatus.Failed;
                    callback?.Invoke(null);
                    foreach (var pendingCallback in _PendingCallbacks)
                    {
                        pendingCallback?.Invoke(null);
                    }
                    _PendingCallbacks.Clear();
                }
            };
            
        }

        public LoaderStatus GetLoaderStatus()
        {
            return _LoaderStatus;
        }

        public void Release()
        {
            if (_AsyncOperationHandle.IsValid())
            {
                Addressables.Release(_AsyncOperationHandle);
            }
            _Asset = null;
            _LoaderStatus = LoaderStatus.None;
            _AsyncOperationHandle = default;
            _PendingCallbacks.Clear();
        }
    }    
}
