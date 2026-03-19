namespace BabelGraph.Domain.Interfaces;

public interface ISynchronizationService
{
    Task ProcessTextInputAsync(string text);
    Task UpdateNodePositionAsync(string nodeName, double x, double y);
}
