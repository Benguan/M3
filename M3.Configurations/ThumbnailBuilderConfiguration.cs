using System.Xml.Serialization;

namespace M3.Configurations
{
    [XmlRoot("ThumbnailBuilderConfiguration")]
    public class ThumbnailBuilderConfiguration
    {
        [XmlElement("SourceFolderPath")]
        public string SourceFolderPath { get; set; }

        [XmlElement("TargetFolderPath")]
        public string TargetFolderPath { get; set; }

    }
}
