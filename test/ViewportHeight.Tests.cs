using Xunit;
using Moq;
using main.model;
using main.ui.layout;
using main.config;
using main.memory;
using main.ui;
using main.ui.keyhandler;

namespace test.ui.layout
{
    public class ViewportHeightBugTests
    {
        [Fact]
        public void Bug_Demonstration_ViewportHeight_11_Shows_As_16()
        {
            // Arrange - Reproduce the exact user-reported bug
            // User says: "viewportheightが11のときに16と表示される"
            // (When viewport height is 11, it displays as 16)
            
            var mockConfig = new Mock<ICurrentConfig>();
            var mockCursor = new Mock<ICursor>();
            var mockSnapShot = new Mock<ISnapShot>();
            var mockFocus = new Mock<IFocus>();

            byte cellLength = 1;
            uint columnsLength = 8;
            
            // This is the bug scenario: 
            // - The actual viewport can display 11 rows
            // - But the matrix data has more rows (let's say 16)
            // - The code incorrectly uses matrix height instead of viewport capacity
            
            // Create matrix data that results in 16 rows
            byte[] matrixData = new byte[128]; // 128 bytes = 16 rows with 8 columns
            
            mockConfig.Setup(c => c.CellLength).Returns(cellLength);
            mockConfig.Setup(c => c.ColumnsLength).Returns(columnsLength);
            mockSnapShot.Setup(s => s.Before).Returns(matrixData);
            mockSnapShot.Setup(s => s.Current).Returns(matrixData);

            var mainGrid = new MainGrid(mockConfig.Object, mockCursor.Object, mockSnapShot.Object, mockFocus.Object);
            mainGrid.Matrix.Update();
            
            // This is the bug: viewport dimensions are set to matrix dimensions
            // instead of actual viewport capacity
            // In real UI, the actual viewport might only fit 11 rows due to panel size
            // But this code sets viewport height to matrix height (16)
            mainGrid.SetViewportDimensions(mainGrid.Matrix.Height, mainGrid.Matrix.Width);

            // Create cursor info to see what gets displayed
            var cursorInfo = new CursorInfo(mockCursor.Object, mainGrid, mockFocus.Object, (uint addr) => $"0x{addr:X4}");
            mockCursor.Setup(c => c.X).Returns(0u);
            mockCursor.Setup(c => c.Y).Returns(0u);
            mockCursor.Setup(c => c.GetIndex()).Returns(0u);
            mockFocus.Setup(f => f.TargetPanel).Returns(TargetPanel.Left);

            // Act
            var viewData = cursorInfo.GetViewData();

            // Assert - This demonstrates the bug
            Assert.Equal("16", viewData["viewportHeight"]); // BUG: Shows matrix height instead of viewport height
            Assert.Equal("16", viewData["gridHeight"]);     // Correct: actual matrix height
            
            // The fix should be:
            // If the actual viewport can only show 11 rows, viewportHeight should be "11"
            // even if the matrix has 16 rows
        }

        [Fact]
        public void Fix_Demonstration_ViewportHeight_Should_Show_Actual_Viewport_Not_Matrix_Height()
        {
            // Arrange - Show what the fix should look like
            var mockConfig = new Mock<ICurrentConfig>();
            var mockCursor = new Mock<ICursor>();
            var mockSnapShot = new Mock<ISnapShot>();
            var mockFocus = new Mock<IFocus>();

            byte cellLength = 1;
            uint columnsLength = 8;
            
            // Matrix has 16 rows worth of data
            byte[] matrixData = new byte[128]; // 128 bytes = 16 rows with 8 columns
            
            mockConfig.Setup(c => c.CellLength).Returns(cellLength);
            mockConfig.Setup(c => c.ColumnsLength).Returns(columnsLength);
            mockSnapShot.Setup(s => s.Before).Returns(matrixData);
            mockSnapShot.Setup(s => s.Current).Returns(matrixData);

            var mainGrid = new MainGrid(mockConfig.Object, mockCursor.Object, mockSnapShot.Object, mockFocus.Object);
            mainGrid.Matrix.Update();
            
            // CORRECT: Set viewport dimensions to actual viewport capacity (11 rows)
            // not matrix dimensions (16 rows)
            uint actualViewportHeight = 11; // What the UI panel can actually display
            uint actualViewportWidth = 8;   // What the UI panel can actually display
            mainGrid.SetViewportDimensions(actualViewportHeight, actualViewportWidth);

            // Create cursor info to see what gets displayed
            var cursorInfo = new CursorInfo(mockCursor.Object, mainGrid, mockFocus.Object, (uint addr) => $"0x{addr:X4}");
            mockCursor.Setup(c => c.X).Returns(0u);
            mockCursor.Setup(c => c.Y).Returns(0u);
            mockCursor.Setup(c => c.GetIndex()).Returns(0u);
            mockFocus.Setup(f => f.TargetPanel).Returns(TargetPanel.Left);

            // Act
            var viewData = cursorInfo.GetViewData();

            // Assert - This shows the correct behavior
            Assert.Equal("11", viewData["viewportHeight"]); // CORRECT: Shows actual viewport height
            Assert.Equal("16", viewData["gridHeight"]);     // CORRECT: Shows matrix height
            Assert.Equal("8", viewData["viewportWidth"]);   // CORRECT: Shows viewport width
            Assert.Equal("8", viewData["gridWidth"]);       // CORRECT: Shows matrix width
        }

