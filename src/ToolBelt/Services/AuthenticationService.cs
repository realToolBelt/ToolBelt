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
    /// <summary>
    /// The basic information for an account in the system.
    /// </summary>
    public abstract class Account
    {
        /// <summary>
        /// Gets or sets the unique identifier for the account.
        /// </summary>
        public string Uid { get; set; }

        /// <summary>
        /// Gets or sets the email address for the account.
        /// </summary>
        public string EmailAddress { get; set; }
    }

    /// <summary>
    /// The account information for a contractor.
    /// </summary>
    /// <seealso cref="ToolBelt.Services.Account" />
    public class ContractorAccount : Account
    {
        /* 
        Fields for Contractors:
        X Company name
        X URL (if any)
        X Email address
        X Physical Address
        X Contact Person's name
        X Phone Numbers and Faxes (2)
        X Social networks (3)
        
        Specialty Area (pull down)
        Billing Information (optional for now)
         */

        /// <summary>
        /// Gets or sets the name of the company.
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// Gets or sets the URL for the company.
        /// </summary>
        public string CompanyUrl { get; set; }

        /// <summary>
        /// Gets or sets the physical address for the company.
        /// </summary>
        public Address PhysicalAddress { get; set; }

        /// <summary>
        /// Gets or sets the primary phone number for the company.
        /// </summary>
        public string PrimaryPhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the secondary phone number for the company.
        /// </summary>
        public string SecondaryPhoneNumber { get; set; }

        public string PrimaryContact { get; set; }

        /*NOTE: Right now we're hard-limiting to 3 social networks, with no category defined */
        public string SocialNetwork1 { get; set; }
        public string SocialNetwork2 { get; set; }
        public string SocialNetwork3 { get; set; }

        public List<TradeSpecialty> Specialties { get; } = new List<TradeSpecialty>();
    }

    /// <summary>
    /// The account information for a tradesman.
    /// </summary>
    /// <seealso cref="ToolBelt.Services.Account" />
    public class TradesemanAccount : Account
    {
        public string Name { get; set; }
    }

    public interface IUserService
    {
        Account AuthenticatedUser { get; }
    }

    public class UserService : IUserService
    {
        public Account AuthenticatedUser { get; }

        public UserService(Account user)
        {
            AuthenticatedUser = user;
        }
    }



    public interface IUserDataStore
    {
        Task<Account> GetUserById(string uid);
    }

    public class FakeUserDataStore : IUserDataStore
    {
        public Task<Account> GetUserById(string uid)
        {
            return Task.FromResult((Account)new ContractorAccount
            {
                Uid = "1234",
                EmailAddress = "john.doe@fake.com",
                CompanyName = "John Doe Contracting"
            });
        }
    }
}
