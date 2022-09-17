using SimpleNetCoreSecurityWeb.Models;
using System.Security.Claims;

namespace SimpleNetCoreSecurityWeb.Services.Impl
{
    public class AccountService : IAccountService
    {
        private List<Account> accounts = new List<Account>()
        {
            new Account()
            {
                Username="acc1",
                Password="123",
                 FullName="Name 1",
                 Roles=new List<string>(){ "SuperAdmin"}
            },
            new Account()
            {
                Username="acc2",
                Password="123",
                 FullName="Name 2",
                 Roles=new List<string>(){ "SuperAdmin", "Admin"}
            },
            new Account()
            {
                Username="acc3",
                Password="123",
                 FullName="Name 3",
                 Roles=new List<string>(){ "Employee"}
            }
        };

        public IEnumerable<Claim> GetClaimsAccount(Account account)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,account.Username)
            };

            foreach(var role in account.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role,role));
            }

            return claims;
        }

        public Account Login(string username, string password)
        {
            return accounts.FirstOrDefault(x => x.Username == username && x.Password == password);
        }
    }
}
