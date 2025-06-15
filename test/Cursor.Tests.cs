using Xunit;
using Moq;
using main.model;
using main.config;

namespace test.model
{
    public class CursorTests
    {
        private Mock<ICurrentConfig> CreateMockConfig(uint columnsLength = 3, uint cellLength = 1, uint sharedMemorySize = 12)
        {
            var mockConfig = new Mock<ICurrentConfig>();
            mockConfig.Setup(c => c.ColumnsLength).Returns(columnsLength);
            mockConfig.Setup(c => c.CellLength).Returns(cellLength);
            mockConfig.Setup(c => c.SharedMemorySize).Returns(sharedMemorySize);
            return mockConfig;
        }

        [Fact]
        public void MoveRight_ShouldIncrementX_WhenNotAtRightBoundary()
        {
            var mockConfig = CreateMockConfig();
            var cursor = new Cursor(mockConfig.Object);
            cursor.X = 1;
            cursor.Y = 0;

            cursor.MoveRight();

            Assert.Equal(2u, cursor.X);
            Assert.Equal(0u, cursor.Y);
        }

        [Fact]
        public void MoveRight_ShouldWrapToNextRow_WhenAtRightBoundary()
        {
            var mockConfig = CreateMockConfig();
            var cursor = new Cursor(mockConfig.Object);
            cursor.X = 2; // At right boundary (columnsLength - 1)
            cursor.Y = 0;

            cursor.MoveRight();

            Assert.Equal(0u, cursor.X);
            Assert.Equal(1u, cursor.Y);
        }

        [Fact]
        public void MoveLeft_ShouldDecrementX_WhenNotAtLeftBoundary()
        {
            var mockConfig = CreateMockConfig();
            var cursor = new Cursor(mockConfig.Object);
            cursor.X = 2;
            cursor.Y = 0;

            cursor.MoveLeft();

            Assert.Equal(1u, cursor.X);
            Assert.Equal(0u, cursor.Y);
        }

        [Fact]
        public void MoveLeft_ShouldStayAtZero_WhenAtLeftBoundary()
        {
            var mockConfig = CreateMockConfig();
            var cursor = new Cursor(mockConfig.Object);
            cursor.X = 0;
            cursor.Y = 1;

            cursor.MoveLeft();

            Assert.Equal(0u, cursor.X);
            Assert.Equal(1u, cursor.Y);
        }

        [Fact]
        public void MoveDown_ShouldIncrementY_WhenNotAtBottomBoundary()
        {
            var mockConfig = CreateMockConfig();
            var cursor = new Cursor(mockConfig.Object);
            cursor.X = 1;
            cursor.Y = 1;

            cursor.MoveDown();

            Assert.Equal(1u, cursor.X);
            Assert.Equal(2u, cursor.Y);
        }

        [Fact]
        public void MoveDown_ShouldWrapToTopAndMoveRight_WhenAtBottomBoundary()
        {
            var mockConfig = CreateMockConfig();
            var cursor = new Cursor(mockConfig.Object);
            cursor.X = 1;
            cursor.Y = 3; // At bottom boundary (height - 1)

            cursor.MoveDown();

            Assert.Equal(2u, cursor.X);
            Assert.Equal(0u, cursor.Y);
        }

        [Fact]
        public void MoveUp_ShouldDecrementY_WhenNotAtTopBoundary()
        {
            var mockConfig = CreateMockConfig();
            var cursor = new Cursor(mockConfig.Object);
            cursor.X = 1;
            cursor.Y = 2;

            cursor.MoveUp();

            Assert.Equal(1u, cursor.X);
            Assert.Equal(1u, cursor.Y);
        }

        [Fact]
        public void MoveUp_ShouldStayAtZero_WhenAtTopBoundary()
        {
            var mockConfig = CreateMockConfig();
            var cursor = new Cursor(mockConfig.Object);
            cursor.X = 1;
            cursor.Y = 0;

            cursor.MoveUp();

            Assert.Equal(1u, cursor.X);
            Assert.Equal(0u, cursor.Y);
        }

        [Fact]
        public void GetIndex_ShouldCalculateCorrectIndex()
        {
            var mockConfig = CreateMockConfig();
            var cursor = new Cursor(mockConfig.Object);
            cursor.X = 2;
            cursor.Y = 1;

            var index = cursor.GetIndex();

            Assert.Equal(5u, index); // 1 * 3 + 2 = 5
        }

        [Fact]
        public void GetIndex_ShouldReturnZero_WhenAtOrigin()
        {
            var mockConfig = CreateMockConfig();
            var cursor = new Cursor(mockConfig.Object);
            cursor.X = 0;
            cursor.Y = 0;

            var index = cursor.GetIndex();

            Assert.Equal(0u, index);
        }
    }
}