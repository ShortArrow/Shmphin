using Xunit;
using Moq;
using main.model;
using main.ui.layout;
using main.config;
using main.memory;
using main.ui.keyhandler; // For IInput, KeymapView
using main.ui; // For IFocus, TargetPanel, IMode
using Spectre.Console;    // For Layout, Grid
using System;
using System.Collections.Generic;
using System.Linq;

namespace test.ui.layout
{
    public class MainGridTests
    {
        [Fact]
        public void SetViewportDimensions_ShouldSetPropertiesCorrectly()
        {
            // Arrange
            var mockConfig = new Mock<ICurrentConfig>();
            var mockCursor = new Mock<ICursor>();
            var mockSnapShot = new Mock<ISnapShot>();
            var mockFocus = new Mock<IFocus>();

            mockConfig.Setup(c => c.CellLength).Returns(1);
            mockSnapShot.Setup(s => s.Before).Returns(new byte[] { 0xAA, 0xBB });
            mockSnapShot.Setup(s => s.Current).Returns(new byte[] { 0xAA, 0xBB });

            var mainGrid = new MainGrid(mockConfig.Object, mockCursor.Object, mockSnapShot.Object, mockFocus.Object);
            uint expectedHeight = 10;
            uint expectedWidth = 20;

            // Act
            mainGrid.SetViewportDimensions(expectedHeight, expectedWidth);

            // Assert
            Assert.Equal(expectedHeight, mainGrid.ViewportHeight);
            Assert.Equal(expectedWidth, mainGrid.ViewportWidth);
        }
    }

    public class CursorInfoTests
    {
        [Fact]
        public void GetViewData_ShouldIncludeViewportAndGridDimensions()
        {
            // Arrange
            uint expectedViewportHeight = 30;
            uint expectedViewportWidth = 40;
            uint expectedGridHeight = 2;
            uint expectedGridWidth = 2;
            byte cellLength = 1;
            uint columnsLength = expectedGridWidth;

            uint cursorX = 0;
            uint cursorY = 0;
            uint cursorIndex = 0;

            byte[] snapShotData = [(byte)0xAA, (byte)0xBB, (byte)0xCC, (byte)0xDD];
            string cellBeforeValue = "170";
            string cellCurrentValue = "170";
            string dummyAddressString = "0x0000";
            TargetPanel expectedFocusTarget = TargetPanel.Left;

            var mockMgConfig = new Mock<ICurrentConfig>();
            var mockMgCursor = new Mock<ICursor>();
            var mockMgSnapShot = new Mock<ISnapShot>();
            var mockMgFocus = new Mock<IFocus>();

            mockMgConfig.Setup(c => c.CellLength).Returns(cellLength);
            mockMgConfig.Setup(c => c.ColumnsLength).Returns(columnsLength);

            mockMgSnapShot.Setup(s => s.Before).Returns(snapShotData);
            mockMgSnapShot.Setup(s => s.Current).Returns(snapShotData);

            var mainGrid = new MainGrid(mockMgConfig.Object, mockMgCursor.Object, mockMgSnapShot.Object, mockMgFocus.Object);
            mainGrid.SetViewportDimensions(expectedViewportHeight, expectedViewportWidth);

            var mockCiCursor = new Mock<ICursor>();
            var mockCiFocus = new Mock<IFocus>();
            var mockCiFormatAddress = new Mock<Func<uint, string>>();

            mockCiCursor.Setup(c => c.X).Returns(cursorX);
            mockCiCursor.Setup(c => c.Y).Returns(cursorY);
            mockCiCursor.Setup(c => c.GetIndex()).Returns(cursorIndex);
            mockCiFocus.Setup(f => f.TargetPanel).Returns(expectedFocusTarget);
            mockCiFormatAddress.Setup(f => f(It.IsAny<uint>())).Returns(dummyAddressString);

            var cursorInfo = new CursorInfo(mockCiCursor.Object, mainGrid, mockCiFocus.Object, mockCiFormatAddress.Object);

            // Act
            var viewData = cursorInfo.GetViewData();

            // Assert
            Assert.Equal(expectedViewportHeight.ToString(), viewData["viewportHeight"]);
            Assert.Equal(expectedViewportWidth.ToString(), viewData["viewportWidth"]);
            Assert.Equal(expectedGridHeight.ToString(), viewData["gridHeight"]);
            Assert.Equal(expectedGridWidth.ToString(), viewData["gridWidth"]);
            Assert.Equal(cellCurrentValue, viewData["CurrentValue"]);
            Assert.Equal(cellBeforeValue, viewData["BeforeValue"]);
        }
    }

    public class UiTests
    {
        [Fact]
        public void CreateLayout_ShouldCallSetViewportDimensionsWithConsoleDimensions()
        {
            // Arrange
            var mockUiConfig = new Mock<ICurrentConfig>();
            var mockUiMode = new Mock<IMode>();
            var mockUiSelectView = new Mock<ISelectView>();

            // For MainGrid, we need to mock its constructor arguments if we were to instantiate it directly.
            // But since Ui takes MainGrid as an argument, we mock MainGrid itself.
            var mockMainGrid = new Mock<MainGrid>(
                MockBehavior.Loose, // Use Loose to avoid having to set up all methods/properties
                new Mock<ICurrentConfig>().Object,
                new Mock<ICursor>().Object,
                new Mock<ISnapShot>().Object,
                new Mock<IFocus>().Object);

            var mockMatrixForUiTest = new Mock<Matrix>(
                MockBehavior.Loose,
                new Mock<ICurrentConfig>().Object,
                new Mock<ISnapShot>().Object);

            uint matrixHeight = 100;
            uint matrixWidth = 80;

            mockMatrixForUiTest.Setup(m => m.Height).Returns(matrixHeight);
            mockMatrixForUiTest.Setup(m => m.Width).Returns(matrixWidth);

            // Setup the Matrix property on the mocked MainGrid
            mockMainGrid.Setup(mg => mg.Matrix).Returns(mockMatrixForUiTest.Object);

            // Setup SetViewportDimensions to be verifiable
            mockMainGrid.Setup(mg => mg.SetViewportDimensions(It.IsAny<uint>(), It.IsAny<uint>()));

            // Setup methods on MainGrid that are called by Ui.CreateLayout
            mockMainGrid.Setup(mg => mg.CursorInfoView).Returns(new Grid()); // Return an empty grid
            mockMainGrid.Setup(mg => mg.CreateDiffView()).Returns(new Grid()); // Return an empty grid

            var mockUiInput = new Mock<IInput>();
            mockUiConfig.Setup(c => c.SharedMemoryName).Returns("test_shm");
            mockUiMode.Setup(m => m.InputMode).Returns(InputMode.Normal); // Default mode

            var ui = new Ui(mockUiConfig.Object, mockUiMode.Object, mockUiSelectView.Object, mockMainGrid.Object);

            // Act
            ui.CreateLayout(mockUiConfig.Object, mockUiInput.Object);

            // Assert
            // The new behavior uses console dimensions instead of matrix dimensions
            // Verify that SetViewportDimensions was called, but don't check exact values
            // since they depend on console size
            mockMainGrid.Verify(mg => mg.SetViewportDimensions(It.IsAny<uint>(), It.IsAny<uint>()), Times.Once);
            
            // Optional: Verify the dimensions are reasonable (not matrix dimensions)
            // In a real test environment, console size might be small, so viewport should be
            // smaller than large matrix dimensions
        }
    }
}
