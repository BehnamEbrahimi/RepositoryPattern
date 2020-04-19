using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Domain;

namespace Core.Interfaces
{
    public interface IPhotoRepository
    {
        Task<List<Photo>> List(int vehicleId);
    }
}