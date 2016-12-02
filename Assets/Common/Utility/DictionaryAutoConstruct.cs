using System.Collections;

namespace System.Collections.Generic
{

    public class DictionaryAutoConstruct<TKey, TValue> : Dictionary<TKey, TValue>
    {
        private Func<TKey, TValue> constructor;

        public DictionaryAutoConstruct(Func<TKey, TValue> constructor)
        {
            this.constructor = constructor;
        }

        new public TValue this[TKey key]
        {
            get
            {
                TValue returnValue;
                if (!TryGetValue(key, out returnValue))
                {
                    Add(key, returnValue = constructor(key));
                }
                return returnValue;
            }
            set
            {
                if (ContainsKey(key))
                {
                    base[key] = value;
                }
                else
                {
                    Add(key, value);
                }
            }
        }
		
    }

}
