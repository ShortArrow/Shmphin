using Xunit;
using main.ui.layout;
using main.model;
using main.memory;
using main.config; // For ICurrentConfig
using Moq; // For Moq
using System; // For byte arrays

namespace test;

// MockConfig for testing purposes
public class MockConfig : ICurrentConfig
{
    public uint ColumnsLength { get; set; } = 8;
    public uint CellLength { get; set; } = 1;
    public string SharedMemoryName { get; set; } = "test_shm";
    public event Action? ConfigChanged;

    public void TriggerConfigChanged() => ConfigChanged?.Invoke();

    public void Update(Args args) { /* No-op for mock */ }
}

public class MainGridTests
{
    private const int DisplayableHeight = 20; // From MainGrid

    private MainGrid SetupMainGridWithMatrixHeight(uint desiredMatrixHeight, uint columnsLength = 8, uint cellLength = 1)
    {
        var mockConfig = new MockConfig { ColumnsLength = columnsLength, CellLength = cellLength };
        var mockCursor = new Mock<ICursor>();
        var mockFocus = new Mock<IFocus>();
        var snapShot = new SnapShot(mockConfig);

        if (desiredMatrixHeight > 0)
        {
            var dataSize = desiredMatrixHeight * columnsLength * cellLength;
            snapShot.Current = new byte[dataSize];
            snapShot.Before = new byte[dataSize];
            // Initialize arrays if needed, for these tests, size is what matters.
        }
        else
        {
            snapShot.Current = Array.Empty<byte>();
            snapShot.Before = Array.Empty<byte>();
        }
        
        // The MainGrid constructor creates its own Matrix and the Matrix constructor calls Update if snapShot.Current is not null.
        var mainGrid = new MainGrid(mockConfig, mockCursor.Object, snapShot, mockFocus.Object);
        // No need to call mainGrid.Matrix.Update() explicitly as SnapShot is prepared before MainGrid creation.
        
        return mainGrid;
    }

    [Fact]
    public void ScrollDown_IncrementsOffset_WhenSpaceAvailable()
    {
        var mainGrid = SetupMainGridWithMatrixHeight(50); // Matrix height > DisplayableHeight
        Assert.Equal(0, mainGrid.TestableScrollOffsetY);

        mainGrid.ScrollDown();
        Assert.Equal(1, mainGrid.TestableScrollOffsetY);
    }

    [Fact]
    public void ScrollDown_StopsAtMaxOffset()
    {
        uint matrixHeight = 30; // e.g. 30 rows
        var mainGrid = SetupMainGridWithMatrixHeight(matrixHeight);
        int expectedMaxOffset = (int)matrixHeight - DisplayableHeight; // 30 - 20 = 10

        for (int i = 0; i < matrixHeight; i++) // Scroll enough times
        {
            mainGrid.ScrollDown();
        }
        
        Assert.Equal(expectedMaxOffset, mainGrid.TestableScrollOffsetY);

        mainGrid.ScrollDown(); // Try to scroll past max
        Assert.Equal(expectedMaxOffset, mainGrid.TestableScrollOffsetY);
    }

    [Fact]
    public void ScrollDown_DoesNotScroll_WhenMatrixHeightLessThanDisplayable()
    {
        var mainGrid = SetupMainGridWithMatrixHeight(10); // Matrix height < DisplayableHeight
        Assert.Equal(0, mainGrid.TestableScrollOffsetY);

        mainGrid.ScrollDown();
        Assert.Equal(0, mainGrid.TestableScrollOffsetY);
    }
    
    [Fact]
    public void ScrollDown_DoesNotScroll_WhenMatrixHeightIsZero()
    {
        var mainGrid = SetupMainGridWithMatrixHeight(0);
        Assert.Equal(0, mainGrid.TestableScrollOffsetY);
        mainGrid.ScrollDown();
        Assert.Equal(0, mainGrid.TestableScrollOffsetY);
    }

    [Fact]
    public void ScrollUp_DecrementsOffset_WhenNotAtTop()
    {
        var mainGrid = SetupMainGridWithMatrixHeight(50);
        mainGrid.ScrollDown(); // scrollOffsetY = 1
        mainGrid.ScrollDown(); // scrollOffsetY = 2
        Assert.Equal(2, mainGrid.TestableScrollOffsetY);

        mainGrid.ScrollUp();
        Assert.Equal(1, mainGrid.TestableScrollOffsetY);
    }

