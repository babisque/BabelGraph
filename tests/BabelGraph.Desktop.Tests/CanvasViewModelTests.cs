using BabelGraph.Desktop.ViewModels;
using BabelGraph.Domain.Entities;
using BabelGraph.Domain.Interfaces;
using Moq;
using Xunit;

namespace BabelGraph.Desktop.Tests;

public class CanvasViewModelTests
{
    [Fact]
    public void Should_Update_Canvas_ViewModel_When_Domain_Raises_Event()
    {
        // Arrange
        var stateMock = new Mock<IDiagramState>();
        var viewModel = new CanvasViewModel(stateMock.Object);
        var newNodes = new List<DiagramNode> { new DiagramNode("Order") };

        // Act
        // Simulate the Application layer notifying that the Domain state has changed
        stateMock.Raise(s => s.DiagramChanged += null, new DiagramChangedEventArgs(newNodes));

        // Assert
        Assert.Single(viewModel.Nodes);
        Assert.Equal("Order", viewModel.Nodes[0].Name);
    }
}
