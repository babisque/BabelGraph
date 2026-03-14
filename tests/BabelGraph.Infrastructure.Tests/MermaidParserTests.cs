using BabelGraph.Domain.Entities;
using BabelGraph.Infrastructure.Parsers;
using Xunit;

namespace BabelGraph.Infrastructure.Tests;

public class MermaidParserTests
{
    [Fact]
    public void Should_Parse_Mermaid_Class_String_Into_Domain_Entity()
    {
        // Arrange
        var parser = new MermaidParser();
        var mermaidString = "classDiagram\nclass BankAccount";

        // Act
        var result = parser.Parse(mermaidString);

        // Assert
        Assert.Single(result);
        var node = result.First();
        Assert.Equal("BankAccount", node.Name);
    }
}
