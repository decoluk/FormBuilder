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
    public sealed class DALObjectWS
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


    }
}
