using System.Threading.Tasks;
using ToolBelt.Data;

namespace ToolBelt.Services
{
    public interface IUserDataStore
    {
        Task<Account> GetUserById(string uid);
    }

    public interface IUserService
    {
        Account AuthenticatedUser { get; }
    }

    public class FakeUserDataStore : IUserDataStore
    {
        public Task<Account> GetUserById(string uid)
        {
            return Task.FromResult((Account)new ContractorAccount
            {
                AccountId = "1234",
                EmailAddress = "john.doe@fake.com",
                CompanyName = "John Doe Contracting"
            });
        }
    }

    public class UserService : IUserService
    {
        public UserService(Account user)
        {
            AuthenticatedUser = user;
        }

        public Account AuthenticatedUser { get; }
    }
}
