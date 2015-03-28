using System;
using System.Collections.Generic;

namespace Assets.Code.DataPipeline
{
    public class IoCResolver
    {
        private readonly Dictionary<Type, object> _items;

        public IoCResolver()
        {
            _items = new Dictionary<Type, object>();
        }

        public void RegisterItem<T>(T item) where T : class, IResolvableItem
        {
            var registerType = typeof (T);

            if (_items.ContainsKey(registerType))
                return;

            _items.Add(registerType, item);
        }

        public T Resolve<T>() where T : class, IResolvableItem
        {
            var targetType = typeof (T);

            if (_items.ContainsKey(targetType))
                return _items[targetType] as T;

            return null;
        }

        public void Resolve<T>(out T subject) where T : class, IResolvableItem
        {
            var targetType = typeof(T);

            if (_items.ContainsKey(targetType))
                subject = _items[targetType] as T;
            else
                subject = null;
        }
    }
}
