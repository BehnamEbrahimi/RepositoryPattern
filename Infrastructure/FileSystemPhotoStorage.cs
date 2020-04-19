using System.IO;
using System.Threading.Tasks;
using Core.Interfaces;
using Core.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System;

namespace Infrastructure
{
    public class FileSystemPhotoStorage : IPhotoStorage
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FileSystemPhotoStorage(IHostingEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _hostingEnvironment = hostingEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<PhotoUploadResult> AddPhoto(IFormFile file)
        {
            var uploadsFolderPath = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");

            if (!Directory.Exists(uploadsFolderPath))
                Directory.CreateDirectory(uploadsFolderPath);

            var publicId = Guid.NewGuid().ToString();
            var fileName = publicId + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            var url = _httpContextAccessor.HttpContext.Request.Scheme;
            url = url + "://" + _httpContextAccessor.HttpContext.Request.Host.Value;
            url = url + "/uploads/" + fileName;

            return new PhotoUploadResult
            {
                PublicId = publicId,
                Url = url
            };
        }

        public string DeletePhoto(string publidId)
        {
            throw new System.NotImplementedException();
        }
    }
}
