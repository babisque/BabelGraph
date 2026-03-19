using BabelGraph.Application.Services;
using BabelGraph.Domain.Entities;
using BabelGraph.Domain.Interfaces;
using BabelGraph.Infrastructure.Interfaces;
using Moq;

namespace BabelGraph.Application.Tests;

public class SynchronizationServiceTests
{
    private readonly Mock<IParserService> _parserMock;
    private readonly Mock<IDiagramState> _stateMock;
    private readonly Mock<IDiagramSerializer> _serializerMock;
    private readonly Mock<IEditorService> _editorMock;
    private readonly SynchronizationService _service;

    public SynchronizationServiceTests()
    {
        _parserMock = new Mock<IParserService>();
        _stateMock = new Mock<IDiagramState>();
        _serializerMock = new Mock<IDiagramSerializer>();
        _editorMock = new Mock<IEditorService>();
        _service = new SynchronizationService(
            _parserMock.Object, 
            _stateMock.Object, 
            _serializerMock.Object,
            _editorMock.Object);
    }

    [Fact]
    public async Task Should_Update_Domain_When_Syntax_Is_Valid()
    {
        // Arrange
        const string validText = "class User { name: string }";
        var parsedNodes = new List<DiagramNode> { new("User") };
        
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
        const string invalidText = "class User {";
        const string errorMessage = "Syntax error: missing closing brace";
        
        _parserMock.Setup(p => p.Parse(invalidText))
                   .Throws(new SyntaxException(errorMessage));

        // Act
        await _service.ProcessTextInputAsync(invalidText);

        // Assert
        _stateMock.Verify(s => s.UpdateDiagram(It.IsAny<IEnumerable<DiagramNode>>()), Times.Never);
        _stateMock.Verify(s => s.SetErrorState(errorMessage), Times.Once);
    }

    [Fact]
    public async Task Should_Only_Process_Input_After_Debounce_Delay()
    {
        // Arrange
        const string text1 = "cl";
        const string text2 = "class";
        const string text3 = "class User";
        
        // Act
        // Simulate rapid typing
        var task1 = _service.ProcessTextInputAsync(text1);
        var task2 = _service.ProcessTextInputAsync(text2);
        var task3 = _service.ProcessTextInputAsync(text3);

        await Task.Delay(500);

        // Assert
        _parserMock.Verify(p => p.Parse(It.IsAny<string>()), Times.Once);
        _parserMock.Verify(p => p.Parse(text3), Times.Once);
    }

    [Fact]
    public async Task Should_Update_Domain_And_Regenerate_Text_When_Node_Moved()
    {
        // Arrange
        const string nodeName = "User";
        const double newX = 150.0;
        const double newY = 200.0;
        var nodes = new List<DiagramNode> { new(nodeName) };
        const string regeneratedText = "class User { ... } // Updated position";

        _stateMock.Setup(s => s.Nodes).Returns(nodes);
        _serializerMock.Setup(s => s.Serialize(nodes)).Returns(regeneratedText);

        // Act
        await _service.UpdateNodePositionAsync(nodeName, newX, newY);

        // Assert
        _stateMock.Verify(s => s.UpdateNodePosition(nodeName, newX, newY), Times.Once);
        
        _serializerMock.Verify(s => s.Serialize(nodes), Times.Once);
        _editorMock.Verify(e => e.UpdateText(regeneratedText), Times.Once);
    }
}
