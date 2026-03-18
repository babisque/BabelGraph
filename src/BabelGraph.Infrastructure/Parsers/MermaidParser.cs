using System;
using System.Collections.Generic;
using System.Linq;
using BabelGraph.Domain.Entities;
using BabelGraph.Infrastructure.Interfaces;

namespace BabelGraph.Infrastructure.Parsers;

public class MermaidParser : IParserService
{
    public IEnumerable<DiagramNode> Parse(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return Enumerable.Empty<DiagramNode>();
        }

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
            ParseERDiagram(lines.Skip(1), nodes);
        }
        else
        {
            // Default to class diagram parsing if no header
            ParseClassDiagram(lines, nodes);
        }

        return nodes;
    }

    private void ParseClassDiagram(IEnumerable<string> lines, List<DiagramNode> nodes)
    {
        DiagramNode? currentNode = null;

        foreach (var line in lines)
        {
            if (line.StartsWith("class "))
            {
                var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length >= 2)
                {
                    var nodeName = parts[1].TrimEnd('{');
                    currentNode = nodes.FirstOrDefault(n => n.Name == nodeName);
                    if (currentNode == null)
                    {
                        currentNode = new DiagramNode(nodeName);
                        nodes.Add(currentNode);
                    }
                }
            }
            else if (line.Contains(":") && currentNode != null)
            {
                // Property or method
                var member = line.TrimStart(':').Trim();
                if (member.Contains("("))
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

    private void ParseERDiagram(IEnumerable<string> lines, List<DiagramNode> nodes)
    {
        foreach (var line in lines)
        {
            // Simple ER entity parsing: "ENTITY_NAME {"
            if (line.Contains("{"))
            {
                var entityName = line.Split('{')[0].Trim();
                if (!string.IsNullOrEmpty(entityName) && !nodes.Any(n => n.Name == entityName))
                {
                    nodes.Add(new DiagramNode(entityName));
                }
            }
            // Simple relationship parsing: "ENTITY1 ||--o{ ENTITY2 : label"
            else if (line.Contains("--"))
            {
                var parts = line.Split(new[] { "--" }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length >= 2)
                {
                    var entity1 = parts[0].Trim().Split(' ').Last();
                    var entity2 = parts[1].Trim().Split(' ')[0];

                    AddNodeIfMissing(entity1, nodes);
                    AddNodeIfMissing(entity2, nodes);
                }
            }
        }
    }

    private void AddNodeIfMissing(string name, List<DiagramNode> nodes)
    {
        if (!string.IsNullOrEmpty(name) && !nodes.Any(n => n.Name == name))
        {
            nodes.Add(new DiagramNode(name));
        }
    }
}
