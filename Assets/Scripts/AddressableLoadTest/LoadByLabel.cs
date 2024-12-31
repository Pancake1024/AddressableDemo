using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Serialization;

namespace Test
{
    public class LoadByLabel : MonoBehaviour
    {
        [FormerlySerializedAs("_Lables")] public List<string> _Keys = new List<string>(){"cube"};
        private AsyncOperationHandle<IList<GameObject>> _Handle;
        private List<GameObject> _Objects = new List<GameObject>();

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                float x =0;
                float z = 0;
                _Handle = Addressables.LoadAssetsAsync<GameObject>(_Keys, addresable =>
                {
                    if (addresable != null)
                    {
                        var obj = Instantiate<GameObject>(addresable, new Vector3(x++ * 2.0f, 0, z * 2.0f), Quaternion.identity,
                            transform);
                        if (x > 9)
                        {
                            x = 0;
                            z++;
                        }
                        _Objects.Add(obj);
                    }
                }, Addressables.MergeMode.Union, false);
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                foreach (var obj in _Objects)
                {
                    GameObject.Destroy(obj);
                }

                _Objects.Clear();
                _Handle.Release();
            }
        }

    }
}