using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Data;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Linq;
using System.IO;


namespace ConfigurationLib
{
    public static class XMLLib
    {
        public static bool IsXMLFormat(string pXML)
        {
            try
            {
                if (pXML.Trim().Length == 0)
                {
                    return false;
                }
                else
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(pXML);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public static string XmlSerialize<T>(this T objectToSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            StringWriter stringWriter = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(stringWriter);
            xmlWriter.Formatting = Formatting.Indented;
            xmlSerializer.Serialize(xmlWriter, objectToSerialize, ns);

            DataSet ds = new DataSet();
            StringReader sr = new StringReader(stringWriter.ToString());
            ds.ReadXml(sr, XmlReadMode.InferSchema);

            System.IO.StringWriter swWriter = new System.IO.StringWriter();
            swWriter.NewLine = "";

            ds.WriteXml(swWriter, XmlWriteMode.IgnoreSchema);
            return swWriter.ToString();
        }

        //if XML size large than 100 MB
        //for large size of xml file using.
        //Save xml inside the file stream, reduce the memory requirment for reading
        public static string GetXmlStreamByList<T>(this T objectToSerialize, string pXMLRoot)
        {
            try
            {
                Guid id = Guid.NewGuid();
                string XMLFileName = id.ToString() + ".xml";
                string xml = "";
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T), new XmlRootAttribute(pXMLRoot));
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                StringWriter stringWriter = new StringWriter();
                XmlTextWriter xmlWriter = new XmlTextWriter(stringWriter);
                xmlWriter.Formatting = Formatting.Indented;
                xmlSerializer.Serialize(xmlWriter, objectToSerialize, ns);

                DataSet ds = new DataSet();
                StringReader sr = new StringReader(stringWriter.ToString());
                ds.ReadXml(sr, XmlReadMode.InferSchema);

                System.IO.StringWriter swWriter = new System.IO.StringWriter();
                swWriter.NewLine = "";

                System.IO.FileStream myFileStream = new System.IO.FileStream
                        (XMLFileName, System.IO.FileMode.Create);
                System.Xml.XmlTextWriter myXmlWriter =
                new System.Xml.XmlTextWriter(myFileStream, System.Text.Encoding.Unicode);
                ds.WriteXml(myXmlWriter);

                myXmlWriter.Close();
                myFileStream.Close();
                xmlWriter.Close();
                stringWriter.Close();
                swWriter.Flush();
                ds.Clear();
                using (var streamReader = new StreamReader(XMLFileName, Encoding.UTF8))
                {
                    xml = streamReader.ReadToEnd();

                    streamReader.Close();
                    System.IO.FileInfo fi = new System.IO.FileInfo(XMLFileName);
                    try
                    {
                        fi.Delete();
                    }
                    catch (System.IO.IOException e)
                    {
                        //ERROR LOG
                        Console.WriteLine(e.Message);
                    }
                }
                return xml;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public static string GetXmlByList<T>(this T objectToSerialize, string pXMLRoot)
        {
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T), new XmlRootAttribute(pXMLRoot));
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                StringWriter stringWriter = new StringWriter();
                XmlTextWriter xmlWriter = new XmlTextWriter(stringWriter);
                xmlWriter.Formatting = Formatting.Indented;
                xmlSerializer.Serialize(xmlWriter, objectToSerialize, ns);

                DataSet ds = new DataSet();
                StringReader sr = new StringReader(stringWriter.ToString());
                ds.ReadXml(sr, XmlReadMode.InferSchema);

                System.IO.StringWriter swWriter = new System.IO.StringWriter();
                swWriter.NewLine = "";

                ds.WriteXml(swWriter, XmlWriteMode.IgnoreSchema);
                return swWriter.ToString();
            }
            catch
            {
                return "";
            }
        }

    }

}
