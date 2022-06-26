using System;
using System.Data;
using System.Data.SqlClient;
using ConfigurationLib;

namespace DALLib
{
    public class SqlDataAccessLayer : DALAccessLayer
    {
        #region Constructors

        public SqlDataAccessLayer() { }
        public SqlDataAccessLayer(Enum_DBConnType DBConnType) 
        {
            string connectionString = "";

            switch (DBConnType)
            {
                case Enum_DBConnType.FORM_BUILDER:
                    connectionString = SYSConfig.Instance.SYSConnectionKey;
                    break;
             
            }

            this.ConnectionString = connectionString; 
        }

        #endregion

        internal override IDbConnection GetDataProviderConnection()
        {
            return new SqlConnection();
        }

        internal override IDbCommand GetDataProviderCommand()
        {
            return new SqlCommand();
        }

        /// <summary>
        /// GetDataProviderDataAdapter
        /// </summary>
        /// <returns></returns>
        internal override IDbDataAdapter GetDataProviderDataAdapter()
        {
            return new SqlDataAdapter();
        }
    }
}
