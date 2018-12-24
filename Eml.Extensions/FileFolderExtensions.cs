using System.IO;

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
    }
}
