using System.Collections.Generic;
using System.Dynamic;

namespace CsvParser
{
    public sealed class CsvRow : DynamicObject
    {
        public CsvRow()
        {
            _csvDynamicDictionary = new Dictionary<string, object>();
        }

        private readonly Dictionary<string, object> _csvDynamicDictionary;

        public object this[string key]
        {
            get { return _csvDynamicDictionary[key]; }
            set { _csvDynamicDictionary[key] = value; }
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var binderName = binder.Name.ToLower();
            return _csvDynamicDictionary.TryGetValue(binderName, out result);
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            string binderName = binder.Name.ToLower();
            if (_csvDynamicDictionary.ContainsValue(value))
            {
                _csvDynamicDictionary[binderName] = value;
            }
            else
            {
                _csvDynamicDictionary.Add(binderName, value);
            }

            return true;
        }
    }
}