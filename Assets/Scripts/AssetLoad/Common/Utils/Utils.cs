using System;

namespace Party
{
    public static class Utils
    {
        //TODO：临时处理，后续依据开发需求修改
        public static string GenerateAssetPath(string path,string level)
        {
            int index = path.LastIndexOf(".", StringComparison.Ordinal);
            if (index != -1)
            {
                path = path.Insert(index, level);
            }

            return path;
        }
    }
}