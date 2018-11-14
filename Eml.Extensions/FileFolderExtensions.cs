using System.IO;

namespace Eml.Extensions
{
    public static class FileFolderExtensions
    {
        /// <summary>
        /// Ensures the destination folder is created before copying.
        /// </summary>
        public static void CopyTo(this FileInfo sourceFile, string destFileName, bool overwrite = true)
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
        public static void CopyTo(this FileInfo sourceFile, DirectoryInfo destDir, bool overwrite = true)
        {
            var targetFileName = Path.Combine(destDir.FullName, sourceFile.Name);

            sourceFile.CopyTo(targetFileName, overwrite);
        }

        /// <summary>
        /// Copy all directory contents recursively.
        /// </summary>
        private static void CopyTo(this DirectoryInfo sourceDir, string destDirName, bool overwrite = true)
        {
            if (!sourceDir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDir);
            }

            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            var dirs = sourceDir.GetDirectories();

            // Get the files in the directory and copy them to the new location.
            foreach (var file in sourceDir.GetFiles())
            {
                var tempPath = Path.Combine(destDirName, file.Name);

                file.CopyTo(tempPath, overwrite);
            }

            foreach (var subDir in dirs)
            {
                var tempPath = Path.Combine(destDirName, subDir.Name);

                subDir.CopyTo(tempPath, overwrite);
            }
        }
    }
}
