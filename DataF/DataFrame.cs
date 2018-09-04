using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DataF
{
    [JsonObject(MemberSerialization.OptIn)]
    public partial class DataFrame : IDisposable
    {
        [JsonProperty]
        private int _rowBound;
        [JsonProperty]
        private int _columnBound;

        public struct CellKey
        {
            public int Row;
            public int Column;
        }
        [JsonProperty]
        private Dictionary<CellKey, object> _dataTable;

        public int ColumnBound
        {
            get => _columnBound;
        }

        public int RowBound
        {
            get => _rowBound;
        }

        public DataFrame(int row, int column)
        {
            _dataTable = new Dictionary<CellKey, object>();
            _rowBound = row;
            _columnBound = column;
        }

        public DataFrame(Dictionary<CellKey, object> dataTable)
        {
            _dataTable = dataTable;
            var cellKeys = dataTable.Keys;

            foreach (var s in cellKeys)
            {
                foreach (var ss in cellKeys)
                {
                    if (s.Column == ss.Column)
                    {
                        _rowBound += 1;
                    }
                }
                break;
            }
            _columnBound = cellKeys.Count / _rowBound;
        }


        public DataFrame(object[,] arr)
        {
            _rowBound = arr.GetUpperBound(0) + 1;
            _columnBound = arr.Length / _rowBound;
            using (DataFrame dataFrame = new DataFrame(_rowBound, _columnBound))
            {
                for (int i = 0; i < _rowBound; i++)
                {
                    for (int j = 0; j < _columnBound; j++)
                    {
                        dataFrame[i, j] = arr[i, j];
                    }
                }
                _dataTable = dataFrame._dataTable;
            }
        }

        public DataFrame(object[][] arr)
        {
            _rowBound = arr.Length;
            _columnBound = 0;
            for (int i = 0; i < _rowBound; i++)
            {
                _columnBound = arr[i].Length > _columnBound ? arr[i].Length : _columnBound;
            }
            using (DataFrame dataFrame = new DataFrame(_rowBound, _columnBound))
            {
                for (int i = 0; i < _rowBound; i++)
                {
                    for (int j = 0; j < arr[i].Length; j++)
                    {
                        dataFrame[i, j] = arr[i][j];
                    }
                }
                _dataTable = dataFrame._dataTable;
            }
        }

        public DataFrame(Dictionary<int, IEnumerable<int>> dictionary)
        {
            _rowBound = dictionary.Count;
            for (int i = 0; i < _rowBound; i++)
            {
                if (i == 0)
                {
                    _columnBound = dictionary[i].Count();
                }
                else
                {
                    _columnBound = _columnBound > dictionary[i].Count() ? _columnBound : dictionary[i].Count();
                }
            }

            using (DataFrame dataFrame = new DataFrame(_rowBound, _columnBound))
            {
                int i = 0;
                foreach (var s in dictionary.Keys)
                {
                    if (dictionary.TryGetValue(s, out var rowElements))
                    {
                        int j = 0;
                        Console.WriteLine();
                        foreach (var r in rowElements)
                        {
                            dataFrame[i, j] = r;
                            j++;
                        }
                        i++;
                    }
                }
                _dataTable = dataFrame._dataTable;
            }
        }

        public DataFrame(string pathcsv)
        {
            FileInfo fInfo = new FileInfo(pathcsv);
            if (!fInfo.Exists)
            {
                throw new FileNotFoundException();
            }

            string[] data = File.ReadAllLines(pathcsv);
            _rowBound = data.Length;
            object[][] dataStr = new object[_rowBound][];
            for (int i = 0; i < _rowBound; i++)
            {
                dataStr[i] = data[i].Split(',');
                if (i == 0)
                {
                    _columnBound = dataStr[i].Length;
                }
                _columnBound = dataStr[i].Length > _columnBound ? dataStr[i].Length : _columnBound;
            }

            using (DataFrame dataFrame = new DataFrame(_rowBound, _columnBound))
            {
                for (int i = 0; i < _rowBound; i++)
                {
                    for (int j = 0; j < dataStr[i].Length; j++)
                    {
                        if (dataStr[i][j] != null)
                        {
                            dataFrame[i, j] = dataStr[i][j];
                        }
                        else
                        {
                            dataFrame[i, j] = null;
                        }
                    }
                }
                _dataTable = dataFrame._dataTable;
            }
        }

        public StringBuilder ShowAsTable()
        {
            StringBuilder _stringBuilder = new StringBuilder("");
            for (int i = 0; i < _rowBound; i++)
            {
                for (int j = 0; j < _columnBound; j++)
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

                    _stringBuilder.Append($"{value}\t");
                }

                _stringBuilder.Append("\n");
            }
            return _stringBuilder;
        }

        public void Dispose()
        {
        }

        public object this[int row, int column]
        {
            get
            {
                if (row < _rowBound && row >= 0
                    && column < _columnBound && column >= 0)
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
                if (row < _rowBound && column < _columnBound)
                {
                    CellKey index = new CellKey
                    {
                        Row = row,
                        Column = column
                    };
                    var selectedColumn = _dataTable.Keys.Where(i => i.Column == column);

                    int counter = 0;
                    CellKey firstIn = index;
                    if (selectedColumn.Count() != 0)
                    {
                        foreach (var s in selectedColumn)
                        {
                            if (counter == 0)
                            {
                                firstIn = s;
                                counter++;
                            }
                            else break;
                        }
                        //
                        if (_dataTable[firstIn].GetType().Name == "String" 
                            && value.GetType().Name == "String")
                        {
                            string nameTypeFirstElement = "String";
                            if (Int32.TryParse(value.ToString(), out var tryGetIntValue1))
                            {
                                if (Int32.TryParse(_dataTable[firstIn].ToString(),
                                    out var tryGetIntValue2))
                                {
                                    _dataTable[index] = tryGetIntValue1;
                                }
                            }
                            else
                            {
                                if (!Int32.TryParse(_dataTable[firstIn].ToString(), 
                                    out var tryGetIntValue3))
                                {
                                    _dataTable[index] = value;
                                }
                            }
                        } // Необходимо ли оставить?
                        else
                        {
                            _dataTable[index] = (_dataTable[firstIn].GetType().Name == value.GetType().Name) ?
                                value : null;
                        }
                    }
                    else
                    {
                        _dataTable[index] = value;
                    }
                }
                else
                {
                    throw new IndexOutOfRangeException();
                }
            }
        }

        public Row this[int rowIndex]
        {
            get
            {
                var _row = new RowsCollection(_dataTable);
                return _row[rowIndex];
            }
        }

        public int ColumnCount
        {
            get => _columnBound;
        }

        public int RowCount
        {
            get => _rowBound;
        }

        public IReadOnlyList<Column> Columns
        {
            get
            {
                List<Column> columns = new List<Column>();
                ColumnsCollection columnsColl = new ColumnsCollection(_dataTable);
                foreach (var r in columnsColl)
                {
                    columns.Add(r.Value);
                }
                return columns;
            }
        }

        public IReadOnlyList<Row> Rows
        {
            get
            {
                List<Row> rows = new List<Row>();
                RowsCollection rowsCol = new RowsCollection(_dataTable);
                foreach (var r in rowsCol)
                {
                    rows.Add(r.Value);
                }
                return rows;
            }
        }

        public void AddRow()
        {
            _rowBound += 1;
        }

        public void AddColumn()
        {
            _columnBound += 1;
        }

        public bool AddDataRow(DataFrame dataFrame, object[] dataArr)
        {
            if (dataFrame.ColumnBound == dataArr.Length)
            {
                dataFrame.AddRow();
                int i = dataFrame.RowBound - 1;
                for (int j = 0; j < dataFrame.ColumnBound; j++)
                {
                    dataFrame[i, j] = dataArr[j];
                }

                return true;
            }
            return false;
        }

        public bool AddDataColumn(DataFrame dataFrame, object[] dataArr)
        {
            if (dataFrame.RowBound == dataArr.Length)
            {
                dataFrame.AddColumn();
                int j = dataFrame.ColumnBound - 1;
                for (int i = 0; i < dataFrame.RowBound; i++)
                {
                    dataFrame[i, j] = dataArr[i];
                }

                return true;
            }
            return false;
        }
    }
}
