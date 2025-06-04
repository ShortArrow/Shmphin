using Xunit;
using Moq;
using main.model;
using main.ui.layout;
using main.config;
using main.memory;
using main.ui.keyhandler; // For IInput, KeymapView
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
            mockSnapShot.Setup(s => s.MemoryBlocks).Returns(new List<IMemoryBlock>());

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
            uint bytesPerRow = expectedGridWidth * cellLength;

            uint cursorX = 0;
            uint cursorY = 0;
            uint cursorIndex = 0;

            byte[] snapShotData = [(byte)0xAA, (byte)0xBB, (byte)0xCC, (byte)0xDD];
            string cellBeforeValue = "AA";
            string cellCurrentValue = "AA";
            string dummyAddressString = "0x0000";
            FocusTarget expectedFocusTarget = FocusTarget.MainGrid;

            var mockMgConfig = new Mock<ICurrentConfig>();
            var mockMgCursor = new Mock<ICursor>();
            var mockMgSnapShot = new Mock<ISnapShot>();
            var mockMgFocus = new Mock<IFocus>();

            mockMgConfig.Setup(c => c.CellLength).Returns(cellLength);
            mockMgConfig.Setup(c => c.BytesPerRow).Returns(bytesPerRow);

            var mockMemoryBlock = new Mock<IMemoryBlock>();
            mockMemoryBlock.Setup(mb => mb.Data).Returns(snapShotData);
            mockMemoryBlock.Setup(mb => mb.BaseAddress).Returns(0);
            mockMemoryBlock.Setup(mb => mb.Length).Returns((uint)snapShotData.Length);

            mockMgSnapShot.Setup(s => s.MemoryBlocks).Returns(new List<IMemoryBlock> { mockMemoryBlock.Object });
            mockMgSnapShot.Setup(s => s.Length).Returns((uint)snapShotData.Length);
            mockMgSnapShot.Setup(s => s.ReadBytes(It.Is<long>(addr => addr == (cursorY * bytesPerRow + cursorX) * cellLength), cellLength))
                          .Returns(new[] { snapShotData[(cursorY * bytesPerRow + cursorX) * cellLength] });

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
        public void CreateLayout_ShouldCallSetViewportDimensionsWithFullMatrixDimensions_AsFallback()
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

            uint expectedMatrixHeight = 100;
            uint expectedMatrixWidth = 80;

            mockMatrixForUiTest.Setup(m => m.Height).Returns(expectedMatrixHeight);
            mockMatrixForUiTest.Setup(m => m.Width).Returns(expectedMatrixWidth);

            // Setup the Matrix property on the mocked MainGrid
            // This requires MainGrid.Matrix to be virtual to be effectively mocked by Moq.
            // If it's not virtual, this setup might not work as expected for a concrete class mock.
            // Let's assume for this test that it can be set up or MainGrid is an interface IMainGrid.
            mockMainGrid.Setup(mg => mg.Matrix).Returns(mockMatrixForUiTest.Object);

            // Setup SetViewportDimensions to be verifiable. This also ideally needs to be virtual.
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
            // Verify that SetViewportDimensions was called on the MainGrid with the matrix's full height and width.
            // This relies on SetViewportDimensions being verifiable (e.g., virtual if MainGrid is a class).
            mockMainGrid.Verify(mg => mg.SetViewportDimensions(expectedMatrixHeight, expectedMatrixWidth), Times.Once);
        }
    }
}