    [Fact]
    public void ScrollUp_StopsAtZeroOffset()
    {
        var mainGrid = SetupMainGridWithMatrixHeight(50);
        mainGrid.ScrollDown(); // scrollOffsetY = 1
        Assert.Equal(1, mainGrid.TestableScrollOffsetY);

        mainGrid.ScrollUp();
        Assert.Equal(0, mainGrid.TestableScrollOffsetY);

        mainGrid.ScrollUp(); // Try to scroll past min
        Assert.Equal(0, mainGrid.TestableScrollOffsetY);
    }

    [Fact]
    public void EnsureRowIsVisible_ScrollsDown_WhenRowIsBelowView()
    {
        var mainGrid = SetupMainGridWithMatrixHeight(100); // Matrix height = 100
        // Initial: scrollOffsetY = 0, visible rows [0..19]
        
        mainGrid.EnsureRowIsVisible(25); // Row 25 is below current viewport
        // Expected: scrollOffsetY = 25 - 20 + 1 = 6. Viewport [6..25]
        Assert.Equal(6, mainGrid.TestableScrollOffsetY);
    }

    [Fact]
    public void EnsureRowIsVisible_ScrollsUp_WhenRowIsAboveView()
    {
        var mainGrid = SetupMainGridWithMatrixHeight(100);
        // Scroll down first to set an initial offset
        for(int i=0; i < 10; i++) mainGrid.ScrollDown(); // scrollOffsetY = 10, visible [10..29]
        Assert.Equal(10, mainGrid.TestableScrollOffsetY);

        mainGrid.EnsureRowIsVisible(5); // Row 5 is above current viewport
        // Expected: scrollOffsetY = 5. Viewport [5..24]
        Assert.Equal(5, mainGrid.TestableScrollOffsetY);
    }

    [Fact]
    public void EnsureRowIsVisible_DoesNotScroll_WhenRowIsVisible()
    {
        var mainGrid = SetupMainGridWithMatrixHeight(100);
        for(int i=0; i < 10; i++) mainGrid.ScrollDown(); // scrollOffsetY = 10, visible [10..29]
        Assert.Equal(10, mainGrid.TestableScrollOffsetY);

        mainGrid.EnsureRowIsVisible(15); // Row 15 is already visible
        Assert.Equal(10, mainGrid.TestableScrollOffsetY); // No change
    }

    [Fact]
    public void EnsureRowIsVisible_HandlesTopBoundary()
    {
        var mainGrid = SetupMainGridWithMatrixHeight(100);
        // scrollOffsetY = 0
        mainGrid.EnsureRowIsVisible(0); // Row 0 is at the top boundary
        Assert.Equal(0, mainGrid.TestableScrollOffsetY);
    }

    [Fact]
    public void EnsureRowIsVisible_HandlesBottomBoundary()
    {
        var mainGrid = SetupMainGridWithMatrixHeight(30); // MatrixHeight = 30
        // scrollOffsetY = 0, visible [0..19]
        mainGrid.EnsureRowIsVisible(29); // Row 29 is at the bottom
        // Expected: scrollOffsetY = 29 - 20 + 1 = 10. Viewport [10..29]
        // Max scroll offset is MatrixHeight - DisplayableHeight = 30 - 20 = 10
        Assert.Equal(10, mainGrid.TestableScrollOffsetY);
    }

    [Fact]
    public void EnsureRowIsVisible_HandlesFullRangeVisibility()
    {
        // Matrix height less than displayable height
        var mainGrid = SetupMainGridWithMatrixHeight(15); 
        // scrollOffsetY = 0, visible [0..14]
        mainGrid.EnsureRowIsVisible(10); 
        Assert.Equal(0, mainGrid.TestableScrollOffsetY); // No change, entire matrix is visible

        mainGrid.EnsureRowIsVisible(0); 
        Assert.Equal(0, mainGrid.TestableScrollOffsetY);

        mainGrid.EnsureRowIsVisible(14); 
        Assert.Equal(0, mainGrid.TestableScrollOffsetY);
    }
    
    [Fact]
    public void EnsureRowIsVisible_WhenMatrixHeightIsZero()
    {
        var mainGrid = SetupMainGridWithMatrixHeight(0);
        mainGrid.EnsureRowIsVisible(0); // Requesting row 0
        Assert.Equal(0, mainGrid.TestableScrollOffsetY); // Should remain 0

        mainGrid.EnsureRowIsVisible(10); // Requesting any row
        Assert.Equal(0, mainGrid.TestableScrollOffsetY); // Should remain 0
    }
}
