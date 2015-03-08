using AdvancedResourceTab.Extension.Views;
using Microsoft.Expression.DesignSurface.UserInterface.ResourcePane;
using Microsoft.Expression.Framework.UserInterface;
using Microsoft.Expression.Utility.Controls;
using Microsoft.Expression.Utility.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace AdvancedResourceTab.Extension.ViewModels
{
    public class AdditionalResourceTabControlsViewModel : INotifyPropertyChanged
    {
        #region Private members

        private ICollectionView _resourceCollection;
        private bool _isSortAscending = true;
        private DelegateCommand _expandAllResourceDictionariesCommand;
        private DelegateCommand _collapseAllResourceDictionariesCommand;
        private AdditionalResourceTabControls _view;

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Properties

        public ICollectionView ResourceCollection
        {
            get { return _resourceCollection; }
            set
            {
                _resourceCollection = value;
                RaisePropertyChanged("ResourceCollection");

                UpdateSorting();
            }
        }

        public DelegateCommand ExpandAllResourceDictionariesCommand
        {
            get 
            {
                if (_expandAllResourceDictionariesCommand == null)
                {
                    _expandAllResourceDictionariesCommand = new DelegateCommand(OnExpandAllResourceDictionariesCommandExecute);
                }
                return _expandAllResourceDictionariesCommand; 
            }
            set
            {
                _expandAllResourceDictionariesCommand = value;
                RaisePropertyChanged("ExpandAllResourceDictionariesCommand");
            }
        }

        public DelegateCommand CollapseAllResourceDictionariesCommand
        {
            get 
            {
                if (_collapseAllResourceDictionariesCommand == null)
                {
                    _collapseAllResourceDictionariesCommand = new DelegateCommand(OnCollapseAllResourceDictionariesCommandExecute);
                }
                return _collapseAllResourceDictionariesCommand; 
            }
            set
            {
                _collapseAllResourceDictionariesCommand = value;
                RaisePropertyChanged("CollapseAllResourceDictionariesCommand");
            }
        }

        public bool IsSortAscending
        {
            get { return _isSortAscending; }
            set
            {
                _isSortAscending = value;
                RaisePropertyChanged("IsSortAscending");

                UpdateSorting();
            }
        }

        public AdditionalResourceTabControls View
        {
            get
            {
                if (_view == null)
                {
                    _view = new AdditionalResourceTabControls();
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

        private void OnExpandAllResourceDictionariesCommandExecute()
        {
            foreach (var item in _resourceCollection)
            {
                var resourceContainer = item as ResourceContainer;
                if (resourceContainer != null)
                {
                    resourceContainer.IsExpanded = true;
                }
            }
        }

        private void OnCollapseAllResourceDictionariesCommandExecute()
        {
            foreach (var item in _resourceCollection)
            {
                var resourceContainer = item as ResourceContainer;
                if (resourceContainer != null)
                {
                    resourceContainer.IsExpanded = false;
                }
            }
        }

        private void UpdateSorting()
        {
            if (_resourceCollection == null)
            {
                return;
            }

            _resourceCollection.SortDescriptions.Clear();
            _resourceCollection.SortDescriptions.Add(new SortDescription("Name", IsSortAscending ? ListSortDirection.Ascending : ListSortDirection.Descending));
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
