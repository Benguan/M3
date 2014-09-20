using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using M3.Configurations;
using M3.Models;

namespace M3.Helpers
{
    public class CaseInsensitiveStringEqualityComparer : IEqualityComparer<string>
    {
        public bool Equals(string x, string y)
        {
            return (string.Compare(x, y, true) == 0);
        }

        public int GetHashCode(string obj)
        {
            return obj.ToUpper().GetHashCode();
        }
    }

    public class FileSystemChangeEventHandler
    {
        private class FileChangeEventArg
        {
            private object m_Sender;
            private FileSystemEventArgs m_Argument;

            public FileChangeEventArg(object sender, FileSystemEventArgs arg)
            {
                m_Sender = sender;
                m_Argument = arg;
            }

            public object Sender
            {
                get { return m_Sender; }
            }
            public FileSystemEventArgs Argument
            {
                get { return m_Argument; }
            }
        }

        private object m_SyncObject;

        private Dictionary<string, Timer> m_Timers;
        private int m_Timeout;
        private bool m_IsFolderChange;

        public event FileSystemEventHandler ActualHandler;

        private FileSystemChangeEventHandler()
        {
            m_SyncObject = new object();
            m_Timers = new Dictionary<string, Timer>(new CaseInsensitiveStringEqualityComparer());
        }

        public FileSystemChangeEventHandler(int timeout)
            : this(timeout, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystemChangeEventHandler"/> class.
        /// </summary>
        /// <param name="timeout">The timeout.</param>
        /// <param name="isFolderChange">if set to <c>true</c> [if the Watcher is to folder change].</param>
        public FileSystemChangeEventHandler(int timeout, bool isFolderChange)
            : this()
        {
            m_Timeout = timeout;
            m_IsFolderChange = isFolderChange;
        }

        public void ChangeEventHandler(object sender, FileSystemEventArgs e)
        {
            lock (m_SyncObject)
            {
                //LogFileChange(m_Timeout, e.FullPath, e.ChangeType, ActualHandler);
                Timer t;

                string watchPath;

                if (m_IsFolderChange)
                {
                    watchPath = Path.GetDirectoryName(e.FullPath);
                }
                else
                {
                    watchPath = e.FullPath;
                }

                // disable the existing timer
                if (m_Timers.ContainsKey(watchPath))
                {
                    t = m_Timers[watchPath];
                    t.Change(Timeout.Infinite, Timeout.Infinite);
                    t.Dispose();
                }

                // add a new timer
                if (ActualHandler != null)
                {
                    t = new Timer(TimerCallback, new FileChangeEventArg(sender, e), m_Timeout, Timeout.Infinite);
                    m_Timers[watchPath] = t;
                }
            }
        }

        private void TimerCallback(object state)
        {
            FileChangeEventArg arg = state as FileChangeEventArg;
            //LogActualHandleFileChange(arg);
            ActualHandler(arg.Sender, arg.Argument);
        }
    }

    public class StorageHelper
    {
        private static FileSystemWatcher s_DataFileWatcher;
        private static FileSystemChangeEventHandler s_FileChangeHandler;
        private static Dictionary<string, DataTable> s_DataTableCache;

        /// <summary>
        /// Field of objects cache;
        /// </summary>
        private static ConcurrentDictionary<string, object> objectCache;

        public static string StorageFolderPath { get; set; }

        static StorageHelper()
        {
            var solutionDirectoryPath = DirectoryHelper.GetSolutionDirectoryPath(AppDomain.CurrentDomain.BaseDirectory);
            StorageFolderPath = Path.Combine(solutionDirectoryPath, ConfigurationManager.CommonConfiguration.StorageFolderPath);

            objectCache = new ConcurrentDictionary<string, object>();
        }

        private static void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            object cachedItem;
            objectCache.TryRemove(e.Name, out cachedItem);
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


        public static List<Category> LoadCategories(string storageName)
        {
            return Get<Gallery>(storageName).Categories as List<Category>;
        }

        /// <summary>
        /// 排序，并添加至内存
        /// </summary>
        /// <returns></returns>
        public static List<Category> GetAllCategories()
        {
            string fileName = ConfigurationManager.CommonConfiguration.GalleryStorageFileName;
            return objectCache.GetOrAdd(fileName, k => LoadCategories(fileName)) as List<Category>;
        }

        public static bool SaveGallery(Gallery gallery)
        {
            return Save(gallery, ConfigurationManager.CommonConfiguration.GalleryStorageFileName);
        }
    }
}