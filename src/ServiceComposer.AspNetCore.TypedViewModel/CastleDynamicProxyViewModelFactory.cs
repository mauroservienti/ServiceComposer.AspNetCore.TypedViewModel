using System;
using System.Collections.Generic;
using System.Linq;
using Castle.DynamicProxy;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ServiceComposer.AspNetCore.TypedViewModel
{
    class CastleDynamicProxyViewModelFactory : IViewModelFactory
    {
        HashSet<Type> typedViewModelTypes = new();
        ProxyGenerator generator = new();

        public void RegisterTypedViewModel(Type viewModelType)
        {
            typedViewModelTypes.Add(viewModelType);
        }

        public DynamicViewModel CreateViewModel(HttpContext httpContext, CompositionContext compositionContext)
        {
            var logger = httpContext.RequestServices.GetRequiredService<ILogger<DynamicViewModel>>();
            var typedViewModel = (TypedViewModel) generator.CreateClassProxy(
                typeof(TypedViewModel),
                typedViewModelTypes.ToArray(),
                ProxyGenerationOptions.Default,
                new object[0],
                new TypedViewModelInterceptor());

            var vm = new TypedDynamicViewModel(logger, compositionContext, typedViewModel);

            return vm;
        }
    }
}