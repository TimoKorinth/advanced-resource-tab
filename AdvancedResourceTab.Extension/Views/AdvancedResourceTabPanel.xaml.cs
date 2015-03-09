using System;
using System.Linq;

namespace AdvancedResourceTab.Extension.Views
{
    using System.Windows.Controls;

    /// <summary>
    ///     Interaction logic for AdvancedResourceTabPanel.xaml
    /// </summary>
    public partial class AdvancedResourceTabPanel
    {
        #region Constructors and Destructors

        public AdvancedResourceTabPanel()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Methods

        internal void FocusTextBox()
        {
            var result = VisualHelper.FindChild<TextBox>(this.searchTextBox);
            if (result != null)
            {
                result.Focus();
            }
        }

        #endregion
    }
}