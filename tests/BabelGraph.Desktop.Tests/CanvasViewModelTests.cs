using BabelGraph.Domain.Interfaces;
using BabelGraph.Application.Services;
using BabelGraph.Desktop.ViewModels;
using BabelGraph.Domain.Entities;
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
        var syncMock = new Mock<ISynchronizationService>();
        var viewModel = new CanvasViewModel(stateMock.Object, syncMock.Object);
        var newNodes = new List<DiagramNode> { new DiagramNode("Order") };

        // Act
        // Simulate the Application layer notifying that the Domain state has changed
        stateMock.Raise(s => s.DiagramChanged += null, new DiagramChangedEventArgs(newNodes));

        // Assert
        Assert.Single(viewModel.Nodes);
        Assert.Equal("Order", viewModel.Nodes[0].Name);
    }

    [Fact]
    public void Should_Update_Node_Coordinates_On_Drag()
    {
        // Arrange
        var stateMock = new Mock<IDiagramState>();
        var syncMock = new Mock<ISynchronizationService>();
        var viewModel = new CanvasViewModel(stateMock.Object, syncMock.Object);
        var node = new DiagramNode("Node1");
        node.UpdatePosition(10, 10);
        viewModel.Nodes.Add(node);

        // Act - Dragging the node by (50, 50) offset
        CanvasViewModel.MoveNode(node, 60, 60);

        // Assert
        Assert.Equal(60, node.X);
        Assert.Equal(60, node.Y);
    }

    [Fact]
    public void Should_Call_SyncService_On_Drag_Completed()
    {
        // Arrange
        var stateMock = new Mock<IDiagramState>();
        var syncMock = new Mock<ISynchronizationService>();
        var viewModel = new CanvasViewModel(stateMock.Object, syncMock.Object);
        var node = new DiagramNode("Node1");
        node.UpdatePosition(100, 100);
        viewModel.Nodes.Add(node);

        // Act - Drag completed
        viewModel.CompleteMoveNode(node);

        // Assert
        syncMock.Verify(s => s.UpdateNodePositionAsync(node.Name, 100, 100), Times.Once);
    }
}
