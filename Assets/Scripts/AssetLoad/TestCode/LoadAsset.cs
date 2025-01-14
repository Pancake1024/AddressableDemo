using UnityEngine;
using UnityEngine.UI;

namespace Party
{
    public class LoadAsset : MonoBehaviour
    {
        public GameObject[] Objs;

        public Image Image;
        public RawImage RawImage;
        public Material Material;
        
        private void Start()
        {
            AssetLoaderManager.Instance.InstantiateAsync("Assets/Raw/Prefabs/Cubes/BlueCube.prefab", go =>
            {
                go.transform.position = new Vector3(-2, 0, 0); 
            });
                
            AssetLoaderManager.Instance.InstantiateAsync("Assets/Raw/Prefabs/Cubes/RedCube.prefab", go =>
            {
                go.transform.position = new Vector3(2, 0, 0);
            });
                
            AssetLoaderManager.Instance.LoadSpriteAsync("Assets/Raw/UI/image1.png", sprite =>
            {
                Image.sprite = sprite;
            });
                
            AssetLoaderManager.Instance.LoadTexture2DAsync("Assets/Raw/UI/RawImage.png", texture =>
            {
                RawImage.texture = texture;
            });
                
            AssetLoaderManager.Instance.LoadMaterialAsync("Assets/Raw/Materials/Red.mat", mat =>
            {
                Material = mat;
            });
        }
    }
}