using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FastMember;

namespace CsvParser
{
    public sealed class CsvParser : IEnumerable<CsvRow>, IDisposable
    {
        public CsvParser(string csvLocation)
            : this(csvLocation, new CsvConfiguration())
        {

        }

        public CsvParser(string csvLocation, CsvConfiguration csvConfiguration) : this(new StreamReader(csvLocation), csvConfiguration)
        {

        }

        public CsvParser(StreamReader streamReader, CsvConfiguration csvConfiguration)
        {
            _streamReader = streamReader;
            _csvConfiguration = csvConfiguration;
            ParseCsvHeader();
        }

        private readonly CsvConfiguration _csvConfiguration;
        private string[] _header;
        private int? _csvLineCount;
        private readonly StreamReader _streamReader;

        public void Initialize()
        {
            GenerateCount();
        }

        public Task InitializeAsync()
        {
            return GenerateCountAsync();
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

                return (int)_csvLineCount;
            }
        }

        public dynamic this[int index]
        {
            get
            {
                if (index > Count)
                {
                    throw new IndexOutOfRangeException(string.Format("Index {0} exceeds CSV line count ({1})", index,
                        Count));
                }

                int linePosition = 0;
                while (linePosition < index)
                {
                    _streamReader.ReadLine();
                    linePosition++;
                }

                string desireRow = _streamReader.ReadLine();
                return ParseRow<ExpandoObject>(desireRow);
            }
        }

        public IEnumerator<CsvRow> GetEnumerator()
        {
            string csvLine;
            while ((csvLine = _streamReader.ReadLine()) != null)
            {
                yield return ParseRow<CsvRow>(csvLine);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        private void Dispose(bool disposing)
        {
            
        }

        private dynamic ParseRow<T>(string row) where T : IDynamicMetaObjectProvider, new()
        {
            string[] splitRow = row.Split(_csvConfiguration.Delimiter);

            ObjectAccessor objectAccessor = ObjectAccessor.Create(new T());

            int index = 0;
            foreach (var header in _header)
            {
                objectAccessor[header] = splitRow[index].Trim();
                index++;
            }

            return objectAccessor.Target;
        }

        private void ParseCsvHeader()
        {
            string concatHeader = _streamReader.ReadLine();
            if (concatHeader != null)
            {
                ParseHeaderInternal(concatHeader);
            }

            ResetStreamPosition();  
        }

        private async Task ParseCsvHeaderAsync()
        {
            string concatHeader = await _streamReader.ReadLineAsync();
            if (concatHeader != null)
            {
                ParseHeaderInternal(concatHeader);
            }
        }

        private void ParseHeaderInternal(string concatHeader)
        {
            _header =
                concatHeader.Split(_csvConfiguration.Delimiter).Select(header => header.ToLower().Trim()).ToArray();
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
    }
}
