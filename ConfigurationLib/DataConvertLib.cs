using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection; 
using System.Data;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Security;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
//using System.Runtime.Serialization.Json;

namespace ConfigurationLib
{
    public class DataConvertLib
    {

        private static readonly DataConvertLib _instance = new DataConvertLib();

        public static DataConvertLib Instance
        {
            get
            {

                return _instance;
            }
        }

        public List<DataRow> DataSetToDataRow(DataSet pDataSet)
        {
            try
            {
                DataTable _DataTable = new DataTable();
                _DataTable = DataSetToDataTable(pDataSet);
                if (_DataTable != null)
                {
                    List<DataRow> _List = _DataTable.AsEnumerable().ToList();
                    return _List;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        public string DataTableToJSon(DataTable pDataTable)
        {
            try
            {
                return JsonConvert.SerializeObject(pDataTable);
            }
            catch
            {
                return "";
            }
        }

        public DataTable DataSetToDataTable(DataSet pDataSet)
        {
            try
            {
                if (pDataSet.Tables.Count > 0)
                {
                    DataTable _DataTable = new DataTable();
                    _DataTable = pDataSet.Tables[0];
                    return _DataTable;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        public List<DataRow> JSonToDataRow(string pJSon)
        {
            DataSet _DataSet = JSonToDataSet(pJSon);
            if (_DataSet != null)
            {
                return DataSetToDataRow(_DataSet);
            }
            else
            {
                return null;
            }
        }

        public DataSet JSonToDataSet(string pJSon)
        {
            try
            {
                if (pJSon.Substring(0, 1) == "[")
                {
                    pJSon = pJSon.Substring(1, pJSon.Length - 2);
                }
                XmlDocument xd = new XmlDocument();
                xd = (XmlDocument)JsonConvert.DeserializeXmlNode("{\"jsonData\":[" + pJSon + "]}", "jsonData");
                DataSet _DataSet = new DataSet();
                _DataSet.ReadXml(new XmlNodeReader(xd));

                return _DataSet;
            }
            catch
            {
                return null;
            }
        }

        public object[] StringToArray(string input, string separator, Type type)
        {
            string[] stringList = input.Split(separator.ToCharArray(),
                                              StringSplitOptions.RemoveEmptyEntries);
            object[] list = new object[stringList.Length];

            for (int i = 0; i < stringList.Length; i++)
            {
                list[i] = Convert.ChangeType(stringList[i], type);
            }

            return list;
        }


        public DataTable ConvertXMLToDataTable(string pXML)
        {
            try
            {
                DataSet ds = new DataSet();
                ds.ReadXml(new XmlTextReader(new StringReader(pXML)));

                DataTable dt = ds.Tables[0];
                return dt;
            }
            catch
            {
                return null;
            }
        }

        public string GetSQLBase64(String pText)
        {
            try
            {
                byte[] byt = System.Text.Encoding.UTF8.GetBytes(pText);
                return GetStringToBase64(Encoding.ASCII.GetString(byt));
            }
            catch
            {
                return "";
            }
        }

        public string GetStringToBase64(string pText)
        {
            try
            {
                byte[] byt = System.Text.Encoding.UTF8.GetBytes(pText);
                return Convert.ToBase64String(byt);
            }
            catch
            {
                return "";
            }
        }

        public string GetBase64ToString(string pText)
        {
            try
            {
                byte[] b = Convert.FromBase64String(pText);
                return System.Text.Encoding.UTF8.GetString(b);
            }
            catch
            {
                return "";
            }
        }

        public string GetXMLByDictionary(Dictionary<string, string> pDict)
        {
            XElement el = new XElement("root",
                pDict.Select(kv => new XElement(kv.Key, kv.Value)));
            return el.ToString();
        }

        public string GetConvertXMLByKey(string pKey, string pValue)
        {
            try
            {
                //XmlConvert.EncodeName(pValue)
                return "<" + pKey + ">" + SecurityElement.Escape(pValue) + "</" + pKey + ">";
            }
            catch
            {
                return "";
            }
        }

        public string GetXMLElementValue(string pXML, string pKey)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(new XmlTextReader(new StringReader(pXML)));
                XmlNodeList elemList = doc.GetElementsByTagName(pKey);
                if (elemList.Count != 0)
                {
                    return elemList[0].InnerXml;
                }
                else
                {
                    return "";
                }
            }
            catch
            {
                return "";
            }
        }

        public string GetXMLElementText(string pXML, string pKey)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(new XmlTextReader(new StringReader(pXML)));
                XmlNodeList elemList = doc.GetElementsByTagName(pKey);
                var se = new SecurityElement("text", elemList[0].InnerText);
                return se.Text;
            }
            catch
            {
                return "";
            }
        }


        public string GetXMLToJSon(string pXML)
        {
            try
            {
                var xml = new XmlDocument();
                xml.LoadXml(pXML);
                string strJSon = Newtonsoft.Json.JsonConvert.SerializeXmlNode(xml);
                //string jsonText = JsonConvert.SerializeXmlNode(xml);

                return strJSon; // Newtonsoft.Json.JsonConvert.SerializeObject(strJSon);
            }
            catch
            {
                return "";
            }
        }

        public XElement GetXElement(XmlNode node)
        {
            XDocument xDoc = new XDocument();
            using (XmlWriter xmlWriter = xDoc.CreateWriter())
                node.WriteTo(xmlWriter);
            return xDoc.Root;
        }

        public XmlNode GetXmlNode( XElement element)
        {
            using (XmlReader xmlReader = element.CreateReader())
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlReader);
                return xmlDoc;
            }
        }

