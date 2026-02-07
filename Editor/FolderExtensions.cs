using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;

namespace mdz.Editor
{
    public static class FolderExtensions
    {
        public static void CreateInvalidFolders(this string path)
        {
            var paths = path.Split('/');  

            path = paths[0];
            for(int i = 1; i < paths.Length; i++)
            {
                var p = paths[i];
                if (!string.IsNullOrWhiteSpace(p))
                {
                    var newPath = path + $"/{p}";
                    if (!AssetDatabase.IsValidFolder(newPath))
                    {
                        var guid = AssetDatabase.CreateFolder(path, p);
                    }
                    path = newPath;
                }
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}