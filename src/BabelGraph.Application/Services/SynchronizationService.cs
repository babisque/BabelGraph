using BabelGraph.Domain.Interfaces;
using BabelGraph.Infrastructure.Interfaces;

namespace BabelGraph.Application.Services;

public class SynchronizationService(IParserService parserService, IDiagramState diagramState)
{
    private CancellationTokenSource? _debounceCts;
    private readonly Lock _lock = new();

    public async Task ProcessTextInputAsync(string text)
    {
        CancellationToken ct;
        lock (_lock)
        {
            _debounceCts?.Cancel();
            _debounceCts = new CancellationTokenSource();
            ct = _debounceCts.Token;
        }

        try
        {
            await Task.Delay(300, ct);
        }
        catch (TaskCanceledException)
        {
            return;
        }

        if (ct.IsCancellationRequested) return;

        try
        {
            var nodes = parserService.Parse(text);
            diagramState.UpdateDiagram(nodes);
            diagramState.SetErrorState(null);
        }
        catch (SyntaxException ex)
        {
            diagramState.SetErrorState(ex.Message);
        }
        catch (Exception)
        {
            diagramState.SetErrorState("An unexpected error occurred during parsing.");
        }
    }
}
