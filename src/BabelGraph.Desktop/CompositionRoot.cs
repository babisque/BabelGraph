using BabelGraph.Domain.Interfaces;
using BabelGraph.Application.Services;
using BabelGraph.Desktop.Models;
using BabelGraph.Desktop.Services;
using BabelGraph.Desktop.ViewModels;
using BabelGraph.Infrastructure.Interfaces;
using BabelGraph.Infrastructure.Parsers;
using BabelGraph.Infrastructure.Serialization;
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
        services.AddSingleton<IDiagramSerializer, DiagramJsonSerializer>();

        // Domain State (shared between layers)
        services.AddSingleton<DiagramState>();
        services.AddSingleton<IDiagramState>(sp => sp.GetRequiredService<DiagramState>());

        // Services
        services.AddSingleton<IEditorService, DesktopEditorService>();
        services.AddSingleton<ISynchronizationService, SynchronizationService>();

        // ViewModels
        services.AddTransient<CanvasViewModel>();
        services.AddTransient<MainWindowViewModel>();

        _serviceProvider = services.BuildServiceProvider();
    }

    public MainWindowViewModel ResolveMainWindowViewModel() => _serviceProvider.GetRequiredService<MainWindowViewModel>();
}
