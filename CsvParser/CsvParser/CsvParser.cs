using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CsvParser
{
    public class CsvParser : IEnumerable<CsvRow>
    {
        private readonly CsvConfiguration _csvConfiguration;
        private string[] _headers;
        private int? _csvLineCount;
        private readonly StreamReader _streamReader;

        public CsvParser(string csvLocation) : this(csvLocation, new CsvConfiguration())
        {

        }

        public CsvParser(string csvLocation, CsvConfiguration csvConfiguration)
        {
            if (string.IsNullOrEmpty(csvLocation))
            {
                throw new ArgumentException("CSV path can't be empty or null", csvLocation);
            }

            _csvConfiguration = csvConfiguration;
            _streamReader = new StreamReader(csvLocation);
            _csvConfiguration = csvConfiguration;
        }

        public bool IsEmpty
        {
            get { return Count != 0; }
        }

        public int Count
        {
            get
            {
                if (_csvLineCount != null)
                {
                    return _csvLineCount.Value;
                }

                GenerateCount();

                return (int) _csvLineCount;
            }
        }

        public void Initialize()
        {
            GenerateCount();
            ParseCsvHeader();
        }

        public async Task InitializeAsync()
        {
            Task generateCount = GenerateCountAsync();
            Task parseHeader = ParseCsvHeaderAsync();

            await Task.WhenAll(generateCount, parseHeader);
        }

        private void ParseCsvHeader()
        {
            string concatHeader = _streamReader.ReadLine();
            if (concatHeader != null)
            {
                _headers = concatHeader.Split(_csvConfiguration.Delimiter);
            }
        }

        private async Task ParseCsvHeaderAsync()
        {
            string concatHeader = await _streamReader.ReadLineAsync();
            if (concatHeader != null)
            {
                _headers = concatHeader.Split(_csvConfiguration.Delimiter);
            }
        }

        private async Task GenerateCountAsync()
        {
            if (_csvLineCount != null)
            {
                return;
            }

            _csvLineCount = 0;
            while (await _streamReader.ReadLineAsync() != null)
            {
                _csvLineCount++;
            }

            ResetStreamPosition();
        }

        private void GenerateCount()
        {
            _csvLineCount = 0;

            while (_streamReader.ReadLine() != null)
            {
                _csvLineCount++;
            }

            ResetStreamPosition();
        }

        private void ResetStreamPosition()
        {
            _streamReader.BaseStream.Position = 0;
            _streamReader.DiscardBufferedData();
        }

        public IEnumerator<CsvRow> GetEnumerator()
        {
            string csvLine;
            while ((csvLine = _streamReader.ReadLine()) != null)
            {
                string[] csvFields = csvLine.Split(_csvConfiguration.Delimiter);
                CsvRow csvRow = new CsvRow();

                int index = 0;
                foreach (var header in _headers)
                {
                    csvRow[header] = csvFields[index];
                    index++;
                }

                yield return csvRow;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
