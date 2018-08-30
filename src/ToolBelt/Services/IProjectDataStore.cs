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
    }
}
