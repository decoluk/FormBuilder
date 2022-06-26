
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using ConfigurationLib;
using DALLib;
using Microsoft.Extensions.Configuration; 

namespace ModelLib
{
    public class Models : DALBLObject
    {
        protected readonly IConfiguration Configuration;

        public enum StoredProcedures
        {
            spaExecuteSP,
            spaPermission
        }

        private void InitializeStoredProcedures()
        {
            SpAdd(StoredProcedures.spaExecuteSP, "dbo.spaExecuteSP");
            SpAdd(StoredProcedures.spaPermission, "dbo.spaPermission");
        }

        public Models()
        {
            InitializeStoredProcedures();
            /*** MUST SETTING ****/
            this.DBConnType = Enum_DBConnType.FORM_BUILDER;  /************** MAKE THE CONNECTION STRING TARGET SERVER LOCATION ****************/
            
            SYSConfig.Instance.SYSConnectionKey = ConfigurationLib.SYSGlobalVariable._ConnectionString;

            this.SYSTEM_DB_CONNECTION_TYPE = Enum.GetName(typeof(Enum_DBConnType), this.DBConnType).ToString();
            this.IsWebServiceMode = ConfigurationLib.SYSGlobalVariable._IsWebServiceMode;
        }


        #region Entity
        public bool SetEntity(string pXML)
        {
            try
            {
                CreateNewSqlCommand(StoredProcedures.spaExecuteSP);
                AddSqlCmdParameter("SQL_STORED_PROCEDURE", "spaEntity");
                AddSqlCmdParameter("TYPE", "ADD");
                AddSqlCmdParameter("DATA", pXML);
                string ResultXML = ExecuteNonQuery();
                if (DataConvertLib.Instance.GetXMLElementValue(ResultXML, "RESULT") == "SUCCESS")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool GetEntity(ref mfbEntityViewModel pList)
        {
            try
            {
                string pXML = "";
                CreateNewSqlCommand(StoredProcedures.spaExecuteSP);
                AddSqlCmdParameter("SQL_STORED_PROCEDURE", "spaEntity");
                AddSqlCmdParameter("TYPE", "GET_ENTITY");

                string ResultXML = ExecuteNonQuery();
                if (DataConvertLib.Instance.GetXMLElementValue(ResultXML, "RESULT") == "SUCCESS")
                {
                    pList = new mfbEntityViewModel();
                    pXML = DataConvertLib.Instance.GetXMLElementValue(ResultXML, "VIEWRESULT");
                    if (!string.IsNullOrEmpty(pXML))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(mfbEntityViewModel));
                        pList = (mfbEntityViewModel)serializer.Deserialize(new StringReader(pXML));
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool GetEntityList(ref mfbEntity_Collection pList)
        {
            try
            {
                string pXML = "";
                CreateNewSqlCommand(StoredProcedures.spaExecuteSP);
                AddSqlCmdParameter("SQL_STORED_PROCEDURE", "spaEntity");
                AddSqlCmdParameter("TYPE", "GET_ENTITY_LIST");

                string ResultXML = ExecuteNonQuery();
                if (DataConvertLib.Instance.GetXMLElementValue(ResultXML, "RESULT") == "SUCCESS")
                {
                    pList = new mfbEntity_Collection();
                    pXML = DataConvertLib.Instance.GetXMLElementValue(ResultXML, "VIEWRESULT");
                    if (!string.IsNullOrEmpty(pXML))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(mfbEntity_Collection));
                        pList = (mfbEntity_Collection)serializer.Deserialize(new StringReader(pXML));
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

    }

    #region ViewModels

    public class mfbEntityViewModel
    {
        public mfbEntity _mfbEntity { get; set; }
        public mfbEntityColumn_Collection _mfbEntityColumn_Collection { get; set; }
    }

    [XmlRoot("mfbEntity_COLLECTION")]
    public class mfbEntity_Collection
    {
        [XmlElement("mfbEntity_ITEM")]
        public List<mfbEntity> tbl_mfbEntity = new List<mfbEntity>();
    }

    [XmlRoot("mfbEntityColumn_COLLECTION")]
    public class mfbEntityColumn_Collection
    {
        [XmlElement("mfbEntityColumn_ITEM")]
        public List<mfbEntityColumn> tbl_mfbEntityColumn = new List<mfbEntityColumn>();
    }

    #endregion
}