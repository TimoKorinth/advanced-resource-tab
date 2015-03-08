using AdvancedResourceTab.Extension.Views;
using Microsoft.Expression.DesignSurface.UserInterface.ResourcePane;
using Microsoft.Expression.Framework.UserInterface;
using Microsoft.Expression.Utility.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace AdvancedResourceTab.Extension.ViewModels
{
    public class AdvancedResourceTabPanelViewModel : INotifyPropertyChanged
    {
        #region Private members

        private ICollectionView _resourceCollection;
        private string _searchText;        
        private bool _isSearchIncludingSubitems;
        private AdvancedResourceTabPanel _view;
        private IWindowService _windowService;

        #endregion

        #region Constructor

        public AdvancedResourceTabPanelViewModel(IWindowService windowService)
        {
            _windowService = windowService;

            var resourcePane = windowService.PaletteRegistry["Designer_ResourcePane"];
            if (resourcePane != null && resourcePane.Content != null)
            {
                resourcePane.Content.Loaded += ResourcePaneLoaded;
            }
        }

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Properties

        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                RaisePropertyChanged("SearchText");

                UpdateFilter();
            }
        }

        public ICollectionView ResourceCollection
        {
            get { return _resourceCollection; }
            set
            {
                _resourceCollection = value;
                RaisePropertyChanged("ResourceCollection");

                UpdateFilter();
            }
        }

        public bool IsSearchIncludingSubitems
        {
            get { return _isSearchIncludingSubitems; }
            set
            {
                _isSearchIncludingSubitems = value;
                RaisePropertyChanged("IsSearchIncludingSubitems");

                UpdateFilter();
            }
        }

        public AdvancedResourceTabPanel View
        {
            get
            {
                if (_view == null)
                {
                    _view = new AdvancedResourceTabPanel();
                    _view.DataContext = this;
                }
                return _view;
            }
            set
            {
                _view = value;
                RaisePropertyChanged("View");
            }
        }

        #endregion        

        #region Private methods

        private void ResourcePaneLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            var itemsControl = VisualHelper.FindChild<ItemsControl>(sender as DependencyObject, "ResourceItemSelector");
            if (itemsControl != null)
            {
                ResourceCollection = CollectionViewSource.GetDefaultView(itemsControl.ItemsSource);
            }
        }

        private void UpdateFilter()
        {
            _resourceCollection.Filter = FilterResources;
        }

        private bool FilterResources(object item)
        {
            var resourceContainer = item as ResourceContainer;
            if (resourceContainer != null && resourceContainer.Name != null && SearchText != null)
            {
                if (resourceContainer.Name.ToLower().Contains(SearchText.ToLower()))
                {
                    return true;
                }
                else
                {
                    if (!IsSearchIncludingSubitems)
                    {
                        return false;
                    }

                    var resourceItems = from r in resourceContainer.ResourceItems 
                                        let resourceEntryItem = (r as ResourceEntryItem)
                                        where resourceEntryItem != null &&
                                        resourceEntryItem.Key.ToLower().Contains(SearchText.ToLower())
                                        select r;
                    if (resourceItems.Count() > 0)
                    {
                        return true;
                    }

                    return false;
                }
            }

            return true;
        }

        private void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        #endregion
    }
}
