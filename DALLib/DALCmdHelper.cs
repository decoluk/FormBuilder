using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.Xml.Linq;
using ConfigurationLib;

namespace DALLib
{
    public static class DALCmdHelper
    {
        #region Constants

        public const string ProcName_VmsMain = "vms_sp_main";
        public const string ProcName_GetActualParams = "vms_sp_main_actual_cmd";
        public const string ParamName_VmsSubCmd = "@pXML";
        
        #endregion

        #region Public Methods

        /// <summary>
        /// WrapCmdIntoViewStateFormat()
        /// This was translated from dbFunctions.BuildXMLSQLCommand()
        /// This special format was created a long time ago for storing SQL commands in the ViewState object.
        /// I put this method is a special static class because it may be needed by many types of objects.
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static string WrapCmdIntoViewStateFormat(SqlCommand cmd)
        {
            // Wrap a VMS-style SqlCommand into our special XML format so that it can be used by our special ExportToExcel methods, etc.
            // Be aware that this could cause some XML encoding issues in some cases because of they way we embed XML in our VMS commands.                
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<r><pn>{0}</pn>", cmd.CommandText);
            for (int i = 0; i < cmd.Parameters.Count - 1; i++)
            {
                sb.AppendFormat("<p n=\"{0}\" ><![CDATA[{1}]]></p>", cmd.Parameters[i].ParameterName, cmd.Parameters[i].Value);
            }
            sb.Append("</r>");
            return sb.ToString();
        }

        /// <summary>
        /// UnWrapCmdFromViewStateFormat()
        /// This was translated from dbFunctions.GetXMLSQLCommand()
        /// This special format was created a long time ago for storing SQL commands in the ViewState object.
        /// I put this method is a special static class because it may be needed by many types of objects.
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static SqlCommand UnWrapCmdFromViewStateFormat(string xml, bool removePaging = true)
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(xml);

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            XmlNode node = xdoc.SelectSingleNode("//pn");
            cmd.CommandText = node.InnerXml;

            XmlNodeList paramlist = xdoc.SelectNodes("//p");
            SqlParameter param;
            for (int i = 0; i < paramlist.Count; i++)
            {
                param = new SqlParameter();
                param.ParameterName = paramlist[i].Attributes["n"].Value;
                param.Value = paramlist[i].InnerText;
                if (param.ParameterName == ParamName_VmsSubCmd)  // This parameter contains the core SP that eventually gets executed.                         
                {
                    if (removePaging)
                    {
                        SqlCommand subcmd = new SqlCommand();
                        if (UnWrapCmdFromXmlParameter(param.Value.ToString(), ref subcmd))
                        {
                            // Extend the range of records returned to effectively return the entire set of data.
                            RemovePagingFromCmd(ref subcmd);
                            param.Value = WrapCmdIntoXmlParameter(subcmd);
                        }
                    }
                }
                cmd.Parameters.Add(param);
            }
            return cmd;
        }

