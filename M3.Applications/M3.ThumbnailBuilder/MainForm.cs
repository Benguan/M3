using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
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
        private static int progress = 0;
        private static int totalFilesCount = 0;
        private static int doneFilesCount = 0;
        private DirectoryInfo sourceFolder;
        private delegate void UpdateProgressEventHandler(int v);
        public MainForm()
        {
            InitializeComponent();
            SourceFolderTextBox.Text = ConfigurationManager.ThumbnailBuilderConfiguration.SourceFolderPath;
            sourceFolder = new DirectoryInfo(SourceFolderTextBox.Text);
            totalFilesCount = sourceFolder.GetFiles("*.jpg", SearchOption.AllDirectories).Length;
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            new Thread(new ThreadStart(Build)).Start();
        }

        private void Build()
        {
            var gallery = new Gallery
            {
                Categories = new List<Category>()
            };

            var startTime = DateTime.Now;
            var year = 0;
            var folders = sourceFolder.GetDirectories();



            var thumbnailMaxWidth = ConfigurationManager.ThumbnailBuilderConfiguration.ThumbnailMaxWidth;
            var thumbnailMaxHeight = ConfigurationManager.ThumbnailBuilderConfiguration.ThumbnailMaxHeight;
            var photoMaxWidth = ConfigurationManager.ThumbnailBuilderConfiguration.PhotoMaxWidth;
            var photoMaxHeight = ConfigurationManager.ThumbnailBuilderConfiguration.PhotoMaxHeight;

            var photosPath = Path.Combine(DirectoryHelper.GetSolutionDirectoryPath(AppDomain.CurrentDomain.BaseDirectory), ConfigurationManager.ThumbnailBuilderConfiguration.TargetFolderPath);

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
                        var thumbnailFullPath = Path.Combine(photosPath, thumbnailFolder, thumbnailFileNameWithFolder);
                        var thumbnailPhotoInfo = ImageHelper.GetThumbnail(thumbnailMaxWidth, thumbnailMaxHeight, file.FullName, thumbnailFullPath, false);
                        var normalFileNameWithFolder = year + file.FullName.Substring(sourceFolder.FullName.Length, file.FullName.Length - sourceFolder.FullName.Length);
                        var normalFullPath = Path.Combine(photosPath, normalFolder, normalFileNameWithFolder);
                        var normalPhotoInfo = ImageHelper.GetThumbnail(photoMaxWidth, photoMaxHeight, file.FullName, normalFullPath, false);

                        photoId++;
                        var photo = new Photo
                        {
                            Id = photoId,
                            Title = Path.GetFileNameWithoutExtension(file.FullName),
                            Height = normalPhotoInfo.Height,
                            Width = normalPhotoInfo.Width,
                            NormalUrl = "/Resources/images/photos/" + normalFolder + "/" + StringHelper.GetUriFromPath(normalFileNameWithFolder),
                            ThumbnailUrl = "/Resources/images/photos/" + thumbnailFolder + "/" + StringHelper.GetUriFromPath(thumbnailFileNameWithFolder)
                        };

                        category.Photos.Add(photo);

                        doneFilesCount++;
                        progress = 100 * doneFilesCount / totalFilesCount;
                        Invoke(new UpdateProgressEventHandler(UpdateProgress), progress);
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

            MessageBox.Show(string.Format("Done!\n共转换{0}个文件，耗时{1}秒", doneFilesCount, (DateTime.Now - startTime).TotalSeconds), "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void UpdateProgress(int v)
        {
            ProgressBar.Value = v;
        }
    }
}
