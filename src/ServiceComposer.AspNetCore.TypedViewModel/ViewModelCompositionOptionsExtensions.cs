using System;
using System.Collections.Generic;
using System.Linq;
using ServiceComposer.AspNetCore.TypedViewModel;

namespace ServiceComposer.AspNetCore
{
    public static class ViewModelCompositionOptionsExtensions
    {
        public static void RegisterTypedViewModel<TViewModel>(this ViewModelCompositionOptions options)
        {
            RegisterTypedViewModels(options, new[] {typeof(TViewModel)});
        }

        public static void RegisterTypedViewModels(this ViewModelCompositionOptions options, Type[] viewModelTypes)
        {
            var invalidTypes = new List<Type>();
            invalidTypes.AddRange(viewModelTypes.Where(vmType => !vmType.IsInterface));
            if (invalidTypes.Any())
            {
                var typesListString = invalidTypes.Aggregate("", (current, vmType) => current + vmType.Name + ", ")
                    .TrimEnd(", ".ToCharArray());
                var message = $"{typesListString} cannot be used as a typed {(invalidTypes.Count == 1 ? "model" : "models")}. Only interfaces are supported.";
                throw new ArgumentException(message);
            }

            if (options.ViewModelFactory is not CastleDynamicProxyViewModelFactory)
            {
                options.ViewModelFactory = new CastleDynamicProxyViewModelFactory();
            }

            var castleDynamicProxyViewModelFactory = (CastleDynamicProxyViewModelFactory) options.ViewModelFactory;
            foreach (var viewModelType in viewModelTypes)
            {
                castleDynamicProxyViewModelFactory.RegisterTypedViewModel(viewModelType);   
            }
        }
    }
}