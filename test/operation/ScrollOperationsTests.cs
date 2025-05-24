using Xunit;
using main.operation;
using main.ui.layout;
using main.model;      // For ICursor
using main.memory;     // For ISnapShot
using main.config;     // For ICurrentConfig
using main.ui;         // For IFocus
using Moq;
using System;
using System.Threading.Tasks;

namespace test.operation;

public class ScrollOperationsTests
{
    private readonly Mock<ICurrentConfig> _mockConfig;
    private readonly Mock<ICursor> _mockCursor;
    private readonly Mock<ISnapShot> _mockSnapShot;
    private readonly Mock<IFocus> _mockFocus;

    public ScrollOperationsTests()
    {
        // Initialize common mocks once for all tests
        _mockConfig = new Mock<ICurrentConfig>();
        _mockCursor = new Mock<ICursor>();
        _mockSnapShot = new Mock<ISnapShot>();
        _mockFocus = new Mock<IFocus>();
    }

    // Helper to create a mock MainGrid instance
    private Mock<MainGrid> CreateMockMainGrid()
    {
        // Moq can mock concrete classes. It will call the constructor.
        // We provide mocks for constructor arguments.
        // If MainGrid's methods (ScrollUp/ScrollDown) were virtual, Moq could override them.
        // Since they are not, Moq will call the actual methods on a proxy.
        // For verifying calls (which is what we want here), this is fine.
        return new Mock<MainGrid>(
            _mockConfig.Object,
            _mockCursor.Object,
            _mockSnapShot.Object,
            _mockFocus.Object
        );
    }

    [Fact]
    public void ScrollUpOperation_Name_ReturnsCorrectValue()
    {
        // Arrange
        var mockMainGrid = CreateMockMainGrid();
        Func<MainGrid> getMainGridFunc = () => mockMainGrid.Object;
        var operation = new ScrollUpOperation(getMainGridFunc);

        // Act
        var name = operation.Name;

        // Assert
        Assert.Equal("scrollup", name);
    }

    [Fact]
    public async Task ScrollUpOperation_Execute_CallsMainGridScrollUp()
    {
        // Arrange
        var mockMainGrid = CreateMockMainGrid();
        Func<MainGrid> getMainGridFunc = () => mockMainGrid.Object;
        var operation = new ScrollUpOperation(getMainGridFunc);

        // Act
        await operation.Execute();

        // Assert
        // Verify that the ScrollUp method on the MainGrid instance was called once.
        mockMainGrid.Verify(m => m.ScrollUp(), Times.Once());
    }

    [Fact]
    public void ScrollUpOperation_Constructor_ThrowsArgumentNullException_ForNullFunc()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => new ScrollUpOperation(null!));
        Assert.Equal("getMainGrid", exception.ParamName);
    }
    
    [Fact]
    public void ScrollUpOperation_Execute_ThrowsInvalidOperationException_IfFuncReturnsNull()
    {
        // Arrange
        Func<MainGrid> getMainGridFunc = () => null!; // Simulate Func returning null
        var operation = new ScrollUpOperation(getMainGridFunc);

        // Act & Assert
        var exception = Assert.ThrowsAsync<InvalidOperationException>(() => operation.Execute());
        Assert.Equal("MainGrid instance cannot be obtained.", exception.Result.Message);
    }

    [Fact]
    public void ScrollDownOperation_Name_ReturnsCorrectValue()
    {
        // Arrange
        var mockMainGrid = CreateMockMainGrid();
        Func<MainGrid> getMainGridFunc = () => mockMainGrid.Object;
        var operation = new ScrollDownOperation(getMainGridFunc);

        // Act
        var name = operation.Name;

        // Assert
        Assert.Equal("scrolldown", name);
    }

    [Fact]
    public async Task ScrollDownOperation_Execute_CallsMainGridScrollDown()
    {
        // Arrange
        var mockMainGrid = CreateMockMainGrid();
        Func<MainGrid> getMainGridFunc = () => mockMainGrid.Object;
        var operation = new ScrollDownOperation(getMainGridFunc);

        // Act
        await operation.Execute();

        // Assert
        mockMainGrid.Verify(m => m.ScrollDown(), Times.Once());
    }

    [Fact]
    public void ScrollDownOperation_Constructor_ThrowsArgumentNullException_ForNullFunc()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => new ScrollDownOperation(null!));
        Assert.Equal("getMainGrid", exception.ParamName);
    }

    [Fact]
    public void ScrollDownOperation_Execute_ThrowsInvalidOperationException_IfFuncReturnsNull()
    {
        // Arrange
        Func<MainGrid> getMainGridFunc = () => null!; // Simulate Func returning null
        var operation = new ScrollDownOperation(getMainGridFunc);

        // Act & Assert
        var exception = Assert.ThrowsAsync<InvalidOperationException>(() => operation.Execute());
        Assert.Equal("MainGrid instance cannot be obtained.", exception.Result.Message);
    }
}
