using Application.Helpers;
using Microsoft.AspNetCore.StaticFiles;

namespace Infrastructure.TestData
{
    public class FilesManager
    {
        public class TestFile
        {
            public string FileName { get; set; }
            public string Type { get; set; }  // MimeType
            public long Size { get; set; }
            public string Extension { get; set; }
            public string PhysicalPath { get; set; }
            public string VirtualPath { get; set; }

            public static string GetMimeType(string filePath)
            {
                var provider = new FileExtensionContentTypeProvider();
                var fileName = System.IO.Path.GetFileName(filePath);
                return provider.TryGetContentType(fileName, out var contentType) ? contentType : "application/octet-stream";
            }
        }

        public static List<TestFile> GetAllFilesFromTestDir()
        {
            // Get the project root directory (going up from the bin folder)
            string projectDirectory = FilesHelper.GetProjectRootDirectory();

            // Combine with the relative path
            string relativePath = "_development_files";
            string directoryPath = Path.Combine(projectDirectory, "wwwroot", relativePath);

            var testFiles = new List<TestFile>();

            // Check if directory exists
            if (!Directory.Exists(directoryPath))
            {
                Console.WriteLine($"Directory not found: {directoryPath}");
                // Optionally create the directory
                // Directory.CreateDirectory(directoryPath);
                return testFiles;
            }

            // Get all files in the directory
            string[] filePaths = Directory.GetFiles(directoryPath);

            foreach (var filePath in filePaths)
            {
                var fileInfo = new FileInfo(filePath);

                // Create virtual path by replacing physical root with virtual root
                // and normalizing the slashes to forward slashes
                string virtualPath = "/" + relativePath + "/" + fileInfo.Name;
                virtualPath = $"https://localhost:7047{virtualPath.Replace("\\", "/").Replace("//", "/")}";

                var testFile = new TestFile
                {
                    FileName = fileInfo.Name,
                    Type = TestFile.GetMimeType(filePath),
                    Size = fileInfo.Length,
                    Extension = fileInfo.Extension,
                    PhysicalPath = filePath,
                    VirtualPath = virtualPath
                };

                testFiles.Add(testFile);
            }

            return testFiles;
        }

        public static List<TestFile> GetImagesFromTestDir()
        {
            var files = GetAllFilesFromTestDir().Where(f => f.Type.Contains("image/")).ToList();
            return files;
        }

        public static List<TestFile> GetDirFiles(string relativePath)
        {

            try
            {
                // Get the project root directory (going up from the bin folder)
                string projectDirectory = FilesHelper.GetProjectRootDirectory();

                string directoryPath = Path.Combine(projectDirectory, "wwwroot", relativePath);

                var testFiles = new List<TestFile>();

                // Check if directory exists
                if (!Directory.Exists(directoryPath))
                {
                    Console.WriteLine($"Directory not found: {directoryPath}");
                    // Optionally create the directory
                    // Directory.CreateDirectory(directoryPath);
                    return testFiles;
                }

                // Get all files in the directory
                string[] filePaths = Directory.GetFiles(directoryPath);

                foreach (var filePath in filePaths)
                {
                    var fileInfo = new FileInfo(filePath);

                    // Create virtual path by replacing physical root with virtual root
                    // and normalizing the slashes to forward slashes
                    string virtualPath = "/" + relativePath + "/" + fileInfo.Name;
                    virtualPath = $"https://localhost:7047{virtualPath.Replace("\\", "/").Replace("//", "/")}";

                    var testFile = new TestFile
                    {
                        FileName = fileInfo.Name,
                        Type = TestFile.GetMimeType(filePath),
                        Size = fileInfo.Length,
                        Extension = fileInfo.Extension,
                        PhysicalPath = filePath,
                        VirtualPath = virtualPath
                    };

                    testFiles.Add(testFile);
                }

                return testFiles;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting files in \"{relativePath}\": {ex.Message}");
                throw;
            }
        }

    }
}

