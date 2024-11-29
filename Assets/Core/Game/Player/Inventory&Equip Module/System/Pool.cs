using System.Collections.Generic;
using System;
using System.Linq;

public sealed class Pool<T> where T : class
{
    public int Count { get { return inactive.Count; } }
    public bool IsEmpty { get { return Count == 0; } }
    public bool CanTake { get { return allowTakingWhenEmpty || !IsEmpty; } }

    private List<T> inactive = new();
    private List<T> active = new();
    private Func<T> creator;
    private bool allowTakingWhenEmpty;

    public Pool(Func<T> creator, int initialCount = 0, bool allowTakingWhenEmpty = true)
    {
        if (creator == null) throw new ArgumentNullException("Null: " + nameof(creator));
        if (initialCount < 0) throw new ArgumentOutOfRangeException("Init count cant be negative: " + nameof(initialCount));

        this.creator = creator;
        inactive.Capacity = initialCount;
        this.allowTakingWhenEmpty = allowTakingWhenEmpty;

        while (inactive.Count < initialCount) 
        {
            inactive.Add(creator());
        }
    }
    public T Take()
    {
        if (IsEmpty)
        {
            if (allowTakingWhenEmpty)
            {
                var obj = creator();
                inactive.Add(obj);
                return TakeInternal();
            }
            else
            {
                return null;
            }
        }
        else
        {
            return TakeInternal();
        }
    }

    public T TakeInternal()
    {
        T obj = inactive[inactive.Count - 1];
        inactive.RemoveAt(inactive.Count - 1);
        active.Add(obj);
        return obj;
    }
    public void Recycle(T item)
    {
        if (!active.Contains(item))
        {
            throw new InvalidOperationException("An item was recycled even though it was not part of the pool");
        }
        inactive.Add(item);
        active.Remove(item);
    }
    public List<T> GetActive()
    {
        return active.ToList();
    }
}
