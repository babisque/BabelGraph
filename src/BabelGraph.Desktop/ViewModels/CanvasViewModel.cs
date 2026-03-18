using System.Collections.ObjectModel;
using System.Linq;
using BabelGraph.Domain.Entities;
using BabelGraph.Domain.Interfaces;
using BabelGraph.Desktop.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BabelGraph.Desktop.ViewModels;

public partial class CanvasViewModel : ViewModelBase
{
    private readonly IDiagramState _diagramState;

    [ObservableProperty]
    private ObservableCollection<DiagramNode> _nodes = new();

    [ObservableProperty]
    private string? _errorMessage;

    public CanvasViewModel(IDiagramState diagramState)
    {
        _diagramState = diagramState;
        _diagramState.DiagramChanged += OnDiagramChanged;
        
        if (_diagramState is DiagramState ds)
        {
            ds.ErrorChanged += (s, e) => ErrorMessage = e;
        }
    }

    private void OnDiagramChanged(object? sender, DiagramChangedEventArgs e)
    {
        Nodes = new ObservableCollection<DiagramNode>(e.Nodes);
    }
}
