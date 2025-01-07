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
            AssetLoaderHelper.AddLoaderWrapper(this, obj =>
            {
                if (RawImage != null)
                {
                    RawImage.texture = obj as Texture;
                }
            });
        }
    }
}