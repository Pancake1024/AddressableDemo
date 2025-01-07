using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Test
{
    public class LoadFromReference : MonoBehaviour
    {
        public AssetReference _AssetReference;
        
        private AsyncOperationHandle<GameObject> _Handle;
        private GameObject _Obj;
        
        private ComponentReference<GameObject> _ComponentReference;

        private void Start()
        {
            _ComponentReference = new ComponentReference<GameObject>(_AssetReference.RuntimeKey.ToString());
            StartCoroutine(Test());
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _Handle = _AssetReference.LoadAssetAsync<GameObject>();
                _Handle.Completed += handle =>
                {
                    if (handle.Status == AsyncOperationStatus.Succeeded)
                    {
                        _Obj = Instantiate(handle.Result,transform);
                    }
                };
                
                // _Handle = _ComponentReference.InstantiateAsync();
                // _Handle.Completed += handle =>
                // {
                //     if (handle.Status == AsyncOperationStatus.Succeeded)
                //     {
                //         _Obj = handle.Result;
                //     }
                // };
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                if (_Obj != null)
                {
                    GameObject.Destroy(_Obj);
                    _Obj = null;
                    _AssetReference.ReleaseInstance(_Obj);
                    _AssetReference.ReleaseAsset();
                }
            }
            
            if (Input.GetKeyDown(KeyCode.P))
            {
                Debug.LogError(AddressablesUtility.GetAddressFromAssetReference(_AssetReference));
            }
        }

        private Dictionary<string,GameObject> _PreloadedObjects = new Dictionary<string, GameObject>();
        IEnumerator Test()
        {
            // var opHandle = Addressables.LoadResourceLocationsAsync(_AssetReference);
            // yield return opHandle;
            //
            // Debug.LogError(opHandle.Result[0].PrimaryKey);

            var loadResourceLocationHandle = Addressables.LoadResourceLocationsAsync("cube", typeof(GameObject));
            if (!loadResourceLocationHandle.IsDone)
                yield return loadResourceLocationHandle;
            
            var opList = new List<AsyncOperationHandle>(loadResourceLocationHandle.Result.Count);
            
            foreach (var location in loadResourceLocationHandle.Result)
            {
                AsyncOperationHandle<GameObject> loadAssetHandle = Addressables.LoadAssetAsync<GameObject>(location);
                loadAssetHandle.Completed += obj =>
                {
                    _PreloadedObjects.Add(location.PrimaryKey, obj.Result);
                };
                opList.Add(loadAssetHandle);
            }
            
            var groupOp = Addressables.ResourceManager.CreateGenericGroupOperation(opList);
            if (!groupOp.IsDone)
            {
                yield return groupOp;
            }
            
            loadResourceLocationHandle.Release();
            
            foreach (var item in _PreloadedObjects)
            {
                Debug.LogError(item.Key + " " +item.Value.name);
            }
        }
    }
}