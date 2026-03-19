using BabelGraph.Domain.Entities;
using BabelGraph.Infrastructure.Interfaces;

namespace BabelGraph.Infrastructure.Parsers;

public class MermaidParser : IParserService
{
    public IEnumerable<DiagramNode> Parse(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return [];

        var lines = text.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var nodes = new List<DiagramNode>();

        if (lines.Length == 0) return nodes;

        var firstLine = lines[0].ToLower();
        if (firstLine.Contains("classdiagram"))
        {
            ParseClassDiagram(lines.Skip(1), nodes);
        }
        else if (firstLine.Contains("erdiagram"))
        {
            ParseErDiagram(lines.Skip(1), nodes);
        }
        else
        {
            ParseClassDiagram(lines, nodes);
        }

        return nodes;
    }

    private static void ParseClassDiagram(IEnumerable<string> lines, List<DiagramNode> nodes)
    {
        DiagramNode? currentNode = null;

        foreach (var line in lines)
        {
            if (line.StartsWith("class "))
            {
                var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length < 2) continue;
                
                var nodeName = parts[1].TrimEnd('{');
                currentNode = nodes.FirstOrDefault(n => n.Name == nodeName);
                if (currentNode != null) continue;
                
                currentNode = new DiagramNode(nodeName);
                nodes.Add(currentNode);
            }
            else if (line.Contains(':') && currentNode != null)
            {
                var member = line.TrimStart(':').Trim();
                if (member.Contains('('))
                {
                    currentNode.Methods.Add(member);
                }
                else
                {
                    currentNode.Attributes.Add(member);
                }
            }
        }
    }

    private void ParseErDiagram(IEnumerable<string> lines, List<DiagramNode> nodes)
    {
        foreach (var line in lines)
        {
            if (line.Contains('{'))
            {
                var entityName = line.Split('{')[0].Trim();
                if (!string.IsNullOrEmpty(entityName) && nodes.All(n => n.Name != entityName))
                {
                    nodes.Add(new DiagramNode(entityName));
                }
            }
            else if (line.Contains("--"))
            {
                var parts = line.Split(["--"], StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length < 2) continue;
                
                var entity1 = parts[0].Trim().Split(' ').Last();
                var entity2 = parts[1].Trim().Split(' ')[0];

                AddNodeIfMissing(entity1, nodes);
                AddNodeIfMissing(entity2, nodes);
            }
        }
    }

    private static void AddNodeIfMissing(string name, List<DiagramNode> nodes)
    {
        if (!string.IsNullOrEmpty(name) && nodes.All(n => n.Name != name))
        {
            nodes.Add(new DiagramNode(name));
        }
    }
}
