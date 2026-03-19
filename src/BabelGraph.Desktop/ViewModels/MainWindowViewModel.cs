using BabelGraph.Domain.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;

namespace BabelGraph.Desktop.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly ISynchronizationService _syncService;
    private readonly IEditorService _editorService;

    [ObservableProperty]
    private string _codeText = string.Empty;

    [ObservableProperty]
    private bool _hasSyntaxError;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    public CanvasViewModel Canvas { get; }

    public MainWindowViewModel(CanvasViewModel canvas, ISynchronizationService syncService, IEditorService editorService)
    {
        Canvas = canvas;
        _syncService = syncService;
        _editorService = editorService;

        Canvas.PropertyChanged += OnCanvasPropertyChanged;

        if (_editorService is Services.DesktopEditorService des)
        {
            des.TextUpdated += text => CodeText = text;
        }
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