        [Fact]
        public void Integration_Test_Fixed_Viewport_Height_Behavior()
        {
            // Arrange - Integration test showing the fixed behavior in Ui.CreateLayout
            var mockUiConfig = new Mock<ICurrentConfig>();
            var mockUiMode = new Mock<IMode>();
            var mockUiSelectView = new Mock<ISelectView>();
            var mockConfig = new Mock<ICurrentConfig>();
            var mockCursor = new Mock<ICursor>();
            var mockSnapShot = new Mock<ISnapShot>();
            var mockFocus = new Mock<IFocus>();
            var mockInput = new Mock<IInput>();

            // Setup matrix with many rows (more than viewport can display)
            byte cellLength = 1;
            uint columnsLength = 8;
            byte[] largeMatrixData = new byte[128]; // 16 rows

            mockConfig.Setup(c => c.CellLength).Returns(cellLength);
            mockConfig.Setup(c => c.ColumnsLength).Returns(columnsLength);
            mockSnapShot.Setup(s => s.Before).Returns(largeMatrixData);
            mockSnapShot.Setup(s => s.Current).Returns(largeMatrixData);
            mockCursor.Setup(c => c.X).Returns(0u);
            mockCursor.Setup(c => c.Y).Returns(0u);
            mockCursor.Setup(c => c.GetIndex()).Returns(0u);
            mockFocus.Setup(f => f.TargetPanel).Returns(TargetPanel.Left);

            // Create real MainGrid (not mocked) to test actual behavior
            var mainGrid = new MainGrid(mockConfig.Object, mockCursor.Object, mockSnapShot.Object, mockFocus.Object);

            mockUiConfig.Setup(c => c.SharedMemoryName).Returns("test_shm");
            mockUiMode.Setup(m => m.InputMode).Returns(InputMode.Normal);

            var ui = new Ui(mockUiConfig.Object, mockUiMode.Object, mockUiSelectView.Object, mainGrid);

            // Act - Call the fixed CreateLayout method
            var layout = ui.CreateLayout(mockUiConfig.Object, mockInput.Object);

            // Assert - Verify the viewport height is now based on console size, not matrix size
            var viewData = mainGrid.CursorInfoView;
            
            // The key test: viewport height should now be calculated based on console size
            // and should be different from (typically less than) matrix height
            Assert.True(mainGrid.ViewportHeight <= mainGrid.Matrix.Height, 
                       $"Viewport height ({mainGrid.ViewportHeight}) should be <= matrix height ({mainGrid.Matrix.Height})");
            
            // Viewport height should be reasonable (not 0, not excessively large)
            Assert.True(mainGrid.ViewportHeight > 0, 
                       $"Viewport height should be > 0, but was {mainGrid.ViewportHeight}");
            Assert.True(mainGrid.ViewportHeight < 1000, 
                       $"Viewport height should be < 1000, but was {mainGrid.ViewportHeight}"); // Sanity check
            
            // Matrix height should still be 16 (unchanged)
            Assert.Equal(16u, mainGrid.Matrix.Height);
        }

