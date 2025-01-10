namespace Party
{
    /// <summary>
    /// 游戏中，低精度资源的自动释放
    /// 防止同种资源的各种精度资源同时存在，导致内存占用过高的问题
    /// </summary>
    public partial class AssetLoaderManager : SingletonMonoBehaviour<AssetLoaderManager>
    {
        private void _UpdateAutoRelease()
        {
            
        }
    }
}