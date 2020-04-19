using Core.Domain;

namespace Core.Interfaces
{
    public interface IJwtGenerator
    {
        string CreateToken(User user);
    }
}