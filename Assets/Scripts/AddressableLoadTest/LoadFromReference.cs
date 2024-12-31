using System;
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
    }
}