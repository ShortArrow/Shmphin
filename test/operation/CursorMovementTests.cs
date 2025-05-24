using Xunit;
using main.operation;
using main.model;
using main.ui.layout;
using main.config; // For ICurrentConfig
using main.memory; // For ISnapShot
using main.ui; // For IFocus
using Moq;
using System;
using System.Threading.Tasks;

namespace test.operation;

public class CursorMovementTests
{
    // Helper to create a mock MainGrid instance for constructor (not for verifying calls on it directly in all tests)
    private Mock<MainGrid> CreateMockMainGrid()
    {
        return new Mock<MainGrid>(
            Mock.Of<ICurrentConfig>(),
            Mock.Of<ICursor>(), // This cursor is for MainGrid's constructor, not the one we test MoveUp/Down with
            Mock.Of<ISnapShot>(),
            Mock.Of<IFocus>()
        );
    }

    // --- MoveUp Tests ---

    [Fact]
    public void MoveUp_Name_ReturnsCorrectValue()
    {
        // Arrange
        var mockCursor = new Mock<ICursor>();
        var mockMainGrid = CreateMockMainGrid();
        Func<MainGrid> getMainGridFunc = () => mockMainGrid.Object;
        var operation = new MoveUp(mockCursor.Object, getMainGridFunc);

        // Act
        var name = operation.Name;

        // Assert
        Assert.Equal("moveup", name);
    }

    [Fact]
    public async Task MoveUp_Execute_CallsDependenciesCorrectly()
    {
        // Arrange
        var mockCursor = new Mock<ICursor>();
        uint expectedCursorY = 5;
        mockCursor.Setup(c => c.Y).Returns(expectedCursorY); // Setup Y to be returned

        var mockMainGrid = CreateMockMainGrid(); // This specific mock instance will be verified
        Func<MainGrid> getMainGridFunc = () => mockMainGrid.Object;
        var operation = new MoveUp(mockCursor.Object, getMainGridFunc);

        // Act
        await operation.Execute();

        // Assert
        mockCursor.Verify(c => c.MoveUp(), Times.Once());
        mockMainGrid.Verify(m => m.EnsureRowIsVisible(It.Is<int>(y => y == (int)expectedCursorY)), Times.Once());
    }

    [Fact]
    public void MoveUp_Constructor_ThrowsArgumentNullException_ForNullCursor()
    {
        // Arrange
        Func<MainGrid> getMainGridFunc = () => CreateMockMainGrid().Object;

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => new MoveUp(null!, getMainGridFunc));
        Assert.Equal("cursor", exception.ParamName);
    }

    [Fact]
    public void MoveUp_Constructor_ThrowsArgumentNullException_ForNullFunc()
    {
        // Arrange
        var mockCursor = new Mock<ICursor>();

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => new MoveUp(mockCursor.Object, null!));
        Assert.Equal("getMainGrid", exception.ParamName);
    }
    
    [Fact]
    public async Task MoveUp_Execute_ThrowsInvalidOperationException_IfFuncReturnsNull()
    {
        // Arrange
        var mockCursor = new Mock<ICursor>();
        Func<MainGrid> getNullMainGridFunc = () => null!;
        var operation = new MoveUp(mockCursor.Object, getNullMainGridFunc);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(async () => await operation.Execute());
        Assert.Equal("MainGrid instance cannot be obtained.", ex.Message);
        
        // Verify that cursor.MoveUp() was still called
        mockCursor.Verify(c => c.MoveUp(), Times.Once());
    }

    // --- MoveDown Tests ---

    [Fact]
    public void MoveDown_Name_ReturnsCorrectValue()
    {
        // Arrange
        var mockCursor = new Mock<ICursor>();
        var mockMainGrid = CreateMockMainGrid();
        Func<MainGrid> getMainGridFunc = () => mockMainGrid.Object;
        var operation = new MoveDown(mockCursor.Object, getMainGridFunc);

        // Act
        var name = operation.Name;

        // Assert
        Assert.Equal("movedown", name);
    }

    [Fact]
    public async Task MoveDown_Execute_CallsDependenciesCorrectly()
    {
        // Arrange
        var mockCursor = new Mock<ICursor>();
        uint expectedCursorY = 7;
        mockCursor.Setup(c => c.Y).Returns(expectedCursorY);

        var mockMainGrid = CreateMockMainGrid();
        Func<MainGrid> getMainGridFunc = () => mockMainGrid.Object;
        var operation = new MoveDown(mockCursor.Object, getMainGridFunc);

        // Act
        await operation.Execute();

        // Assert
        mockCursor.Verify(c => c.MoveDown(), Times.Once());
        mockMainGrid.Verify(m => m.EnsureRowIsVisible(It.Is<int>(y => y == (int)expectedCursorY)), Times.Once());
    }

    [Fact]
    public void MoveDown_Constructor_ThrowsArgumentNullException_ForNullCursor()
    {
        // Arrange
        Func<MainGrid> getMainGridFunc = () => CreateMockMainGrid().Object;
        
        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => new MoveDown(null!, getMainGridFunc));
        Assert.Equal("cursor", exception.ParamName);
    }

    [Fact]
    public void MoveDown_Constructor_ThrowsArgumentNullException_ForNullFunc()
    {
        // Arrange
        var mockCursor = new Mock<ICursor>();

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => new MoveDown(mockCursor.Object, null!));
        Assert.Equal("getMainGrid", exception.ParamName);
    }
    
    [Fact]
    public async Task MoveDown_Execute_ThrowsInvalidOperationException_IfFuncReturnsNull()
    {
        // Arrange
        var mockCursor = new Mock<ICursor>();
        Func<MainGrid> getNullMainGridFunc = () => null!;
        var operation = new MoveDown(mockCursor.Object, getNullMainGridFunc);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => operation.Execute());
        Assert.Equal("MainGrid instance cannot be obtained.", ex.Message);
        
        // Verify that cursor.MoveDown() was still called
        mockCursor.Verify(c => c.MoveDown(), Times.Once());
    }
}
