using System.Threading.Tasks;
using Core.Domain;

namespace Core.Interfaces
{
    public interface IModelRepository
    {
        Task<Model> Details(int id);
    }
}