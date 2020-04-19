using System.Threading.Tasks;
using Core.Domain;
using Core.Types;

namespace Core.Interfaces
{
    public interface IVehicleRepository
    {
        Task<Vehicle> Details(int id, bool includeRelated = true);
        void Create(Vehicle vehicle);
        void Delete(Vehicle vehicle);
        Task<Envelope<Vehicle>> List(VehicleFilter filter);
    }
}