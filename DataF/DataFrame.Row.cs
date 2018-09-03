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
                get => _dataFrame[_rowIndex, index];
            }

            public int Count
            {
                get => _dataFrame.ColumnBound;
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
                    get => _dataFrame[_rowIndex, _columnIndex];
                }

                public void Dispose()
                {
                }

                public bool MoveNext()
                {
                    if (_columnIndex < _dataFrame.ColumnBound)
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

            public IEnumerator<object> GetEnumerator()
            {
                return new RowEnumerator(_rowIndex, _dataFrame);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return new RowEnumerator(_rowIndex, _dataFrame);
            }
        }
    }
}
