using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.UI
{
    public class PageData
    {
        private Dictionary<string, object> _data;

        public PageData()
        {
            _data = new Dictionary<string, object>();
        }

        public PageData(int capacity)
        {
            _data = new Dictionary<string, object>(capacity);
        }

        public void Add(string key, object data)
        {
            _data.Add(key, data);
        }

        public T Get<T>(string key)
        {
            object datum = Get(key);

            try
            {
                return (T)datum;
            }
            catch
            {
                throw new System.Exception(string.Format("Could not cast data object '{0}' to type '{1}'", key, typeof(T).Name));
            }
        }

        public object Get(string key)
        {
            object datum;

            if (!_data.TryGetValue(key, out datum))
                throw new System.Exception(string.Format("No object found for key '{0}'", key));

            return datum;
        }

        public bool TryGet(string key, out object datum)
        {
            return _data.TryGetValue(key, out datum);
        }

        public bool TryGet<T>(string key, out T datum)
        {
            object datumObj;

            if (_data.TryGetValue(key, out datumObj))
            {
                try
                {
                    datum = (T)datumObj;
                    return true;
                }
                catch
                {
                    throw new System.Exception(string.Format("Could not cast data object '{0}' to type '{1}'", key, typeof(T).Name));
                }
            }

            datum = default(T);
            return false;
        }
    }
}