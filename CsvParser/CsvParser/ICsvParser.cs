using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvParser
{
    public interface ICsvParser
    {
        CsvConfiguration CsvConfiguration { get; }
        int Count { get; }
        bool IsEmpty { get; }
        List<CsvRow> WhereEquals<T>(string fieldName, T fieldValue) where T : IComparable<T>, IConvertible;
        List<CsvRow> WhereGreaterThan<T>(string fieldName, T fieldValue) where T : IComparable<T>, IConvertible;
        List<CsvRow> WhereLessThan<T>(string fieldName, T fieldValue) where T : IComparable<T>, IConvertible;
    }
}
