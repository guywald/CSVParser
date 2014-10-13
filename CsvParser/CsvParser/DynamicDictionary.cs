using System.Collections.Generic;
using System.Dynamic;

namespace CsvParser
{
    public class DynamicDictionary<TValue> : DynamicObject
    {
        public DynamicDictionary()
        {
            _dictionary = new Dictionary<string, TValue>();
        }

        private readonly Dictionary<string, TValue> _dictionary;

        public int Count
        {
            get
            {
                return _dictionary.Count;
            }
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var binderName = binder.Name.ToLower();


            TValue value;
            if (!_dictionary.TryGetValue(binderName, out value))
            {
                result = null;
                return false;
            }

            result = value;
            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            string binderName = binder.Name.ToLower();
            _dictionary[binderName] = (TValue) value;

            return true;
        }

        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            string key = (string) indexes[0];

            TValue value;
            if (_dictionary.TryGetValue(key, out value))
            {
                result = value;
                return true;
            }

            result = null;
            return false;
        }

        public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
        {
            string key = (string) indexes[0];
            _dictionary[key] = (TValue) value;

            return true;
        }
    }
}
