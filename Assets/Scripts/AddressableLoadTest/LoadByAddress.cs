using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Test
{
    public class LoadByAddress : MonoBehaviour
    {
        public string _Address;
        private AsyncOperationHandle<GameObject> _Handle;
        private GameObject _Obj;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _Handle = Addressables.LoadAssetAsync<GameObject>(_Address);
                _Handle.Completed += handle =>
                {
                    if (handle.Status == AsyncOperationStatus.Succeeded)
                    {
                        _Obj = Instantiate(handle.Result,transform);
                    }
                };
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                if (_Obj != null)
                {
                    GameObject.Destroy(_Obj);
                    _Obj = null;
                    _Handle.Release();
                }
            }
        }
    }
}