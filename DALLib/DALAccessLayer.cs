using System;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.Web;
using System.Text;
using System.Xml.Linq;

namespace DALLib
{
    public abstract class DALAccessLayer
    {
        // Private Members                
        private string _connectionString;
        private IDbConnection connection;
        private IDbCommand command;
        private IDbTransaction transaction;

        
        /// <summary>
        /// Gets/sets the string used to open a database.
        /// </summary>
        public string ConnectionString 
        { 
            get 
            {
                // Make sure conection string is not empty.
                if (_connectionString.Trim() == string.Empty || _connectionString.Trim().Length == 0)
                {
                   throw new ArgumentException("The database connection string is invalid.");                    
                }
                else
                {
                    return _connectionString;
                }
            }
            set 
            {
                _connectionString = value;
            }
        }

        // Since this is an abstract class, for better documentation and readability of source code, 
        // class is defined with an explicit protected constructor
        protected DALAccessLayer() { }


        #region Abstract Methods

        /// <summary>
        /// Data provider specific implementation for accessing relational databases.
        /// </summary>
        /// <returns></returns>
        internal abstract IDbConnection GetDataProviderConnection();
        /// <summary>
        /// Data provider specific implementation for executing SQL statement while connected to a data source.
        /// </summary>
        /// <returns></returns>
        internal abstract IDbCommand GetDataProviderCommand();
        /// <summary>
        /// Data provider specific implementation for filling the DataSet.
        /// </summary>
        /// <returns></returns>
        internal abstract IDbDataAdapter GetDataProviderDataAdapter();

        #endregion

        #region Database Transaction

        /// <summary>
        /// Begins a database transaction.
        /// </summary>
        public void BeginTransaction()
        {
            if (transaction != null)
                return;

            try
            {
                // instantiate a connection object
                connection = GetDataProviderConnection();
                connection.ConnectionString = this.ConnectionString;
                // open connection
                connection.Open();
                // begin a database transaction with a read committed isolation level
                transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);
            }
            catch
            {
                connection.Close();

                throw;
            }
        }

        /// <summary>
        /// Commits the database transaction.
        /// </summary>
        public void CommitTransaction()
        {
            if (transaction == null)
                return;

            try
            {
                // Commit transaction
                transaction.Commit();
            }
            catch
            {
                // rollback transaction
                RollbackTransaction();
                throw;
            }
            finally
            {
                connection.Close();
                transaction = null;
            }
        }

        /// <summary>
        /// Rolls back a transaction from a pending state.
        /// </summary>
        public void RollbackTransaction()
        {
            if (transaction == null)
                return;

            try
            {
                transaction.Rollback();
            }
            catch { }
            finally
            {
                connection.Close();
                transaction = null;
            }
        }

        #endregion

        #region ExecuteDataReader

        /// <summary>
        /// Executes the CommandText against the Connection and builds an IDataReader.
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public IDataReader ExecuteDataReader(string commandText)
        {
            return this.ExecuteDataReader(commandText, CommandType.Text, null);
        }

        /// <summary>
        /// Executes the CommandText against the Connection and builds an IDataReader.
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public IDataReader ExecuteDataReader(string commandText, CommandType commandType)
        {
            return this.ExecuteDataReader(commandText, commandType, null);
        }

        /// <summary>
        /// Executes a parameterized CommandText against the Connection and builds an IDataReader.
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public IDataReader ExecuteDataReader(string commandText, IDataParameter[] commandParameters)
        {
            return this.ExecuteDataReader(commandText, CommandType.Text, commandParameters);
        }

        /// <summary>
        /// Executes a stored procedure against the Connection and builds an IDataReader.
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public IDataReader ExecuteDataReader(string commandText, CommandType commandType, IDataParameter[] commandParameters)
        {
            DateTime dtStart = new DateTime();
            DateTime dtEnd = new DateTime();

            try
            {

                PrepareCommand(commandType, commandText, commandParameters);

                IDataReader dr;

                dtStart = System.DateTime.UtcNow;

                if (transaction == null)
                    // Generate the reader. CommandBehavior.CloseConnection causes the
                    // the connection to be closed when the reader object is closed
                    dr = command.ExecuteReader(CommandBehavior.CloseConnection);
                else
                    dr = command.ExecuteReader();

                dtEnd = System.DateTime.UtcNow;

                return dr;

            }
            catch (Exception e)
            {
                if (transaction != null)
                    RollbackTransaction();

                throw;
            }
            finally
            {                               
                command.Dispose();
                command = null;
            }
        }

        #endregion

