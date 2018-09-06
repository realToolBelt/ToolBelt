using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToolBelt.Models;

namespace ToolBelt.Services
{
    public interface IProjectDataStore
    {
        Task<IEnumerable<TradeSpecialty>> GetTradeSpecialtiesAsync();

        /// <summary>
        /// Loads the new projects.
        /// </summary>
        /// <param name="itemsPerPage">The maximum number of items to load.</param>
        /// <returns>The projects.</returns>
        Task<IEnumerable<Project>> LoadNewProjects(int itemsPerPage);

        /// <summary>
        /// Loads projects newer than the given <paramref name="project" />.
        /// </summary>
        /// <param name="project">
        /// The project that will be the origin point for determining "new" projects.
        /// </param>
        /// <param name="itemsPerPage">The maximum number of items to load.</param>
        /// <returns>The projects.</returns>
        Task<IEnumerable<Project>> LoadNewProjects(Project project, int itemsPerPage);

        /// <summary>
        /// Loads projects older than the given <paramref name="project" />.
        /// </summary>
        /// <param name="project">
        /// The project that will be the origin point for determining "older" projects.
        /// </param>
        /// <param name="itemsPerPage">The maximum number of items to load.</param>
        /// <returns>The projects.</returns>
        Task<IEnumerable<Project>> LoadOldProjects(Project project, int itemsPerPage);
    }

    public class FakeProjectDataStore : IProjectDataStore
    {
        private readonly Random _random;

        public FakeProjectDataStore()
        {
            _random = new Random();
        }

        public async Task<IEnumerable<TradeSpecialty>> GetTradeSpecialtiesAsync()
        {
            // introduce a delay to emulate network latency
            await RandomDelay().ConfigureAwait(false);

            return await Task.FromResult(
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
                .Select((specialty, index) => new TradeSpecialty(index, specialty))).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Project>> LoadNewProjects(int itemsPerPage)
        {
            // introduce a delay to emulate network latency
            await RandomDelay().ConfigureAwait(false);

            return await Task.FromResult(
                Enumerable.Range(0, itemsPerPage)
                .Reverse()
                .Select(idx => new Project
                {
                    Id = idx,
                    Name = $"Project {idx}",
                    EstimatedStartDate = DateTime.Now.AddDays(idx),
                    EstimatedEndDate = DateTime.Now.AddDays(idx + 30)
                })).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Project>> LoadNewProjects(Project project, int itemsPerPage)
        {
            // introduce a delay to emulate network latency
            await RandomDelay().ConfigureAwait(false);

            return await Task.FromResult(
                Enumerable.Range(project.Id + 1, itemsPerPage)
                .Reverse()
                .Select(idx => new Project
                {
                    Id = idx,
                    Name = $"Project {idx}",
                    EstimatedStartDate = DateTime.Now.AddDays(idx),
                    EstimatedEndDate = DateTime.Now.AddDays(idx + 30)
                })).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Project>> LoadOldProjects(Project project, int itemsPerPage)
        {
            // introduce a delay to emulate network latency
            await RandomDelay().ConfigureAwait(false);

            return await Task.FromResult(
                Enumerable.Range(project.Id - itemsPerPage, itemsPerPage)
                .Reverse()
                .Select(idx => new Project
                {
                    Id = idx,
                    Name = $"Project {idx}",
                    EstimatedStartDate = DateTime.Now.AddDays(idx),
                    EstimatedEndDate = DateTime.Now.AddDays(idx + 30)
                })).ConfigureAwait(false);
        }

        private async Task RandomDelay() => await Task.Delay(_random.Next(2000, 10000)).ConfigureAwait(false);
    }
}
