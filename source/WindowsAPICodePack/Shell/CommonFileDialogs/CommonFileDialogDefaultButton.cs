using Microsoft.WindowsAPICodePack.Shell.Resources;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;

namespace Microsoft.WindowsAPICodePack.Dialogs.Controls
{
    /// <summary>
    /// Exposes properties of default Ok/Select/Save button in the common file dialog controls.
    /// </summary>
    public class CommonFileDialogDefaultButton : CommonFileDialogDefaultControl, ICommonFileDialogIndexedControls
    {
        // Use fixed identifier for default button drop down menu
        internal const int DropDownId = 0x4ff;

        private IFileDialogCustomize customizedDialog;

        private readonly Collection<CommonFileDialogDropDownItem> items = new Collection<CommonFileDialogDropDownItem>();
        /// <summary>
        /// Gets the collection of CommonFileDialogDropDownItem objects.
        /// </summary>
        public Collection<CommonFileDialogDropDownItem> Items
        {
            get { return items; }
        }

        /// <summary>
        /// Creates a new instance of this class.
        /// </summary>
        internal CommonFileDialogDefaultButton() : base(DialogDefaultControlIds.Ok) { }


        #region ICommonFileDialogIndexedControls Members

        private int selectedIndex = -1;
        /// <summary>
        /// Gets or sets the current index of the selected item.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes")]
        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                // Don't update property if it hasn't changed
                if (selectedIndex == value)
                    return;

                if (HostingDialog == null)
                {
                    selectedIndex = value;
                    return;
                }

                // Only update this property if it has a valid value
                if (value >= 0 && value < items.Count)
                {
                    selectedIndex = value;
                    // Cannot set the currently selected item
                    // IFileDialogCustomize.SetSelectedControlItem is not valid for DropDown
                    ApplyPropertyChange("SelectedIndex");
                }
                else
                {
                    throw new IndexOutOfRangeException(LocalizedMessages.DefaultButtonIndexOutsideBounds);
                }
            }
        }

        /// <summary>
        /// Occurs when the SelectedIndex is changed.
        /// </summary>
        /// 
        /// <remarks>
        /// By initializing the SelectedIndexChanged event with an empty
        /// delegate, it is not necessary to check  
        /// if the SelectedIndexChanged is not null.
        /// 
        /// </remarks>
        public event EventHandler SelectedIndexChanged = delegate { };

        /// <summary>
        /// Raises the SelectedIndexChanged event if this control is 
        /// enabled.
        /// </summary>
        /// <remarks>Because this method is defined in an interface, we can either
        /// have it as public, or make it private and explicitly implement (like below).
        /// Making it public doesn't really help as its only internal (but can't have this 
        /// internal because of the interface)
        /// </remarks>
        void ICommonFileDialogIndexedControls.RaiseSelectedIndexChangedEvent()
        {
            // Make sure that this control is enabled and has a specified delegate
            if (Enabled)
                SelectedIndexChanged(this, EventArgs.Empty);
        }

        #endregion

        /// <summary>
        /// Initialize the default control of the dialog object
        /// </summary>
        /// <param name="dialog">Target dialog</param>
        internal override void Attach(IFileDialogCustomize dialog)
        {
            Debug.Assert(dialog != null, "CommonFileDialogOkButton.Attach: dialog parameter can not be null");

            customizedDialog = dialog;

            var items = this.Items;
            if (items.Count > 0)
            {
                dialog.EnableOpenDropDown(DropDownId);

                // Add the combo box items
                for (int index = 0; index < items.Count; index++)
                    dialog.AddControlItem(DropDownId, index, items[index].Text);

                // Cannot initialize the currently selected item
                // IFileDialogCustomize.SetSelectedControlItem is not valid for DropDown
            }

            base.Attach(dialog);
        }

        internal void SyncValue()
        {
            if (customizedDialog != null)
            {
                int index;
                customizedDialog.GetSelectedControlItem(DropDownId, out index);
                SelectedIndex = index;
            }
        }

    }

    /// <summary>
    /// Creates a ComboBoxItem for the Common File Dialog.
    /// </summary>
    public class CommonFileDialogDropDownItem
    {
        private string text = string.Empty;
        /// <summary>
        /// Gets or sets the string that is displayed for this item.
        /// </summary>
        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        /// <summary>
        /// Creates a new instance of this class.
        /// </summary>
        public CommonFileDialogDropDownItem()
        {
        }

        /// <summary>
        /// Creates a new instance of this class with the specified text.
        /// </summary>
        /// <param name="text">The text to use for the combo box item.</param>
        public CommonFileDialogDropDownItem(string text)
        {
            this.text = text;
        }
    }

}
