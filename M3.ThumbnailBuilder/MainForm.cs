using System;
using System.IO;
using System.Windows.Forms;
using M3.Helpers;

namespace M3.ThumbnailBuilder
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            var sourceFolder = new DirectoryInfo(SourceFolderTextBox.Text);
            var targetFolder = new DirectoryInfo(TargetFolderTextBox.Text);
            if (!targetFolder.Exists)
            {
                targetFolder.Create();
            }
            var files = sourceFolder.GetFiles("*.jpg");

            foreach (var file in files)
            {
                ImageHelper.GetThumbnail(100, 100, file.FullName, Path.Combine(TargetFolderTextBox.Text, file.Name));
            }
        }
    }
}
