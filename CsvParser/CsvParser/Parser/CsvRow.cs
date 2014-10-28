using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace CsvParser.Parser
{
    /// <summary>
    /// CsvRow is a dynamic object. It describes a given row of a CSV file
    /// </summary>
    public sealed class CsvRow : DynamicObject
    {
        public CsvRow()
        {
            _csvDictionary = new Dictionary<string, object>();
        }

        private readonly Dictionary<string, object> _csvDictionary;

        public object this[string key]
        {
            get { return _csvDictionary[key]; }
            set { _csvDictionary[key] = value; }
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var binderName = binder.Name.ToLower();
            return _csvDictionary.TryGetValue(binderName, out result);
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            string binderName = binder.Name.ToLower();
            if (_csvDictionary.ContainsValue(value))
            {
                _csvDictionary[binderName] = value;
            }
            else
            {
                _csvDictionary.Add(binderName, value);
            }

            return true;
        }

        public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
        {
            string keyToApply = (string)indexes[0];
            if (_csvDictionary.ContainsKey(keyToApply))
            {
                _csvDictionary[keyToApply] = value;
                return true;
            }

            _csvDictionary.Add(keyToApply, value);
            return false;
        }

        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            string keyToRetrieve = (string) indexes[0];
            return _csvDictionary.TryGetValue(keyToRetrieve, out result);
        }
    }
}