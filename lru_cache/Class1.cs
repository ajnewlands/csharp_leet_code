using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace lru_cache
{
    /// <summary>
    /// Note that a KeyedCollection can be accessed by index or key, and maintains the indices in insertion order.
    /// </summary>
    public class LRUCache
    {
        protected class OrderedHashSet<T> : KeyedCollection<T, T>
        {
            protected override T GetKeyForItem(T item)
            {
                return item;
            }
        }

        Dictionary<int, int> Cache; // Cache
        OrderedHashSet<int> LRULookup;
        int capacity;

        public LRUCache(int capacity)
        {
            // Initialize both structures with preset capacity
            Cache = new Dictionary<int, int>(capacity);
            LRULookup = new OrderedHashSet<int>();
            this.capacity = capacity;
        }

        // return the cached value (and updated access order) or -1
        public int Get(int key)
        {
            if (Cache.ContainsKey(key))
            {
                // Update access time by moving the key to the end of the list
                LRULookup.Remove(key);
                LRULookup.Add(key);
            }

            return Cache.GetValueOrDefault(key,-1);
        }

        public void Put(int key, int value)
        {
            if (Cache.ContainsKey(key)) // Update value pointed to by existing key
            {
                LRULookup.Remove(key);
                Cache.Remove(key);
            }
            else if (Cache.Count == capacity) // cache is full - remove the oldest
            {
                var oldest = ((Collection<int>)LRULookup)[0];
                Cache.Remove(oldest);
                LRULookup.RemoveAt(0);
            }
            LRULookup.Add(key); // newest item goes on the end of the list.
            Cache.Add(key, value);
        }
    }
}
