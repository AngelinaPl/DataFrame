using System;
using System.Collections.Generic;
using System.Text;
using linaPl.DataFrame.Interfaces;
using static linaPl.DataFrame.DataFrame.DataFrame;

namespace TestFramework
{
    public class MockDataFrame : IDataFrame
    {
        Dictionary<CellKey, object> _dataTable;
        public int RowBound { get; private set; }
        public int ColumnBound { get; private set; }
        public MockDataFrame(Dictionary<CellKey, object> dataTable)
        {
            _dataTable = dataTable;
            _dataTable = dataTable;
            var cellKeys = dataTable.Keys;

            foreach (var s in cellKeys)
            {
                foreach (var ss in cellKeys)
                {
                    if (s.Column == ss.Column)
                    {
                        RowBound += 1;
                    }
                }
                break;
            }
            ColumnBound = cellKeys.Count / RowBound;
        }

        public MockDataFrame(object[,] arr)
        {
            _dataTable = new Dictionary<CellKey, object>();
            RowBound = arr.GetUpperBound(0);
            ColumnBound = arr.Length / RowBound;
        }

        public MockDataFrame(object[][] arr)
        {
            RowBound = arr.Length;
            ColumnBound = 0;
            _dataTable = new Dictionary<CellKey, object>();
            for (int i = 0; i < RowBound; i++)
            {
                ColumnBound = arr[i].Length > ColumnBound ? arr[i].Length : ColumnBound;
            }

            for (int i = 0; i < RowBound; i++)
            {
                for (int j = 0; j < arr[i].Length; j++)
                {
                    CellKey index = new CellKey()
                    {
                        Row = i,
                        Column = j
                    };
                    _dataTable.Add(index,arr[i][j]);
                }
            }
        }

        public object this[int row, int column]
        {
            get
            {
                if (row < RowBound && row >= 0
                    && column < ColumnBound && column >= 0)
                {
                    var index = new CellKey
                    {
                        Row = row,
                        Column = column
                    };

                    _dataTable.TryGetValue(index, out var value);
                    return value;
                }
                throw new IndexOutOfRangeException();
            }
            set
            {
                if (row < RowBound && column < ColumnBound)
                {
                    var index = new CellKey
                    {
                        Row = row,
                        Column = column
                    };
                    //var selectedColumnKeys = _dataTable.Keys
                    //    .Where(i => i.Column == column)
                    //    .ToArray();
                    
                    _dataTable[index] = value;
                }
                else
                {
                    throw new IndexOutOfRangeException();
                }
            }
        }
        

        public StringBuilder PrintAsTable()
        {
            StringBuilder stringBuilder = new StringBuilder("");
            for (int i = 0; i < RowBound; i++)
            {
                for (int j = 0; j < ColumnBound; j++)
                {
                    object value;
                    CellKey index = new CellKey()
                    {
                        Row = i,
                        Column = j
                    };
                    if (_dataTable.TryGetValue(index, out value))
                    {
                    }

                    stringBuilder.Append($"{value}\t");
                }

                stringBuilder.Append("\n");
            }
            return stringBuilder;
        }
        
        public void AddRow()
        {
            RowBound += 1;
        }

        public void AddColumn()
        {
            ColumnBound += 1;
        }

        public bool DeleteColumn(int column)
        {
            if (column >= 0 && column < ColumnBound)
            {
                ColumnBound -= 1;
                return true;
            }

            return false;
        }

        public bool DeleteRow(int row)
        {
            if (row >= 0 && row < RowBound)
            {
                RowBound -= 1;
                return true;
            }

            return false;
        }
    }
    
}
