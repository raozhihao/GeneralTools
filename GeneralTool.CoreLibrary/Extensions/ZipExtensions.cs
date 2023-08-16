using System.IO;
using System.IO.Compression;

namespace GeneralTool.CoreLibrary.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class ZipExtensions
    {
        public static string Zip(this string directoryPath)
        {
            DirectoryInfo directory = new DirectoryInfo(directoryPath);
            string direName = Path.GetFileNameWithoutExtension(directory.FullName);

            string destFullName = Path.Combine(directory.Parent.FullName, direName + ".zip");
            if (File.Exists(destFullName))
                File.Delete(destFullName);
            ZipFile.CreateFromDirectory(directory.FullName, destFullName, CompressionLevel.Fastest, false);
            return destFullName;
        }

        public static string UnZip(this string zipFile, string saveDir = "")
        {
            if (string.IsNullOrWhiteSpace(saveDir))
            {
                string fileDir = Path.GetDirectoryName(zipFile);
                string fileName = Path.GetFileNameWithoutExtension(zipFile);
                saveDir = Path.Combine(fileDir, fileName);
                if (Directory.Exists(saveDir))
                    Directory.Delete(saveDir, true);
            }
            
            ZipFile.ExtractToDirectory(zipFile, saveDir);
            return saveDir;
        }
    }
}
