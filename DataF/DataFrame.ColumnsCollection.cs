using System;
using System.Collections;
using System.Collections.Generic;

namespace DataF
{
    partial class DataFrame
    {
        public class ColumnsCollection : IReadOnlyDictionary<int, Column>
        {
            private static DataFrame _dataFrame;
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

            public class ColumnKeysCollection : IReadOnlyList<int>
            {
                public int this[int index]
                {
                    get => index;
                }

                public int Count
                {
                    get => _dataFrame.RowBound * _dataFrame.ColumnBound;
                }

                public struct ColumnKesCollectionEnumerator : IEnumerator<int>
                {
                    private int _index;

                    public ColumnKesCollectionEnumerator(DataFrame dataFrame)
                    {
                        _index = -1;
                    }
                    public int Current
                    {
                        get => _index;
                    }

                    object IEnumerator.Current
                    {
                        get => _index;
                    }

                    public void Dispose()
                    {
                    }

                    public bool MoveNext()
                    {
                        if (_index < _dataFrame.ColumnBound)
                        {
                            _index += 1;
                            return true;
                        }
                        return false;
                    }

                    public void Reset()
                    {
                        _index = -1;
                    }
                }

                public IEnumerator<int> GetEnumerator()
                {
                    return new ColumnKesCollectionEnumerator(_dataFrame);
                }

                IEnumerator IEnumerable.GetEnumerator()
                {
                    return new ColumnKesCollectionEnumerator(_dataFrame);
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
                get => _dataFrame._columnBound;
            }

            public bool ContainsKey(int key)
            {
                if (key >= 0 && key < _dataFrame._columnBound)
                {
                    return true;
                }
                return false;
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
                    get => new Column(_columnIndex, _dataFrame);
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
                    return false;
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
                value = null;
                return false;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return new ColumnsCollectionEnumerator(_dataFrame);
            }
        }
    }
}
