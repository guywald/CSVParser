using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvParser.Exceptions;

namespace CsvParser
{
    public class CsvParser : ICsvParser, IEnumerable<CsvRow>, IDisposable
    {
        public CsvParser(string csvLocation)
            : this(csvLocation, new CsvConfiguration())
        {
        }

        public CsvParser(string csvLocation, CsvConfiguration csvConfiguration)
            : this(new StreamReader(csvLocation), csvConfiguration)
        {
        }

        public CsvParser(StreamReader streamReader, CsvConfiguration csvConfiguration)
        {
            _streamReader = streamReader;
            CsvConfiguration = csvConfiguration;
            Initialize();
        }

        private string[] _header;
        private long _currentLinePosition;
        private int? _csvLineCount;
        private readonly StreamReader _streamReader;

        private void Initialize()
        {
            ParseCsvHeader();
            GenerateCount();
        }
        private List<CsvRow> WhereInternal<T>(string fieldName, Predicate<T> predicate) where T : IComparable<T>, IConvertible
        {
            if (string.IsNullOrEmpty(fieldName))
            {
                throw new ArgumentException("Field cannot be null or empty", fieldName);
            }

            var csvRows = new List<CsvRow>();
            while (!_streamReader.EndOfStream)
            {
                string currentRow = ReadLine();
                dynamic parsedRow = ParseRow(currentRow);

                string parsedField = null;

                try
                {
                    parsedField = parsedRow[fieldName];

                    T convertedField = (T) Convert.ChangeType(parsedField, typeof (T));
                    if (predicate(convertedField))
                    {
                        csvRows.Add(parsedRow);
                    }
                }
                catch (KeyNotFoundException e)
                {
                    throw new CsvParseException(
                        string.Format("The given key {0} is not a field header in the CSV file", fieldName), e);
                }
                catch (FormatException e)
                {
                    throw new CsvParseException(string.Format("Could not convert CSV field \"{0}\" to type {1}", parsedField,
                        typeof (T).FullName), e);
                }
            }

            ResetStreamToBeginning();
            return csvRows;
        }
        private dynamic ParseRow(string row)
        {
            string[] splitRow = row.Split(CsvConfiguration.Delimiter);

            dynamic csvRow = new CsvRow();

            int index = 0;
            foreach (var title in _header)
            {
                csvRow[title] = splitRow[index].Trim();
                index++;
            }

            return csvRow;
        }

        private void ParseCsvHeader()
        {
            string concatHeader = _streamReader.ReadLine();
            if (concatHeader != null)
            {
                ParseHeaderInternal(concatHeader);
            }
        }

        private void ParseHeaderInternal(string concatHeader)
        {
            _header =
                concatHeader.Split(CsvConfiguration.Delimiter).Select(header => header.ToLower().Trim()).ToArray();

            ResetStreamToBeginning();
        }

        private void GenerateCount()
        {
            _csvLineCount = 0;

            while (ReadLine() != null)
            {
                _csvLineCount++;
            }

            ResetStreamToBeginning();
        }

        private void ResetStreamToBeginning()
        {
            _currentLinePosition = 0;
            _streamReader.BaseStream.Position = 0;
            _streamReader.DiscardBufferedData();
            ReadLine();
        }

        private string ReadLine()
        {
            _currentLinePosition++;
            return _streamReader.ReadLine();
        }

        /// <summary>
        /// Indicates if the underlying CSV is empty
        /// </summary>
        public bool IsEmpty
        {
            get { return Count == 0; }
        }

        /// <summary>
        /// Searches for equality given a field name and value
        /// </summary>
        /// <typeparam name="T">Field value type</typeparam>
        /// <param name="fieldName">The field name in the CSV header</param>
        /// <param name="fieldValue">The value to match agains't</param>
        /// <returns>A list of matching rows</returns>
        public List<CsvRow> WhereEquals<T>(string fieldName, T fieldValue) where T : IComparable<T>, IConvertible
        {
            return WhereInternal<T>(fieldName, convertedValue => fieldValue.CompareTo(convertedValue) == Decimal.Zero);
        }

        public List<CsvRow> WhereGreaterThan<T>(string fieldName, T fieldValue) where T : IComparable<T>, IConvertible
        {
            return WhereInternal<T>(fieldName, convertedValue => fieldValue.CompareTo(convertedValue) < Decimal.Zero);
        }

        public List<CsvRow> WhereLessThan<T>(string fieldName, T fieldValue) where T : IComparable<T>, IConvertible
        {
            return WhereInternal<T>(fieldName, convertedValue => fieldValue.CompareTo(convertedValue) > Decimal.Zero);
        }


        public CsvConfiguration CsvConfiguration { get; private set; }

        public int Count
        {
            get
            {
                if (_csvLineCount != null)
                {
                    return _csvLineCount.Value;
                }

                GenerateCount();

                return (int)_csvLineCount;
            }
        }

        public dynamic this[int index]
        {
            get
            {
                if (index > Count)
                {
                    throw new ArgumentOutOfRangeException("index");
                }

                if (_currentLinePosition > index)
                {
                    ResetStreamToBeginning();
                }

                while (_currentLinePosition < index + 1)
                {
                    ReadLine();
                }

                string desireRow = ReadLine();
                dynamic parsedRow = ParseRow(desireRow);

                return parsedRow;
            }
        }

        public IEnumerator<CsvRow> GetEnumerator()
        {
            string csvLine;

            while ((csvLine = ReadLine()) != null)
            {
                yield return ParseRow(csvLine);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _streamReader.Dispose();
            }
        }

    }
}
