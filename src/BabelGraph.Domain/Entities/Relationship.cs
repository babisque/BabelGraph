namespace BabelGraph.Domain.Entities;

public class Relationship
{
    public DiagramNode Source { get; private set; }
    public DiagramNode Target { get; private set; }

    public Relationship(DiagramNode source, DiagramNode target)
    {
        Source = source;
        Target = target;
    }
}
