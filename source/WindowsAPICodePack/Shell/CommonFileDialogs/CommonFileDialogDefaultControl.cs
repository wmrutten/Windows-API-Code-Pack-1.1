using System;
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
        internal CommonFileDialogDefaultControl(int id) : base(id) { }

        /// <summary>
        /// Gets or sets a value that determines if this control is enabled.
        /// </summary>
        /// <exception cref="InvalidOperationException">The control is not available.</exception>
        public bool Enabled
        {
            get { return DialogNativeMethods.IsWindowEnabled(this.SafeGetHandle()) != 0; }
            set { DialogNativeMethods.EnableWindow(this.SafeGetHandle(), value ? 1 : 0); }
        }

        /// <summary>
        /// Gets or sets a boolean value that indicates whether  
        /// this control is visible.
        /// </summary>
        /// <exception cref="InvalidOperationException">The control is not available.</exception>
        public bool Visible
        {
            get { return DialogNativeMethods.IsWindowVisible(this.SafeGetHandle()) != 0; }
            set { DialogNativeMethods.ShowWindow(this.SafeGetHandle(), value ? DialogNativeMethods.ShowWindowCommands.SW_SHOW : DialogNativeMethods.ShowWindowCommands.SW_HIDE); }
        }

        /// <summary>
        /// Gets or sets the text string that is displayed on the control.
        /// </summary>
        /// <exception cref="InvalidOperationException">The control is not available.</exception>
        public string Text
        {
            get
            {
                var hWnd = this.SafeGetHandle();
                var l = DialogNativeMethods.GetWindowTextLength(hWnd);
                var sb = new StringBuilder("", l + 5);
                l = DialogNativeMethods.GetWindowText(hWnd, sb, l + 2);
                return sb.ToString();
            }
            set
            {
                DialogNativeMethods.SetWindowText(this.SafeGetHandle(), value);
            }
        }
    }
}
