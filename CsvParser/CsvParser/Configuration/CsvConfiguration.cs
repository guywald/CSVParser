using System;
using System.Text;

namespace CsvParser.Configuration
{
    /// <summary>
    /// Describes the fields needed for a given CsvParser
    /// </summary>
    public class CsvConfiguration
    {
        public CsvConfiguration()
        {
            Delimiter = ',';
            BufferSize = 4096;
            Encoding = Encoding.UTF8;
        }

        private char _delimiter;
        /// <summary>
        /// The delimiter to break fields by.
        /// </summary>
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
        /// <summary>
        /// BufferSize to read from the underlying Stream.
        /// </summary>
        public int BufferSize { get; set; }
        /// <summary>
        /// Determines which encoding should be used to read from the underlying Stream.
        /// </summary>
        public Encoding Encoding { get; set; }
        /// <summary>
        /// Ignore whitespaces in csv fields and headers.
        /// </summary>
        public bool IgnoreWhiteSpaces { get; set; }
    }
}
