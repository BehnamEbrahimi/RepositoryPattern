using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IMakeRepository
    {
        Task<List<Make>> List();
    }
}