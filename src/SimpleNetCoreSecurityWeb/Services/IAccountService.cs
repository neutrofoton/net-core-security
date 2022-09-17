using SimpleNetCoreSecurityWeb.Models;
using System.Security.Claims;

namespace SimpleNetCoreSecurityWeb.Services
{
    public interface IAccountService
    {
        Account Login(string username, string password);

        IEnumerable<Claim> GetClaimsAccount(Account account);
    }
}
