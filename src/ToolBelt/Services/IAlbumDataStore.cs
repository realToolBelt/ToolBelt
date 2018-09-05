using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolBelt.Models;

namespace ToolBelt.Services
{
    public interface IAlbumDataStore
    {
        Task<IEnumerable<Album>> GetAlbumsAsync(int userId);
    }

    public class FakeAlbumDataStore : IAlbumDataStore
    {
        public Task<IEnumerable<Album>> GetAlbumsAsync(int userId)
        {
            return Task.FromResult(
                Enumerable.Range(0, 6)
                .Select(idx => new Album
                {
                    Id = idx,
                    Name = $"Album {idx}",
                    Description = $"{idx}"
                }));
        }
    }
}
