using System;
using System.Collections.Generic;
using BabelGraph.Domain.Entities;

namespace BabelGraph.Domain.Interfaces;

public class DiagramChangedEventArgs : EventArgs
{
    public IEnumerable<DiagramNode> Nodes { get; }

    public DiagramChangedEventArgs(IEnumerable<DiagramNode> nodes)
    {
        Nodes = nodes;
    }
}

public interface IDiagramState
{
    event EventHandler<DiagramChangedEventArgs> DiagramChanged;
    void UpdateDiagram(IEnumerable<DiagramNode> nodes);
    void SetErrorState(string? error);
}
