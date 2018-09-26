using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ToolBelt.Data;
using ToolBelt.Models;

namespace ToolBelt.Services
{
    public interface IProjectDataStore
    {
        /// <summary>
        /// Deletes the given <paramref name="project" /> asynchronously.
        /// </summary>
        /// <param name="project">The project to delete.</param>
        /// <returns>An awaitable task.</returns>
        Task DeleteProjectAsync(Project project);

        Task<IEnumerable<Trade>> GetTradesAsync();

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

        /// <summary>
        /// Loads projects for a given user.
        /// </summary>
        /// <param name="userId">The ID of the user to load projects for.</param>
        /// <returns>The projects for the user.</returns>
        Task<IEnumerable<Project>> LoadProjectsForUser(string userId);
    }

    public class FakeProjectDataStore : IProjectDataStore
    {
        private readonly Random _random;

        public FakeProjectDataStore()
        {
            _random = new Random();
        }
        public partial class MockProjectData
        {
            [JsonProperty("projects")]
            public Project[] Projects { get; set; }
        }

        internal static class Converter
        {
            public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Include,
                MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
                DateParseHandling = DateParseHandling.DateTime,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                Converters = {
                    new IsoDateTimeConverter { DateTimeFormat = "MM/dd/yyyy" }
                },
            };
        }

        private async Task<IEnumerable<Project>> LoadProjects()
        {
            var assembly = typeof(FakeProjectDataStore).Assembly;
            using (Stream stream = assembly.GetManifestResourceStream("ToolBelt.MOCK_PROJECT_DATA.json"))
            {
                using (StreamReader sr = new StreamReader(stream))
                {
                    string data = await sr.ReadToEndAsync().ConfigureAwait(false);
                    return await Task.Run(() => JsonConvert.DeserializeObject<MockProjectData>(data, Converter.Settings).Projects).ConfigureAwait(false);
                }
            }
        }

        public Task DeleteProjectAsync(Project project)
        {
            //project.Status = ProjectStatus.Deleted;

            // delete...
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<Trade>> GetTradesAsync()
        {
            // introduce a delay to emulate network latency
            //await RandomDelay().ConfigureAwait(false);

            return await Task.FromResult(
                new[]
                {
                    "Roofing",
                    "Siding",
                    "Flooring",
                    "Framing",
                    "Dry-wall",
                    "Tiling",
                    "Masonry (stone work)",
                    "Chimney",
                    "Concrete",
                    "Asphalt",
                    "Remodeling (bathrooms)",
                    "Remodeling (kitchens)",
                    "Carpentry (finished)",
                    "Carpentry (rough)",
                    "Painting (interior)",
                    "Painting (exterior)",
                    "Plumbing",
                    "Electrical",
                    "Landscaping",
                    "Appliance Installation",
                    "Window Replacement",
                    "Welding",
                    "Decks",
                    "Fencing",
                    "Tile",
                    "Kitchen Remodeling",
                    "Bathroom Remodeling",
                    "Concrete"
                }
                .Select((specialty, index) => new Trade { TradeId = index, Name = specialty })).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Project>> LoadNewProjects(int itemsPerPage)
        {
            return (await LoadProjects()).Skip(50).Take(itemsPerPage).ToArray();
        }

        public async Task<IEnumerable<Project>> LoadNewProjects(Project project, int itemsPerPage)
        {
            return (await LoadProjects()).TakeWhile(p => p.ProjectId < project.ProjectId).Reverse().Take(itemsPerPage).ToArray();
        }

        public async Task<IEnumerable<Project>> LoadOldProjects(Project project, int itemsPerPage)
        {
            return (await LoadProjects()).SkipWhile(p => p.ProjectId <= project.ProjectId).Take(itemsPerPage).ToArray();
        }

        public async Task<IEnumerable<Project>> LoadProjectsForUser(string userId)
        {
            // just return a random subset of projects
            return (await LoadProjects()).OrderBy(x => _random.Next()).Take(15).ToArray();
        }

        private async Task RandomDelay() => await Task.Delay(_random.Next(2000, 10000)).ConfigureAwait(false);
    }
}
