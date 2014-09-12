using System;
using System.IO;
using System.Windows.Forms;
using M3.Configurations;
using M3.Helpers;

namespace M3.ThumbnailBuilder
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            SourceFolderTextBox.Text = ConfigurationManager.ThumbnailBuilderConfiguration.SourceFolderPath;
            TargetFolderTextBox.Text = ConfigurationManager.ThumbnailBuilderConfiguration.TargetFolderPath;
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            var sourceFolder = new DirectoryInfo(SourceFolderTextBox.Text);

            var fileCount = 0;
            var startTime = DateTime.Now;
            var year = 0;
            var folders = sourceFolder.GetDirectories();
            foreach (var folder in folders)
            {
                var files = folder.GetFiles("*.jpg", SearchOption.AllDirectories);
                if (files.Length > 0)
                {
                    try
                    {
                        year = files[0].LastWriteTime.Year;
                    }
                    catch { }
                    foreach (var file in files)
                    {
                        var newPath = year + file.FullName.Substring(sourceFolder.FullName.Length, file.FullName.Length - sourceFolder.FullName.Length);
                        ImageHelper.GetThumbnail(100, 100, file.FullName, Path.Combine(TargetFolderTextBox.Text, newPath));
                        fileCount++;
                    }
                }
            }

            MessageBox.Show(string.Format("Done!\n共转换{0}个文件，耗时{1}秒", fileCount, (DateTime.Now - startTime).TotalSeconds), "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
