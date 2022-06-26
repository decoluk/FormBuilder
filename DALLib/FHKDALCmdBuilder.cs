using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Web;

namespace DALLib
{
    public class SYSDALCmdBuilder
    {
        private DALCmdBuilder _DALCmdBuilder = new DALCmdBuilder();

        public string ActualStoredProcedureName { get { return _DALCmdBuilder.StoredProcedureName; } } 

        public bool IgnoreStandardParameters { get; set; }

        public string _XMLValue { get; set; }

        #region Public Methods

        /// <summary>
        /// Use this method to start building a new command. (Any existing commands will be cleared)
        /// </summary>
        /// <param name="storedProcName"></param>
        public void CreateNewCommand(string storedProcName, bool clearAllParams = true)
        {
            _DALCmdBuilder.CreateNewCommand(storedProcName, clearAllParams);
        }

        /// <summary>
        /// Use this method to start building a new command. (Any existing commands will be cleared)
        /// You can pass custom BuildWhereClause name and NextPage name to this method.
        /// </summary>
        /// <param name="storedProcName"></param>
        /// <param name="buildWCName"></param>
        /// <param name="nextPageName"></param>
        public void CreateNewCommand(string storedProcName, string buildWCName, string nextPageName, bool clearAllParams = true)
        {
            _DALCmdBuilder.CreateNewCommand(storedProcName, clearAllParams);
        }

        /// <summary>
        /// Adds a string parameter to the SQL command.
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="paramValue"></param>
        public void AddParameter(string paramName, string paramValue)
        {
            _DALCmdBuilder.AddParameter(paramName, paramValue);
        }

        /// <summary>
        /// Adds an int parameter to the SQL command.
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="paramValue"></param>
        public void AddParameter(string paramName, int paramValue)
        {
            _DALCmdBuilder.AddParameter(paramName, paramValue);
        }

        /// <summary>
        /// Adds a bool parameter to the SQL command.
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="paramValue"></param>
        public void AddParameter(string paramName, bool paramValue)
        {
            _DALCmdBuilder.AddParameter(paramName, paramValue);
        }

        /// <summary>
        /// Adds a uniqueidentifier parameter to the SQL command.
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="paramValue"></param>
        public void AddParameter(string paramName, Guid paramValue)
        {
            _DALCmdBuilder.AddParameter(paramName, paramValue);
        }

        public void AddParameter(string paramName, byte[] paramValue)
        {
            _DALCmdBuilder.AddParameter(paramName, paramValue);
        }

        /// <summary>
        /// ClearParameters(). Clear all parameters on the command object.
        /// </summary>
        public void ClearParameters()
        {
            _DALCmdBuilder.ClearParameters();
        }

        #endregion

        public IDataParameter[] Parameters
        {
            get
            {
                var parameters = new SqlParameter[5];

                var value = DALCmdHelper.WrapParameterIntoXml(_DALCmdBuilder.Command);
                parameters[0] = new SqlParameter("@pXML", SqlDbType.Xml) { Value = value };
                parameters[1] = new SqlParameter("@pRtnXML", SqlDbType.Xml) { Value = "",Direction = ParameterDirection.Output };
                return parameters;
            }
        }

        public IDataParameter[] WSParameters
        {
            get
            {
                var parameters = new SqlParameter[5];

                parameters[0] = new SqlParameter("@pXML", SqlDbType.Xml) { Value = _XMLValue };
                parameters[1] = new SqlParameter("@pRtnXML", SqlDbType.Xml) { Value = "", Direction = ParameterDirection.Output };
                return parameters;
            }
        }

        public string GetParameterXML()
        {
            return DALCmdHelper.WrapParameterIntoXml(_DALCmdBuilder.Command);
        }
    }
}
