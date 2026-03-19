using BabelGraph.Domain.Entities;
using BabelGraph.Infrastructure.Serialization;
using System.Text.Json;
using Xunit;

namespace BabelGraph.Infrastructure.Tests;

public class JsonSerializerTests
{
    [Fact]
    public void Should_Serialize_Diagram_State_To_Valid_Json()
    {
        // Arrange
        var serializer = new DiagramJsonSerializer();
        var node = new DiagramNode("UserNode");
        node.UpdatePosition(120.5, 450.0);
        var nodes = new List<DiagramNode> { node };

        // Act
        var json = DiagramJsonSerializer.Serialize(nodes);

        // Assert
        using var document = JsonDocument.Parse(json);
        var root = document.RootElement;
        
        // Check if it's an array or a wrapper object (depending on design)
        var firstNode = root.EnumerateArray().First();
        
        Assert.Equal("UserNode", firstNode.GetProperty("Name").GetString());
        Assert.Equal(120.5, firstNode.GetProperty("X").GetDouble());
        Assert.Equal(450.0, firstNode.GetProperty("Y").GetDouble());
    }
}
