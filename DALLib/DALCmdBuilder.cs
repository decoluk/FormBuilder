using System;
using System.Collections;
using System.Data.SqlClient;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Data;
using System.Data.Common;

namespace DALLib
{
    public class DALCmdBuilder
    {
        #region Private Members

        private string _storedProcName = string.Empty;
        private ArrayList _params = new ArrayList();

        #endregion

        #region Properties

        public string StoredProcedureName { get { return _storedProcName; } } // No setter because we want users to use CreateNewCommand() 

        // Not sure if we are using this or not, but it is here if we need it.   
        public IDataParameter[] Parameters
        {
            get
            {
                IDataParameter[] parameters = new SqlParameter[_params.Count];
                int c = 0;
                foreach (SqlParameter param in _params)
                {
                    parameters[c] = new SqlParameter(param.ParameterName, param.SqlDbType, param.Size);
                    parameters[c++].Value = param.Value;
                }
                return parameters;
            }
        }

        public string ParameterString
        {
            get
            {
                return BuildParameterString();
            }
        }

        // Build a SqlCommand object from the stored procedure name and array of parameters in _params().
        public SqlCommand Command
        {
            get
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = _storedProcName;
                cmd.CommandType = CommandType.StoredProcedure;
                foreach (SqlParameter param in _params)
                {
                    SqlParameter parm = new SqlParameter();
                    parm.ParameterName = param.ParameterName;
                    parm.Value = param.Value.ToString();
                    // We don't care about the data type.
                    cmd.Parameters.Add(parm);
                }
                return cmd;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor for SqlCmdBuilder class.
        /// </summary>        
        public DALCmdBuilder()
        {

        }

        /// <summary>
        /// Constructor for SqlCmdBuilder class that sets the Stored Procedure Name and clears all parameters..
        /// </summary>
        /// <param name="storedProcName"></param>
        public DALCmdBuilder(string storedProcName)
        {
            this.CreateNewCommand(storedProcName);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// This will set the stored procedure name, and clear all of the parameters.
        /// Use CreateNewCommand() to clear everything and start over.
        /// The main reason for having CreateNewCommand() is that it clears all parameters.
        /// clearAllParams defaults to true.
        /// However, we still leave the developer with the option of not clearing the parameters.
        /// </summary>
        /// <param name="storedProcName"></param>
        /// <param name="clearAllParams"></param>
        public void CreateNewCommand(string storedProcName, bool clearAllParams = true)
        {
            _storedProcName = storedProcName.Trim();  // A leading or trailing space in the SP name will cause vms_sp_main to behave unexpectedly.

            // Leave the developer with the option of NOT clearing the parameters.                
            // This is useful when the developer needs to add custom parameters to a standand command.
            if (clearAllParams)
            {
                _params.Clear();
            }
        }

        /// <summary>
        /// Adds a string parameter to the SQL command.
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="paramValue"></param>
        public void AddParameter(string paramName, XmlDocument paramValue)
        {
            if (!ParameterExists(paramName))  // Avoid adding a parameter more than once on a command object.
            {
                SqlParameter param = new SqlParameter(paramName, SqlDbType.Xml);
                param.Value = paramValue;
                _params.Add(param);
            }
        }
        /// <summary>
        /// Adds a string parameter to the SQL command.
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="paramValue"></param>
        public void AddParameter(string paramName, string paramValue)
        {
            if (!ParameterExists(paramName))  // Avoid adding a parameter more than once on a command object.
            {
                if (paramValue == null)
                {
                    paramValue = string.Empty;
                }
                SqlParameter param = new SqlParameter(paramName, SqlDbType.VarChar, paramValue.Length);
                param.Value = paramValue;
                _params.Add(param);
            }
        }

        /// <summary>
        /// Adds an int parameter to the SQL command.
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="paramValue"></param>
        public void AddParameter(string paramName, int paramValue)
        {
            if (!ParameterExists(paramName))  // Avoid adding a parameter more than once on a command object.
            {
                SqlParameter param = new SqlParameter(paramName, SqlDbType.Int);
                param.Value = paramValue;
                _params.Add(param);
            }
        }

        /// <summary>
        /// Adds a bit parameter to the SQL command.
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="paramValue"></param>
        public void AddParameter(string paramName, bool paramValue)
        {
            if (!ParameterExists(paramName))  // Avoid adding a parameter more than once on a command object.
            {
                SqlParameter param = new SqlParameter(paramName, SqlDbType.Bit);
                param.Value = paramValue ? 1 : 0;
                _params.Add(param);
            }
        }

        /// <summary>
        /// Adds a uniqueidentifier parameter to the SQL command.
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="paramValue"></param>
        public void AddParameter(string paramName, Guid paramValue)
        {
            if (!ParameterExists(paramName))  // Avoid adding a parameter more than once on a command object.
            {
                SqlParameter param = new SqlParameter(paramName, SqlDbType.UniqueIdentifier);
                param.Value = paramValue.ToString();
                _params.Add(param);
            }
        }

        public void AddParameter(string paramName, byte[] paramValue)
        {
            if (!ParameterExists(paramName))  // Avoid adding a parameter more than once on a command object.
            {
                SqlParameter param = new SqlParameter(paramName, SqlDbType.VarBinary);
                param.Value = paramValue.ToString();
                _params.Add(param);
            }
        }

        /// <summary>
        /// ClearParameters(). Clear all parameters on the command object.
        /// </summary>
        public void ClearParameters()
        {
            _params.Clear();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// ParameterExists()
        /// This can be used to prevent adding a parameter to a command more than once.
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        private bool ParameterExists(string paramName)
        {
            foreach (SqlParameter param in _params)
            {
                if (param.ParameterName == paramName)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// BuildParameterString()
        /// </summary>
        /// <returns></returns>
        private string BuildParameterString()
        {
            StringBuilder cmd = new StringBuilder();

            bool isFirstParam = true;
            foreach (SqlParameter param in _params)
            {
                cmd.Append(isFirstParam ? "" : ", ");
                isFirstParam = false;

                cmd.Append(param.ParameterName);
                cmd.Append("=");

                switch (param.SqlDbType)
                {

                    case SqlDbType.Char:
                    case SqlDbType.NChar:
                    case SqlDbType.NText:
                    case SqlDbType.NVarChar:
                    case SqlDbType.Text:
                    case SqlDbType.VarChar:
                    case SqlDbType.Xml:
                    case SqlDbType.UniqueIdentifier:
                    case SqlDbType.Variant:
                        // String types require single quotes.
                        cmd.Append("'");
                        cmd.Append(param.Value.ToString());
                        cmd.Append("'");
                        break;

                    case SqlDbType.BigInt:
                    case SqlDbType.Binary:
                    case SqlDbType.Bit:
                    case SqlDbType.Date:
                    case SqlDbType.DateTime:
                    case SqlDbType.DateTime2:
                    case SqlDbType.DateTimeOffset:
                    case SqlDbType.Decimal:
                    case SqlDbType.Float:
                    case SqlDbType.Image:
                    case SqlDbType.Int:
                    case SqlDbType.Money:
                    case SqlDbType.Real:
                    case SqlDbType.SmallDateTime:
                    case SqlDbType.SmallInt:
                    case SqlDbType.SmallMoney:
                    case SqlDbType.Time:
                    case SqlDbType.Timestamp:
                    case SqlDbType.TinyInt:
                    case SqlDbType.Udt:
                    case SqlDbType.VarBinary:
                        cmd.Append(param.Value.ToString());
                        break;

                    default:
                        // Cast these to string, essentially.
                        cmd.Append("'");
                        cmd.Append(param.Value.ToString());
                        cmd.Append("'");
                        break;
                }
            }
            return cmd.ToString();
        }

        #endregion                    
    }
}
