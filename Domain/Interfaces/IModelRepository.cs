using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IModelRepository
    {
        Task<Model> Details(int id);
    }
}