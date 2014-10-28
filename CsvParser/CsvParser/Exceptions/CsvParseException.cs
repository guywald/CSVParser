using System;

namespace CsvParser.Exceptions
{
    /// <summary>
    /// A csv exception to indicate something is went wrong during parsing.
    /// </summary>
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
