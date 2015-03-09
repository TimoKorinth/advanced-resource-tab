namespace AdvancedResourceTab.Extension.ViewModels
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Threading;

    using AdvancedResourceTab.Extension.Views;

    using Microsoft.Expression.DesignSurface.UserInterface.ResourcePane;
    using Microsoft.Expression.Framework.UserInterface;

    public class AdvancedResourceTabPanelViewModel : INotifyPropertyChanged
    {
        #region Fields

        private DebounceManager debounceManager;

        private bool isSearchIncludingSubitems;

        private ICollectionView resourceCollection;

        private string searchText;

        private AdvancedResourceTabPanel view;

        #endregion

        #region Constructors and Destructors

        public AdvancedResourceTabPanelViewModel(IWindowService windowService)
        {
            PaletteRegistryEntry resourcePane = windowService.PaletteRegistry["Designer_ResourcePane"];
            if (resourcePane != null && resourcePane.Content != null)
            {
                resourcePane.Content.Loaded += this.ResourcePaneLoaded;
            }
            this.isSearchIncludingSubitems = true;

            this.debounceManager = new DebounceManager(TimeSpan.FromSeconds(0.2), Dispatcher.CurrentDispatcher);
            this.debounceManager.StartExecution += (sender, args) => this.UpdateFilter();
        }

        #endregion

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Public Properties

        public bool IsSearchIncludingSubitems
        {
            get
            {
                return this.isSearchIncludingSubitems;
            }
            set
            {
                this.isSearchIncludingSubitems = value;
                this.RaisePropertyChanged("IsSearchIncludingSubitems");

                this.UpdateFilter();
            }
        }

        public ICollectionView ResourceCollection
        {
            get
            {
                return this.resourceCollection;
            }
            set
            {
                this.resourceCollection = value;
                this.RaisePropertyChanged("ResourceCollection");

                this.UpdateFilter();
            }
        }

        public string SearchText
        {
            get
            {
                return this.searchText;
            }
            set
            {
                this.searchText = value;
                this.RaisePropertyChanged("SearchText");

                this.debounceManager.RequestEventExecution();
            }
        }

        public AdvancedResourceTabPanel View
        {
            get
            {
                if (this.view == null)
                {
                    this.view = new AdvancedResourceTabPanel();
                    this.view.DataContext = this;
                }
                return this.view;
            }
            set
            {
                this.view = value;
                this.RaisePropertyChanged("View");
            }
        }

        #endregion

        #region Methods

        private bool FilterResources(object item)
        {
            var resourceContainer = item as ResourceContainer;
            if (resourceContainer != null && resourceContainer.Name != null && this.SearchText != null)
            {
                if (resourceContainer.Name.ToLower().Contains(this.SearchText.ToLower()))
                {
                    return true;
                }
                if (!this.IsSearchIncludingSubitems)
                {
                    return false;
                }

                var resourceItems = from r in resourceContainer.ResourceItems
                    let resourceEntryItem = (r as ResourceEntryItem)
                    where resourceEntryItem != null && resourceEntryItem.Key.ToLower().Contains(this.SearchText.ToLower())
                    select resourceEntryItem;

                return resourceItems.Any();
            }

            return true;
        }

        private void RaisePropertyChanged(string property)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        private void ResourcePaneLoaded(object sender, RoutedEventArgs e)
        {
            var itemsControl = VisualHelper.FindChild<ItemsControl>(sender as DependencyObject, "ResourceItemSelector");
            if (itemsControl != null)
            {
                this.ResourceCollection = CollectionViewSource.GetDefaultView(itemsControl.ItemsSource);
            }
        }

        private void UpdateFilter()
        {
            this.resourceCollection.Filter = this.FilterResources;
        }

        #endregion
    }
}