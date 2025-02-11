using System;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;

public class LruCache<TKey, TValue>
{
    private readonly MemoryCache _cache;
    private readonly LinkedList<TKey> _lruList;
    private readonly Dictionary<TKey, LinkedListNode<TKey>> _lruMap;
    private readonly int _capacity;

    public LruCache(int capacity)
    {
        _capacity = capacity;
        _cache = new MemoryCache(new MemoryCacheOptions());
        _lruList = new LinkedList<TKey>();
        _lruMap = new Dictionary<TKey, LinkedListNode<TKey>>();
    }

    public TValue Get(TKey key)
    {
        if (_cache.TryGetValue(key, out TValue value))
        {
            MoveToRecent(key);
            return value;
        }
        return default;
    }

    public void Set(TKey key, TValue value)
    {
        if (_cache.TryGetValue(key, out _))
        {
            _cache.Set(key, value);
            MoveToRecent(key);
        }
        else
        {
            if (_lruList.Count >= _capacity)
            {
                RemoveLeastRecentlyUsed();
            }
            _cache.Set(key, value);
            var node = new LinkedListNode<TKey>(key);
            _lruList.AddFirst(node);
            _lruMap[key] = node;
        }
    }

    private void MoveToRecent(TKey key)
    {
        if (_lruMap.TryGetValue(key, out var node))
        {
            _lruList.Remove(node);
            _lruList.AddFirst(node);
        }
    }

    private void RemoveLeastRecentlyUsed()
    {
        var leastUsedNode = _lruList.Last;
        if (leastUsedNode != null)
        {
            _lruList.RemoveLast();
            _lruMap.Remove(leastUsedNode.Value);
            _cache.Remove(leastUsedNode.Value);
        }
    }
}