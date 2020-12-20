using System;
using Microsoft.AspNetCore.Http;
using ServiceComposer.AspNetCore.TypedViewModel;

namespace ServiceComposer.AspNetCore
{
    public static class HttpRequestExtensions
    {
        public static T GetComposedResponseModel<T>(this HttpRequest request) where T : class
        {
            var vm = request.GetComposedResponseModel();
            if (vm is TypedDynamicViewModel {TypedViewModel: T}) 
            {
                return (T)vm.TypedViewModel;
            }

            var message = $"Cannot convert view model to {typeof(T).Name}. Make sure " +
                          $"{typeof(T).Name} was registered as typed view model at configuration " +
                          $"time by calling options.RegisterTypedViewModel<{typeof(T).Name}>().";
            throw new InvalidCastException(message);
        }
    }
}