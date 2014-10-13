using System;
using System.Dynamic;

namespace CsvParser
{
    public sealed class CsvRow
    {
        private readonly dynamic _csvFields;

        internal CsvRow()
        {
            _csvFields = new DynamicDictionary<string>();
        }
       
        public string this[string key]
        {
            get { return _csvFields[key]; }
            set { _csvFields[key] = value; }
        }
    }
}