using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScriptServerStation.HelpClasses.Cache.MyCache
{
    public class XyxCache : ICacheOption
    {

        public bool Exists(string key)
        {
            throw new NotImplementedException();
        }

        public T GetValue<T>(string key)
        {
            throw new NotImplementedException();
        }

        public bool Remove(string key)
        {
            throw new NotImplementedException();
        }

        public void SetValue(string key, object obj)
        {
            throw new NotImplementedException();
        }

        public void SetValue(string key, object obj, DateTimeOffset timeOffset)
        {
            throw new NotImplementedException();
        }
    }
}
