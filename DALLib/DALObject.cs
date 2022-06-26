using System;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Xml.Linq;
using System.Xml;
using System.Collections.Generic;
using ConfigurationLib;

namespace DALLib
{
    public sealed class DALObject
    {
        List<string> _sqlCmdExecuted = new List<string>();

        private Enum_DBConnType _DBConnType;

        private SYSDALCmdBuilder _cmdbd = new SYSDALCmdBuilder(); 

        public List<string> SqlCmdHistory
        {
            get { return _sqlCmdExecuted; }
        }

        public Enum_DBConnType DBConnType
        {
            get { return _DBConnType; }
            set
            {
                _DBConnType = value;
            }
        }

        public void CreateNewCommand(string storedProcedureName, bool clearAllParams = true)
        {
            // Note that _buildWCName and _nextPageName could have been set from the outside using the corresponding Public Properties on this object.
            _cmdbd.CreateNewCommand(storedProcedureName, clearAllParams);
        }

        /// <summary>
        /// Add a string parameter
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="paramValue"></param>
        public void AddCmdParameter(string paramName, string paramValue)
        {
            _cmdbd.AddParameter(paramName, paramValue);
        }

        /// <summary>
        /// Add an int parameter
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="paramValue"></param>
        public void AddCmdParameter(string paramName, int paramValue)
        {
            _cmdbd.AddParameter(paramName, paramValue);
        }

        /// <summary>
        /// Adds a bit as a parameter.
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="paramValue"></param>
        public void AddCmdParameter(string paramName, bool paramValue)
        {
            _cmdbd.AddParameter(paramName, paramValue);
        }

        /// <summary>
        /// Adds a uniqueidentifier as a parameter.
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="paramValue"></param>
        public void AddCmdParameter(string paramName, Guid paramValue)
        {
            _cmdbd.AddParameter(paramName, paramValue);
        }

        public void AddCmdParameter(string paramName, byte[] paramValue)
        {
            _cmdbd.AddParameter(paramName, paramValue);
        }

        /// <summary>
        /// ClearParameters(). Clear all parameters on the command object.
        /// </summary>
        public void ClearCmdParameters()
        {
            _cmdbd.ClearParameters();
        }

        /// <summary>
        /// GetDataTable(0
        /// </summary>
        /// <returns></returns>
        public DataTable GetDataTable()
        {
            DALAccessLayer dal = DALAccessLayerFactory.GetDataAccessLayer(_DBConnType);  // Always use a new GetDataAccessLayer so that commands, etc. are cleared.
            return dal.ExecuteDataTable(_cmdbd.ActualStoredProcedureName, CommandType.StoredProcedure, _cmdbd.Parameters);
        }

        /// <summary>
        /// GetDataSet()
        /// </summary>
        /// <returns></returns>
        public DataSet GetDataSet()
        {
            DALAccessLayer dal = DALAccessLayerFactory.GetDataAccessLayer(_DBConnType);  // Always use a new GetDataAccessLayer so that commands, etc. are cleared.
            return dal.ExecuteDataSet(_cmdbd.ActualStoredProcedureName, CommandType.StoredProcedure, _cmdbd.Parameters);
        }

        /// <summary>
        /// GetDataReader
        /// </summary>
        /// <returns></returns>
        public SqlDataReader GetDataReader()
        {
            DALAccessLayer dal = DALAccessLayerFactory.GetDataAccessLayer(_DBConnType); // Always use a new GetDataAccessLayer so that commands, etc. are cleared.
            return (SqlDataReader)dal.ExecuteDataReader(_cmdbd.ActualStoredProcedureName, CommandType.StoredProcedure, _cmdbd.Parameters);
        }

        /// <summary>
        /// GetScalar
        /// </summary>
        /// <returns></returns>
        public object GetScalar()
        {
            DALAccessLayer dal = DALAccessLayerFactory.GetDataAccessLayer(_DBConnType);         // Always use a new GetDataAccessLayer so that commands, etc. are cleared.
            return (object)dal.ExecuteScalar(_cmdbd.ActualStoredProcedureName, CommandType.StoredProcedure, _cmdbd.Parameters);
        }

        /// <summary>
        /// ExecuteNonQuery
        /// </summary>
        public string ExecuteNonQuery()
        {
            DALAccessLayer dal = DALAccessLayerFactory.GetDataAccessLayer(_DBConnType);        // Always use a new GetDataAccessLayer so that commands, etc. are cleared.
            return dal.ExecuteQuery(_cmdbd.ActualStoredProcedureName, CommandType.StoredProcedure, _cmdbd.Parameters);
        }

        public string ExecuteWSQuery(string pXML)
        {
            DALAccessLayer dal = DALAccessLayerFactory.GetDataAccessLayer(_DBConnType);        // Always use a new GetDataAccessLayer so that commands, etc. are cleared.
            _cmdbd._XMLValue = pXML;
            return dal.ExecuteQuery(_cmdbd.ActualStoredProcedureName, CommandType.StoredProcedure, _cmdbd.WSParameters);
        }

        public bool GetParameterXML(ref string pXML)
        {
            try
            {
                pXML = _cmdbd.GetParameterXML();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string ExecuteQuery()
        {
            DALAccessLayer dal = DALAccessLayerFactory.GetDataAccessLayer(_DBConnType);        // Always use a new GetDataAccessLayer so that commands, etc. are cleared.
            return dal.ExecuteQuery(_cmdbd.ActualStoredProcedureName, _cmdbd.Parameters);
        
        }

        /// <summary>        
        /// GetStringFromDataReader   
        /// </summary>                
        public string GetStringFromDataReader()
        {
            StringBuilder stringresult = new StringBuilder();
            using (SqlDataReader dr = GetDataReader())
            {
                while (dr.Read())
                {
                    stringresult.Append(Convert.ToString(dr[0]));
                }
                dr.Close(); dr.Dispose();
            }
            return stringresult.ToString();
        }

        /// <summary>
        /// GetXMLDoc
        /// </summary>
        /// <returns></returns>
        public XmlDocument GetXMLDoc()
        {
            DALAccessLayer dal = DALAccessLayerFactory.GetDataAccessLayer(_DBConnType);
            return dal.ExecuteXMLDocument(_cmdbd.ActualStoredProcedureName, CommandType.StoredProcedure, _cmdbd.Parameters);
        }

        /// <summary>
        /// Returns a Linq to XML XDocument as opposed to a W3C XML DOM XmlDocument. These are much easier to work with.
        /// </summary>
        /// <returns></returns>
        public XDocument GetXDoc()
        {
            DALAccessLayer dal = DALAccessLayerFactory.GetDataAccessLayer(_DBConnType);
            return dal.ExecuteXDocument(_cmdbd.ActualStoredProcedureName, CommandType.StoredProcedure, _cmdbd.Parameters);
        }
    }
}
