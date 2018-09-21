using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static linaPl.DataFrame.DataFrame;

namespace linaPl.DataFrame.Interfaces
{
    public interface IDataFrame
    {
        Type DefineColumnType(Dictionary<CellKey, object> dataTable, int column);
        StringBuilder PrintAsTable();
        void ChangeType(Dictionary<CellKey, object> dataTable, int indexColumn, Type type);
        void AddRow();
        void AddColumn();

    }
}
