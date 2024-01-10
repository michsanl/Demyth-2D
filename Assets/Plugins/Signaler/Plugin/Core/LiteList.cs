namespace echo17.Signaler.Core
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// This is a super light implementation of an array that 
    /// behaves like a list, automatically allocating new memory
    /// when needed, but not releasing it to garbage collection.
    /// </summary>
    /// <typeparam name="T">The type of the list</typeparam>
    public class LiteList<T> :
        IList,
        IEnumerable
    {
        /// <summary>
        /// internal storage of list data
        /// </summary>
        public T[] data;

        /// <summary>
        /// The number of elements in the list
        /// </summary>
        private int _count = 0;

        /// <summary>
        /// Indexed access to the list items
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public T this[int index]
        {
            get { return data[index]; }
            set { data[index] = value; }
        }

        /// <summary>
        /// Converts the array to a list
        /// </summary>
        /// <returns></returns>
        public List<T> ToList()
        {
            return new List<T>(data);
        }

        /// <summary>
        /// Resizes the array when more memory is needed.
        /// </summary>
        private void ResizeArray()
        {
            T[] newData;

            if (data != null)
                newData = new T[Math.Max(data.Length << 1, 64)];
            else
                newData = new T[64];

            if (data != null && _count > 0)
                data.CopyTo(newData, 0);

            data = newData;
        }

        /// <summary>
        /// Instead of releasing the memory to garbage collection, 
        /// the list size is set back to zero
        /// </summary>
        public void Clear()
        {
            _count = 0;
        }

        /// <summary>
        /// Returns the first element of the list
        /// </summary>
        /// <returns></returns>
        public T First()
        {
            if (data == null || _count == 0) return default(T);
            return data[0];
        }

        /// <summary>
        /// Returns the last element of the list
        /// </summary>
        /// <returns></returns>
        public T Last()
        {
            if (data == null || _count == 0) return default(T);
            return data[_count - 1];
        }

        /// <summary>
        /// Adds a new element to the array, creating more
        /// memory if necessary
        /// </summary>
        /// <param name="item"></param>
        /// <returns>Index of inserted item</returns>
        public int Add(T item)
        {
            if (data == null || _count == data.Length)
                ResizeArray();

            data[_count] = item;
            _count++;

            return _count - 1;
        }

        /// <summary>
        /// Adds a new element to the start of the array, creating more
        /// memory if necessary
        /// </summary>
        /// <param name="item"></param>
        public void AddStart(T item)
        {
            Insert(0, item);
        }

        /// <summary>
        /// Adds a range of items
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public int AddRange(IEnumerable<T> collection)
        {
            var count = 0;
            foreach (var item in collection)
            {
                Add(item);
                count++;
            }
            return count;
        }

        /// <summary>
        /// Inserts a new element to the array at the index specified, creating more
        /// memory if necessary
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        public void Insert(int index, T item)
        {
            if (data == null || _count == data.Length)
                ResizeArray();

            for (var i = _count; i > index; i--)
            {
                data[i] = data[i - 1];
            }

            data[index] = item;
            _count++;
        }

        /// <summary>
        /// Inserts a range of items
        /// </summary>
        /// <param name="index"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        public int InsertRange(int index, IEnumerable<T> collection)
        {
            var count = 0;
            foreach (var item in collection)
            {
                Insert(index + count, item);
                count++;
            }
            return count;
        }

        /// <summary>
        /// Removes an item from the start of the data
        /// </summary>
        /// <param name="ignoreSort">Whether the array can be unsorted for speed</param>
        /// <returns></returns>
        public T RemoveStart(bool ignoreSort = false)
        {
            return RemoveAt(0, ignoreSort);
        }

        /// <summary>
        /// Removes an item from the index of the data
        /// </summary>
        /// <param name="ignoreSort">Whether the array can be unsorted for speed</param>
        /// <returns></returns>
        public T RemoveAt(int index, bool ignoreSort = false)
        {
            if (data != null && _count != 0)
            {
                T oldItem = data[index];

                if (ignoreSort)
                {
                    // ignore the sort, so just move the last item into
                    // this item's spot. This is faster but will get the
                    // array out of order.

                    data[index] = data[_count - 1];
                }
                else
                {
                    // keep the sort, so move all items up one

                    for (var i = index; i < _count - 1; i++)
                    {
                        data[i] = data[i + 1];
                    }
                }

                _count--;
                data[_count] = default(T);

                return oldItem;
            }
            else
            {
                return default(T);
            }
        }

        /// <summary>
        /// Removes an item from the data
        /// </summary>
        /// <param name="item"></param>
        /// <param name="ignoreSort">Whether the array can be unsorted for speed</param>
        /// <returns></returns>
        public T Remove(T item, bool ignoreSort = false)
        {
            if (data != null && _count != 0)
            {
                for (var i = 0; i < _count; i++)
                {
                    if (data[i].Equals(item))
                    {
                        return RemoveAt(i, ignoreSort);
                    }
                }
            }

            return default(T);
        }

        /// <summary>
        /// Removes an item from the data if the comparison callback has a match with the passed comparison value
        /// </summary>
        /// <typeparam name="C">The type of the comparison value</typeparam>
        /// <param name="compareValue">The value to pass to the comparer for the iterated item</param>
        /// <param name="comparer">The comparison callback to evalute equality</param>
        /// <param name="ignoreSort">Whether the array can be unsorted for speed</param>
        /// <returns></returns>
        public T Remove<C>(C compareValue, Func<T, C, bool> comparer, bool ignoreSort = false)
        {
            if (data != null && _count != 0)
            {
                for (var i = 0; i < _count; i++)
                {
                    if (comparer(data[i], compareValue))
                    {
                        return RemoveAt(i, ignoreSort);
                    }
                }
            }

            return default(T);
        }

        /// <summary>
        /// Removes an item from the end of the data
        /// </summary>
        /// <returns></returns>
        public T RemoveEnd()
        {
            if (data != null && _count != 0)
            {
                _count--;
                T oldItem = data[_count];
                data[_count] = default(T);

                return oldItem;
            }
            else
            {
                return default(T);
            }
        }

        /// <summary>
        /// Removes a range of items from the list
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        /// <param name="ignoreSort"></param>
        /// <returns></returns>
        public IEnumerable<T> RemoveRange(int startIndex, int count, bool ignoreSort = false)
        {
            if (startIndex < 0 || startIndex > _count - 1) { throw new ArgumentException("StartIndex must be within the bounds of the LiteList [0.." + _count.ToString() + "]"); }
            if (count <= 0) { throw new ArgumentException("Count must be greater than zero"); }
            if (startIndex + count > _count - 1) { throw new ArgumentException("StartIndex + count must be within the bounds of the LiteList [0.." + _count.ToString() + "]"); }

            List<T> oldItems = new List<T>();
            for (var i = startIndex; i < startIndex + count; i++)
            {
                oldItems.Add(RemoveAt(i, ignoreSort));
            }

            return oldItems;
        }

        /// <summary>
        /// Determines if the data contains the item
        /// </summary>
        /// <param name="item">The item to compare</param>
        /// <returns>True if the item exists in teh data</returns>
        public bool Contains(T item)
        {
            return IndexOf(item) != -1;
        }

        /// <summary>
        /// Returns the index of an item
        /// </summary>
        /// <param name="item">Item to search</param>
        /// <returns>Index of the item</returns>
        public int IndexOf(T item)
        {
            if (data == null)
                return -1;

            for (var i = 0; i < _count; i++)
            {
                if (data[i].Equals(item))
                    return i;
            }

            return -1;
        }

        /// <summary>
        /// Used for enumeration
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }

        public LiteListEnum<T> GetEnumerator()
        {
            return new LiteListEnum<T>(this);
        }

        #region IList implementation

        public bool IsFixedSize
        {
            get
            {
                return false;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public bool IsSynchronized
        {
            get
            {
                return false;
            }
        }

        public object SyncRoot
        {
            get
            {
                return this;
            }
        }

        public int Count
        {
            get
            {
                return _count;
            }
        }

        object IList.this[int index]
        {
            get { return data[index]; }
            set { data[index] = (T)value; }
        }

        public int Add(object value)
        {
            return Add((T)value);
        }

        public bool Contains(object value)
        {
            return Contains((T)value);
        }

        public int IndexOf(object value)
        {
            return IndexOf((T)value);
        }

        public void Insert(int index, object value)
        {
            Insert(index, (T)value);
        }

        public void Remove(object value)
        {
            Remove((T)value, ignoreSort: false);
        }

        public void RemoveAt(int index)
        {
            RemoveAt(index, ignoreSort: false);
        }

        public void CopyTo(Array array, int index)
        {
            var j = index;
            for (var i = 0; i < _count; i++)
            {
                array.SetValue(data[i], j);
                j++;
            }
        }

        #endregion
    }

    public class LiteListEnum<T> : IEnumerator
    {
        public LiteList<T> list;

        // Enumerators are positioned before the first element
        // until the first MoveNext() call.
        int position = -1;

        public LiteListEnum(LiteList<T> list)
        {
            this.list = list;
        }

        public bool MoveNext()
        {
            position++;
            return (position < list.Count);
        }

        public void Reset()
        {
            position = -1;
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        public T Current
        {
            get
            {
                try
                {
                    return list[position];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }
    }
}