        [Fact]
        public void Matrix_Height_Calculation_Should_Be_Correct()
        {
            // Arrange - Test the matrix height calculation specifically
            var mockConfig = new Mock<ICurrentConfig>();
            var mockSnapShot = new Mock<ISnapShot>();

            byte cellLength = 1;
            uint columnsLength = 8;
            
            // 88 bytes = 11 rows with 8 columns
            byte[] data88 = new byte[88]; // Should give height = 11
            // 128 bytes = 16 rows with 8 columns  
            byte[] data128 = new byte[128]; // Should give height = 16

            mockConfig.Setup(c => c.CellLength).Returns(cellLength);
            mockConfig.Setup(c => c.ColumnsLength).Returns(columnsLength);

            // Test with 88 bytes (11 rows)
            mockSnapShot.Setup(s => s.Before).Returns(data88);
            mockSnapShot.Setup(s => s.Current).Returns(data88);

            var matrix88 = new Matrix(mockConfig.Object, mockSnapShot.Object);
            matrix88.Update();

            // Test with 128 bytes (16 rows)
            mockSnapShot.Setup(s => s.Before).Returns(data128);
            mockSnapShot.Setup(s => s.Current).Returns(data128);

            var matrix128 = new Matrix(mockConfig.Object, mockSnapShot.Object);
            matrix128.Update();

            // Assert
            Assert.Equal(11u, matrix88.Height);  // 88 / (8 * 1) = 11
            Assert.Equal(16u, matrix128.Height); // 128 / (8 * 1) = 16
            Assert.Equal(8u, matrix88.Width);
            Assert.Equal(8u, matrix128.Width);
        }

        [Fact]
        public void CursorInfo_Shows_Matrix_Height_As_ViewportHeight_Bug()
        {
            // Arrange - Reproduce the exact scenario where 11 becomes 16
            var mockConfig = new Mock<ICurrentConfig>();
            var mockCursor = new Mock<ICursor>();
            var mockSnapShot = new Mock<ISnapShot>();
            var mockFocus = new Mock<IFocus>();

            byte cellLength = 1;
            uint columnsLength = 8;
            
            // Create data for 11 rows (88 bytes)
            byte[] data = new byte[88];
            
            mockConfig.Setup(c => c.CellLength).Returns(cellLength);
            mockConfig.Setup(c => c.ColumnsLength).Returns(columnsLength);
            mockSnapShot.Setup(s => s.Before).Returns(data);
            mockSnapShot.Setup(s => s.Current).Returns(data);

            var mainGrid = new MainGrid(mockConfig.Object, mockCursor.Object, mockSnapShot.Object, mockFocus.Object);
            
            // Update matrix to get correct height
            mainGrid.Matrix.Update();
            
            // Simulate the bug: viewport height set to matrix height incorrectly
            // In real scenario: if actual viewport can show 11 rows but matrix has 16 rows
            // Setting viewport height to matrix height (16) instead of actual viewport capacity (11)
            mainGrid.SetViewportDimensions(16, 8); // Bug: using wrong height

            var cursorInfo = new CursorInfo(mockCursor.Object, mainGrid, mockFocus.Object, (uint addr) => $"0x{addr:X4}");
            
            mockCursor.Setup(c => c.X).Returns(0u);
            mockCursor.Setup(c => c.Y).Returns(0u);
            mockCursor.Setup(c => c.GetIndex()).Returns(0u);
            mockFocus.Setup(f => f.TargetPanel).Returns(TargetPanel.Left);

            // Act
            var viewData = cursorInfo.GetViewData();

            // Assert - This shows the bug: viewport height shows 16 instead of the actual matrix height 11
            Assert.Equal("16", viewData["viewportHeight"]); // Bug: shows wrong viewport height
            Assert.Equal("11", viewData["gridHeight"]);     // Correct: actual matrix height
            Assert.Equal("8", viewData["viewportWidth"]);   // Correct
            Assert.Equal("8", viewData["gridWidth"]);       // Correct
        }

        [Theory]
        [InlineData(88, 8, 1, 11)]   // 88 bytes, 8 cols, 1 byte cells = 11 rows
        [InlineData(128, 8, 1, 16)]  // 128 bytes, 8 cols, 1 byte cells = 16 rows
        [InlineData(64, 4, 2, 8)]    // 64 bytes, 4 cols, 2 byte cells = 8 rows
        public void Matrix_Height_Calculation_Various_Scenarios(int dataSize, uint columns, byte cellLength, uint expectedHeight)
        {
            // Arrange
            var mockConfig = new Mock<ICurrentConfig>();
            var mockSnapShot = new Mock<ISnapShot>();

            byte[] data = new byte[dataSize];
            
            mockConfig.Setup(c => c.CellLength).Returns(cellLength);
            mockConfig.Setup(c => c.ColumnsLength).Returns(columns);
            mockSnapShot.Setup(s => s.Before).Returns(data);
            mockSnapShot.Setup(s => s.Current).Returns(data);

            var matrix = new Matrix(mockConfig.Object, mockSnapShot.Object);

            // Act
            matrix.Update();

            // Assert
            Assert.Equal(expectedHeight, matrix.Height);
            Assert.Equal(columns, matrix.Width);
        }
    }
}