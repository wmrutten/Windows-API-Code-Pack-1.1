using System;
using System.Text;

namespace Microsoft.WindowsAPICodePack.Dialogs.Controls
{
    /// <summary>
    /// Exposes properties of default controls in the common file dialog controls.
    /// </summary>
    public class CommonFileDialogDefaultControl : DialogControl
    {
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
            get { return DialogNativeMethods.IsWindowEnabled(SafeGetHandle()) != 0; }
            set { DialogNativeMethods.EnableWindow(SafeGetHandle(), value ? 1 : 0); }
        }

        /// <summary>
        /// Gets or sets a boolean value that indicates whether  
        /// this control is visible.
        /// </summary>
        /// <exception cref="InvalidOperationException">The control is not available.</exception>
        public bool Visible
        {
            get { return DialogNativeMethods.IsWindowVisible(SafeGetHandle()) != 0; }
            set { DialogNativeMethods.ShowWindow(SafeGetHandle(), value ? DialogNativeMethods.ShowWindowCommands.SW_SHOW : DialogNativeMethods.ShowWindowCommands.SW_HIDE); }
        }

        /// <summary>
        /// Gets or sets the text string that is displayed on the control.
        /// </summary>
        /// <exception cref="InvalidOperationException">The control is not available.</exception>
        public string Text
        {
            get
            {
                var hWnd = SafeGetHandle();
                var l = DialogNativeMethods.GetWindowTextLength(hWnd);
                var sb = new StringBuilder("", l + 5);
                l = DialogNativeMethods.GetWindowText(hWnd, sb, l + 2);
                return sb.ToString();
            }
            set
            {
                DialogNativeMethods.SetWindowText(SafeGetHandle(), value);
            }
        }

        private IntPtr SafeGetHandle()
        {
            var hWnd = GetHandle();
            if (hWnd == IntPtr.Zero) { throw new InvalidOperationException(); }
            return hWnd;
        }

        private IntPtr GetHandle()
        {
            // HACK
            var dlg = HostingDialog as CommonFileDialog;
            if (dlg != null)
            {
                var hDlg = dlg.GetHandle();
                if (hDlg != null)
                {
                    return DialogNativeMethods.GetDlgItem(hDlg, Id);
                }
            }
            return IntPtr.Zero;
        }

    }
}
