namespace BabelGraph.Domain.Entities;

public class Relationship(DiagramNode source, DiagramNode target)
{
    public DiagramNode Source { get; private set; } = source;
    public DiagramNode Target { get; private set; } = target;
}
