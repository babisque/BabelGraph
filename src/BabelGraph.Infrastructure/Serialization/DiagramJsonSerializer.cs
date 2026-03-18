using System.Collections.Generic;
using System.Text.Json;
using BabelGraph.Domain.Entities;

namespace BabelGraph.Infrastructure.Serialization;

public class DiagramJsonSerializer
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
