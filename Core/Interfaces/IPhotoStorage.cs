using System.Threading.Tasks;
using Core.Types;
using Microsoft.AspNetCore.Http;

namespace Core.Interfaces
{
    public interface IPhotoStorage
    {
        Task<PhotoUploadResult> AddPhoto(IFormFile file);
        string DeletePhoto(string publidId);
    }
}