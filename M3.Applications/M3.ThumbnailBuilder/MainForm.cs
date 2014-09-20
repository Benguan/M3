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
        //CONFIGURATION
        private string inputFolder = string.Empty;
        private string thumbnailFolder = "thumbnail";
        private string normalFolder = "normal";
        private static int progress = 0;
        private static int totalFilesCount = 0;
        private static int doneFilesCount = 0;
        private DirectoryInfo sourceFolder;
        private delegate void UpdateProgressEventHandler(int v);

        private Gallery sourceGallery;

        public MainForm()
        {
            InitializeComponent();
            SourceFolderTextBox.Text = ConfigurationManager.ThumbnailBuilderConfiguration.SourceFolderPath;
            sourceGallery = StorageHelper.GetGallery();
        }


        private void StartButton_Click(object sender, EventArgs e)
        {
            doneFilesCount = 0;
            sourceFolder = new DirectoryInfo(SourceFolderTextBox.Text);
            totalFilesCount = sourceFolder.GetFiles("*.jpg", SearchOption.AllDirectories).Length;
            inputFolder = SourceFolderTextBox.Text;
            new Thread(new ThreadStart(Build)).Start();
        }

        private void Build()
        {
            try
            {
                if (sourceGallery == null )
                {
                    sourceGallery = new Gallery {Categories = new List<Category>()};
                }

                var gallery = new Gallery {Categories = new List<Category>()};

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
                    var files = folder.GetFiles("*.jpg", SearchOption.AllDirectories);

                    if (files.Length <= 0)
                    {
                        break;
                    }

                    var category = new Category
                        {
                            Name = folder.Name,
                            Photos = new List<Photo>()
                        };


                    var photoId = 0;

                    var lastFolderName = "";

                    foreach (var file in files)
                    {
                        string[] directoryList = file.DirectoryName.Split('\\');

                        string folderName = directoryList[directoryList.Length - 1]; //向上获取一层Folder名
                        //string folderName = string.Join("-", file.DirectoryName.Replace(inputFolder + "\\", "").Split('\\'));         //获取每一层folder名，之间用-隔开

                        if (IsExistInGallery(folderName))
                        {
                            this.InvokeProcessBar();
                            continue;
                        }

                        if (!folderName.Equals(lastFolderName))
                        {
                            lastFolderName = folderName;
                            category = new Category
                                {
                                    Name = folderName,
                                    Photos = new List<Photo>()
                                };
                            try
                            {
                                year = file.LastWriteTime.Year;
                                category.Year = year;
                            }
                            catch
                            {
                                category.Year = 0;
                            }
                            gallery.Categories.Add(category);
                        }

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
                        this.InvokeProcessBar();
                    }
                }

                int categoryId = sourceGallery.MaxId;

                foreach (var category in gallery.Categories)
                {
                    categoryId++;
                    category.Id = categoryId;
                }

                if (gallery.Categories != null && gallery.Categories.Count > 0)
                {
                    sourceGallery.Categories.AddRange(gallery.Categories);
                }

                sourceGallery.Categories.Sort(CompareByYear);

                int pageId = 0;
                foreach (Category category in sourceGallery.Categories)
                {
                    pageId++;
                    category.Page = pageId;
                }

                try
                {
                    StorageHelper.SaveGallery(sourceGallery);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("SaveGallery Error:" + ex.Message);
                }


                MessageBox.Show(string.Format("Done!\n共转换{0}个文件，耗时{1}秒", doneFilesCount, (DateTime.Now - startTime).TotalSeconds), "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Build Error:"+ ex.Message);
            }
        }

        private static int CompareByYear(Category a, Category b)
        {
            if (a == null)
            {
                if (b == null)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                if (b == null)
                {
                    return 1;
                }
                else
                {
                    return a.Year.CompareTo(b.Year);
                }
            }
        }

        private void UpdateProgress(int v)
        {
            ProgressBar.Value = v;
        }

        /// <summary>
        /// 已经存在XML里的不再做Build.
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public bool IsExistInGallery(string categoryName)
        {
            if (sourceGallery != null && sourceGallery.Categories != null && sourceGallery.Categories.Count > 0)
            {
                return sourceGallery.Categories.Exists(m => m.Name == categoryName);
            }
            return false;
        }

        public void InvokeProcessBar()
        {
            doneFilesCount++;
            progress = 100*doneFilesCount/totalFilesCount;
            Invoke(new UpdateProgressEventHandler(UpdateProgress), progress);
        }


    }
}