using System.Windows;
using System.Windows.Interop;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.IO;
using Microsoft.WindowsAPICodePack.Shell;
using System.Runtime.InteropServices;

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

        private void ButtonBrowseFolder_Click(object sender, RoutedEventArgs e)
        {
            using (var dlg = new CommonOpenFileDialog()
            {
                IsFolderPicker = true,
                EnsureFileExists = true
            })
            {
                dlg.FolderChanging += BrowseFolderDialog_FolderChanging;
                dlg.SelectionChanged += BrowseFolderDialog_SelectionChanged;
                dlg.FileOk += BrowseFolderDialog_FileOk;
                if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    MessageBox.Show("You selected: " + dlg.FileName);
                }
                dlg.SelectionChanged -= BrowseFolderDialog_SelectionChanged;
                dlg.FolderChanging -= BrowseFolderDialog_FolderChanging;
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

        private void BrowseFolderDialog_FolderChanging(object sender, CommonFileDialogFolderChangeEventArgs e)
        {
            e.Cancel = IsEqualOrSubfolderOf(KnownFolders.Documents.Path, e.Folder);
            if (e.Cancel)
            {
                FolderSelection.Text = "NOT ALLOWED!";
            }
        }

        private void BrowseFolderDialog_SelectionChanged(object sender, EventArgs e)
        {
            var dlg = (CommonOpenFileDialog)sender;
            FolderSelection.Text = dlg?.SelectedFileName
                + Environment.NewLine
                + string.Join("|", dlg.SelectedFileNames);
        }

        private void BrowseFolderDialog_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var dlg = (CommonOpenFileDialog)sender;
            e.Cancel = IsEqualOrSubfolderOf(KnownFolders.Documents.Path, dlg.SelectedFileName);
            if (e.Cancel)
            {
                FolderSelection.Text = "NOT ALLOWED!";
            }
        }


    }
}