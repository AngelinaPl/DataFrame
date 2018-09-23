using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using linaPl.DataFrame.Interfaces;

namespace linaPl.DataFrame.DataFrame
{
    public partial class DataFrame : IDisposable, IDataFrame
    {
        private int _rowBound;
        private int _columnBound;

        public struct CellKey
        {
            // ReSharper disable once MemberHidesStaticFromOuterClass
            public int Row;
            // ReSharper disable once MemberHidesStaticFromOuterClass
            public int Column;
        }

        private Dictionary<CellKey, object> _dataTable;

        public int ColumnBound => _columnBound;

        public int RowBound => _rowBound;

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
            using (var dataFrame = new DataFrame(_rowBound, _columnBound))
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
            _dataTable = new Dictionary<CellKey, object>();
            _rowBound = arr.Length;
            _columnBound = 0;
            for (int i = 0; i < _rowBound; i++)
            {
                _columnBound = arr[i].Length > _columnBound ? arr[i].Length : _columnBound;
            }
            for (int i = 0; i < _rowBound; i++)
            {
                for (int j = 0; j < arr[i].Length; j++)
                {
                    CellKey index = new CellKey()
                    {
                        Row = i,
                        Column = j
                    };
                    _dataTable.Add(index, arr[i][j]);
                    var type = DefineColumnType(_dataTable, j);
                    ChangeType(_dataTable, j, type);
                }
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

            using (var dataFrame = new DataFrame(_rowBound, _columnBound))
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

        public DataFrame(string csvPath)
        {
            var fInfo = new FileInfo(csvPath);
            if (!fInfo.Exists)
            {
                throw new FileNotFoundException();
            }

            var data = File.ReadAllLines(csvPath);
            _rowBound = data.Length;

            var dataStr = new object[_rowBound][];
            for (int i = 0; i < _rowBound; i++)
            {
                // ReSharper disable once CoVariantArrayConversion
                dataStr[i] = data[i].Split(',');
                if (i == 0)
                {
                    _columnBound = dataStr[i].Length;
                }
                _columnBound = dataStr[i].Length > _columnBound ? dataStr[i].Length : _columnBound;
            }

            var rawStrTable = data
                .Select(str => str.Split(','))
                .ToArray();

            int length = rawStrTable[0].Length;
            if (rawStrTable.Any(arr => arr.Length != length))
                throw new Exception("Bad CSV file!"); // TODO Exeption and message

            using (var dataFrame = new DataFrame(_rowBound, _columnBound))
            {
                for (int i = 0; i < _rowBound; i++)
                {
                    for (int j = 0; j < rawStrTable[i].Length; j++)
                    {
                        if (rawStrTable[i][j] != null)
                        {
                            dataFrame[i, j] = rawStrTable[i][j];
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

        private static readonly Dictionary<Type, int> TypesDictionary = new Dictionary<Type, int>()
        {
            { typeof(Int32), 0},
            { typeof(Double), 1},
            { typeof(String), 2}
        };
        public Type DefineColumnType(Dictionary<CellKey, object> dataTable, int column)
        {
            var putColumnKeys = dataTable.Keys
                .Where(i => i.Column == column).ToArray();

            Type typeRet = null;

            if (putColumnKeys.Length >= 0)
            {
                int keyOfType = -1;

                foreach (var index in putColumnKeys)
                {
                    if (dataTable.TryGetValue(index, out var valueTableElement))
                    {
                        if (TypesDictionary.TryGetValue(valueTableElement.GetType(), out var numberType))
                        {
                            if (keyOfType < numberType)
                            {
                                keyOfType = numberType;
                                typeRet = valueTableElement.GetType();
                            }
                        }
                    }
                }
            }
            return typeRet;
        }

        public StringBuilder PrintAsTable()
        {
            StringBuilder stringBuilder = new StringBuilder("");
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

                    stringBuilder.Append($"{value}\t");
                }

                stringBuilder.Append("\n");
            }
            return stringBuilder;
        }

        public void Dispose()
        {
        }

        private Dictionary<Type, Action<Dictionary<CellKey, object>, int>> _typeDictionary;
        public void ChangeType(Dictionary<CellKey, object> dataTable, int indexColumn, Type type)
        {
            _typeDictionary = new Dictionary<Type, Action<Dictionary<CellKey, object>, int>>()
            {
                { typeof(String), delegate { ChangeColumnOnString(dataTable, indexColumn);}},
                { typeof(Int32), delegate { ChangeColumnOnInt(dataTable, indexColumn);}},
                { typeof(Double), delegate { ChangeColumnOnDouble(dataTable, indexColumn);}}
            };
            if (_typeDictionary.TryGetValue(type, out var methodChange))
            {
                methodChange(dataTable, indexColumn);
            }

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
                    var index = new CellKey
                    {
                        Row = row,
                        Column = column
                    };
                    //var selectedColumnKeys = _dataTable.Keys
                    //    .Where(i => i.Column == column)
                    //    .ToArray();

                    _dataTable[index] = value;
                    var type = DefineColumnType(_dataTable, column);
                    ChangeType(_dataTable, column, type);
                }
                else
                {
                    throw new IndexOutOfRangeException();
                }
            }
        }

        private static void ChangeColumnOnString(Dictionary<CellKey, object> dataTable, int column)
        {
            var selectedColumnKeys = dataTable.Keys
                .Where(i => i.Column == column)
                .ToArray();
            foreach (var index in selectedColumnKeys)
            {
                dataTable[index] = dataTable[index].ToString();
            }
        }

        private static void ChangeColumnOnInt(Dictionary<CellKey, object> dataTable, int column)
        {
            var selectedColumnKeys = dataTable.Keys
                .Where(i => i.Column == column)
                .ToArray();
            foreach (var index in selectedColumnKeys)
            {
                dataTable[index] = Convert.ToInt32(dataTable[index]);
            }
        }

        private static void ChangeColumnOnDouble(Dictionary<CellKey, object> dataTable, int column)
        {
            var selectedColumnKeys = dataTable.Keys
                .Where(i => i.Column == column)
                .ToArray();
            foreach (var index in selectedColumnKeys)
            {
                dataTable[index] = Convert.ToDouble(dataTable[index]);
            }
        }

        public Row this[int rowIndex]
        {
            get
            {
                var row = new RowsCollection(_dataTable);
                return row[rowIndex];
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

        public bool DeleteRow(int row)
        {
            if (row < _rowBound && row >= 0)
            {
                CellKey indexNew;
                CellKey indexOld;
                for (int  i = row; i < _rowBound; i++)
                {
                    for (int j = 0; j < _columnBound; j++)
                    {
                        indexOld = new CellKey()
                        {
                            Row = i + 1,
                            Column = j
                        };
                        if (i < _rowBound - 1)
                        {
                            indexNew = new CellKey()
                            {
                                Row = i,
                                Column = j
                            };
                            _dataTable[indexNew] = _dataTable[indexOld];
                        }
                        else if (i == _rowBound - 1)
                        {
                            _dataTable[indexOld] = null;
                        }
                    }
                }
                _rowBound -= 1;
                return true;
            }
            return false;
        }

        public bool DeleteColumn(int column)
        {
            if (column < _columnBound && column >= 0)
            {
                CellKey indexNew;
                CellKey indexOld;
                for (int j = column; j < _columnBound - 1; j++)
                {
                    for (int i = 0; i < _rowBound; i++)
                    {
                        indexOld = new CellKey()
                        {
                            Row = i,
                            Column = j + 1
                        };
                        if (i < _columnBound - 1)
                        {
                            indexNew = new CellKey()
                            {
                                Row = i,
                                Column = j
                            };
                            _dataTable[indexNew] = _dataTable[indexOld];
                        }
                        else if (i == _columnBound - 1)
                        {
                            _dataTable[indexOld] = null;
                        }
                    }
                }
                _columnBound -= 1;
                return true;
            }
            return false;
        }
    }
}