        /// <summary>
        /// WrapCmdIntoXmlParameter()
        /// Take a SqlCommand object and convert it to an xml string that can be passed to vms_sp_main
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="spParams"></param>
        /// <returns></returns>
        public static string WrapCmdIntoXmlParameter(SqlCommand cmd)
        {
            // Wrap SQL Command into an xml parameter that can be passed to vms_sp_main.
            XElement el = new XElement("SPElements", new XElement("name", cmd.CommandText));
            XDocument xml = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), new XComment("Stored Procedure Elements"), el);
            foreach (SqlParameter param in cmd.Parameters)
            {
                xml.Element("SPElements").Add(new XElement("parm",
                                              new XElement("parmName", param.ParameterName),
                                              new XElement("value", param.Value)));
                // We don't care about the data type.
            }
            return xml.ToString();
        }


        public static string WrapParameterIntoXml(SqlCommand cmd)
        {
            // Wrap SQL Command into an xml parameter that can be passed to vms_sp_main.
            XElement el = new XElement("ROOT");
            XDocument xml = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), el);
            foreach (SqlParameter param in cmd.Parameters)
            {
                if (TryParseXml(param.Value.ToString()) == true)
                {
                    XElement xmlValue = XElement.Parse(param.Value.ToString(),
                    LoadOptions.None);
                    xml.Element("ROOT").Add(new XElement(
                                new XElement(param.ParameterName, xmlValue)));
                }
                else
                {
                    xml.Element("ROOT").Add(new XElement(
                                new XElement(param.ParameterName, param.Value)));
                }

                // We don't care about the data type.
            }
            return xml.ToString();
        }

        private static bool TryParseXml(string pXML)
        {
            XmlDocument xml = new XmlDocument();
            try
            {
                xml.LoadXml(pXML);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// UnWrapCmdFromXmlParameter()
        /// Take an xml string intended to be a parameter for vms_sp_main, and convert it into a SqlCommand object.
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static bool UnWrapCmdFromXmlParameter(string xml, ref SqlCommand cmd)
        {
            XDocument xDoc;
            try
            {
                xDoc = XDocument.Parse(xml);
            }
            catch (Exception ex)
            {
                return false;  // Ok to swallow exception here.
            }

            bool foundSPname = false;
            //bool foundSPparam = false; // Not all SPs have parameters.
            XElement nodes = xDoc.Element("SPElements");
            if (nodes != null)
            {
                foreach (XElement node in nodes.Elements())
                {
                    if (node.Name.ToString() == "name")
                    {
                        cmd.CommandText = node.Value.ToString();
                        cmd.CommandType = CommandType.StoredProcedure;
                        foundSPname = true;
                    }
                    else if (node.Name.ToString() == "parm")
                    {
                        XElement parmname = node.Element("parmName");
                        XElement parmvalue = node.Element("value");
                        if (parmname != null && parmvalue != null)
                        {
                            SqlParameter param = new SqlParameter();
                            param.ParameterName = parmname.Value.ToString();
                            param.SqlDbType = SqlDbType.NVarChar;    // All parameters in the sub-command passed to vms_sp_main are eventually converted to NVARCHAR(Max) anyways.
                            param.Value = parmvalue.Value.ToString();
                            cmd.Parameters.Add(param);
                            //foundSPparam = true; // Not all SPs have parameters.
                        }
                    }
                }
            }
            //return (foundSPname && foundSPparam);
            return (foundSPname);   // Not all SPs have parameters.
        }

        public static bool GetActualSubCmdFromMainCmd(IDbCommand mainCmd, Enum_DBConnType DBConnType, ref SqlCommand subcmd)
        {
            bool retval = false;
            if (mainCmd.CommandText.Contains(ProcName_VmsMain))
            {
                foreach (IDataParameter param in mainCmd.Parameters)
                {
                    if (param.ParameterName == ParamName_VmsSubCmd)
                    {
                        string subCmdString = param.Value.ToString();
                        DALCmdBuilder sqlcmd = new DALCmdBuilder();
                        sqlcmd.CreateNewCommand(ProcName_GetActualParams);
                        sqlcmd.AddParameter(ParamName_VmsSubCmd, subCmdString);
                        DALAccessLayer dal = DALAccessLayerFactory.GetDataAccessLayer(DBConnType);
                        string subCmdStringActual = (string)dal.ExecuteScalar(sqlcmd.StoredProcedureName, CommandType.StoredProcedure, sqlcmd.Parameters);
                        dal = null;
                        retval = UnWrapCmdFromXmlParameter(subCmdStringActual, ref subcmd);
                        break;
                    }
                }
            }
            return retval;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// RemovePagingFromCmd()
        /// Takes an SqlCommand object and extends the range of records return to effectively contain all records in the set of data.
        /// </summary>
        /// <param name="cmd"></param>
        private static void RemovePagingFromCmd(ref SqlCommand cmd)
        {
            foreach (SqlParameter parm in cmd.Parameters)
            {
                switch (parm.ParameterName)
                {
                    case "@StartAt":
                    case "@start_at":
                        parm.Value = "0";
                        break;
                    case "@NumOfRecs":
                    case "@num_of_recs":
                    case "@NumberOfRecords":
                    case "@number_of_records":
                        parm.Value = "50000";
                        break;
                    default:
                        break;
                }
            }
        }

        #endregion
    }
   
}
