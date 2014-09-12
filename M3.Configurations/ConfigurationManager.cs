namespace M3.Configurations
{
    public class ConfigurationManager
    {
        private static InternalConfiguration Config;

        static ConfigurationManager()
        {
            if (Config == null)
            {
                Config = InternalConfiguration.GetInstance();
            }
        }

        public static ThumbnailBuilderConfiguration ThumbnailBuilderConfiguration
        {
            get { return Config.ThumbnailBuilderConfiguration; }
            set { Config.ThumbnailBuilderConfiguration = value; }
        }


        private class InternalConfiguration : ConfigurationManagerBase
        {
            private static InternalConfiguration Config;
            public static InternalConfiguration GetInstance()
            {
                if (Config == null)
                {
                    Config = new InternalConfiguration();
                }
                return Config;
            }

            private ThumbnailBuilderConfiguration thumbnailBuilderConfiguration;

            public ThumbnailBuilderConfiguration ThumbnailBuilderConfiguration
            {
                get
                {
                    thumbnailBuilderConfiguration = GetConfiguration<ThumbnailBuilderConfiguration>("ConfigurationFilePath");
                    return thumbnailBuilderConfiguration;
                }
                set { thumbnailBuilderConfiguration = value; }
            }
        }
    }
}
