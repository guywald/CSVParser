using System;
using System.Text;

namespace CsvParser
{
    public class CsvConfiguration
    {
        public CsvConfiguration()
        {
            Delimiter = ',';
            BufferSize = 2048;
            Encoding = Encoding.UTF8;
        }

        private char _delimiter;
        public char Delimiter
        {
            get { return _delimiter; }
            set
            {
                if (value == '\n')
                {
                    throw new ArgumentException("New line is not a valid delimiter");
                }

                if (value == '\r')
                {
                    throw new ArgumentException("Carriage return is not a valid delimiter");
                }

                if (value == '\0')
                {
                    throw new Exception("Null is not a valid delimiter");
                }

                _delimiter = value;
            }
        }
        public int BufferSize { get; set; }
        public Encoding Encoding { get; set; }
        public bool IgnoreWhiteSpaces { get; set; }
        public bool RemoveEmptyLines { get; set; }
    }
}
