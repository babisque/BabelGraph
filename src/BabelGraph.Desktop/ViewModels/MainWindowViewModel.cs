using BabelGraph.Application.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;

namespace BabelGraph.Desktop.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly SynchronizationService _syncService;

    [ObservableProperty]
    private string _codeText = string.Empty;

    [ObservableProperty]
    private bool _hasSyntaxError;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    public CanvasViewModel Canvas { get; }

    public MainWindowViewModel(CanvasViewModel canvas, SynchronizationService syncService)
    {
        Canvas = canvas;
        _syncService = syncService;

        Canvas.PropertyChanged += OnCanvasPropertyChanged;
    }

    partial void OnCodeTextChanged(string value)
    {
        _ = _syncService.ProcessTextInputAsync(value);
    }

    private void OnCanvasPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(CanvasViewModel.ErrorMessage)) return;
        
        ErrorMessage = Canvas.ErrorMessage ?? string.Empty;
        HasSyntaxError = !string.IsNullOrEmpty(ErrorMessage);
    }
}