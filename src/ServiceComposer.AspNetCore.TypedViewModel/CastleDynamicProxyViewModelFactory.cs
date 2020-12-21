using System;
using System.Collections.Generic;
using System.Linq;
using Castle.DynamicProxy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ServiceComposer.AspNetCore.TypedViewModel
{
    class CastleDynamicProxyViewModelFactory : IViewModelFactory
    {
        private static ProxyGenerator generator = new();

        public object CreateViewModel(HttpContext httpContext, ICompositionContext compositionContext)
        {
            var endpoint = httpContext.GetEndpoint();
            var metadata = endpoint?.Metadata;
            var viewModelTypes = metadata?.OfType<TypedViewModelAttribute>()
                .Select(a => a.Type)
                .ToArray();

            TypedViewModel typedViewModel = null;
            if (viewModelTypes != null && viewModelTypes.Any())
            {
                typedViewModel = (TypedViewModel)generator.CreateClassProxy(
                    typeof(TypedViewModel),
                    viewModelTypes,
                    ProxyGenerationOptions.Default,
                    new object[0],
                    new TypedViewModelInterceptor());
            }

            var vm = new TypedDynamicViewModel(typedViewModel);

            return vm;
        }
    }
}