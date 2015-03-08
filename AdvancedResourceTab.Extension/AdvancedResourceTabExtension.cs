using AdvancedResourceTab.Extension.ViewModels;
using Microsoft.Expression.DesignSurface.UserInterface.ResourcePane;
using Microsoft.Expression.Extensibility;
using Microsoft.Expression.Framework.UserInterface;
using Microsoft.Expression.Utility.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace AdvancedResourceTab.Extension
{
    [Export(typeof(IPackage))]
    public class AdvancedResourceTabExtension : IPackage
    {
        #region Private members

        private ItemsControl _itemsControl;

        #endregion

        #region IPackage implementation

        public void Load(IServices services)
        {
            var windowService = services.GetService<IWindowService>();

            var advancedResourceTabPanelViewModel = new AdvancedResourceTabPanelViewModel(windowService);
            windowService.RegisterPalette("AdvancedResourceTab", advancedResourceTabPanelViewModel.View, "Advanced Resource Tab");

            var resourcePane = windowService.PaletteRegistry["Designer_ResourcePane"];
            if (resourcePane != null && resourcePane.Content != null)
            {
                resourcePane.Content.Loaded += ResourcePaneLoaded;
            }
        }

        public void Unload()
        {
        }

        #endregion

        #region Private methods

        private void ResourcePaneLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (_itemsControl != null)
            {
                return;
            }

            _itemsControl = VisualHelper.FindChild<ItemsControl>(sender as DependencyObject, "ResourceItemSelector");
            if (_itemsControl != null)
            {
                var resourceCollection = CollectionViewSource.GetDefaultView(_itemsControl.ItemsSource);
                var radioButtonGroup = VisualHelper.FindChild<RadioButtonGroup>(sender as DependencyObject);
                if (radioButtonGroup != null)
                {
                    var parentGrid = radioButtonGroup.Parent as Grid;
                    if (parentGrid != null)
                    {
                        var additionalResourceTabControlsViewModel = new AdditionalResourceTabControlsViewModel();
                        additionalResourceTabControlsViewModel.ResourceCollection = resourceCollection;
                        var additionalResourceTabControls = additionalResourceTabControlsViewModel.View;
                        parentGrid.Children.Add(additionalResourceTabControls);
                    }
                }
            }
        }

        #endregion
    }
}
