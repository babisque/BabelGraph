using System.Collections.ObjectModel;
using System.Threading.Tasks;
using BabelGraph.Desktop.Models;
using BabelGraph.Domain.Entities;
using BabelGraph.Domain.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BabelGraph.Desktop.ViewModels;

public partial class CanvasViewModel : ViewModelBase
{
    private readonly IDiagramState _diagramState;
    private readonly ISynchronizationService _synchronizationService;

    [ObservableProperty]
    private ObservableCollection<DiagramNode> _nodes = [];

    [ObservableProperty]
    private string? _errorMessage;

    public CanvasViewModel(IDiagramState diagramState, ISynchronizationService synchronizationService)
    {
        _diagramState = diagramState;
        _synchronizationService = synchronizationService;
        _diagramState.DiagramChanged += OnDiagramChanged;
        
        if (_diagramState is DiagramState ds)
        {
            ds.ErrorChanged += (s, e) => ErrorMessage = e;
        }
    }

    public static void MoveNode(DiagramNode node, double x, double y)
    {
        node.UpdatePosition(x, y);
    }

    public async Task CompleteMoveNode(DiagramNode node)
    {
        await _synchronizationService.UpdateNodePositionAsync(node.Name, node.X, node.Y);
    }

    private void OnDiagramChanged(object? sender, DiagramChangedEventArgs e)
    {
        Nodes = new ObservableCollection<DiagramNode>(e.Nodes);
    }
}
