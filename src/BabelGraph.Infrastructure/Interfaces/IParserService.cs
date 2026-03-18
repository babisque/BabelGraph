using System;
using System.Collections.Generic;
using BabelGraph.Domain.Entities;

namespace BabelGraph.Infrastructure.Interfaces;

public class SyntaxException : Exception
{
    public SyntaxException(string message) : base(message)
    {
    }
}

public interface IParserService
{
    IEnumerable<DiagramNode> Parse(string text);
}
