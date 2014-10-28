using System;

namespace CsvParser.Exceptions
{
    [Serializable]
    public class CsvParseException : Exception
    {
        public CsvParseException(string message) : base(message)
        {
            
        }

        public CsvParseException(string message, Exception inneException) : base(message, inneException)
        {
            
        }
    }
}
