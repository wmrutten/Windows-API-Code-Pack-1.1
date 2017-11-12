﻿using System;

namespace Microsoft.WindowsAPICodePack.Dialogs
{
    // Dlgs.h
    // https://msdn.microsoft.com/en-us/library/windows/desktop/ms646960(v=vs.85).aspx

    internal enum DialogDefaultControlIds
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