        public JObject GetJSonObject(string pJSON)
        {
            try
            {
                JObject json = JObject.Parse(pJSON);
                return json;
            }
            catch
            {
                return null;
            }
        }

        public string GetJSonElementValueByKey(string pJSON, string pKey)
        {
            try
            {
                JObject json = JObject.Parse(pJSON);
                //JObject firstItemSnippet = JObject.Parse(json["items"][0]["snippet"].ToString());
                return (string)json.SelectToken(pKey);
            }
            catch(Exception ex)
            {
                return "";
            }
        }

        public string GetJSonElementValueByRootKey(string pJSON, string pRoot, string pKey)
        {
            try
            {
                JObject json = JObject.Parse(pJSON);
                return (string)json.SelectToken(pRoot)[pKey];
            }
            catch
            {
                return "";
            }
        }

        public string GetDictionaryToJSon(Dictionary<string, string> pDic)
        {
            return JsonConvert.SerializeObject(pDic);
        }


        public string GetNumToEngTxt(string pTxt)
        {
            try
            {
                string strRtn = "";
                foreach (var txt in pTxt)
                {
                    switch (txt.ToString())
                    {
                        case "0":
                            strRtn = strRtn + " ZERO ";
                            break;
                        case "1":
                            strRtn = strRtn + " One ";
                            break;
                        case "2":
                            strRtn = strRtn + " Two ";
                            break;
                        case "3":
                            strRtn = strRtn + " Three ";
                            break;
                        case "4":
                            strRtn = strRtn + " Four ";
                            break;
                        case "5":
                            strRtn = strRtn + " Five ";
                            break;
                        case "6":
                            strRtn = strRtn + " Six ";
                            break;
                        case "7":
                            strRtn = strRtn + " Seven ";
                            break;
                        case "8":
                            strRtn = strRtn + " Eight ";
                            break;
                        case "9":
                            strRtn = strRtn + " Nine ";
                            break;
                    }
                }
                return strRtn;
            }
            catch
            {
                return "";
            }
        }

