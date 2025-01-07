using UnityEngine;
using UnityEngine.UI;

namespace Party
{
    public class ImageLoaderWrapper : AbstractLoaderWrapper
    {
        public Image Image;
        
        protected override void OnAwake()
        {
            Image = GetComponent<Image>();
        }

        protected override void LoadAssetAsync(string path)
        {
            AssetLoaderHelper.AddLoaderWrapper(this, obj =>
            {
                if (Image != null)
                {
                    Image.sprite = obj as Sprite;
                }
            });
        }
    }
}