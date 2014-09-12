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

        [XmlElement("ThumbnailMaxWidth")]
        public int ThumbnailMaxWidth { get; set; }

        [XmlElement("ThumbnailMaxHeight")]
        public int ThumbnailMaxHeight { get; set; }

        [XmlElement("PhotoMaxWidth")]
        public int PhotoMaxWidth { get; set; }

        [XmlElement("PhotoMaxHeight")]
        public int PhotoMaxHeight { get; set; }

    }
}
