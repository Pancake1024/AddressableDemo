using UnityEngine;
using UnityEngine.UI;

namespace Party
{
    public class RawImageLoaderWrapper : AbstractLoaderWrapper
    {
        public RawImage RawImage;
        
        protected override void OnAwake()
        {
            RawImage = GetComponent<RawImage>();
        }

        protected override void LoadAssetAsync(string path)
        {
            AssetLoaderManager.Instance.AddLoaderWrapper(this, obj =>
            {
                if (_IsDestory)
                {
                    return;
                }
                if (RawImage != null)
                {
                    RawImage.texture = obj as Texture;
                }
            });
        }
    }
}