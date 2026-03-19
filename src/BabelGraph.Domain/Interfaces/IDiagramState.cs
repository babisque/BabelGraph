using BabelGraph.Domain.Entities;

namespace BabelGraph.Domain.Interfaces;

public class DiagramChangedEventArgs(IEnumerable<DiagramNode> nodes) : EventArgs
{
    public IEnumerable<DiagramNode> Nodes { get; } = nodes;
}

public interface IDiagramState
{
    event EventHandler<DiagramChangedEventArgs> DiagramChanged;
    IEnumerable<DiagramNode> Nodes { get; }
    void UpdateDiagram(IEnumerable<DiagramNode> nodes);
    void SetErrorState(string? error);
    void UpdateNodePosition(string nodeName, double x, double y);
}
