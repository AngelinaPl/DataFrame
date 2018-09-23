using System.Collections;
using System.Collections.Generic;

namespace linaPl.DataFrame.DataFrame
{
    partial class DataFrame
    {
        public class RowsCollection : IReadOnlyDictionary<int, Row>
        {
            private static DataFrame _dataFrame;

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
                get => new Row(key, _dataFrame);
            }

            class RowKeysCollection : IReadOnlyList<int>
            {
                public int this[int index]
                {
                    get => index;
                }
                public int Count
                {
                    get => _dataFrame.RowBound * _dataFrame.ColumnBound;
                }

                public struct RowKeyCollectionEnumerator : IEnumerator<int>
                {
                    private int _index;

                    // ReSharper disable once UnusedParameter.Local
                    public RowKeyCollectionEnumerator(DataFrame dataFrame)
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
                        if (_index < _dataFrame.RowBound)
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

                public IEnumerator GetEnumerator()
                {
                    return new RowKeyCollectionEnumerator(_dataFrame);
                }

                IEnumerator<int> IEnumerable<int>.GetEnumerator()
                {
                    return new RowKeyCollectionEnumerator(_dataFrame);
                }
            }

            public IEnumerable<int> Keys
            {
                get => new RowKeysCollection();
            }

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
                get => _dataFrame._rowBound;
            }

            public bool ContainsKey(int key)
            {
                if (key >= 0 && key < _dataFrame._rowBound)
                {
                    return true;
                }
                return false;
            }

            public struct RowsCollectionEnumerator : IEnumerator<KeyValuePair<int, Row>>
            {
                private int _rowIndex;
                // ReSharper disable once MemberHidesStaticFromOuterClass
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
                        Row row = new Row(_rowIndex, _dataFrame);
                        return row[_rowIndex];
                    }
                }

                KeyValuePair<int, Row> IEnumerator<KeyValuePair<int, Row>>.Current
                {
                    get
                    {
                        var row = new KeyValuePair<int, Row>
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
                    return false;
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
                value = null;
                return false;
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
    }
}
