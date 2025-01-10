using UnityEngine;

namespace Party
{
    public class MeshLoaderWrapper : AbstractLoaderWrapper
    {
        public MeshFilter MeshFilter;
        
        protected override void OnAwake()
        {
            MeshFilter = GetComponent<MeshFilter>();
        }

        protected override void LoadAssetAsync(string path)
        {
            AssetLoaderManager.Instance.AddLoaderWrapper(this, obj =>
            {
                if (MeshFilter != null)
                {
                    MeshFilter.mesh = obj as Mesh;
                }
            });
        }
    }
}