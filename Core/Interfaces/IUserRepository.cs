using System.Threading.Tasks;
using Core.Domain;

namespace Core.Interfaces
{
    public interface IUserRepository
    {
        Task<User> Details(string username);
    }
}