namespace AdvancedResourceTab.Extension
{
    using System.Diagnostics;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;

    using AdvancedResourceTab.Extension.ViewModels;

    using Microsoft.Expression.Framework;
    using Microsoft.Expression.Framework.UserInterface;
    using Microsoft.Expression.Utility.Controls;
    using Microsoft.Expression.Utility.Extensions;

    public class AdvancedResourceTabExtension : IPackage
    {
        #region Fields

        private ItemsControl itemsControl;

        private AdvancedResourceTabPanelViewModel mainViewModel;

        private bool isControlPressed;

        #endregion

        #region Public Methods and Operators

        public void Load(IServices services)
        {
            var windowService = services.GetService<IWindowService>();
            windowService.MainWindow.KeyUp += this.MainWindowKeyUp;
            windowService.MainWindow.KeyDown += this.MainWindowKeyDown;
            this.mainViewModel = new AdvancedResourceTabPanelViewModel(windowService);
            windowService.RegisterPalette("AdvancedResourceTab", this.mainViewModel.View, "Advanced Resource Tab");

            var resourcePane = windowService.PaletteRegistry["Designer_ResourcePane"];
            if (resourcePane != null && resourcePane.Content != null)
            {
                resourcePane.Content.Loaded += this.ResourcePaneLoaded;
            }
            else
            {
                Debug.WriteLine("\t ERROR");
            }
        }

        public void Unload()
        {
        }

        #endregion

        #region Private Methods

        private void MainWindowKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl)
            {
                isControlPressed = true;
            }
        }

        private void MainWindowKeyUp(object sender, KeyEventArgs e)
        {
            if (this.isControlPressed && e.Key == Key.Oem1)
            {
                this.mainViewModel.View.FocusTextBox();
            }
            this.isControlPressed = false;
        }

        private void ResourcePaneLoaded(object sender, RoutedEventArgs e)
        {
            if (this.itemsControl != null)
            {
                return;
            }

            this.itemsControl = VisualHelper.FindChild<ItemsControl>(sender as DependencyObject, "ResourceItemSelector");
            if (this.itemsControl != null)
            {
                var resourceCollection = CollectionViewSource.GetDefaultView(this.itemsControl.ItemsSource);
                var radioButtonGroup = VisualHelper.FindChild<RadioButtonGroup>(sender as DependencyObject);
                if (radioButtonGroup != null)
                {
                    var parentGrid = radioButtonGroup.Parent as Grid;
                    if (parentGrid != null)
                    {
                        var additionalResourceTabControlsViewModel = new AdditionalResourceTabControlsViewModel { ResourceCollection = resourceCollection };

                        var additionalResourceTabControls = additionalResourceTabControlsViewModel.View;
                        parentGrid.Children.Add(additionalResourceTabControls);

                    }
                }
            }
        }

        #endregion
    }
}