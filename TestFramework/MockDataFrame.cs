using linaPl.DataFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using linaPl.DataFrame.Interfaces;
using static linaPl.DataFrame.DataFrame;

namespace TestFramework
{
    public class MockDataFrame : IDataFrame
    {
        Dictionary<CellKey, object> _dataTable;
        public int RowBound { get; private set; } = 0;
        public int ColumnBound { get; private set; } = 0;
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
                    var selectedColumnKeys = _dataTable.Keys
                        .Where(i => i.Column == column)
                        .ToArray();
                    
                    _dataTable[index] = value;
                }
                else
                {
                    throw new IndexOutOfRangeException();
                }
            }
        }

        


        Type IDataFrame.DefineColumnType(Dictionary<DataFrame.CellKey, object> dataTable, int column)
        {
            throw new NotImplementedException();
        }

        StringBuilder IDataFrame.PrintAsTable()
        {
            throw new NotImplementedException();
        }

        void IDataFrame.ChangeType(Dictionary<DataFrame.CellKey, object> dataTable, int indexColumn, Type type)
        {
            throw new NotImplementedException();
        }

        void IDataFrame.AddRow()
        {
            throw new NotImplementedException();
        }

        void IDataFrame.AddColumn()
        {
            throw new NotImplementedException();
        }
    }
    
}
