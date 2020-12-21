using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Dynamic;
using Microsoft.Extensions.Logging;

namespace ServiceComposer.AspNetCore.TypedViewModel
{
    class TypedDynamicViewModel : DynamicObject
    {
        readonly ConcurrentDictionary<string, object> _properties = new();

        public TypedDynamicViewModel( TypedViewModel typedViewModel)
        {
            if (typedViewModel != null)
            {
                TypedViewModel = typedViewModel;
                TypedViewModel.ViewModel = this;
            }
        }
        public TypedViewModel TypedViewModel { get; }

        public override bool TryGetMember(GetMemberBinder binder, out object result) => _properties.TryGetValue(binder.Name, out result);

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            _properties.AddOrUpdate(binder.Name, value, (key, existingValue) => value);
            return true;
        }

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            foreach (var item in _properties.Keys)
            {
                yield return item;
            }
        }

        public void SetPropertyValue(string name, object value)
        {
            _properties.AddOrUpdate(name, value, (key, existingValue) => value);
        }

        public object GetPropertyValue(string name)
        {
            if (_properties.ContainsKey(name))
            {
                return _properties[name];
            }

            return null;
        }
    }
}