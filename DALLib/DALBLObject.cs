using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Data.SqlClient;
using ConfigurationLib;

namespace DALLib
{
    public abstract class DALBLObject
    {
        private DALObject dalobj = new DALObject();

        private Dictionary<Enum, String> spLookup = new Dictionary<Enum, String>();

        protected string SPName { get; set; }

        protected string SYSTEM_DB_CONNECTION_TYPE { get; set; }

        protected bool IsWebServiceMode { get; set; }

        private Enum_DBConnType _DBConnType;

        protected string _WebServicePath { get; set; }

        // private backing field
        private Enum _storedProcedure;
        /// <summary>
        /// Stored Procedure property.
        /// </summary>
        protected Enum StoredProcedure
        {
            get { return _storedProcedure; }
            set
            {
                _storedProcedure = value;
                SPName = spLookup[_storedProcedure];
            }
        }

        protected Enum_DBConnType DBConnType
        {
            get { return _DBConnType; }
            set
            {
                _DBConnType = value;
                dalobj.DBConnType = value;
            }
        }

        protected void SpAdd(Enum e, string pName)
        {

            spLookup.Add(e, pName);
        }

        public void CreateNewSqlCommand(Enum spKey, bool clearAllParams = true)
        {
            StoredProcedure = spKey;  // Must set this in order for the "_Standard()" methods to work. (e.g. when calling methods from outside this class)
            if (ConfigurationLib.SYSGlobalVariable._IsWebServiceMode == false)
            {
                CreateNewSqlCommand(spLookup[spKey], clearAllParams);
            }
            else
            {
                dalobj = new DALObject();
                dalobj.AddCmdParameter("SYSTEM_STORE_PROCEDURE", Enum.GetName(spKey.GetType(), spKey).ToString());
            }
        }

        protected void CreateNewSqlCommand(string storedProcedureName, bool clearAllParams = true)
        {
            // Note that _buildWCName and _nextPageName could have been set from the outside using the corresponding Public Properties on this object.
            if (ConfigurationLib.SYSGlobalVariable._IsWebServiceMode == false)
            {
                dalobj.CreateNewCommand(storedProcedureName, clearAllParams);
            }
            else
            {
                dalobj.AddCmdParameter("SYSTEM_STORE_PROCEDURE", storedProcedureName);
            }
        }

        protected void AddSqlCmdParameter(string paramName, string paramValue)
        {
            dalobj.AddCmdParameter(paramName, paramValue);
        }

        protected void AddSqlCmdParameter(string paramName, bool paramValue)
        {
            dalobj.AddCmdParameter(paramName, paramValue);
        }

        protected void AddSqlCmdParameter(string paramName, Guid paramValue)
        {
            dalobj.AddCmdParameter(paramName, paramValue);
        }

        protected void AddSqlCmdParameter(string paramName, byte[] paramValue)
        {
            //byte data convert to base64tostring
            dalobj.AddCmdParameter(paramName, paramValue);
        }

        protected DataTable GetDataTable()
        {
            return dalobj.GetDataTable();
        }

        protected DataTable GetDataTable(string tableName)
        {
            DataTable dt = dalobj.GetDataTable();
            dt.TableName = tableName;
            return dt;
        }

        protected DataSet GetDataSet()
        {
            return dalobj.GetDataSet();
        }

        protected SqlDataReader GetDataReader()
        {
            return dalobj.GetDataReader();
        }

        protected object GetScalar()
        {
            return dalobj.GetScalar();
        }

        protected string ExecuteQuery()
        {
            SetDefaultParameter();
            return dalobj.ExecuteQuery();
        }

        protected string ExecuteWSQuery(string pXML)
        {
            return dalobj.ExecuteWSQuery(pXML);
        }


        protected bool ExecuteByWebServices(ref string pResult)
        {
            try
            {
                SetDefaultParameter();
                string pXML = "";
                if (dalobj.GetParameterXML(ref pXML))
                {
                    if (GetPost(ConfigurationLib.DataConvertLib.Instance.GetStringToBase64(pXML), ref pResult))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        private bool GetPost(string pXML, ref string pResult)
        {
            try
            {
                //XDocument Sendingxml = xml;

                // Create a request using a URL that can receive a post. 
                WebRequest request =
                    WebRequest.Create(ConfigurationLib.SYSGlobalVariable._WebServicePath);
                // Set the Method property of the request to POST.
                request.Method = "POST";

                // Create POST data and convert it to a byte array.
                string postData = pXML;    //My problem is here as I need postData  as XDocument.

                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                // Set the ContentType property of the WebRequest.
                request.ContentType = "application/x-www-form-urlencoded";
                // Set the ContentLength property of the WebRequest.
                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
                // Get the response.
                WebResponse response = request.GetResponse();
                // Display the status.
                Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                // Get the stream containing content returned by the server.
                dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                pResult = reader.ReadToEnd();
                // Display the content.
                //MessageBox.Show(responseFromServer);
                // Clean up the streams.
                reader.Close();
                dataStream.Close();
                response.Close();
                if (pResult == "FAIL" || string.IsNullOrEmpty(pResult))
                {
                    return false;
                }
                else
                {
                    pResult = ConfigurationLib.DataConvertLib.Instance.GetBase64ToString(pResult);
                    return true;
                }
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        protected string ExecuteNonQuery()
        {
            try
            {
                SetDefaultParameter();
                return dalobj.ExecuteNonQuery();
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        protected XmlDocument GetXMLDoc()
        {
            return dalobj.GetXMLDoc();
        }

        protected void SetDefaultParameter()
        {
            dalobj.AddCmdParameter("coCode", ConfigurationLib.SYSGlobalVariable.CompNo);
            dalobj.AddCmdParameter("usCode", ConfigurationLib.SYSGlobalVariable.usCode);
            dalobj.AddCmdParameter("nlCode", ConfigurationLib.SYSGlobalVariable.nlCode);
            dalobj.AddCmdParameter("shpCode", ConfigurationLib.SYSGlobalVariable.shpCode);
            dalobj.AddCmdParameter("cmCode", ConfigurationLib.SYSGlobalVariable.cmCode);
            dalobj.AddCmdParameter("SYSTEM_DB_CONNECTION_TYPE", SYSTEM_DB_CONNECTION_TYPE);
        }
    }
}