        #region ExecuteDataSet

        /// <summary>
        /// Adds or refreshes rows in the DataSet to match those in the data source using the DataSet name, and creates a DataTable named "Table".
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(string commandText)
        {
            return this.ExecuteDataSet(commandText, CommandType.Text, null);
        }

        /// <summary>
        /// Adds or refreshes rows in the DataSet to match those in the data source using the DataSet name, and creates a DataTable named "Table".
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(string commandText, CommandType commandType)
        {
            return this.ExecuteDataSet(commandText, commandType, null);
        }

        /// <summary>
        /// Adds or refreshes rows in the DataSet to match those in the data source using the DataSet name, and creates a DataTable named "Table".
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(string commandText, IDataParameter[] commandParameters)
        {
            return this.ExecuteDataSet(commandText, CommandType.Text, commandParameters);
        }

        /// <summary>
        /// Adds or refreshes rows in the DataSet to match those in the data source using the DataSet name, and creates a DataTable named "Table".
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(string commandText, CommandType commandType, IDataParameter[] commandParameters)
        {
            DateTime dtStart = new DateTime();
            DateTime dtEnd = new DateTime();
            try
            {
                PrepareCommand(commandType, commandText, commandParameters);
                //create the DataAdapter & DataSet
                IDbDataAdapter da = GetDataProviderDataAdapter();
                da.SelectCommand = command;
                DataSet ds = new DataSet();

                //fill the DataSet using default values for DataTable names, etc.
                dtStart = System.DateTime.UtcNow;
                da.Fill(ds);
                dtEnd = System.DateTime.UtcNow;
                //return the dataset
                return ds;
            }
            catch
            {
                if (transaction != null)
                    RollbackTransaction();

                throw;
            }
            finally
            {                              
                connection.Close();
                connection.Dispose();
                command.Dispose();
                command = null;
            }
        }

        #endregion

        #region ExecuteDataTable

