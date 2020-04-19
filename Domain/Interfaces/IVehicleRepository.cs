using System.Threading.Tasks;
using Domain.Types;

namespace Domain.Interfaces
{
    public interface IVehicleRepository
    {
        Task<Vehicle> Details(int id, bool includeRelated = true);
        void Create(Vehicle vehicle);
        void Delete(Vehicle vehicle);
        Task<Envelope<Vehicle>> List(VehicleFilter filter);
    }
}