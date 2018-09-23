using NUnit.Framework;
using System.Collections.Generic;
using TestFramework;
using static linaPl.DataFrame.DataFrame.DataFrame;

namespace UnitTest
{
    [TestFixture]
    class DataFrameTests
    {
        [SetUp]
        public void SetUp()
        {

        }

        [Test]
        public void Test0000_ConstructorWithDataTable()
        {
            // Arrange
            Dictionary<CellKey, object> dataTable = new Dictionary<CellKey, object>()
            {
                {new CellKey() {Row = 0, Column = 0}, 0 },
                {new CellKey() {Row = 0, Column = 1}, 0 },
                {new CellKey() {Row = 1, Column = 0}, 0 },
                {new CellKey() {Row = 1, Column = 1}, 0 },
                {new CellKey() {Row = 2, Column = 0}, 0 },
                {new CellKey() {Row = 2, Column = 1}, 0 },
                {new CellKey() {Row = 3, Column = 0}, 0 },
                {new CellKey() {Row = 3, Column = 1}, 0 },
            };

            // Act
            MockDataFrame dataFrame = new MockDataFrame(dataTable);

            // Assert
            Assert.IsNotNull(dataFrame);
        }

        [Test]
        public void Test0001_ConstructorWithArray()
        {
            // Arrange
            object[,] array = new object[2, 4];
            array[0, 0] = 13;
            array[1, 0] = 14.432;

            array[0, 1] = "str";
            array[1, 1] = 432;

            array[0, 2] = 12.1;
            array[1, 2] = 56;

            array[0, 3] = 12.1;
            array[1, 3] = 12.3;

            // Act
            MockDataFrame dataFrame = new MockDataFrame(array);
            int row = array.GetUpperBound(0);
            int column = array.Length / row;

            // Assert
            Assert.IsNotNull(dataFrame);
            Assert.AreEqual(dataFrame.RowBound, row);
            Assert.AreEqual(dataFrame.ColumnBound, column);
        }

        [Test]
        public void Test0002_ConstructorWithJaggedArray()
        {
            // Arrange
            object[][] array = new object[4][];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = new object[i + 1];
            }

            array[0][0] = 00;

            array[1][0] = 10;
            array[1][1] = "str";

            array[2][0] = 20;
            array[2][1] = "str";
            array[2][2] = 22;

            array[3][0] = 30;
            array[3][1] = "str";
            array[3][2] = 32.4;
            array[3][3] = 33;

            // Act
            MockDataFrame dataFrame = new MockDataFrame(array);

            // Assert
            Assert.IsNotNull(dataFrame);
            for (int i = 0; i < dataFrame.RowBound; i++)
            {
                for (int j = 0; j < array[i].Length; j++)
                {
                    Assert.AreEqual(dataFrame[i, j], array[i][j]);
                }
            }
        }

        [Test]
        public void Test0003_AddRow()
        {
            // Arrange
            object[,] array = new object[2, 4];
            MockDataFrame dataFrame = new MockDataFrame(array);

            // Act
            dataFrame.AddRow();

            // Assert
            Assert.AreEqual(array.GetUpperBound(0) + 1, dataFrame.RowBound);
        }

        [Test]
        public void Test0004_AddColumn()
        {
            // Arrange
            object[,] array = new object[2, 4];
            MockDataFrame dataFrame = new MockDataFrame(array);

            // Act
            dataFrame.AddColumn();

            // Assert
            Assert.AreEqual((array.Length / array.GetUpperBound(0) + 1), dataFrame.ColumnBound);
        }

        [Test]
        public void Test0005_PrintAsTable()
        {
            // Arrange
            object[,] array = new object[2, 4];
            MockDataFrame dataFrame = new MockDataFrame(array);

            // Act
            var table = dataFrame.PrintAsTable();

            // Assert
            Assert.IsNotNull(table);
        }

        [Test]
        public void Test0006_DeleteColumn()
        {
            // Arrange
            object[,] array = new object[2, 4];
            MockDataFrame dataFrame = new MockDataFrame(array);

            // Act
            dataFrame.DeleteColumn(0);

            // Assert
            Assert.AreEqual((array.Length / array.GetUpperBound(0) - 1), dataFrame.ColumnBound);
        }

        [Test]
        public void Test0007_DeleteRow()
        {
            // Arrange
            object[,] array = new object[2, 4];
            MockDataFrame dataFrame = new MockDataFrame(array);

            // Act
            dataFrame.DeleteRow(0);

            // Assert
            Assert.AreEqual((array.GetUpperBound(0) - 1), dataFrame.RowBound);
        }

        [TearDown]
        public void TearDown()
        {
        }
    }
}
