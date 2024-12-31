using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Test
{
    public class AddressableTestCode : MonoBehaviour
    {
            public        AssetReferenceTexture2D    texture2D;
            public AssetReferenceMaterial material;
            
            [AssetReferenceUILabelRestriction("cube")]
            public AssetReference labelRestrictedReference;
            
            void Start()
            {
                var tex2dHandle = texture2D.LoadAssetAsync();
                tex2dHandle.Completed += handle =>
                {
                    var tex2d = handle.Result;
                    Debug.Log(tex2d);
                };
                
                var materialHandle = material.LoadAssetAsync();
                materialHandle.Completed += handle =>
                {
                    var mat = handle.Result;
                    Debug.Log(mat);
                };
            }
    }

    [Serializable]
    public class AssetReferenceMaterial : AssetReferenceT<Material>
    {
        public AssetReferenceMaterial(string guid) : base(guid)
        {
        }
    }
}