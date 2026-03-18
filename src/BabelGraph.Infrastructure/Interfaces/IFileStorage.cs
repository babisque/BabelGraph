using System.Threading.Tasks;

namespace BabelGraph.Infrastructure.Interfaces;

public interface IFileStorage
{
    Task SaveAsync(string filePath, string content);
    Task<string> LoadAsync(string filePath);
}
