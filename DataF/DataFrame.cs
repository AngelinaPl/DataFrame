using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DataF
{
    public class DataFrame : IDisposable
    {
        private int _rowBound;
        private int _columnBound;

        public struct CellKey
        {
            public int Row;
            public int Column;
        }
        private Dictionary<CellKey, object> DataTable;

        public DataFrame(int row, int column)
        {
            DataTable = new Dictionary<CellKey, object>();
            _rowBound = row;
            _columnBound = column;
        }

        public DataFrame(Dictionary<CellKey, object> dataTable)
        {
            DataTable = dataTable;
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
                DataTable = dataFrame.DataTable;
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
                DataTable = dataFrame.DataTable;
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
                        int lenght = rowElements.Count();
                        Console.WriteLine();
                        foreach (var r in rowElements)
                        {
                            dataFrame[i, j] = r;
                            j++;
                        }
                        i++;
                    }
                }
                DataTable = dataFrame.DataTable;
            }
        }

        public DataFrame (string pathcsv)
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
                    for (int j = 0; j < dataStr[i].Length ; j++)
                    {
                        if (dataStr[i][j] != null)
                        { dataFrame[i, j] = dataStr[i][j]; }
                        else
                        {
                            dataFrame[i, j] = null;
                        }
                    }
                }
                DataTable = dataFrame.DataTable;
            }
        }

        public void Show()
        {
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
                    if (DataTable.TryGetValue(index, out value))
                    {
                        value = value;
                    }
                    else
                    {
                        value = null;
                    }
                    Console.Write(value + "\t");
                }
                Console.WriteLine();
            }
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

                    DataTable.TryGetValue(index, out var value);
                    return value;
                }
                else
                {
                    throw new IndexOutOfRangeException();
                }
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
                    var selectedColumn = from d in DataTable.Keys
                                         where d.Column == column
                                         select d;
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

                        DataTable[index] = (DataTable[firstIn].GetType().Name == value.GetType().Name) ?
                            value : null;
                    }
                    else
                    {
                        DataTable[index] = value;
                    }
                }
                else
                {
                    throw new IndexOutOfRangeException();
                }

            }
        }

        IReadOnlyList<object> _ListReturn;
        public object this[int index]
        {
            get
            {
                return _ListReturn[index];
            }

        }

        public int ColumnCount
        {
            get
            {
                return _columnBound;
            }
        }

        public int RowCount
        {
            get
            {
                return _rowBound;
            }
        }

        public class Row : IReadOnlyList<object>
        {
            private readonly int _rowIndex;
            private readonly DataFrame _dataFrame;

            public Row(int rowIndex, DataFrame dataFrame)
            {
                _rowIndex = rowIndex;
                _dataFrame = dataFrame;
            }

            public Row(int rowIndex, Dictionary<CellKey, object> dataTable)
            {
                _rowIndex = rowIndex;
                _dataFrame = new DataFrame(dataTable);
            }

            public object this[int index]
            {
                get
                {
                    return _dataFrame[_rowIndex, index];
                }
            }

            public int Count
            {
                get
                {
                    return _dataFrame._columnBound;
                }
            }

            public struct RowEnumerator : IEnumerator<object>
            {
                private int _columnIndex;
                private DataFrame _dataFrame;
                private int _rowIndex;

                public RowEnumerator(int rowIndex, DataFrame dataFrame)
                {
                    _columnIndex = -1;
                    _dataFrame = dataFrame;
                    _rowIndex = rowIndex;
                }

                public object Current
                {
                    get
                    {
                        return _dataFrame[_rowIndex, _columnIndex];
                    }
                }

                public void Dispose()
                {

                }

                public bool MoveNext()
                {
                    if (_columnIndex < _dataFrame._columnBound)
                    {
                        _columnIndex += 1;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                public void Reset()
                {
                    _columnIndex = -1;
                }
            }

            public IEnumerator<object> GetEnumerator()
            {
                return new RowEnumerator(_rowIndex, _dataFrame);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return new RowEnumerator(_rowIndex, _dataFrame);
            }

        }

        public class RowsCollection : IReadOnlyDictionary<int, Row>
        {
            private DataFrame _dataFrame;

            public RowsCollection(DataFrame dataFrame)
            {
                _dataFrame = dataFrame;
            }

            public RowsCollection(Dictionary<CellKey, object> dataTable)
            {
                _dataFrame = new DataFrame(dataTable);
            }

            public Row this[int key]
            {
                get
                {
                    return new Row(key, _dataFrame);
                }
            }

            private List<int> _keys = new List<int>();
            public IEnumerable<int> Keys
            {
                get
                {
                    for (int i = 0; i < _dataFrame._rowBound; i++)
                    {
                        _keys.Add(i);
                    }
                    return _keys;
                }
            }
            //Enumarator

            private List<Row> _values = new List<Row>();
            public IEnumerable<Row> Values
            {
                get
                {
                    for (int i = 0; i < _dataFrame._rowBound; i++)
                    {
                        _values.Add(new Row(i, _dataFrame));
                    }
                    return _values;
                }
            }

            public int Count
            {
                get
                {
                    return _dataFrame._rowBound;
                }
            }

            public bool ContainsKey(int key)
            {
                if (key >= 0 && key < _dataFrame._rowBound)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public struct RowsCollectionEnumerator : IEnumerator<KeyValuePair<int, Row>>
            {
                private int _rowIndex;
                private DataFrame _dataFrame;

                public RowsCollectionEnumerator(DataFrame dataFrame)
                {
                    _rowIndex = -1;
                    _dataFrame = dataFrame;
                }

                object IEnumerator.Current
                {
                    get
                    {
                        Row _row = new Row(_rowIndex, _dataFrame);
                        return _row[_rowIndex];
                    }
                }

                KeyValuePair<int, Row> IEnumerator<KeyValuePair<int, Row>>.Current
                {
                    get
                    {
                        KeyValuePair<int, Row> row = new KeyValuePair<int, Row>
                            (_rowIndex, new Row(_rowIndex, _dataFrame));
                        return row;
                    }
                }

                public void Dispose()
                {

                }

                public bool MoveNext()
                {
                    if (_rowIndex < _dataFrame._rowBound)
                    {
                        _rowIndex += 1;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                public void Reset()
                {
                    _rowIndex = -1;
                }
            }

            public bool TryGetValue(int key, out Row value)
            {
                if (key >= 0 && key < _dataFrame._rowBound)
                {
                    value = new Row(key, _dataFrame);
                    return true;
                }
                else
                {
                    value = null;
                    return false;
                }
            }

            public IEnumerator<KeyValuePair<int, Row>> GetEnumerator()
            {
                return new RowsCollectionEnumerator(_dataFrame);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return new RowsCollectionEnumerator(_dataFrame);
            }
        }

        public class Column : IReadOnlyList<object>
        {
            private int _columnIndex;
            private DataFrame _dataFrame;

            public Column(int columnIndex, DataFrame dataFrame)
            {
                _columnIndex = columnIndex;
                _dataFrame = dataFrame;
            }

            public Column(int columnIndex, Dictionary<CellKey, object> dataTable)
            {
                _columnIndex = columnIndex;
                _dataFrame = new DataFrame(dataTable);
            }

            public object this[int index]
            {
                get
                {
                    return _dataFrame[index, _columnIndex];
                }
            }

            public int Count
            {
                get
                {
                    return _dataFrame._rowBound;
                }
            }

            public struct ColumnEnumerator : IEnumerator<object>
            {
                private int _columnIndex;
                private int _rowIndex;
                private DataFrame _dataFrame;

                public ColumnEnumerator(int columnIndex, DataFrame dataFrame)
                {
                    _columnIndex = columnIndex;
                    _rowIndex = -1;
                    _dataFrame = dataFrame;
                }

                public object Current
                {
                    get
                    {
                        return _dataFrame[_rowIndex, _columnIndex];
                    }
                }

                public void Dispose()
                {

                }

                public bool MoveNext()
                {
                    if (_rowIndex < _dataFrame._rowBound)
                    {
                        _rowIndex += 1;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                public void Reset()
                {
                    _rowIndex = -1;
                }
            }

            public IEnumerator<object> GetEnumerator()
            {
                return new ColumnEnumerator(_columnIndex, _dataFrame);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return new ColumnEnumerator(_columnIndex, _dataFrame);
            }
        }

        public class ColumnsCollection : IReadOnlyDictionary<int, Column>
        {
            private DataFrame _dataFrame;
            private Column _column;

            public ColumnsCollection(DataFrame dataFrame)
            {
                _dataFrame = dataFrame;
            }

            public ColumnsCollection(Dictionary<CellKey, object> dataTable)
            {
                _dataFrame = new DataFrame(dataTable);
            }

            public Column this[int key]
            {
                get
                {
                    _column = new Column(key, _dataFrame);
                    return _column;
                }
            }

            private List<int> _keys = new List<int>();
            public IEnumerable<int> Keys
            {
                get
                {
                    for (int i = 0; i < _dataFrame._columnBound; i++)
                    {
                        _keys.Add(i);
                    }
                    return _keys;
                }
            }

            private List<Column> _values = new List<Column>();
            public IEnumerable<Column> Values
            {
                get
                {
                    for (int i = 0; i < _dataFrame._columnBound; i++)
                    {
                        _values.Add(new Column(i, _dataFrame));
                    }
                    return _values;
                }
            }

            public int Count
            {
                get
                {
                    return _dataFrame._columnBound;
                }
            }

            public bool ContainsKey(int key)
            {
                if (key >= 0 && key < _dataFrame._columnBound)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public struct ColumnsCollectionEnumerator : IEnumerator<KeyValuePair<int, Column>>
            {
                private int _columnIndex;
                private DataFrame _dataFrame;

                public ColumnsCollectionEnumerator(DataFrame dataFrame)
                {
                    _columnIndex = -1;
                    _dataFrame = dataFrame;
                }

                public KeyValuePair<int, Column> Current
                {
                    get
                    {
                        KeyValuePair<int, Column> column = new KeyValuePair<int, Column>
                            (_columnIndex, new Column(_columnIndex, _dataFrame));
                        return column;
                    }
                }

                object IEnumerator.Current
                {
                    get
                    {
                        return new Column(_columnIndex, _dataFrame);
                    }
                }

                public void Dispose()
                {

                }

                public bool MoveNext()
                {
                    if (_columnIndex < _dataFrame._columnBound)
                    {
                        _columnIndex += 1;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                public void Reset()
                {
                    _columnIndex = -1;
                }
            }

            public IEnumerator<KeyValuePair<int, Column>> GetEnumerator()
            {
                return new ColumnsCollectionEnumerator(_dataFrame);
            }

            public bool TryGetValue(int key, out Column value)
            {
                if (key >= 0 && key < _dataFrame._columnBound)
                {
                    value = new Column(key, _dataFrame);
                    return true;
                }
                else
                {
                    value = null;
                    return false;
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return new ColumnsCollectionEnumerator(_dataFrame);
            }
        }

        public IReadOnlyList<Column> Columns
        {
            get
            {
                List<Column> columns = new List<Column>();
                ColumnsCollection columnsColl = new ColumnsCollection(DataTable);
                foreach (var r in columnsColl)
                {
                    columns.Add(r.Value);
                }
                _ListReturn = columns;
                return columns;
            }
        }

        public IReadOnlyList<Row> Rows
        {
            get
            {
                List<Row> rows = new List<Row>();
                RowsCollection rowsCol = new RowsCollection(DataTable);
                foreach (var r in rowsCol)
                {
                    rows.Add(r.Value);
                }
                _ListReturn = rows;
                return rows;
            }
            //
        }

        //public IReadOnlyList<Object> Show
        //{
        //    get
        //    {
        //        List<object> showList = new List<object>();
        //        foreach(var sL in showList)
        //        {
        //            Console.Write(sL + "\t");
        //        }
        //        return showList;
        //    }
        //}



        //public new void Add(string key, object value)
        //{

        //}

    }


}
