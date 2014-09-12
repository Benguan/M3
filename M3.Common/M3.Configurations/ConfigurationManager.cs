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

        public static CommonConfiguration CommonConfiguration
        {
            get { return Config.CommonConfiguration; }
            set { Config.CommonConfiguration = value; }
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
                    thumbnailBuilderConfiguration = GetConfiguration<ThumbnailBuilderConfiguration>("ThumbnailBuilderConfigurationFilePath");
                    return thumbnailBuilderConfiguration;
                }
                set
                {
                    thumbnailBuilderConfiguration = value;
                }
            }

            private CommonConfiguration commonConfiguration;
            public CommonConfiguration CommonConfiguration
            {
                get
                {
                    commonConfiguration = GetConfiguration<CommonConfiguration>("CommonConfigurationFilePath");
                    return commonConfiguration;
                }
                set
                {
                    commonConfiguration = value;
                }
            }
        }
    }
}
