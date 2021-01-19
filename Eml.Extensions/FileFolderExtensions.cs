using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Eml.Extensions
{
    public static class FileFolderExtensions
    {
        /// <summary>
        /// Ensures the destination folder is created before copying.
        /// </summary>
        public static void CopyFileTo(this FileInfo sourceFile, string destFileName, bool overwrite = true)
        {
            var destinationFolderName = Path.GetDirectoryName(destFileName);

            if (destinationFolderName == null)
            {
                throw new DirectoryNotFoundException("Destination directory is null.");
            }

            if (!Directory.Exists(destinationFolderName))
            {
                Directory.CreateDirectory(destinationFolderName);
            }

            sourceFile.CopyTo(destFileName, overwrite);
        }

        /// <summary>
        /// Copy file into the specified directory.
        /// </summary>
        public static void CopyFileTo(this FileInfo sourceFile, DirectoryInfo destDir, bool overwrite = true)
        {
            var targetFileName = Path.Combine(destDir.FullName, sourceFile.Name);

            sourceFile.CopyFileTo(targetFileName, overwrite);
        }

        /// <summary>
        /// Copy all directory contents recursively.
        /// </summary>
        public static void CopyFolderTo(this DirectoryInfo sourceDir, string destDirName, bool overwrite = true, string fileSearchPattern = "")
        {
            if (!sourceDir.Exists) throw new DirectoryNotFoundException($"Source directory does not exist or could not be found: {sourceDir}");

            if (!Directory.Exists(destDirName)) Directory.CreateDirectory(destDirName);

            var dirs = sourceDir.GetDirectories();
            var files = string.IsNullOrWhiteSpace(fileSearchPattern) ? sourceDir.GetFiles() : sourceDir.GetFiles(fileSearchPattern);

            foreach (var file in files)
            {
                var tempPath = Path.Combine(destDirName, file.Name);

                file.CopyFileTo(tempPath, overwrite);
            }

            foreach (var subDir in dirs)
            {
                var tempPath = Path.Combine(destDirName, subDir.Name);

                subDir.CopyFolderTo(tempPath, overwrite);
            }
        }

        /// <summary>
        /// GetBinDirectory using the assembly of T
        /// </summary>
        public static string GetFullPath<T>(this string fn, string relativePath)
        {
            var binDirectory = TypeExtensions.GetBinDirectory<T>();

            return Path.Combine(binDirectory, relativePath, fn);
        }

        /// <summary>
        /// T will be used to get the current directory.
        /// </summary>
        public static async Task<string> GetJsonAsStringAsync<T>(this string jsonFile, string relativePath)
            where T : class
        {
            jsonFile = jsonFile.TrimRight(".json");

            var fullPath = $"{jsonFile}.json".GetFullPath<T>(relativePath);
            var jsonText = await File.ReadAllTextAsync(fullPath);

            return jsonText;
        }

        /// <summary>
        /// T will be used to get the current directory.
        /// </summary>
        public static string GetJsonAsString<T>(this string jsonFile, string relativePath)
            where T : class
        {
            jsonFile = jsonFile.TrimRight(".json");

            var fullPath = $"{jsonFile}.json".GetFullPath<T>(relativePath);
            var jsonText = File.ReadAllText(fullPath);

            return jsonText;
        }

        /// <summary>
        /// Deserialize json files for Seeding purposes.
        /// T is also used to get the current directory.
        /// Ex: private const string RELATIVE_FOLDER_DATA_SOURCES = @"TestArtifacts\Migrations\SeedDataSources";
        /// </summary>
        public static List<T> GetJsonStubs<T>(this string jsonFile, string relativeFolder)
            where T : class
        {
            var jsonText = jsonFile.GetJsonAsString<T>(relativeFolder);
            var initialData = JsonConvert.DeserializeObject<List<T>>(jsonText);

            return initialData;
        }

        /// <summary>
        /// Deserialize json files for Seeding purposes.
        /// T is also used to get the current directory.
        /// Ex: private const string RELATIVE_FOLDER_DATA_SOURCES = @"TestArtifacts\Migrations\SeedDataSources";
        /// </summary>
        public static async Task<List<T>> GetJsonStubsAsync<T>(this string jsonFile, string relativeFolder)
            where T : class
        {
            var jsonText = await jsonFile.GetJsonAsStringAsync<T>(relativeFolder);
            var initialData = JsonConvert.DeserializeObject<List<T>>(jsonText);

            return initialData;
        }

        /// <summary>
        /// Deserialize json files for Seeding purposes.
        /// T is also used to get the current directory.
        /// Ex: private const string RELATIVE_FOLDER_DATA_SOURCES = @"TestArtifacts\Migrations\SeedDataSources";
        /// </summary>
        public static T GetJsonStub<T>(this string jsonFile, string relativeFolder)
            where T : class
        {

            var jsonText = jsonFile.GetJsonAsString<T>(relativeFolder);
            var initialData = JsonConvert.DeserializeObject<T>(jsonText);

            return initialData;
        }

        /// <summary>
        /// Deserialize json files for Seeding purposes.
        /// T is also used to get the current directory.
        /// Ex: private const string RELATIVE_FOLDER_DATA_SOURCES = @"TestArtifacts\Migrations\SeedDataSources";
        /// </summary>
        public static async Task<T> GetJsonStubAsync<T>(this string jsonFile, string relativeFolder)
            where T : class
        {
            var jsonText = await jsonFile.GetJsonAsStringAsync<T>(relativeFolder);
            var initialData = JsonConvert.DeserializeObject<T>(jsonText);

            return initialData;
        }
    }
}
