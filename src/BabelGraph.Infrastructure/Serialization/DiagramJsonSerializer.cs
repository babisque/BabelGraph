using System.Text.Json;
using BabelGraph.Domain.Entities;
using BabelGraph.Domain.Interfaces;

namespace BabelGraph.Infrastructure.Serialization;

public class DiagramJsonSerializer : IDiagramSerializer
{
    public string Serialize(IEnumerable<DiagramNode> nodes)
    {
        return JsonSerializer.Serialize(nodes, new JsonSerializerOptions { WriteIndented = true });
    }

    public IEnumerable<DiagramNode> Deserialize(string json)
    {
        return JsonSerializer.Deserialize<IEnumerable<DiagramNode>>(json) ?? new List<DiagramNode>();
    }
}
