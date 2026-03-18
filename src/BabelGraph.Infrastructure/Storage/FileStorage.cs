using System;
using System.IO;
using System.Threading.Tasks;
using BabelGraph.Infrastructure.Interfaces;

namespace BabelGraph.Infrastructure.Storage;

public class FileStorage : IFileStorage
{
    public async Task SaveAsync(string filePath, string content)
    {
        var tempPath = Path.Combine(Path.GetDirectoryName(filePath) ?? string.Empty, $".{Path.GetFileName(filePath)}.tmp");
        
        try
        {
            // Write to temporary file
            await File.WriteAllTextAsync(tempPath, content);
            
            // Atomic swap
            if (File.Exists(filePath))
            {
                File.Replace(tempPath, filePath, null);
            }
            else
            {
                File.Move(tempPath, filePath);
            }
        }
        catch (Exception)
        {
            if (File.Exists(tempPath))
            {
                File.Delete(tempPath);
            }
            throw;
        }
    }

    public async Task<string> LoadAsync(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("The specified diagram file was not found.", filePath);
        }

        return await File.ReadAllTextAsync(filePath);
    }
}
