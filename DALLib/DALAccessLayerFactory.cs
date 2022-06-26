using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConfigurationLib;

namespace DALLib
{
    public sealed class DALAccessLayerFactory
    {
        private DALAccessLayerFactory() { }

        public static DALAccessLayer GetDataAccessLayer(Enum_DBConnType DBConnType)
        {
            // return data access layer provider
            return new SqlDataAccessLayer(DBConnType);   // DBConfig is a singleton.         
        }
    }
}
