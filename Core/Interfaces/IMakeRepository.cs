using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Domain;

namespace Core.Interfaces
{
    public interface IMakeRepository
    {
        Task<List<Make>> List();
    }
}