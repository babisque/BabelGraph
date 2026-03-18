using BabelGraph.Application.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Threading.Tasks;

namespace BabelGraph.Desktop.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly SynchronizationService _syncService;

    [ObservableProperty]
    private string _codeInput = string.Empty;

    public CanvasViewModel Canvas { get; }

    public MainWindowViewModel(CanvasViewModel canvas, SynchronizationService syncService)
    {
        Canvas = canvas;
        _syncService = syncService;
    }

    partial void OnCodeInputChanged(string value)
    {
        // Fire and forget text processing
        _ = _syncService.ProcessTextInputAsync(value);
    }
}
