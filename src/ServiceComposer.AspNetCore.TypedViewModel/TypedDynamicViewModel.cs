using Microsoft.Extensions.Logging;

namespace ServiceComposer.AspNetCore.TypedViewModel
{
    class TypedDynamicViewModel : DynamicViewModel
    {
        public TypedDynamicViewModel(ILogger<DynamicViewModel> logger, CompositionContext compositionContext, TypedViewModel typedViewModel)
            : base(logger, compositionContext)
        {
            TypedViewModel = typedViewModel;
            TypedViewModel.ViewModel = this;
        }
        public TypedViewModel TypedViewModel { get; }
        
        public void SetPropertyValue(string name, object value)
        {
            GetProperties()[name] = value;
        }

        public object GetPropertyValue(string name)
        {
            var props= GetProperties();
            if (props.ContainsKey(name))
            {
                return props[name];
            }

            return null;
        }
    }
}