        public bool GetXMLByDatatable(DataTable dt,ref string pResult)
        {
            try
            {
                System.IO.StringWriter swWriter = new System.IO.StringWriter();
                 
                dt.WriteXml(swWriter, XmlWriteMode.IgnoreSchema, false);
                pResult = swWriter.ToString();

                return true;
            }
            catch
            {
                return false;
            }
        }


        public string GetJSonElementInXMLNodeValueByKey(string pJSON, string pKey)
        {
            try
            {
                JObject json = JObject.Parse(pJSON);
                //JObject firstItemSnippet = JObject.Parse(json["items"][0]["snippet"].ToString());
                return (string)json.SelectToken("ROOT")["XML"][pKey];
            }
            catch (Exception ex)
            {
                return "";
            }
        }


        //public DataTable ToDataTable<T>(IList<T> data)
        //{
        //    PropertyDescriptorCollection properties =
        //        TypeDescriptor.GetProperties(typeof(T));
        //    DataTable table = new DataTable();
        //    foreach (PropertyDescriptor prop in properties)
        //        table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
        //    foreach (T item in data)
        //    {
        //        DataRow row = table.NewRow();
        //        foreach (PropertyDescriptor prop in properties)
        //            row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
        //        table.Rows.Add(row);
        //    }
        //    return table;
        //}

        public string RemoveHexCharacters(string inString)
        {
            if (inString == null) return null;

            StringBuilder newString = new StringBuilder();
            char ch;

            for (int i = 0; i < inString.Length; i++)
            {

                ch = inString[i];
                // remove any characters outside the valid UTF-8 range as well as all control characters
                // except tabs and new lines
                //if ((ch < 0x00FD && ch > 0x001F) || ch == '\t' || ch == '\n' || ch == '\r')
                //if using .NET version prior to 4, use above logic
                if (XmlConvert.IsXmlChar(ch)) //this method is new in .NET 4
                {
                    newString.Append(ch);
                }
                else
                {
                    newString.Append(" ");
                }
            }
            return newString.ToString();

        }

        public static DataTable CreateTable(IEnumerable<IDictionary<string, object>> parents)
        {
            var table = new DataTable();

            //excuse the meaningless variable names

            var c = parents.FirstOrDefault(x => x.Values
                                                 .OfType<IEnumerable<IDictionary<string, object>>>()
                                                 .Any());
            var p = c ?? parents.FirstOrDefault();
            if (p == null)
                return table;

            var headers = p.Where(x => x.Value is string)
                           .Select(x => x.Key)
                           .Concat(c == null ?
                                   Enumerable.Empty<string>() :
                                   c.Values
                                    .OfType<IEnumerable<IDictionary<string, object>>>()
                                    .First()
                                    .SelectMany(x => x.Keys))
                           .Select(x => new DataColumn(x))
                           .ToArray();
            table.Columns.AddRange(headers);

            foreach (var parent in parents)
            {
                var children = parent.Values
                                     .OfType<IEnumerable<IDictionary<string, object>>>()
                                     .ToArray();

                var length = children.Any() ? children.Length : 1;

                var parentEntries = parent.Where(x => x.Value is string)
                                          .ToLookup(x => x.Key, x => x.Value);
                var childEntries = children.SelectMany(x => x.First())
                                           .ToLookup(x => x.Key, x => x.Value);

                var allEntries = parentEntries.Concat(childEntries)
                                              .ToDictionary(x => x.Key, x => x.ToArray());

                var addedRows = Enumerable.Range(0, length)
                                          .Select(x => new
                                          {
                                              relativeIndex = x,
                                              actualIndex = table.Rows.IndexOf(table.Rows.Add())
                                          })
                                          .ToArray();

                foreach (DataColumn col in table.Columns)
                {
                    object[] columnRows;
                    if (!allEntries.TryGetValue(col.ColumnName, out columnRows))
                        continue;

                    foreach (var row in addedRows)
                        table.Rows[row.actualIndex][col] = columnRows[row.relativeIndex];
                }
            }

            return table;
        }



    }
}
