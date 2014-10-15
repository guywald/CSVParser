using System.Dynamic;
using System.Linq.Expressions;

namespace CsvParser
{
    public sealed class CsvRow : IDynamicMetaObjectProvider
    {
        private readonly dynamic _csvFields;

        public CsvRow()
        {
            _csvFields = new DynamicDictionary<string>();
        }
       
        public string this[string key]
        {
            get { return _csvFields[key]; }
            set { _csvFields[key] = value; }
        }

        public DynamicMetaObject GetMetaObject(Expression parameter)
        {
            return ((IDynamicMetaObjectProvider) _csvFields).GetMetaObject(Expression.Constant(_csvFields));
        }
    }
}