using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Markup;
using DefaultUiCleanedResharpedDec16.Assets.Controls;
using DefaultUiCleanedResharpedDec16.DataTypes.Interfaces;

namespace DefaultUiCleanedResharpedDec16.ViewModel.Common
{
    public class VmManager : VmBase
    {
        private IPageViewModel       _currentPageViewModel;
        private List<IPageViewModel> _pageViewModels;
        public List<IPageViewModel> PageViewModels => _pageViewModels ?? (_pageViewModels = new List<IPageViewModel>());

        public IPageViewModel CurrentPageViewModel
        {
            get { return _currentPageViewModel; }
            set
            {
                if (_currentPageViewModel != value)
                {
                    _currentPageViewModel = value;
                    _currentPageViewModel.ViewNavigationSelected = true;

                    StatusTextHandler.StatusText = _currentPageViewModel.ViewDisplayName;

                    RaisePropertyChangedEvent();
                }
            }
        }

        public void ChangeViewModel(IPageViewModel viewmodel)
        {
            if (!PageViewModels.Contains(viewmodel)) PageViewModels.Add(viewmodel);
            CurrentPageViewModel = PageViewModels.FirstOrDefault(vm => vm == viewmodel);
        }

        //
        // Crazy Ivans Data Template Creator
        //
        public void RegisterDataTemplate<TViewModel, TView>() where TView : FrameworkElement
        {
            RegisterDataTemplate(typeof(TViewModel), typeof(TView));
            PageViewModels.Add((IPageViewModel) Activator.CreateInstance(typeof(TViewModel)));
        }

        public void RegisterDataTemplate(Type viewModelType, Type viewType)
        {
            var template = CreateTemplate(viewModelType, viewType);

            var key = template.DataTemplateKey;
            if (key != null && Application.Current.Resources.Contains(key))
                Application.Current.Resources.Remove(key);

            if (key != null) Application.Current.Resources.Add(key, template);
        }

        private DataTemplate CreateTemplate(Type viewModelType, Type viewType)
        {
            const string xamlTemplate = "<DataTemplate DataType=\"{{x:Type vm:{0}}}\"><v:{1} /></DataTemplate>";
            var xaml = string.Format(xamlTemplate, viewModelType.Name, viewType.Name);
            var context = new ParserContext {XamlTypeMapper = new XamlTypeMapper(new string[0])};

            context.XamlTypeMapper.AddMappingProcessingInstruction("vm", viewModelType.Namespace,
                viewModelType.Assembly.FullName);
            context.XamlTypeMapper.AddMappingProcessingInstruction("v", viewType.Namespace, viewType.Assembly.FullName);

            context.XmlnsDictionary.Add("", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
            context.XmlnsDictionary.Add("x", "http://schemas.microsoft.com/winfx/2006/xaml");
            context.XmlnsDictionary.Add("vm", "vm");
            context.XmlnsDictionary.Add("v", "v");

            var template = (DataTemplate) XamlReader.Parse(xaml, context);
            return template;
        }
    }
}