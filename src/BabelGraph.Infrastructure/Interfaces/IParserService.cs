using BabelGraph.Domain.Entities;

namespace BabelGraph.Infrastructure.Interfaces;

public class SyntaxException(string message) : Exception(message);

public interface IParserService
{
    IEnumerable<DiagramNode> Parse(string text);
}
