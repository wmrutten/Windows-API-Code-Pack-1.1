using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Microsoft.WindowsAPICodePack.Dialogs
{
    internal static class DialogNativeMethods
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetDlgItem(IntPtr hwnd, int childID);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern int IsWindowEnabled(IntPtr hwnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern int EnableWindow(IntPtr hwnd, int bEnable);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern int IsWindowVisible(IntPtr hwnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern int ShowWindow(IntPtr hWnd, ShowWindowCommands nCmdShow);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SetWindowText(IntPtr hwnd, String lpString);

        public enum ShowWindowCommands
        {
            /// <summary>
            /// Hides the window and activates another window.
            /// </summary>
            SW_HIDE = 0,
            /// <summary>
            /// Activates and displays a window. If the window is minimized or maximized, the system restores it to its original size and position. An application should specify this flag when displaying the window for the first time.         
            /// </summary>
            SW_SHOWNORMAL = 1,
            /// <summary>
            /// Activates the window and displays it as a minimized window.
            /// </summary>
            SW_SHOWMINIMIZED = 2,
            /// <summary>
            /// Activates the window and displays it as a maximized window.
            /// </summary>
            SW_SHOWMAXIMIZED = 3,
            /// <summary>
            /// Maximizes the specified window.
            /// </summary>
            SW_MAXIMIZE = 3,
            /// <summary>
            /// Displays a window in its most recent size and position. This value is similar to SW_SHOWNORMAL, except that the window is not activated.
            /// </summary>
            SW_SHOWNOACTIVATE = 4,
            /// <summary>
            /// Activates the window and displays it in its current size and position.
            /// </summary>
            SW_SHOW = 5,
            /// <summary>
            /// Minimizes the specified window and activates the next top-level window in the Z order.
            /// </summary>
            SW_MINIMIZE = 6,
            /// <summary>
            /// Displays the window as a minimized window. This value is similar to SW_SHOWMINIMIZED, except the window is not activated.
            /// </summary>
            SW_SHOWMINNOACTIVE = 7,
            /// <summary>
            /// Displays the window in its current size and position. This value is similar to SW_SHOW, except that the window is not activated.
            /// </summary>
            SW_SHOWNA = 8,
            /// <summary>
            /// Activates and displays the window. If the window is minimized or maximized, the system restores it to its original size and position. An application should specify this flag when restoring a minimized window.
            /// </summary>
            SW_RESTORE = 9,
            /// <summary>
            /// Sets the show state based on the SW_ value specified in the STARTUPINFO structure passed to the CreateProcess function by the program that started the application.
            /// </summary>
            SW_SHOWDEFAULT = 10,
            /// <summary>
            /// Minimizes a window, even if the thread that owns the window is not responding. This flag should only be used when minimizing windows from a different thread.
            /// </summary>
            SW_FORCEMINIMIZE = 11,
        }

        // Dialog Box Command IDs
        // cf. TaskDialogCommonButtonReturnIds
        public enum DialogCommonButtonIds
        {
            Ok = 1,
            Cancel = 2,
            Abort = 3,
            Retry = 4,
            Ignore = 5,
            Yes = 6,
            No = 7,
            Close = 8,
            // Help = 9,
            // TryAgain = 10,
            // Continue = 11,
        }

        // Dlgs.h
        // https://msdn.microsoft.com/en-us/library/windows/desktop/ms646960(v=vs.85).aspx
        public enum DialogCommonControlIds
        {
            /// <summary>(IDOK) The OK command button (push button)</summary>
            Ok = 1,
            /// <summary>(IDCANCEL) The Cancel command button (push button)</summary>
            Cancel = 2,
            /// <summary>(pshHelp = psh15) The Help command button (push button)</summary>
            Help = 0x40e,
            /// <summary>(chx1) The read-only check box</summary>
            ReadOnly = 0x410,
            /// <summary>(stc1) Label for the lst1 list box</summary>
            ListLabel = 0x440,
            /// <summary>(stc2) Label for the cmb1 combo box</summary>
            FileTypeLabel = 0x441,  // "Files of type"
            /// <summary>(stc3) Label for the edt1 edit control</summary>
            FileLabel = 0x442,      // "File Name"
            /// <summary>(stc4) Label for the cmb2 combo box</summary>
            FolderLabel = 0x443,    // "Look In"
            /// <summary>(lst1) List box that displays the contents of the current drive or folder</summary>
            List = 0x461,
            /// <summary>(cmb1) Drop-down combo box that displays the list of file type filters</summary>
            FormatCombo = 0x470,
            /// <summary>(cmb2) Drop-down combo box that displays the current drive or folder, and that allows the user to select a drive or folder to open</summary>
            FolderCombo = 0x471,
            /// <summary>(edt1) Edit control that displays the name of the current file, or in which the user can type the name of the file to open</summary>
            FileText = 0x480,
        }
    }
}
