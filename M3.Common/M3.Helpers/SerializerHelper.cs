using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace M3.Helpers
{
    public class SerializerHelper
    {
        private static bool Serialize(object o, string path, bool isBinaryFile)
        {
            try
            {
                var fileInfo = new FileInfo(path);
                if (!fileInfo.Directory.Exists)
                {
                    fileInfo.Directory.Create();
                }

                if (isBinaryFile)
                {
                    var formatter = new BinaryFormatter();
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        formatter.Serialize(stream, o);
                        return true;
                    }
                }
                else
                {
                    var serializer = new XmlSerializer(o.GetType());
                    using (var writer = new XmlTextWriter(path, Encoding.UTF8))
                    {
                        writer.Formatting = Formatting.Indented;
                        var namespaces = new XmlSerializerNamespaces();
                        namespaces.Add("", "");
                        serializer.Serialize(writer, o, namespaces);
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        private static T Deserialize<T>(string path, bool isBinaryFile)
        {
            T obj;
            try
            {
                if (isBinaryFile)
                {
                    var formatter = new BinaryFormatter();
                    using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                    {
                        obj = (T)formatter.Deserialize(stream);
                    }
                }
                else
                {
                    var serializer = new XmlSerializer(typeof(T));
                    using (var reader = new XmlTextReader(path))
                    {
                        obj = (T)serializer.Deserialize(reader);
                    }
                }
            }
            catch
            {
                obj = default(T);
            }
            return obj;
        }

        public static string SerializeToString(object o)
        {
            string xml = "";
            try
            {
                var serializer = new XmlSerializer(o.GetType());
                using (var memoryStream = new MemoryStream())
                {
                    using (var writer = new XmlTextWriter(memoryStream, Encoding.UTF8))
                    {
                        writer.Formatting = Formatting.Indented;
                        var n = new XmlSerializerNamespaces();
                        n.Add("", "");
                        serializer.Serialize(writer, o, n);

                        memoryStream.Seek(0, SeekOrigin.Begin);
                        using (var reader = new StreamReader(memoryStream))
                        {
                            xml = reader.ReadToEnd();
                        }
                    }
                }
            }
            catch { xml = ""; }
            return xml;
        }

        public static T DeserializeFromString<T>(string xml)
        {
            T obj;
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
                {
                    obj = (T)serializer.Deserialize(memoryStream);
                }
            }
            catch
            {
                obj = default(T);
            }
            return obj;
        }

        public static bool XmlSerialize(object o, string path)
        {
            return Serialize(o, path, false);
        }

        public static T XmlDeserialize<T>(string path)
        {
            return Deserialize<T>(path, false);
        }

        public static bool BinarySerialize(object o, string path)
        {
            return Serialize(o, path, true);
        }

        public static T BinaryDeserialize<T>(string path)
        {
            return Deserialize<T>(path, true);
        }
    }
}
