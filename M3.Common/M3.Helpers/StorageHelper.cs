﻿using System;
using System.IO;
using M3.Configurations;
using M3.Models;

namespace M3.Helpers
{
    public class StorageHelper
    {
        public static string StorageFolderPath { get; set; }

        static StorageHelper()
        {
            var currentDirectory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            var parent = currentDirectory;
            while (parent != null && parent.Name != "M3.Applications" && parent.Name != "M3.Website")
            {
                parent = parent.Parent;
            }
            if (parent != null)
            {
                StorageFolderPath = Path.Combine(parent.Parent.FullName, ConfigurationManager.CommonConfiguration.StorageFolderPath);
            }
            else
            {
                StorageFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), ConfigurationManager.CommonConfiguration.StorageFolderPath);
            }
        }

        private static bool Save(object o, string storageName)
        {
            var storagePath = Path.Combine(StorageFolderPath, storageName);
            return SerializerHelper.XmlSerialize(o, storagePath);
        }

        private static T Get<T>(string storageName)
        {
            var storagePath = Path.Combine(StorageFolderPath, storageName);
            return SerializerHelper.XmlDeserialize<T>(storagePath);
        }

        public static Gallery GetGallery()
        {
            return Get<Gallery>(ConfigurationManager.CommonConfiguration.GalleryStorageFileName);
        }

        public static bool SaveGallery(Gallery gallery)
        {
            return Save(gallery, ConfigurationManager.CommonConfiguration.GalleryStorageFileName);
        }
    }
}