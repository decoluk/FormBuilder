using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using ConfigurationLib;
using Microsoft.Extensions.Configuration;

namespace DALLib
{
    public sealed class SYSConfig
    {
        private string _SYSConnectionString;
      

        private static readonly SYSConfig _instance = new SYSConfig();

        public string SYSConnectionKey
        {
            get { return _SYSConnectionString; }
            set { _SYSConnectionString = value; }
        }


        public static SYSConfig Instance
        {
            get {
                
                return _instance; 
            }
        }

        SYSConfig()
        {
            //GET CONNECTION STRING FROM REGEDIT 
            //_SYSConnectionString = "";
            // ConfigurationManager.
            //this.Configuration.GetSection("AppSettings")["Site"]
            //var builder = new ConfigurationBuilder();
            //IConfigurationRoot config = builder.Build();
            if (_SYSConnectionString == null)
                _SYSConnectionString = ConfigurationLib.SYSGlobalVariable._ConnectionString;
            //if (KEY_IS_DECRYPT == "F")
            //{
            //    _SYSConnectionString = ConfigurationManager.AppSettings["FHK_DB_CONNECTION_STRING"];
            //}
            //else
            //{
            //    //INIT CONNECTION STRING BY KEY
            //    //byte[] passwordBytes = EncryptionLib.GetPasswordBytes(FHKGlobalVariable._SystemEncodeKey);

            //    //if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["FHKDATASYNC_DB_CONNECTION_STRING"]))
            //    //{
            //    //    _FHKDataSyncConnectionKey = EncryptionLib.Decrypt(ConfigurationManager.AppSettings["FHKDATASYNC_DB_CONNECTION_STRING"], passwordBytes);
            //    //}
            //}

        }

    }
}
