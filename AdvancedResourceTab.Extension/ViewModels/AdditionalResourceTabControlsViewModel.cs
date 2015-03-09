namespace AdvancedResourceTab.Extension.ViewModels
{
    using System;
    using System.Linq;
    using System.ComponentModel;

    using AdvancedResourceTab.Extension.Views;

    using Microsoft.Expression.DesignSurface.UserInterface.ResourcePane;
    using Microsoft.Expression.Utility.Data;

    public class AdditionalResourceTabControlsViewModel : INotifyPropertyChanged
    {
        #region Fields

        private DelegateCommand collapseAllResourceDictionariesCommand;

        private DelegateCommand expandAllResourceDictionariesCommand;

        private bool isSortAscending = true;

        private ICollectionView resourceCollection;

        private AdditionalResourceTabControls view;

        #endregion

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Public Properties

        public DelegateCommand CollapseAllResourceDictionariesCommand
        {
            get
            {
                if (this.collapseAllResourceDictionariesCommand == null)
                {
                    this.collapseAllResourceDictionariesCommand = new DelegateCommand(this.OnCollapseAllResourceDictionariesCommandExecute);
                }
                return this.collapseAllResourceDictionariesCommand;
            }
            set
            {
                this.collapseAllResourceDictionariesCommand = value;
                this.RaisePropertyChanged("CollapseAllResourceDictionariesCommand");
            }
        }

        public DelegateCommand ExpandAllResourceDictionariesCommand
        {
            get
            {
                if (this.expandAllResourceDictionariesCommand == null)
                {
                    this.expandAllResourceDictionariesCommand = new DelegateCommand(this.OnExpandAllResourceDictionariesCommandExecute);
                }
                return this.expandAllResourceDictionariesCommand;
            }
            set
            {
                this.expandAllResourceDictionariesCommand = value;
                this.RaisePropertyChanged("ExpandAllResourceDictionariesCommand");
            }
        }

        public bool IsSortAscending
        {
            get
            {
                return this.isSortAscending;
            }
            set
            {
                this.isSortAscending = value;
                this.RaisePropertyChanged("IsSortAscending");

                this.UpdateSorting();
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

                this.UpdateSorting();
            }
        }

        public AdditionalResourceTabControls View
        {
            get
            {
                if (this.view == null)
                {
                    this.view = new AdditionalResourceTabControls();
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

        private void OnCollapseAllResourceDictionariesCommandExecute()
        {
            foreach (object item in this.resourceCollection)
            {
                var resourceContainer = item as ResourceContainer;
                if (resourceContainer != null)
                {
                    resourceContainer.IsExpanded = false;
                }
            }
        }

        private void OnExpandAllResourceDictionariesCommandExecute()
        {
            foreach (object item in this.resourceCollection)
            {
                var resourceContainer = item as ResourceContainer;
                if (resourceContainer != null)
                {
                    resourceContainer.IsExpanded = true;
                }
            }
        }

        private void RaisePropertyChanged(string property)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        private void UpdateSorting()
        {
            if (this.resourceCollection == null)
            {
                return;
            }

            this.resourceCollection.SortDescriptions.Clear();
            this.resourceCollection.SortDescriptions.Add(new SortDescription("Name", this.IsSortAscending ? ListSortDirection.Ascending : ListSortDirection.Descending));
        }

        #endregion
    }
}