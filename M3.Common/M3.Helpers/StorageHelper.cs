using System;
using System.IO;
using M3.Configurations;
using M3.Models;

namespace M3.Helpers
{
    public class StorageHelper
    {
        public static string StorageFolderName { get; set; }
        public static string AppDirectory { get; set; }

        static StorageHelper()
        {
            StorageFolderName = ConfigurationManager.CommonConfiguration.StorageFolderName;
            AppDirectory = AppDomain.CurrentDomain.BaseDirectory;
        }

        private static bool Save(object o, string storageName)
        {
            var storagePath = Path.Combine(AppDirectory, StorageFolderName, storageName);
            return SerializerHelper.XmlSerialize(o, storagePath);
        }

        private static T Get<T>(string storageName)
        {
            var storagePath = Path.Combine(AppDirectory, StorageFolderName, storageName);
            return SerializerHelper.XmlDeserialize<T>(storagePath);
        }

        public static Gallery Get()
        {
            return Get<Gallery>(ConfigurationManager.CommonConfiguration.StorageFolderName);
        }

        public static bool Save(object o)
        {
            return Save(o, ConfigurationManager.CommonConfiguration.StorageFolderName);
        }
    }
}