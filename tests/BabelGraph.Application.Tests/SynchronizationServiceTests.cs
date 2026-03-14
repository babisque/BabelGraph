using BabelGraph.Application.Services;
using BabelGraph.Domain.Entities;
using BabelGraph.Domain.Interfaces;
using BabelGraph.Infrastructure.Interfaces;
using Moq;
using Xunit;

namespace BabelGraph.Application.Tests;

public class SynchronizationServiceTests
{
    private readonly Mock<IParserService> _parserMock;
    private readonly Mock<IDiagramState> _stateMock;
    private readonly SynchronizationService _service;

    public SynchronizationServiceTests()
    {
        _parserMock = new Mock<IParserService>();
        _stateMock = new Mock<IDiagramState>();
        _service = new SynchronizationService(_parserMock.Object, _stateMock.Object);
    }

    [Fact]
    public async Task Should_Update_Domain_When_Syntax_Is_Valid()
    {
        // Arrange
        var validText = "class User { name: string }";
        var parsedNodes = new List<DiagramNode> { new DiagramNode("User") };
        
        _parserMock.Setup(p => p.Parse(validText)).Returns(parsedNodes);

        // Act
        await _service.ProcessTextInputAsync(validText);

        // Assert
        _stateMock.Verify(s => s.UpdateDiagram(parsedNodes), Times.Once);
        _stateMock.Verify(s => s.SetErrorState(null), Times.Once);
    }

    [Fact]
    public async Task Should_Lock_State_And_Return_Error_When_Syntax_Is_Invalid()
    {
        // Arrange
        var invalidText = "class User {";
        var errorMessage = "Syntax error: missing closing brace";
        
        _parserMock.Setup(p => p.Parse(invalidText))
                   .Throws(new SyntaxException(errorMessage));

        // Act
        await _service.ProcessTextInputAsync(invalidText);

        // Assert
        // State should NOT be updated with new nodes
        _stateMock.Verify(s => s.UpdateDiagram(It.IsAny<IEnumerable<DiagramNode>>()), Times.Never);
        
        // State should be notified of the error
        _stateMock.Verify(s => s.SetErrorState(errorMessage), Times.Once);
        
        // State should remain "Frozen" (implied by lack of update)
    }

    [Fact]
    public async Task Should_Only_Process_Input_After_Debounce_Delay()
    {
        // Arrange
        var text1 = "cl";
        var text2 = "class";
        var text3 = "class User";
        
        // Act
        // Simulate rapid typing
        var task1 = _service.ProcessTextInputAsync(text1);
        var task2 = _service.ProcessTextInputAsync(text2);
        var task3 = _service.ProcessTextInputAsync(text3);

        // Wait longer than the 300ms debounce
        await Task.Delay(500);

        // Assert
        // Parser should only be called for the final state
        _parserMock.Verify(p => p.Parse(It.IsAny<string>()), Times.Once);
        _parserMock.Verify(p => p.Parse(text3), Times.Once);
    }
}
