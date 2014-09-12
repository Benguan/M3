using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using M3.Configurations;
using M3.Helpers;
using M3.Models;

namespace M3.ThumbnailBuilder
{
    public partial class MainForm : Form
    {
        private string thumbnailFolder = "thumbnail";
        private string normalFolder = "normal";
        public MainForm()
        {
            InitializeComponent();
            SourceFolderTextBox.Text = ConfigurationManager.ThumbnailBuilderConfiguration.SourceFolderPath;
            TargetFolderTextBox.Text = ConfigurationManager.ThumbnailBuilderConfiguration.TargetFolderPath;
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            var gallery = new Gallery
            {
                Categories = new List<Category>()
            };
            var sourceFolder = new DirectoryInfo(SourceFolderTextBox.Text);
            var fileCount = 0;
            var startTime = DateTime.Now;
            var year = 0;
            var folders = sourceFolder.GetDirectories();
            foreach (var folder in folders)
            {
                var category = new Category
                {
                    Name = folder.Name,
                    Photos = new List<Photo>()
                };
                var files = folder.GetFiles("*.jpg", SearchOption.AllDirectories);
                if (files.Length > 0)
                {
                    try
                    {
                        year = files[0].LastWriteTime.Year;
                        category.Year = year;
                    }
                    catch
                    {
                        category.Year = 0;
                    }
                    var photoId = 0;
                    foreach (var file in files)
                    {
                        var thumbnailFileNameWithFolder = year + file.FullName.Substring(sourceFolder.FullName.Length, file.FullName.Length - sourceFolder.FullName.Length);
                        var thumbnailFullPath = Path.Combine(TargetFolderTextBox.Text, thumbnailFolder, thumbnailFileNameWithFolder);
                        ImageHelper.GetThumbnail(100, 100, file.FullName, thumbnailFullPath, false);
                        var normalFileNameWithFolder = year + file.FullName.Substring(sourceFolder.FullName.Length, file.FullName.Length - sourceFolder.FullName.Length);
                        var normalFullPath = Path.Combine(TargetFolderTextBox.Text, normalFolder, normalFileNameWithFolder);
                        ImageHelper.GetThumbnail(1000, 800, file.FullName, normalFullPath, false);

                        photoId++;
                        var photo = new Photo
                        {
                            Id = photoId,
                            Title = Path.GetFileNameWithoutExtension(file.FullName),
                            NormalUrl = "Resources/images/" + normalFolder + "/" + StringHelper.GetUriFromPath(normalFileNameWithFolder),
                            ThumbnailUrl = "Resources/images/" + thumbnailFolder + "/" + StringHelper.GetUriFromPath(thumbnailFileNameWithFolder)
                        };

                        category.Photos.Add(photo);

                        fileCount++;
                    }
                }
                gallery.Categories.Add(category);
            }

            gallery.Categories.Sort();
            var categoryId = 0;
            foreach (var category in gallery.Categories)
            {
                categoryId++;
                category.Id = categoryId;
            }

            StorageHelper.SaveGallery(gallery);

            MessageBox.Show(string.Format("Done!\n共转换{0}个文件，耗时{1}秒", fileCount, (DateTime.Now - startTime).TotalSeconds), "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
