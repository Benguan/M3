using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace M3.Configurations
{
    public class ObjectXmlSerializer
    {
        public static T FromXML<T>(string xml) where T : class
        {
            StringReader textReader = null;
            T local2;
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                textReader = new StringReader(xml);
                T local = (T)serializer.Deserialize(textReader);
                textReader.Close();
                local2 = local;
            }
            catch
            {
                local2 = default(T);
            }
            finally
            {
                if (textReader != null)
                {
                    textReader.Close();
                }
            }
            return local2;
        }

        public static T LoadFromXml<T>(string fileName) where T : class
        {
            FileStream stream = null;
            T local;
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                stream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                local = (T)serializer.Deserialize(stream);
            }
            catch
            {
                local = default(T);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
            return local;
        }

        public static T LoadFromXmlMessage<T>(string xmlMessage) where T : class
        {
            StringReader textReader = null;
            T local;
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                textReader = new StringReader(xmlMessage);
                local = (T)serializer.Deserialize(textReader);
            }
            catch
            {
                local = default(T);
            }
            finally
            {
                if (textReader != null)
                {
                    textReader.Dispose();
                }
            }
            return local;
        }

        public static string ToStringXmlMessage<T>(T t) where T : class
        {
            StringWriter writer = null;
            string str;
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                writer = new StringWriter();
                serializer.Serialize((TextWriter)writer, t);
                str = writer.ToString();
            }
            catch (Exception)
            {
                str = string.Empty;
            }
            finally
            {
                if (writer != null)
                {
                    writer.Dispose();
                }
            }
            return str;
        }

        public static string ToXML<T>(T instance)
        {
            UTF8StringWriter writer = null;
            string str;
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                StringBuilder sb = new StringBuilder();
                writer = new UTF8StringWriter(sb);
                serializer.Serialize((TextWriter)writer, instance);
                str = sb.ToString();
            }
            catch
            {
                str = null;
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
            }
            return str;
        }

        private class UTF8StringWriter : StringWriter
        {
            public UTF8StringWriter(StringBuilder sb)
                : base(sb)
            {
            }

            public override System.Text.Encoding Encoding
            {
                get
                {
                    return System.Text.Encoding.UTF8;
                }
            }
        }
    }
}
