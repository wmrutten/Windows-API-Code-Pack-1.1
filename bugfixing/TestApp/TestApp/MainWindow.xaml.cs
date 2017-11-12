﻿using System.Windows;
using System.Windows.Interop;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.IO;
using Microsoft.WindowsAPICodePack.Shell;
using System.Runtime.InteropServices;
using Microsoft.WindowsAPICodePack.Dialogs.Controls;
using System.Diagnostics;

namespace TestApp
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonTaskDialogIconFix_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new TaskDialog
            {
                Caption = "Caption",
                DetailsCollapsedLabel = "DetailsCollapsedLabel",
                DetailsExpanded = true,
                DetailsExpandedLabel = "DetailsExpandedLabel",
                DetailsExpandedText = "DetailsExpandedText",
                ExpansionMode = TaskDialogExpandedDetailsLocation.ExpandContent,
                FooterCheckBoxChecked = true,
                FooterCheckBoxText = "FooterCheckBoxText",
                FooterIcon = TaskDialogStandardIcon.Information,
                FooterText = "FooterText",
                HyperlinksEnabled = true,
                Icon = TaskDialogStandardIcon.Shield,
                InstructionText = "InstructionText",
                ProgressBar = new TaskDialogProgressBar {Value = 100},
                StandardButtons =
                    TaskDialogStandardButtons.Ok | TaskDialogStandardButtons.Yes | TaskDialogStandardButtons.No |
                    TaskDialogStandardButtons.Cancel | TaskDialogStandardButtons.Close | TaskDialogStandardButtons.Retry,
                DefaultButton = TaskDialogDefaultButton.Retry,
                StartupLocation = TaskDialogStartupLocation.CenterScreen,
                Text = "Text"
            })
            {
                dialog.Show();
            }
        }

        private void ButtonTaskDialogCustomButtonClose_Click(object sender, RoutedEventArgs e)
        {
            var helper = new WindowInteropHelper(this);
            var td = new TaskDialog {OwnerWindowHandle = helper.Handle};

            var closeLink = new TaskDialogCommandLink("close", "Close", "Closes the task dialog");
            closeLink.Click += (o, ev) => td.Close(TaskDialogResult.CustomButtonClicked);
            var closeButton = new TaskDialogButton("closeButton", "Close");
            closeButton.Click += (o, ev) => td.Close(TaskDialogResult.CustomButtonClicked);

            // Enable one or the other; can't have both at the same time
            td.Controls.Add(closeLink);
            //td.Controls.Add(closeButton);

            // needed since none of the buttons currently closes the TaskDialog
            td.Cancelable = true;

            switch (td.Show())
            {
                case TaskDialogResult.CustomButtonClicked:
                    MessageBox.Show("The task dialog was closed by a custom button");
                    break;
                case TaskDialogResult.Cancel:
                    MessageBox.Show("The task dialog was canceled");
                    break;
                default:
                    MessageBox.Show("The task dialog was closed by other means");
                    break;
            }
        }

        const string ID_GROUP = "ID_GROUP";
        const string ID_LABEL = "ID_LABEL";
        const string ID_CHECK = "ID_CHECK";

        private void ButtonBrowseFolder_Click(object sender, RoutedEventArgs e)
        {
            using (var dlg = new CommonOpenFileDialog()
            {
                IsFolderPicker = true,
                EnsureFileExists = true,
                InitialDirectoryShellContainer =
                    // Is there an easier way ???
                    ShellContainer.FromParsingName(KnownFolders.Documents.ParsingName) as ShellFolder
            })
            {
                var grp = new CommonFileDialogGroupBox(ID_GROUP, "");
                grp.Text = "#" + grp.Id + " Custom options";

                var chk = new CommonFileDialogCheckBox(ID_CHECK, "");
                chk.Text = "#" + chk.Id + ": Include subfolders";
                // dlg.Controls.Add(chk);
                grp.Items.Add(chk);

                var lbl = new CommonFileDialogLabel(ID_LABEL, "");
                lbl.Text = "#" + lbl.Id + ": Selection is valid?";
                // dlg.Controls.Add(lbl);
                grp.Items.Add(lbl);

                dlg.Controls.Add(grp);

                var defButton = dlg.DefaultButton;
                // defButton.Text = "Ok";
                var item1 = new CommonFileDialogDropDownItem("Select");
                var item2 = new CommonFileDialogDropDownItem("Delete");
                defButton.Items.Add(item1);
                defButton.Items.Add(item2);

                // TODO expose IFileDialogCustomize.EnableOpenDropDown
                // https://social.msdn.microsoft.com/Forums/windowsdesktop/en-US/5890ff0d-8432-42bf-a52b-1b772d873d40/ifiledialogcustomize-edit-box-label-and-preferred-length?forum=windowsuidevelopment
                //   There is no FILEOPENDIALOGOPTIONS equivalent to OPENFILENAME.OFN_READONLY or OFN_HIDEREADONLY.
                //   IFileDialogCustomize::EnableOpenDropDown must be called.
                //   "Open" and "Open Read-Only" control items must be added using AddControlItem.
                //   Then GetSelectedControlItem must be called to get the choice made after the return from Show(). 
                //   IFileDialogCustomize does not support a ListBox.  Even if it did, its width would not be controllable.

                dlg.DialogOpening += BrowseFolderDialog_DialogOpening;
                dlg.FolderChanging += BrowseFolderDialog_FolderChanging;
                dlg.SelectionChanged += BrowseFolderDialog_SelectionChanged;
                dlg.FileOk += BrowseFolderDialog_FileOk;
                if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    MessageBox.Show("You selected: " + dlg.FileName + "\r\nButton index: " + defButton.SelectedIndex);
                }
                dlg.DialogOpening -= BrowseFolderDialog_DialogOpening;
                dlg.FolderChanging -= BrowseFolderDialog_FolderChanging;
                dlg.SelectionChanged -= BrowseFolderDialog_SelectionChanged;
                dlg.FileOk -= BrowseFolderDialog_FileOk;
            };
        }

        static bool IsEqualOrSubfolderOf(string path, string other)
        {
            if (string.IsNullOrEmpty(path) || string.IsNullOrEmpty(other)) { return false; }
            var l1 = path.Length;
            var l2 = other.Length;
            if (l2 == l1)
            {
                // return StringComparer.OrdinalIgnoreCase.Equals(l1, l2);
                return String.Compare(path, 0, other, 0, l1, StringComparison.OrdinalIgnoreCase) == 0;
            }
            else if (l2 > l1)
            {
                return String.Compare(path, 0, other, 0, l1, StringComparison.OrdinalIgnoreCase) == 0
                    && other[l1] == Path.DirectorySeparatorChar;
            }
            return false;
        }

        void toggleSelectButton(CommonOpenFileDialog dlg, bool enabled)
        {
            if (dlg != null)
            {
                var btn = dlg.DefaultButton;
                btn.Enabled = enabled;
            }
        }

        private void BrowseFolderDialog_DialogOpening(object sender, EventArgs e)
        {
            var dlg = (CommonOpenFileDialog)sender;
            // ...
        }

        private void BrowseFolderDialog_FolderChanging(object sender, CommonFileDialogFolderChangeEventArgs e)
        {
            e.Cancel = !IsEqualOrSubfolderOf(KnownFolders.Documents.Path, e.Folder);
            if (e.Cancel)
            {
                FolderSelection.Text = "NOT ALLOWED!";
            }

            var dlg = (CommonOpenFileDialog)sender;
            var lbl = dlg.Controls[ID_LABEL] as CommonFileDialogLabel;
            if (lbl != null)
            {
                lbl.Text = e.Cancel ? "Error! Please select (a subfolder of) your personal My Documents folder." : "";
                toggleSelectButton(dlg, !e.Cancel);
            }

        }

        private void BrowseFolderDialog_SelectionChanged(object sender, EventArgs e)
        {
            var dlg = (CommonOpenFileDialog)sender;
            FolderSelection.Text = dlg?.SelectedFileName
                + Environment.NewLine
                + string.Join("|", dlg.SelectedFileNames);
        }

        // TODO: verify x32/x64 ...

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        static extern IntPtr GetDlgItem(IntPtr hwnd, int childID);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        static extern int EnableWindow(IntPtr hwnd, int bEnable);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        static extern int IsWindowEnabled(IntPtr hwnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        static extern int ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        static extern IntPtr GetWindow(IntPtr hWnd, int cmd);

        // The retrieved handle identifies the child window at the top of the Z order, if the specified window is a parent window; otherwise, the retrieved handle is NULL.The function examines only child windows of the specified window.It does not examine descendant windows.
        const int GW_CHILD = 5;

        // The retrieved handle identifies the enabled popup window owned by the specified window (the search uses the first such window found using GW_HWNDNEXT); otherwise, if there are no enabled popup windows, the retrieved handle is that of the specified window.
        const int GW_ENABLEDPOPUP = 6;

        // The retrieved handle identifies the window of the same type that is highest in the Z order.
        // If the specified window is a topmost window, the handle identifies a topmost window.If the specified window is a top-level window, the handle identifies a top-level window. If the specified window is a child window, the handle identifies a sibling window.
        const int GW_HWNDFIRST = 0;

        // The retrieved handle identifies the window of the same type that is lowest in the Z order.
        // If the specified window is a topmost window, the handle identifies a topmost window. If the specified window is a top-level window, the handle identifies a top-level window. If the specified window is a child window, the handle identifies a sibling window.
        const int GW_HWNDLAST = 1;

        // The retrieved handle identifies the window below the specified window in the Z order.
        const int GW_HWNDNEXT = 2;

        // If the specified window is a topmost window, the handle identifies a topmost window. If the specified window is a top-level window, the handle identifies a top-level window. If the specified window is a child window, the handle identifies a sibling window.
        // If the specified window is a topmost window, the handle identifies a topmost window. If the specified window is a top-level window, the handle identifies a top-level window. If the specified window is a child window, the handle identifies a sibling window.
        const int GW_HWNDPREV = 3;

        // The retrieved handle identifies the specified window's owner window, if any. For more information, see Owned Windows.
        const int GW_OWNER = 4;

        private void BrowseFolderDialog_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var dlg = (CommonOpenFileDialog)sender;
            e.Cancel = !IsEqualOrSubfolderOf(KnownFolders.Documents.Path, dlg.SelectedFileName);
            if (e.Cancel)
            {
                FolderSelection.Text = "NOT ALLOWED!";
            }
        }

    }
}