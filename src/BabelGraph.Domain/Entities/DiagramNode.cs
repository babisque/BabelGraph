using System.Collections.Generic;
using BabelGraph.Domain.Exceptions;

namespace BabelGraph.Domain.Entities;

public class DiagramNode
{
    public string Name { get; private set; }
    public List<string> Attributes { get; private set; } = new();
    public List<string> Methods { get; private set; } = new();
    public double X { get; private set; }
    public double Y { get; private set; }

    public DiagramNode(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Node name cannot be null or empty.");
        }
        Name = name;
    }

    public void UpdatePosition(double x, double y)
    {
        if (x < 0 || y < 0)
        {
            throw new DomainException("Coordinates cannot be negative.");
        }
        X = x;
        Y = y;
    }
}
