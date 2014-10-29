using CsvParser.Configuration;
using CsvParser.Parser;
using System;
using System.Collections.Generic;

namespace CsvParser.Interfaces
{
    public interface ICsvParser
    {
        CsvConfiguration CsvConfiguration { get; }
        long Count { get; }
        bool IsEmpty { get; }
        List<CsvRow> WhereEquals<T>(string fieldName, T fieldValue) where T : IComparable<T>, IConvertible;
        List<CsvRow> WhereGreaterThan<T>(string fieldName, T fieldValue) where T : IComparable<T>, IConvertible;
        List<CsvRow> WhereLessThan<T>(string fieldName, T fieldValue) where T : IComparable<T>, IConvertible;
    }
}