        /// <summary>
        /// Adds or refreshes rows in the DataSet to match those in the data source using the DataSet name, and creates a DataTable named "Table".
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string commandText)
        {
            return this.ExecuteDataTable(commandText, CommandType.Text, null);
        }

        /// <summary>
        /// Adds or refreshes rows in the DataSet to match those in the data source using the DataSet name, and creates a DataTable named "Table".
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string commandText, CommandType commandType)
        {
            return this.ExecuteDataTable(commandText, commandType, null);
        }

        /// <summary>
        /// Adds or refreshes rows in the DataSet to match those in the data source using the DataSet name, and creates a DataTable named "Table".
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string commandText, IDataParameter[] commandParameters)
        {
            return this.ExecuteDataTable(commandText, CommandType.Text, commandParameters);
        }

        /// <summary>
        /// Adds or refreshes rows in the DataSet to match those in the data source using the DataSet name, and creates a DataTable named "Table".
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string commandText, CommandType commandType, IDataParameter[] commandParameters)
        {
            DateTime dtStart = new DateTime();
            DateTime dtEnd = new DateTime();
            try
            {
                PrepareCommand(commandType, commandText, commandParameters);
                //create the DataAdapter & DataSet

                DataTable dt = new DataTable("Table1");

                IDataReader dr;
               
                //fill the DataSet using default values for DataTable names, etc.
                dtStart = System.DateTime.UtcNow;
                if (transaction == null)
                    // Generate the reader. CommandBehavior.CloseConnection causes the
                    // the connection to be closed when the reader object is closed
                    dr = command.ExecuteReader(CommandBehavior.CloseConnection);
                else
                    dr = command.ExecuteReader();

                dt.Load(dr); 
                dtEnd = System.DateTime.UtcNow;
                dr.Close();
                dr.Dispose();
                //return the datatable
                return dt;
            }
            catch
            {
                if (transaction != null)
                    RollbackTransaction();

                throw;
            }
            finally
            {                          
                connection.Close();
                connection.Dispose();
                command.Dispose();
                command = null;
            }
        }

        #endregion

        #region ExecuteQuery

        /// <summary>
        ///  Executes an SQL statement against the Connection object of a .NET Framework data provider, and returns the number of rows affected.
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public string ExecuteQuery(string commandText)
        {
            return this.ExecuteQuery(commandText, CommandType.Text, null);
        }

        /// <summary>
        /// Executes an SQL statement against the Connection object of a .NET Framework data provider, and returns the number of rows affected.
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public string ExecuteQuery(string commandText, CommandType commandType)
        {
            return this.ExecuteQuery(commandText, commandType, null);
        }

        /// <summary>
        /// Executes an SQL parameterized statement against the Connection object of a .NET Framework data provider, and returns the number of rows affected.
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public string ExecuteQuery(string commandText, IDataParameter[] commandParameters)
        {
            return this.ExecuteQuery(commandText, CommandType.Text, commandParameters);
        }

        /// <summary>
        /// Executes a stored procedure against the Connection object of a .NET Framework data provider, and returns the number of rows affected.
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public string ExecuteQuery(string commandText, CommandType commandType, IDataParameter[] commandParameters)
        {
            DateTime dtStart = new DateTime();
            DateTime dtEnd = new DateTime();
            string strRtn = "";
            try
            {
                PrepareCommand(commandType, commandText, commandParameters);

                // execute command
                dtStart = System.DateTime.UtcNow;
                int intAffectedRows = command.ExecuteNonQuery();
                dtEnd = System.DateTime.UtcNow;

                if (command.Parameters["@pRtnXML"] != null)
                {
                    SqlParameter RtnXML = (SqlParameter)command.Parameters["@pRtnXML"];
                    strRtn = RtnXML.Value.ToString();
                }
                // return no of affected records
                return strRtn;
            }
            catch
            {
                if (transaction != null)
                    RollbackTransaction();

                return "";
            }
            finally
            {
                try
                {
                    connection.Close();
                    connection.Dispose();
                    command.Dispose();
                    command = null;
                }
                catch
                {
                }
            }
        }

        #endregion

        #region ExecuteScalar

        /// <summary>
        /// Executes the query, and returns the first column of the first row in the resultset returned by the query. Extra columns or rows are ignored.
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public object ExecuteScalar(string commandText)
        {
            return this.ExecuteScalar(commandText, CommandType.Text, null);
        }

        /// <summary>
        /// Executes the query, and returns the first column of the first row in the resultset returned by the query. Extra columns or rows are ignored.
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public object ExecuteScalar(string commandText, CommandType commandType)
        {
            return this.ExecuteScalar(commandText, commandType, null);
        }

        /// <summary>
        /// Executes a parameterized query, and returns the first column of the first row in the resultset returned by the query. Extra columns or rows are ignored.
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public object ExecuteScalar(string commandText, IDataParameter[] commandParameters)
        {
            return this.ExecuteScalar(commandText, CommandType.Text, commandParameters);
        }

        /// <summary>
        /// Executes a stored procedure, and returns the first column of the first row in the resultset returned by the query. Extra columns or rows are ignored.
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public object ExecuteScalar(string commandText, CommandType commandType, IDataParameter[] commandParameters)
        {
            DateTime dtStart = new DateTime();
            DateTime dtEnd = new DateTime();
            try
            {
                PrepareCommand(commandType, commandText, commandParameters);

                // execute command
                dtStart = System.DateTime.UtcNow;
                object objValue = command.ExecuteScalar();
                dtEnd = System.DateTime.UtcNow;

                // check on value
                if (objValue != DBNull.Value)
                    // return value
                    return objValue;
                else
                    // return null instead of dbnull value
                    return null;
            }
            catch
            {
                if (transaction != null)
                    RollbackTransaction();

                throw;
            }
            finally
            {                             
                connection.Close();
                connection.Dispose();
                command.Dispose();
                command = null;
            }
        }

        #endregion

        #region ExecuteXMLDocument

        /// <summary>
        /// Adds or refreshes rows in the DataSet to match those in the data source using the DataSet name, and creates a DataTable named "Table".
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public XmlDocument ExecuteXMLDocument(string commandText)
        {
            return this.ExecuteXMLDocument(commandText, CommandType.Text, null);
        }

        /// <summary>
        /// Adds or refreshes rows in the DataSet to match those in the data source using the DataSet name, and creates a DataTable named "Table".
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public XmlDocument ExecuteXMLDocument(string commandText, CommandType commandType)
        {
            return this.ExecuteXMLDocument(commandText, commandType, null);
        }

        /// <summary>
        /// Adds or refreshes rows in the DataSet to match those in the data source using the DataSet name, and creates a DataTable named "Table".
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public XmlDocument ExecuteXMLDocument(string commandText, IDataParameter[] commandParameters)
        {
            return this.ExecuteXMLDocument(commandText, CommandType.Text, commandParameters);
        }

        /// <summary>
        /// Adds or refreshes rows in the DataSet to match those in the data source using the DataSet name, and creates a DataTable named "Table".
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public XmlDocument ExecuteXMLDocument(string commandText, CommandType commandType, IDataParameter[] commandParameters)
        {
            XmlDocument XMLDoc = new XmlDocument();
            DateTime dtStart = new DateTime();
            DateTime dtEnd = new DateTime();
            try
            {
                PrepareCommand(commandType, commandText, commandParameters);

                //fill the XmlDocument using ExecuteXmlReader.
                dtStart = System.DateTime.UtcNow;
                SqlCommand SQLcommand = new SqlCommand();
                SQLcommand = (SqlCommand)command;
                XMLDoc.Load(SQLcommand.ExecuteXmlReader());
                dtEnd = System.DateTime.UtcNow;
                //return the XmlDocument
                return XMLDoc;
            }
            catch
            {
                if (transaction != null)
                    RollbackTransaction();

                throw;
            }
            finally
            {
                connection.Close();
                connection.Dispose();
                command.Dispose();
                command = null;
            }
        }

        #endregion

        #region ExecuteXDocument

        /// <summary>
        /// Adds or refreshes rows in the DataSet to match those in the data source using the DataSet name, and creates a DataTable named "Table".
        /// Works with XDocument in the System.Xml.Linq namespace as opposed to the old W3C XML DOM XmlDocument.
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public XDocument ExecuteXDocument(string commandText)
        {
            return this.ExecuteXDocument(commandText, CommandType.Text, null);
        }

        /// <summary>
        /// Adds or refreshes rows in the DataSet to match those in the data source using the DataSet name, and creates a DataTable named "Table".
        /// Works with XDocument in the System.Xml.Linq namespace as opposed to the old W3C XML DOM XmlDocument.
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public XDocument ExecuteXDocument(string commandText, CommandType commandType)
        {
            return this.ExecuteXDocument(commandText, commandType, null);
        }

        /// <summary>
        /// Adds or refreshes rows in the DataSet to match those in the data source using the DataSet name, and creates a DataTable named "Table".
        /// Works with XDocument in the System.Xml.Linq namespace as opposed to the old W3C XML DOM XmlDocument.
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public XDocument ExecuteXDocument(string commandText, IDataParameter[] commandParameters)
        {
            return this.ExecuteXDocument(commandText, CommandType.Text, commandParameters);
        }

        /// <summary>
        /// Adds or refreshes rows in the DataSet to match those in the data source using the DataSet name, and creates a DataTable named "Table".
        /// Works with XDocument in the System.Xml.Linq namespace as opposed to the old W3C XML DOM XmlDocument.
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public XDocument ExecuteXDocument(string commandText, CommandType commandType, IDataParameter[] commandParameters)
        {
            XDocument xDoc = new XDocument();
            DateTime dtStart = new DateTime();
            DateTime dtEnd = new DateTime();
            try
            {
                PrepareCommand(commandType, commandText, commandParameters);

                //fill the XDocument using ExecuteXmlReader.
                dtStart = System.DateTime.UtcNow;
                SqlCommand SQLcommand = new SqlCommand();
                SQLcommand = (SqlCommand)command;
                xDoc = XDocument.Load(SQLcommand.ExecuteXmlReader());                
                dtEnd = System.DateTime.UtcNow;
                //return the XDocument
                return xDoc;
            }
            catch
            {
                if (transaction != null)
                    RollbackTransaction();

                throw;
            }
            finally
            {
                connection.Close();
                connection.Dispose();
                command.Dispose();
                command = null;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// This method opens (if necessary) and assigns a connection, transaction, command type and parameters 
        /// to the provided command. 
        /// </summary>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        private void PrepareCommand(CommandType commandType, string commandText, IDataParameter[] commandParameters)
        {
            // provide the specific data provider connection object, if the connection object is null
            if (connection == null)
            {
                connection = GetDataProviderConnection();
                connection.ConnectionString = this.ConnectionString;
            }

            // if the provided connection is not open, then open it
            if (connection.State != ConnectionState.Open)
                connection.Open();

            // Provide the specific data provider command object, if the command object is null
            if (command == null)
                command = GetDataProviderCommand();

            // associate the connection with the command
            command.Connection = connection;
            // set the command text (stored procedure name or SQL statement)
            command.CommandText = commandText;
            // set the command type
            command.CommandType = commandType;
            // set timeout to 500
            command.CommandTimeout = 500;
            // if a transaction is provided, then assign it.
            if (transaction != null)
                command.Transaction = transaction;

            // attach the command parameters if they are provided
            if (commandParameters != null)
            {
                foreach (IDataParameter param in commandParameters)
                {
                    if (param != null)
                    {
                        command.Parameters.Add(param);
                    }
                }
            }
        }


        #endregion
    }
}
