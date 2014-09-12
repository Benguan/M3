using System;
using System.IO;

namespace M3.Configurations
{
    public abstract class ConfigurationManagerBase
    {
        protected T GetConfiguration<T>(string key) where T : class
        {
            return this.GetConfiguration<T>(null, key, true);
        }

        protected T GetConfiguration<T>(string cacheKey, string key) where T : class
        {
            return this.GetConfiguration<T>(cacheKey, key, true);
        }

        protected T GetConfiguration<T>(string cacheKey, string key, bool needLog) where T : class
        {
            string str = cacheKey ?? key;

            string configurationFile = GetConfigurationFile(key);
            if (!string.IsNullOrEmpty(configurationFile))
            {
                T local = this.LoadConfiguration<T>(str, configurationFile, needLog);
                return local;
            }

            return null;
        }

        private T LoadConfiguration<T>(string cacheKey, string configFile, bool needLog) where T : class
        {
            T local = ObjectXmlSerializer.LoadFromXml<T>(configFile);
            if (local == null)
            {
                throw new LoadFileException(typeof(T).Name, configFile);
            }
            return local;
        }

        [Serializable]
        public class LoadFileException : ApplicationException
        {
            private string m_FileName;
            private string m_TypeName;

            public LoadFileException(string typeName, string fileName)
            {
                this.m_FileName = fileName;
                this.m_TypeName = typeName;
            }

            public override string Message
            {
                get
                {
                    return string.Format("Unable to load file {0} for type {1}", this.m_FileName, this.m_TypeName);
                }
            }
        }

        public static string GetConfigurationFile(string appSection)
        {
            if (System.Configuration.ConfigurationManager.AppSettings[appSection] != null)
            {
                string path = System.Configuration.ConfigurationManager.AppSettings[appSection];
                if (File.Exists(path))
                {
                    return Path.GetFullPath(path);
                }
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path.Replace('/', '\\').TrimStart(new char[] { '\\' }));
            }
            return string.Empty;
        }
    }
}
