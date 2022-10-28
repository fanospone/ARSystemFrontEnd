using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace ARSystem.Domain.Repositories
{
    public class Helper
    {
        /// <summary>
        /// Serialize Object to XML
        /// </summary>
        /// <typeparam name="T">Object</typeparam>
        /// <param name="dataToSerialize">Object</param>
        /// <returns></returns>
        public static string XmlSerializer<T>(T dataToSerialize)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;

            XmlSerializer xsSubmit = new XmlSerializer(typeof(T));
            StringWriter sw = new StringWriter();
            using (XmlWriter writer = XmlWriter.Create(sw, settings))
            {
                var xmlns = new XmlSerializerNamespaces();
                xmlns.Add(string.Empty, string.Empty);

                xsSubmit.Serialize(writer, dataToSerialize, xmlns);
                return sw.ToString();
            }
        }

        /// <summary>
        /// Deserialize XML to Object
        /// </summary>
        /// <typeparam name="T">Object</typeparam>
        /// <param name="xmlText">xml</param>
        /// <returns></returns>
        public static T XMLDeserializer<T>(string xmlText)
        {
            var stringReader = new System.IO.StringReader(xmlText);
            var serializer = new XmlSerializer(typeof(T));
            return (T)serializer.Deserialize(stringReader);
        }
    }
}
