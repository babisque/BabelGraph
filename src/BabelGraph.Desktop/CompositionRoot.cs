using BabelGraph.Application.Services;
using BabelGraph.Desktop.Models;
using BabelGraph.Desktop.ViewModels;
using BabelGraph.Domain.Interfaces;
using BabelGraph.Infrastructure.Interfaces;
using BabelGraph.Infrastructure.Parsers;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BabelGraph.Desktop;

public class CompositionRoot
{
    private readonly IServiceProvider _serviceProvider;

    public CompositionRoot()
    {
        var services = new ServiceCollection();

        // Infrastructure
        services.AddSingleton<IParserService, MermaidParser>();

        // Domain State (shared between layers)
        services.AddSingleton<DiagramState>();
        services.AddSingleton<IDiagramState>(sp => sp.GetRequiredService<DiagramState>());

        // Application Services
        services.AddSingleton<SynchronizationService>();

        // ViewModels
        services.AddTransient<CanvasViewModel>();
        services.AddTransient<MainWindowViewModel>();

        _serviceProvider = services.BuildServiceProvider();
    }

    public MainWindowViewModel ResolveMainWindowViewModel() => _serviceProvider.GetRequiredService<MainWindowViewModel>();
}
