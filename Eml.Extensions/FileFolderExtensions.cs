﻿using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

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
        /// <para><typeparamref name="T" /> will be used to get the current directory of <paramref name="fileName"></paramref>.</para>
        /// </summary>
        public static string GetFullPath<T>(this string fileName, string relativePath)
        {
            var binDirectory = TypeExtensions.GetBinDirectory<T>();

            return Path.Combine(binDirectory, relativePath, fileName);
        }

        /// <summary>
        /// Deserialize json files for Seeding purposes.
        /// <para><typeparamref name="T" /> will be used to get the current directory of <paramref name="jsonFile"></paramref>.</para>
        /// <para>Example:</para>
        /// <code language="c#">private const string RELATIVE_FOLDER_DATA_SOURCES = @"TestArtifacts\Migrations\SeedDataSources";</code>
        /// </summary>
        public static async Task<string> GetJsonAsStringAsync<T>(this string jsonFile, string relativePath)
            where T : class
        {
            jsonFile = jsonFile.TrimRight(".json");

            var jsonText = await $"{jsonFile}.json".GetFileContentsAsStringAsync<T>(relativePath);

            return jsonText;
        }

        /// <summary>
        /// Deserialize json files for Seeding purposes.
        /// <para><typeparamref name="T" /> will be used to get the current directory of <paramref name="jsonFile"></paramref>.</para>
        /// <para>Example:</para>
        /// <code language="c#">private const string RELATIVE_FOLDER_DATA_SOURCES = @"TestArtifacts\Migrations\SeedDataSources";</code>
        /// </summary>
        public static string GetJsonAsString<T>(this string jsonFile, string relativePath)
            where T : class
        {
            jsonFile = jsonFile.TrimRight(".json");

            var jsonText = $"{jsonFile}.json".GetFileContentsAsString<T>(relativePath);

            return jsonText;
        }

        /// <summary>
        /// Deserialize json files for Seeding purposes.
        /// <para><typeparamref name="T" /> will be used to get the current directory of <paramref name="jsonFile"></paramref>.</para>
        /// <para>Example:</para>
        /// <code language="c#">private const string RELATIVE_FOLDER_DATA_SOURCES = @"TestArtifacts\Migrations\SeedDataSources";</code>
        /// </summary>
        public static List<T> GetJsonStubs<T>(this string jsonFile, string relativeFolder)
            where T : class
        {
            var jsonText = jsonFile.GetJsonAsString<T>(relativeFolder);
            var initialData = JsonConvert.DeserializeObject<List<T>>(jsonText);

            return initialData;
        }

        public static async Task<string> GetFileContentsAsStringAsync(this string fullPath)
        {
            var jsonText = await File.ReadAllTextAsync(fullPath);

            return jsonText;
        }

        public static string GetFileContentsAsString(this string fullPath)
        {
            var jsonText = File.ReadAllText(fullPath);

            return jsonText;
        }

        /// <summary>
        /// Uses the current directory to get the full path.
        /// </summary>
        public static async Task<string> GetFileContentsAsStringAsync(this string fileName, string relativeFolder)
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var fullPath = Path.Combine(currentDirectory, relativeFolder, fileName);
            var jsonText = await fullPath.GetFileContentsAsStringAsync();

            return jsonText;
        }

        /// <summary>
        /// <para><typeparamref name="T" /> will be used to get the current directory of <paramref name="fileName"></paramref>.</para>
        /// <para>Example:</para>
        /// <code language="c#">private const string RELATIVE_FOLDER = "Stubs";</code>
        /// </summary>
        public static async Task<string> GetFileContentsAsStringAsync<T>(this string fileName, string relativeFolder)
        {
            var fullPath = fileName.GetFullPath<T>(relativeFolder);
            var jsonText = await fullPath.GetFileContentsAsStringAsync();

            return jsonText;
        }

        /// <summary>
        /// <para><typeparamref name="T" /> will be used to get the current directory of <paramref name="fileName"></paramref>.</para>
        /// <para>Example:</para>
        /// <code language="c#">private const string RELATIVE_FOLDER = "Stubs";</code>
        /// </summary>
        public static string GetFileContentsAsString<T>(this string fileName, string relativeFolder)
        {
            var fullPath = fileName.GetFullPath<T>(relativeFolder);
            var jsonText = fullPath.GetFileContentsAsString();

            return jsonText;
        }

        /// <summary>
        /// Deserialize json files for Seeding purposes.
        /// <para><typeparamref name="T" /> will be used to get the current directory of <paramref name="jsonFile"></paramref>.</para>
        /// <para>Example:</para>
        /// <code language="c#">private const string RELATIVE_FOLDER_DATA_SOURCES = @"TestArtifacts\Migrations\SeedDataSources";</code>
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
        /// <para><typeparamref name="T" /> will be used to get the current directory of <paramref name="jsonFile"></paramref>.</para>
        /// <para>Example:</para>
        /// <code language="c#">private const string RELATIVE_FOLDER_DATA_SOURCES = @"TestArtifacts\Migrations\SeedDataSources";</code>
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
        /// <para><typeparamref name="T" /> will be used to get the current directory of <paramref name="jsonFile"></paramref>.</para>
        /// <para>Example:</para>
        /// <code language="c#">private const string RELATIVE_FOLDER_DATA_SOURCES = @"TestArtifacts\Migrations\SeedDataSources";</code>
        /// </summary>
        public static async Task<T> GetJsonStubAsync<T>(this string jsonFile, string relativeFolder)
            where T : class
        {
            var jsonText = await jsonFile.GetJsonAsStringAsync<T>(relativeFolder);
            var initialData = JsonConvert.DeserializeObject<T>(jsonText);

            return initialData;
        }

        /// <summary>
        /// Generate the full path from the assembly of <typeparamref name="T" />.
        /// <para>Pass file extension with a dot. Ex: .xml, .txt</para>
        /// <para>The .dll file extension of the assembly will be replaced with the parameter <paramref name="fileExtension"></paramref>.</para>
        /// </summary>
        public static string GetNewPathFromAssemblyOf<T>(this string fileExtension)
            where T : class
        {
            var assembly = typeof(T).Assembly;
            var assemblyName = typeof(T).Assembly.GetName().Name;
            var binDirectory = Path.GetDirectoryName(assembly.Location) ?? string.Empty;
            var xmlFile = $"{assemblyName}{fileExtension}";
            var xmlPath = Path.Combine(binDirectory, xmlFile);

            return xmlPath;
        }
    }
}
