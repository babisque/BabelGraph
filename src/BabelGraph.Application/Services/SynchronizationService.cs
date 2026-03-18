using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BabelGraph.Domain.Entities;
using BabelGraph.Domain.Interfaces;
using BabelGraph.Infrastructure.Interfaces;

namespace BabelGraph.Application.Services;

public class SynchronizationService
{
    private readonly IParserService _parserService;
    private readonly IDiagramState _diagramState;
    private CancellationTokenSource? _debounceCts;
    private readonly object _lock = new();

    public SynchronizationService(IParserService parserService, IDiagramState diagramState)
    {
        _parserService = parserService;
        _diagramState = diagramState;
    }

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
            var nodes = _parserService.Parse(text);
            _diagramState.UpdateDiagram(nodes);
            _diagramState.SetErrorState(null);
        }
        catch (SyntaxException ex)
        {
            _diagramState.SetErrorState(ex.Message);
        }
        catch (Exception)
        {
            _diagramState.SetErrorState("An unexpected error occurred during parsing.");
        }
    }
}
