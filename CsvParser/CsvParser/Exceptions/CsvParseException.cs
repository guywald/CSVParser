using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
