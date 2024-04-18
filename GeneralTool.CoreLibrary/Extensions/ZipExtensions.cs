using System.IO;
using System.IO.Compression;

namespace GeneralTool.CoreLibrary.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class ZipExtensions
    {
        /// <summary>
        /// 将文件夹打包
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <returns></returns>
        public static void Zip(this string directoryPath,string destFullName)
        {
            DirectoryInfo directory = new DirectoryInfo(directoryPath);
           
            if (File.Exists(destFullName))
                File.Delete(destFullName);
            ZipFile.CreateFromDirectory(directory.FullName, destFullName, CompressionLevel.Fastest, false);
        }

        /// <summary>
        /// 解压zip包
        /// </summary>
        /// <param name="zipFile"></param>
        /// <param name="saveDir"></param>
        public static void UnZip(this string zipFile, string saveDir = "")
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
        }
    }
}
