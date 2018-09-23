using System.Text;

namespace linaPl.DataFrame.Interfaces
{
    public interface IDataFrame
    {
        StringBuilder PrintAsTable();
        void AddRow();
        void AddColumn();
        bool DeleteColumn(int column);

        bool DeleteRow(int row);
    }
}
