using System;
using System.Collections.Concurrent;
using Castle.DynamicProxy;

namespace ServiceComposer.AspNetCore.TypedViewModel
{
    class TypedViewModelInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            var proxy = (TypedViewModel) invocation.Proxy;
            if (invocation.Method.Name.StartsWith("set_"))
            {
                var propName = invocation.Method.Name.Substring(4);
                proxy.ViewModel.SetPropertyValue(propName, invocation.Arguments[0]);
                return;
            }
            
            if (invocation.Method.Name.StartsWith("get_"))
            {
                var propName = invocation.Method.Name.Substring(4);
                invocation.ReturnValue = proxy.ViewModel.GetPropertyValue(propName) 
                                         ?? GetDefaultValue(invocation.Method.ReturnType);
                return;
            }

            invocation.Proceed();
        }
        
        static readonly ConcurrentDictionary<Type, object> typeDefaults = new();
        static object GetDefaultValue(Type type)
        {
            return type.IsValueType
                ? typeDefaults.GetOrAdd(type, Activator.CreateInstance)
                : null;
        }
    }
}