using BabelGraph.Domain.Entities;

namespace BabelGraph.Domain.Interfaces;

public interface IDiagramSerializer
{
    string Serialize(IEnumerable<DiagramNode> nodes);
}
