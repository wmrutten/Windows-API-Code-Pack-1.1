using System;
using System.Diagnostics;
using System.Text;

namespace Microsoft.WindowsAPICodePack.Dialogs.Controls
{
    /// <summary>
    /// Exposes properties of default controls in the common file dialog controls.
    /// </summary>
    public class CommonFileDialogDefaultControl : DialogControl
    {
        // id represents the actual OS control id, as assigned by SetWindowLong & GWL_ID
        // e.g. IDOK = 1, IDCANCEL = 2 etc. (defined in Dlgs.h)
        // For custom controls, id represents internal COM value and OS control id == 0 always
        // Also, IFileDialogCustomize does not expose custom control handles (hWnd)

        // TODO
        // Support IFileDialogCustomize.EnableOpenDropDown()
        // public Collection<CommonFileDialogOpenDropDownItem> Items
        // Implement ICommonFileDialogIndexedControls
        // Move to CommonFileDialogDefaultButton subclass ?
        // Also for CommonFileDialogButton

        /// <summary>
        /// Creates a new instance of this class for a default dialog control with the specified ID.
        /// </summary>
        internal CommonFileDialogDefaultControl(DialogDefaultControlIds id) : base((int)id) { }

        /// <summary>
        /// Holds the text that is displayed for this control.
        /// </summary>
        private string textValue;

        /// <summary>
        /// Gets or sets the text string that is displayed on the control.
        /// </summary>
        public virtual string Text
        {
            get { return textValue; }
            set
            {
                // Don't update this property if it hasn't changed
                if (value != Text)
                {
                    textValue = value;
                    ApplyPropertyChange("Text");
                }
            }
        }

        private bool enabled = true;
        /// <summary>
        /// Gets or sets a value that determines if this control is enabled.  
        /// </summary>
        public bool Enabled
        {
            get { return enabled; }
            set
            {
                // Don't update this property if it hasn't changed
                if (value == Enabled) { return; }

                enabled = value;
                ApplyPropertyChange("Enabled");
            }
        }

        private bool visible = true;
        /// <summary>
        /// Gets or sets a boolean value that indicates whether  
        /// this control is visible.
        /// </summary>
        public bool Visible
        {
            get { return visible; }
            set
            {
                // Don't update this property if it hasn't changed
                if (value == Visible) { return; }

                visible = value;
                ApplyPropertyChange("Visible");
            }
        }

        /// <summary>
        /// Initialize the default control of the dialog object
        /// </summary>
        /// <param name="dialog">Target dialog</param>
        internal virtual void Attach(IFileDialogCustomize dialog)
        {
            Debug.Assert(dialog != null, "CommonFileDialogDefaultControl.Attach: dialog parameter can not be null");

            // Sync additional properties
            SyncUnmanagedProperties();
        }

        internal virtual void SyncUnmanagedProperties()
        {
            ApplyPropertyChange("Enabled");
            ApplyPropertyChange("Visible");
        }

        internal void SyncText()
        {
            if (textValue == null)
            {
                var hWnd = this.GetHandle();
                if (hWnd != IntPtr.Zero)
                {
                    var l = DialogNativeMethods.GetWindowTextLength(hWnd);
                    var sb = new StringBuilder("", l + 5);
                    l = DialogNativeMethods.GetWindowText(hWnd, sb, l + 2);
                    textValue = sb.ToString();
                }
            }
            else
            {
                ApplyPropertyChange("Text");
            }
        }

    }
}
