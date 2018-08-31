using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToolBelt.Models;

namespace ToolBelt.Services
{
    public interface IProjectDataStore
    {
        Task<IEnumerable<Project>> GetProjectsAsync();

        Task<IEnumerable<TradeSpecialty>> GetTradeSpecialtiesAsync();
    }

    public class FakeProjectDataStore : IProjectDataStore
    {
        public Task<IEnumerable<Project>> GetProjectsAsync()
        {
            return Task.FromResult(
                Enumerable.Range(0, 20)
                .Select(idx => new Project
                {
                    Id = idx,
                    Name = $"Project {idx}",
                    EstimatedStartDate = DateTime.Now.AddDays(idx),
                    EstimatedEndDate = DateTime.Now.AddDays(idx + 30)
                }));
        }

        public Task<IEnumerable<TradeSpecialty>> GetTradeSpecialtiesAsync()
        {
            return Task.FromResult(
                new[]
                {
                    "Laborer",
                    "Carpenter",
                    "Construction manager",
                    "Painter",
                    "Admin support",
                    "Plumber",
                    "Professional",
                    "Heat A/C mech",
                    "Operating engineer",
                    "Repairer",
                    "Manager",
                    "Electrician",
                    "Roofer",
                    "Truck driver",
                    "Brickmason",
                    "Foreman",
                    "Service",
                    "Drywall",
                    "Welder",
                    "Carpet and tile",
                    "Material moving",
                    "Concrete",
                    "Ironworker",
                    "Helper",
                    "Insulation",
                    "Sheet metal",
                    "Fence Erector",
                    "Highway Maint",
                    "Misc worker",
                    "Inspector",
                    "Glazier",
                    "Plasterer",
                    "Dredge",
                    "Power-line installer",
                    "Driller",
                    "Elevator",
                    "Paving",
                    "Iron reinforcement",
                    "Boilermaker",
                    "Other"
                }
                .Select((specialty, index) => new TradeSpecialty(index, specialty)));
        }
    }
}
