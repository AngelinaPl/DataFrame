using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataF
{
    partial class DataFrame
    {
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
                get => _dataFrame[index, _columnIndex];
            }

            public int Count
            {
                get => _dataFrame.RowBound;
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
                    get => _dataFrame[_rowIndex, _columnIndex];
                }

                public void Dispose()
                {
                }

                public bool MoveNext()
                {
                    if (_rowIndex < _dataFrame.RowBound)
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

            public IEnumerator<object> GetEnumerator()
            {
                return new ColumnEnumerator(_columnIndex, _dataFrame);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return new ColumnEnumerator(_columnIndex, _dataFrame);
            }
        }
    }
}
