using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace GymManagementBLL.Services.AttachmentService
{
    public class AttachmentService : IAttachmentService
    {

        public AttachmentService( IWebHostEnvironment webHost)
        {
            _webHost = webHost;
        }

        private readonly string[] allowedExtensions = { ".jpg", ".jpeg", ".png" };
        private readonly long MaxFileSize = 5 * 1024 * 1024; // 5 MB
        private readonly IWebHostEnvironment _webHost;

        public string? Upload(string FolderName, IFormFile File)
        {
            try
            {
                if (FolderName is null || File is null || File.Length == 0) return null;
                if (File.Length > MaxFileSize) return null;

                var extension = Path.GetExtension(File.FileName).ToLower();
                if (!allowedExtensions.Contains(extension)) return null;

                var FolderPath = Path.Combine(_webHost.WebRootPath, "images", FolderName);
                if (!Directory.Exists(FolderPath))
                {
                    Directory.CreateDirectory(FolderPath);
                }
                var fileName = Guid.NewGuid().ToString() + extension;
                var filePath = Path.Combine(FolderPath, fileName);
                using var fileStream = new FileStream(filePath, FileMode.Create);
                File.CopyTo(fileStream);

                return fileName;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed To Upload File To Folder = {FolderName} : {ex}");
                return null;
            }


        }

        public bool Delete(string FolderName, string FileName)
        {
            try
            {
                if (FolderName is null || FileName is null) return false;
                var filePath = Path.Combine(_webHost.WebRootPath, "images", FolderName, FileName);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed To Delete File With Name = {FileName} : {ex}");
                return false;
            }

        }
    }
}
