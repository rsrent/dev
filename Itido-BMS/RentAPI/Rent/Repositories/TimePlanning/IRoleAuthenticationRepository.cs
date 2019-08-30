using Rent.Models;

namespace Rent.Repositories.TimePlanning
{
    public interface IRoleAuthenticationRepository
    {
        bool IsAdmin(int requester);
        bool IsManager(int requester);
        bool IsUser(int requester);
        bool IsAdminOrManager(int requester);
        bool IsClientAdmin(int requester);
        bool IsClientManager(int requester);
        bool IsClientAdminOrClientManager(int requester);
        string GetRole(int requester);
    }
}