using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ToolBelt.Models;
using Xamarin.Forms;

namespace ToolBelt.Services
{
        // TODO: Should be INPC...I think...
    public class User
    {
        public string Uid { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public AccountType AccountType { get; set; }
    }

    public interface IUserService
    {
        User AuthenticatedUser { get; }
    }

    public class UserService : IUserService
    {
        public User AuthenticatedUser { get; }

        public UserService(User user)
        {
            AuthenticatedUser = user;
        }
    }



    public interface IUserDataStore
    {
        Task<User> GetUserById(string uid);
    }

    public class FakeUserDataStore : IUserDataStore
    {
        public Task<User> GetUserById(string uid)
        {
            return Task.FromResult(new User
            {
                Uid = "1234",
                Name = "John",
                Email = "john.doe@fakeemail.com",
                AccountType = AccountType.Contractor
            });
        }
    }
}
