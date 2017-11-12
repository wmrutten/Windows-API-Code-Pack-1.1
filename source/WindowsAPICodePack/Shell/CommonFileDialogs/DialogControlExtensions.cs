using System;

namespace Microsoft.WindowsAPICodePack.Dialogs.Controls
{
    /// <summary>
    /// Extension methods for <see cref="DialogControl"/>.
    /// </summary>
    public static class DialogControlExtensions
    {
        /// <summary>
        /// Returns the window handle of the specified dialog control, if it exists, or <see cref="IntPtr.Zero"/> otherwise.
        /// </summary>
        /// <exception cref="ArgumentNullException" />
        public static IntPtr GetHandle(this DialogControl control)
        {
            if (control == null) { throw new ArgumentNullException(nameof(control)); }
            var id = control.Id;
            if (id != 0)
            {
                // HACK
                var dlg = control.HostingDialog as CommonFileDialog;
                if (dlg != null)
                {
                    var hDlg = dlg.Handle;
                    if (hDlg != null)
                    {
                        return DialogNativeMethods.GetDlgItem(hDlg, id);
                    }
                }
            }
            return IntPtr.Zero;
        }

    }
}
