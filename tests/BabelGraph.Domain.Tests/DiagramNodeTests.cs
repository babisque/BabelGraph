using BabelGraph.Domain.Entities;
using BabelGraph.Domain.Exceptions;
using Xunit;

namespace BabelGraph.Domain.Tests;

public class DiagramNodeTests
{
    [Fact]
    public void Should_Create_DiagramNode_When_Name_Is_Valid()
    {
        // Arrange
        var nodeName = "UserNode";

        // Act
        var node = new DiagramNode(nodeName);

        // Assert
        Assert.Equal(nodeName, node.Name);
        Assert.Empty(node.Attributes);
        Assert.Empty(node.Methods);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public void Should_Throw_DomainException_When_Node_Name_Is_Empty(string invalidName)
    {
        // Act & Assert
        Assert.Throws<DomainException>(() => new DiagramNode(invalidName));
    }

    [Fact]
    public void Should_Update_Node_Coordinates_Successfully()
    {
        // Arrange
        var node = new DiagramNode("NodeA");
        var newX = 150.5;
        var newY = 200.0;

        // Act
        node.UpdatePosition(newX, newY);

        // Assert
        Assert.Equal(newX, node.X);
        Assert.Equal(newY, node.Y);
    }

    [Theory]
    [InlineData(-1, 10)]
    [InlineData(10, -5)]
    [InlineData(-100, -100)]
    public void Should_Throw_Exception_When_Coordinates_Are_Negative(double invalidX, double invalidY)
    {
        // Arrange
        var node = new DiagramNode("NodeA");

        // Act & Assert
        Assert.Throws<DomainException>(() => node.UpdatePosition(invalidX, invalidY));
    }

    [Fact]
    public void Should_Create_Relationship_Between_Two_Valid_Nodes()
    {
        // Arrange
        var sourceNode = new DiagramNode("Source");
        var targetNode = new DiagramNode("Target");

        // Act
        var relationship = new Relationship(sourceNode, targetNode);

        // Assert
        Assert.Equal(sourceNode, relationship.Source);
        Assert.Equal(targetNode, relationship.Target);
    }
}
