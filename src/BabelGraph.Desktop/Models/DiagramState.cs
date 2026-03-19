using System;
using System.Collections.Generic;
using System.Linq;
using BabelGraph.Domain.Entities;
using BabelGraph.Domain.Interfaces;

namespace BabelGraph.Desktop.Models;

public class DiagramState : IDiagramState
{
    public event EventHandler<DiagramChangedEventArgs>? DiagramChanged;
    public event EventHandler<string?>? ErrorChanged;

    private IEnumerable<DiagramNode> _currentNodes = new List<DiagramNode>();
    private string? _currentError;

    public IEnumerable<DiagramNode> Nodes => _currentNodes;

    public void UpdateDiagram(IEnumerable<DiagramNode> nodes)
    {
        _currentNodes = nodes;
        DiagramChanged?.Invoke(this, new DiagramChangedEventArgs(nodes));
    }

    public void SetErrorState(string? error)
    {
        _currentError = error;
        ErrorChanged?.Invoke(this, error);
    }

    public void UpdateNodePosition(string nodeName, double x, double y)
    {
        var node = _currentNodes.FirstOrDefault(n => n.Name == nodeName);
        node?.UpdatePosition(x, y);
    }
}
