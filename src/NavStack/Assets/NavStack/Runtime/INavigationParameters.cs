using System.Collections;
using System.Collections.Generic;

namespace NavStack
{
    public class NavigationParameters : IDictionary<object, object>
    {
        readonly Dictionary<object, object> dictionary = new();

        public object this[object key]
        {
            get => dictionary[key];
            set => dictionary[key] = value;
        }

        public ICollection<object> Keys => dictionary.Keys;
        public ICollection<object> Values => dictionary.Values;

        public int Count => dictionary.Count;
        bool ICollection<KeyValuePair<object, object>>.IsReadOnly => false;

        public void Add(object key, object value)
        {
            dictionary.Add(key, value);
        }

        public void Add(KeyValuePair<object, object> item)
        {
            dictionary.Add(item.Key, item.Value);
        }

        public void Clear()
        {
            dictionary.Clear();
        }

        bool ICollection<KeyValuePair<object, object>>.Contains(KeyValuePair<object, object> item)
        {
            return ((IDictionary<object, object>)dictionary).Contains(item);
        }

        public bool ContainsKey(object key)
        {
            return dictionary.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<object, object>[] array, int arrayIndex)
        {
            ((IDictionary<object, object>)dictionary).CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<object, object>> GetEnumerator()
        {
            return ((IDictionary<object, object>)dictionary).GetEnumerator();
        }

        public bool Remove(object key)
        {
            return dictionary.Remove(key);
        }

        public bool Remove(KeyValuePair<object, object> item)
        {
            return ((IDictionary<object, object>)dictionary).Remove(item);
        }

        public bool TryGetValue(object key, out object value)
        {
            return dictionary.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return dictionary.GetEnumerator();
        }
    }
}