using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IVehicleRepository
    {
        Task<Vehicle> Details(int id, bool includeRelated = true);
        void Create(Vehicle vehicle);
        void Delete(Vehicle vehicle);
    }
}