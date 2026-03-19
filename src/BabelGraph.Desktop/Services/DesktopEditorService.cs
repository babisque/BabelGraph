using System;
using BabelGraph.Domain.Interfaces;

namespace BabelGraph.Desktop.Services;

public class DesktopEditorService : IEditorService
{
    public event Action<string>? TextUpdated;

    public void UpdateText(string text)
    {
        TextUpdated?.Invoke(text);
    }
}
