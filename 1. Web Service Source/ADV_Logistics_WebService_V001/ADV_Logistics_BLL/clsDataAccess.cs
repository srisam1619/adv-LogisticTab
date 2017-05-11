using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Net.Mail;
using System.Text;
using System.IO;
using System.Drawing;
using System.Collections.Generic;

namespace ADV_Logistics_BLL
{
    public class clsDataAccess
    {
        clsLog oLog = new clsLog();
        public string sErrDesc = string.Empty;
        SAPbobsCOM.Company oDICompany;
        string RTN_SUCCESS = "SUCCESS";

        public static string ConnectionString = ConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString;
        string sEmailFrom = ConfigurationManager.AppSettings["EmailFrom"];
        string sSMTPUser = ConfigurationManager.AppSettings["SMTPUser"];
        string sSMTPPassword = ConfigurationManager.AppSettings["SMTPPassword"];
        string sSMTPHost = ConfigurationManager.AppSettings["SMTPHost"];
        int iSMTPPort = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"]);
        int iSMTPConnTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPConnTimeout"]);
        string sServiceMailId = ConfigurationManager.AppSettings["ServiceMailId"];

        #region Login

        public DataSet LoginValidation(DataSet oDTCompanyList, string sUserName, string sPassword, string sCompany)
        {
            DataSet oDataset = new DataSet();
            string sFuncName = string.Empty;
            string sProcName = string.Empty;
            DataView oDTView = new DataView();

            try
            {
                sFuncName = "LoginValidation()";
                sProcName = "VS_LG_SP020_Web_LoginValidation";

                oLog.WriteToDebugLogFile("Starting Function ", sFuncName);

                oLog.WriteToDebugLogFile("Calling Run_StoredProcedure() " + sProcName, sFuncName);

                if (oDTCompanyList != null && oDTCompanyList.Tables.Count > 0)
                {
                    oDTView = oDTCompanyList.Tables[0].DefaultView;

                    oDTView.RowFilter = "U_DBName= '" + sCompany + "'";
                    if (oDTView != null && oDTView.Count > 0)
                    {
                        oDataset = SqlHelper.ExecuteDataSet(oDTView[0]["U_ConnString"].ToString(), CommandType.StoredProcedure, sProcName,
                            Data.CreateParameter("@UserCode", sUserName), Data.CreateParameter("@Password", sPassword), Data.CreateParameter("@Company", sCompany));
                        oLog.WriteToDebugLogFile("Completed With SUCCESS  ", sFuncName);
                    }
                    else
                    {
                        oLog.WriteToDebugLogFile("No Data in OUSR table for the selected Company", sFuncName);
                        return oDataset;
                    }
                }
                else
                {
                    oLog.WriteToDebugLogFile("There is No Company List in the Holding Company ", sFuncName);
                    return oDataset;
                }

                return oDataset;
            }
            catch (Exception Ex)
            {
                sErrDesc = Ex.Message.ToString();
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                throw Ex;
            }
        }

        #endregion

        #region Driver Management Module
        public DataSet Get_CompanyList()
        {
            DataSet oDataset;
            string sFuncName = string.Empty;
            string sProcName = string.Empty;

            try
            {
                sFuncName = "Get_CompanyList()";
                sProcName = "VS_SP001_Web_GetCompanyList";
                oLog.WriteToDebugLogFile("Starting Function ", sFuncName);

                oLog.WriteToDebugLogFile("Calling Run_StoredProcedure() " + sProcName, sFuncName);

                oDataset = SqlHelper.ExecuteDataSet(ConnectionString, CommandType.StoredProcedure, sProcName);

                oLog.WriteToDebugLogFile("Completed With SUCCESS  ", sFuncName);

                return oDataset;
            }
            catch (Exception Ex)
            {
                sErrDesc = Ex.Message.ToString();
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                throw Ex;
            }
        }

        public DataSet InsertUpdateDeleteDriver(DataSet oDSCompanyList, DataTable oDTDriverList, string sCompany)
        {
            DataSet oDataset = new DataSet();
            string sFuncName = string.Empty;
            string sProcName = string.Empty;
            DataView oDTView = new DataView();

            try
            {
                sFuncName = "InsertUpdateDeleteDriver()";
                sProcName = "VS_LG_SP018_Web_InsertUpdateDeleteDriver";

                oLog.WriteToDebugLogFile("Starting Function ", sFuncName);

                oLog.WriteToDebugLogFile("Calling Run_StoredProcedure() " + sProcName, sFuncName);

                if (oDSCompanyList != null && oDSCompanyList.Tables.Count > 0)
                {
                    oDTView = oDSCompanyList.Tables[0].DefaultView;

                    oDTView.RowFilter = "U_DBName= '" + sCompany + "'";
                    if (oDTView != null && oDTView.Count > 0)
                    {
                        DataRow dr = oDTDriverList.Rows[0];
                        oDataset = SqlHelper.ExecuteDataSet(oDTView[0]["U_ConnString"].ToString(), CommandType.StoredProcedure, sProcName,
                            Data.CreateParameter("@Id", dr["Id"]), Data.CreateParameter("@DriverId", dr["DriverId"]), Data.CreateParameter("@DriverName", dr["DriverName"]),
                             Data.CreateParameter("@UserName", dr["UserName"]), Data.CreateParameter("@Password", dr["Password"]), Data.CreateParameter("@ContactNumber", dr["ContactNumber"]),
                             Data.CreateParameter("@IsActive", dr["IsActive"]),
                              Data.CreateParameter("@TypeofOperation", dr["TypeofOperation"]));
                        oLog.WriteToDebugLogFile("Completed With SUCCESS  ", sFuncName);
                    }
                    else
                    {
                        oLog.WriteToDebugLogFile("No Data in OUSR table for the selected Company", sFuncName);
                        return oDataset;
                    }
                }
                else
                {
                    oLog.WriteToDebugLogFile("There is No Company List in the Holding Company ", sFuncName);
                    return oDataset;
                }

                return oDataset;
            }
            catch (Exception Ex)
            {
                sErrDesc = Ex.Message.ToString();
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                throw Ex;
            }
        }

        public DataSet GetAllDriverList(DataSet oDSCompanyList, string sCompany)
        {
            DataSet oDataset = new DataSet();
            string sFuncName = string.Empty;
            string sProcName = string.Empty;
            DataView oDTView = new DataView();
            try
            {
                sFuncName = "GetAllDriverList()";
                sProcName = "VS_LG_SP019_Web_GetAllDriverList";
                oLog.WriteToDebugLogFile("Starting Function ", sFuncName);

                if (oDSCompanyList != null && oDSCompanyList.Tables.Count > 0)
                {
                    oDTView = oDSCompanyList.Tables[0].DefaultView;

                    oDTView.RowFilter = "U_DBName= '" + sCompany + "'";
                    if (oDTView != null && oDTView.Count > 0)
                    {
                        oDataset = SqlHelper.ExecuteDataSet(oDTView[0]["U_ConnString"].ToString(), CommandType.StoredProcedure, sProcName);
                        oLog.WriteToDebugLogFile("Completed With SUCCESS  ", sFuncName);
                    }
                    else
                    {
                        oLog.WriteToDebugLogFile("No Data in OUSR table for the selected Company", sFuncName);
                    }
                }
                else
                {
                    oLog.WriteToDebugLogFile("There is No Company List in the Holding Company ", sFuncName);
                }

                return oDataset;
            }
            catch (Exception Ex)
            {
                sErrDesc = Ex.Message.ToString();
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                throw Ex;
            }
        }

        public DataSet GetAllDriverNames(DataSet oDSCompanyList, string sCompany)
        {
            DataSet oDataset = new DataSet();
            string sFuncName = string.Empty;
            string sProcName = string.Empty;
            DataView oDTView = new DataView();
            try
            {
                sFuncName = "GetAllDriverNames()";
                sProcName = "VS_LG_SP032_Web_DriverName";
                oLog.WriteToDebugLogFile("Starting Function ", sFuncName);

                if (oDSCompanyList != null && oDSCompanyList.Tables.Count > 0)
                {
                    oDTView = oDSCompanyList.Tables[0].DefaultView;

                    oDTView.RowFilter = "U_DBName= '" + sCompany + "'";
                    if (oDTView != null && oDTView.Count > 0)
                    {
                        oDataset = SqlHelper.ExecuteDataSet(oDTView[0]["U_ConnString"].ToString(), CommandType.StoredProcedure, sProcName);
                        oLog.WriteToDebugLogFile("Completed With SUCCESS  ", sFuncName);
                    }
                    else
                    {
                        oLog.WriteToDebugLogFile("No Data in OUSR table for the selected Company", sFuncName);
                    }
                }
                else
                {
                    oLog.WriteToDebugLogFile("There is No Company List in the Holding Company ", sFuncName);
                }

                return oDataset;
            }
            catch (Exception Ex)
            {
                sErrDesc = Ex.Message.ToString();
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                throw Ex;
            }
        }

        #endregion

        #region Call Assignment

        public DataSet GetCallAssignement(DataSet oDSCompanyList, string sCompany, string sDriverName)
        {
            DataSet oDataset = new DataSet();
            string sFuncName = string.Empty;
            string sProcName = string.Empty;
            DataView oDTView = new DataView();
            try
            {
                sFuncName = "GetCallAssignement()";
                sProcName = "VS_LG_SP021_Web_CallAssignment";
                oLog.WriteToDebugLogFile("Starting Function ", sFuncName);

                if (oDSCompanyList != null && oDSCompanyList.Tables.Count > 0)
                {
                    oDTView = oDSCompanyList.Tables[0].DefaultView;

                    oDTView.RowFilter = "U_DBName= '" + sCompany + "'";
                    if (oDTView != null && oDTView.Count > 0)
                    {
                        oDataset = SqlHelper.ExecuteDataSet(oDTView[0]["U_ConnString"].ToString(), CommandType.StoredProcedure, sProcName,
                            Data.CreateParameter("@DriverName", sDriverName));
                        oLog.WriteToDebugLogFile("Completed With SUCCESS  ", sFuncName);
                    }
                    else
                    {
                        oLog.WriteToDebugLogFile("No Data in OUSR table for the selected Company", sFuncName);
                    }
                }
                else
                {
                    oLog.WriteToDebugLogFile("There is No Company List in the Holding Company ", sFuncName);
                }

                return oDataset;
            }
            catch (Exception Ex)
            {
                sErrDesc = Ex.Message.ToString();
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                throw Ex;
            }
        }

        public DataSet GetCallAssignement_ItemList(DataSet oDSCompanyList, string sCompany, string sDriverName)
        {
            DataSet oDataset = new DataSet();
            string sFuncName = string.Empty;
            string sProcName = string.Empty;
            DataView oDTView = new DataView();
            try
            {
                sFuncName = "GetCallAssignement_ItemList()";
                sProcName = "VS_LG_SP022_Web_CallAssignment_ItemsList";
                oLog.WriteToDebugLogFile("Starting Function ", sFuncName);

                if (oDSCompanyList != null && oDSCompanyList.Tables.Count > 0)
                {
                    oDTView = oDSCompanyList.Tables[0].DefaultView;

                    oDTView.RowFilter = "U_DBName= '" + sCompany + "'";
                    if (oDTView != null && oDTView.Count > 0)
                    {
                        oDataset = SqlHelper.ExecuteDataSet(oDTView[0]["U_ConnString"].ToString(), CommandType.StoredProcedure, sProcName,
                            Data.CreateParameter("@DriverName", sDriverName));
                        oLog.WriteToDebugLogFile("Completed With SUCCESS  ", sFuncName);
                    }
                    else
                    {
                        oLog.WriteToDebugLogFile("No Data in OUSR table for the selected Company", sFuncName);
                    }
                }
                else
                {
                    oLog.WriteToDebugLogFile("There is No Company List in the Holding Company ", sFuncName);
                }

                return oDataset;
            }
            catch (Exception Ex)
            {
                sErrDesc = Ex.Message.ToString();
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                throw Ex;
            }
        }

        public DataSet GetCompletedCall(DataSet oDSCompanyList, string sCompany, string sDriverName)
        {
            DataSet oDataset = new DataSet();
            string sFuncName = string.Empty;
            string sProcName = string.Empty;
            DataView oDTView = new DataView();
            try
            {
                sFuncName = "GetCompletedCall()";
                sProcName = "VS_LG_SP023_Web_GetCompletedCall";
                oLog.WriteToDebugLogFile("Starting Function ", sFuncName);

                if (oDSCompanyList != null && oDSCompanyList.Tables.Count > 0)
                {
                    oDTView = oDSCompanyList.Tables[0].DefaultView;

                    oDTView.RowFilter = "U_DBName= '" + sCompany + "'";
                    if (oDTView != null && oDTView.Count > 0)
                    {
                        oDataset = SqlHelper.ExecuteDataSet(oDTView[0]["U_ConnString"].ToString(), CommandType.StoredProcedure, sProcName,
                            Data.CreateParameter("@DriverName", sDriverName));
                        oLog.WriteToDebugLogFile("Completed With SUCCESS  ", sFuncName);
                    }
                    else
                    {
                        oLog.WriteToDebugLogFile("No Data in OUSR table for the selected Company", sFuncName);
                    }
                }
                else
                {
                    oLog.WriteToDebugLogFile("There is No Company List in the Holding Company ", sFuncName);
                }

                return oDataset;
            }
            catch (Exception Ex)
            {
                sErrDesc = Ex.Message.ToString();
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                throw Ex;
            }
        }

        public DataSet GetCallAssignement_CompletedItemsList(DataSet oDSCompanyList, string sCompany, string sDriverName)
        {
            DataSet oDataset = new DataSet();
            string sFuncName = string.Empty;
            string sProcName = string.Empty;
            DataView oDTView = new DataView();
            try
            {
                sFuncName = "GetCallAssignement_CompletedItemsList()";
                sProcName = "VS_LG_SP024_Web_CallAssignment_CompletedItemsList";
                oLog.WriteToDebugLogFile("Starting Function ", sFuncName);

                if (oDSCompanyList != null && oDSCompanyList.Tables.Count > 0)
                {
                    oDTView = oDSCompanyList.Tables[0].DefaultView;

                    oDTView.RowFilter = "U_DBName= '" + sCompany + "'";
                    if (oDTView != null && oDTView.Count > 0)
                    {
                        oDataset = SqlHelper.ExecuteDataSet(oDTView[0]["U_ConnString"].ToString(), CommandType.StoredProcedure, sProcName,
                            Data.CreateParameter("@DriverName", sDriverName));
                        oLog.WriteToDebugLogFile("Completed With SUCCESS  ", sFuncName);
                    }
                    else
                    {
                        oLog.WriteToDebugLogFile("No Data in OUSR table for the selected Company", sFuncName);
                    }
                }
                else
                {
                    oLog.WriteToDebugLogFile("There is No Company List in the Holding Company ", sFuncName);
                }

                return oDataset;
            }
            catch (Exception Ex)
            {
                sErrDesc = Ex.Message.ToString();
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                throw Ex;
            }
        }

        public string UpdateDragOrder(DataTable dt, string sCompany)
        {
            string sFuncName = string.Empty;
            try
            {
                sFuncName = "UpdateDragOrder()";
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                using (SqlCommand command = connection.CreateCommand())
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        oLog.WriteToDebugLogFile("Before Updating UDF", sFuncName);
                        command.CommandText = "UPDATE ODLN SET U_DragOrder = '" + item["DragOrder"] + "' where DocNum = " + item["DocNum"] + " and DocEntry = " + item["DocEntry"] + "";
                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                        command.Dispose();
                        oLog.WriteToDebugLogFile("After Updating UDF", sFuncName);
                    }
                    return "SUCCESS";
                }
            }
            catch (Exception ex)
            {
                sErrDesc = ex.Message.ToString();
                oLog.WriteToDebugLogFile("Completed with ERROR", sFuncName);
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                return sErrDesc;
            }
        }

        public DataSet SearchCallAssignment(DataSet oDSCompanyList, string sCompany, DataTable dtSearch)
        {
            DataSet oSearchDataset = null;
            string sFuncName = string.Empty;
            string sProcName = string.Empty;
            DataView oDTView = new DataView();
            try
            {
                sFuncName = "SearchCallAssignment()";
                sProcName = "VS_LG_SP030_Web_SearchCallAssignment";
                oLog.WriteToDebugLogFile("Starting Function ", sFuncName);

                if (oDSCompanyList != null && oDSCompanyList.Tables.Count > 0)
                {
                    oDTView = oDSCompanyList.Tables[0].DefaultView;

                    oDTView.RowFilter = "U_DBName= '" + sCompany + "'";
                    if (oDTView != null && oDTView.Count > 0)
                    {
                        DataRow dr = null;
                        dr = dtSearch.Rows[0];
                        oSearchDataset = new DataSet();
                        oSearchDataset = SqlHelper.ExecuteDataSet(oDTView[0]["U_ConnString"].ToString(), CommandType.StoredProcedure, sProcName,
                            Data.CreateParameter("@DriverName", dr["DriverName"].ToString()),
                            Data.CreateParameter("@DoFromDate", dr["DoFromDate"].ToString()),
                            Data.CreateParameter("@DoToDate", dr["DoToDate"].ToString()),
                            Data.CreateParameter("@DocNumber", dr["DocNumber"].ToString()),
                            Data.CreateParameter("@SvcCall", dr["SvcCall"].ToString()),
                            Data.CreateParameter("@CustomerName", dr["CustomerName"].ToString()),
                            Data.CreateParameter("@ItemCode", dr["ItemCode"].ToString()),
                            Data.CreateParameter("@ItemName", dr["ItemName"].ToString()),
                            Data.CreateParameter("@AMPM", dr["AMPM"].ToString()));
                        oLog.WriteToDebugLogFile("Completed With SUCCESS  ", sFuncName);
                    }
                    else
                    {
                        oLog.WriteToDebugLogFile("No Data in OUSR table for the selected Company", sFuncName);
                    }
                }
                else
                {
                    oLog.WriteToDebugLogFile("There is No Company List in the Holding Company ", sFuncName);
                }

                return oSearchDataset;
            }
            catch (Exception Ex)
            {
                sErrDesc = Ex.Message.ToString();
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                throw Ex;
            }
        }

        public DataSet SearchCallAssignment_ItemList(DataSet oDSCompanyList, string sCompany, DataTable dtSearch)
        {
            DataSet oDataset = new DataSet();
            string sFuncName = string.Empty;
            string sProcName = string.Empty;
            DataView oDTView = new DataView();
            try
            {
                sFuncName = "SearchCallAssignment_ItemList()";
                sProcName = "VS_LG_SP031_Web_SearchCallAssignment_ItemList";
                oLog.WriteToDebugLogFile("Starting Function ", sFuncName);

                if (oDSCompanyList != null && oDSCompanyList.Tables.Count > 0)
                {
                    oDTView = oDSCompanyList.Tables[0].DefaultView;

                    oDTView.RowFilter = "U_DBName= '" + sCompany + "'";
                    if (oDTView != null && oDTView.Count > 0)
                    {
                        DataRow dr = dtSearch.Rows[0];
                        oDataset = SqlHelper.ExecuteDataSet(oDTView[0]["U_ConnString"].ToString(), CommandType.StoredProcedure, sProcName,
                            Data.CreateParameter("@DriverName", dr["DriverName"].ToString()),
                            Data.CreateParameter("@DoFromDate", dr["DoFromDate"].ToString()),
                            Data.CreateParameter("@DoToDate", dr["DoToDate"].ToString()),
                            Data.CreateParameter("@DocNumber", dr["DocNumber"].ToString()),
                            Data.CreateParameter("@SvcCall", dr["SvcCall"].ToString()),
                            Data.CreateParameter("@CustomerName", dr["CustomerName"].ToString()),
                            Data.CreateParameter("@ItemCode", dr["ItemCode"].ToString()),
                            Data.CreateParameter("@ItemName", dr["ItemName"].ToString()),
                            Data.CreateParameter("@AMPM", dr["AMPM"].ToString()));
                        oLog.WriteToDebugLogFile("Completed With SUCCESS  ", sFuncName);
                    }
                    else
                    {
                        oLog.WriteToDebugLogFile("No Data in OUSR table for the selected Company", sFuncName);
                    }
                }
                else
                {
                    oLog.WriteToDebugLogFile("There is No Company List in the Holding Company ", sFuncName);
                }

                return oDataset;
            }
            catch (Exception Ex)
            {
                sErrDesc = Ex.Message.ToString();
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                throw Ex;
            }
        }

        #endregion

        #region ETA

        public DataSet UpdateETA(DataSet oDSCompanyList, DataTable oDTCallInfo, string sCompany)
        {
            DataSet oDataset = new DataSet();
            string sFuncName = string.Empty;
            string sProcName = string.Empty;
            DataView oDTView = new DataView();

            try
            {
                sFuncName = "UpdateETA()";
                sProcName = "VS_LG_SP025_Web_UpdateETA";

                oLog.WriteToDebugLogFile("Starting Function ", sFuncName);

                oLog.WriteToDebugLogFile("Calling Run_StoredProcedure() " + sProcName, sFuncName);

                if (oDSCompanyList != null && oDSCompanyList.Tables.Count > 0)
                {
                    oDTView = oDSCompanyList.Tables[0].DefaultView;

                    oDTView.RowFilter = "U_DBName= '" + sCompany + "'";
                    if (oDTView != null && oDTView.Count > 0)
                    {
                        DataRow dr = oDTCallInfo.Rows[0];
                        oDataset = SqlHelper.ExecuteDataSet(oDTView[0]["U_ConnString"].ToString(), CommandType.StoredProcedure, sProcName,
                            Data.CreateParameter("@Type", dr["TYPE"]), Data.CreateParameter("@DocNum", dr["DocNum"]),
                             Data.CreateParameter("@DocEntry", dr["DocEntry"]), Data.CreateParameter("@SvcCall", dr["SvcCall"]),
                             Data.CreateParameter("@ETADATE", dr["ETADATE"]), Data.CreateParameter("@ETATIME", dr["ETATIME"]));
                        oLog.WriteToDebugLogFile("Completed With SUCCESS  ", sFuncName);
                    }
                    else
                    {
                        oLog.WriteToDebugLogFile("No Data in OUSR table for the selected Company", sFuncName);
                        return oDataset;
                    }
                }
                else
                {
                    oLog.WriteToDebugLogFile("There is No Company List in the Holding Company ", sFuncName);
                    return oDataset;
                }

                return oDataset;
            }
            catch (Exception Ex)
            {
                sErrDesc = Ex.Message.ToString();
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                throw Ex;
            }
        }

        public string SaveAttachments(DataTable oDTAttachment, string sCompany)
        {
            DataSet oDataset = new DataSet();
            string sFuncName = string.Empty;
            string sProcName = string.Empty;
            DataView oDTView = new DataView();
            string sResult = string.Empty;

            try
            {
                sFuncName = "SaveAttachments()";

                oLog.WriteToDebugLogFile("Starting Function ", sFuncName);
                oLog.WriteToDebugLogFile("Connecting Company", sFuncName);
                oDICompany = ConnectToTargetCompany(sCompany);
                oDICompany.StartTransaction();
                oLog.WriteToDebugLogFile("Company Connected successfully", sFuncName);
                oDTView = oDTAttachment.DefaultView;

                SAPbobsCOM.Documents oDocument = default(SAPbobsCOM.Documents);
                oDocument = oDICompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDeliveryNotes);
                for (int i = 0; i <= oDTView.Count - 1; i++)
                {
                    if (oDocument.GetByKey(Convert.ToInt32(oDTView[i]["DocEntry"])))
                    {
                        if (oDTView[i]["IsNew"].ToString() == "Y")
                        {
                            SAPbobsCOM.Attachments2 oAtt = (SAPbobsCOM.Attachments2)oDICompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oAttachments2);
                            string sFullPath = oDTView[i]["FilePath"].ToString() + "\\" + oDTView[i]["FileName"].ToString() + "." + oDTView[i]["FileExtension"].ToString();
                            if (System.IO.File.Exists(sFullPath))
                            {
                                int iErr = 0;
                                int iAbsEntry = 0;
                                DataSet ds = FetchValueInDataset("Select Top 1 AtcEntry from ODLN where DocEntry = " + oDocument.DocEntry + "");
                                if (ds != null && ds.Tables.Count > 0)
                                {
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        iAbsEntry = Convert.ToInt32(ds.Tables[0].Rows[0]["AtcEntry"]);
                                    }
                                }
                                if (oAtt.GetByKey(iAbsEntry))
                                {
                                    oAtt.Lines.Add();
                                    oAtt.Lines.SetCurrentLine(oAtt.Lines.Count - 1);
                                    oAtt.Lines.SourcePath = oDTView[i]["FilePath"].ToString();
                                    oAtt.Lines.FileName = oDTView[i]["FileName"].ToString();
                                    oAtt.Lines.FileExtension = oDTView[i]["FileExtension"].ToString();

                                    iErr = oAtt.Update();
                                }
                                else
                                {
                                    oAtt.Lines.SourcePath = oDTView[i]["FilePath"].ToString();
                                    oAtt.Lines.FileName = oDTView[i]["FileName"].ToString();
                                    oAtt.Lines.FileExtension = oDTView[i]["FileExtension"].ToString();
                                    oAtt.Lines.Add();

                                    iErr = oAtt.Add();
                                }

                                int AttEntry = 0;
                                if (iErr == 0)
                                {
                                    AttEntry = int.Parse(oDICompany.GetNewObjectKey());

                                    sResult = UpdateQuery("UPDATE ODLN SET AtcEntry = " + AttEntry + " where DocEntry = " + oDocument.DocEntry + "");
                                    //oDocument.AttachmentEntry = AttEntry;

                                    //iErr = oDocument.Update();
                                    //if (iErr != 0)
                                    //{
                                    //    sResult = oDICompany.GetLastErrorDescription();
                                    //    return sResult;
                                    //}
                                    //sResult = "SUCCESS";

                                }
                                else
                                {
                                    sResult = oDICompany.GetLastErrorDescription();
                                }
                            }
                        }
                    }
                }
                oDICompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);

                return sResult;
            }
            catch (Exception Ex)
            {
                sErrDesc = Ex.Message.ToString();
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                return sResult = sErrDesc;
            }
        }

        public DataSet Get_BitmapPath(DataSet oDTCompanyList, string sCompany)
        {
            DataSet oDataset = new DataSet();
            string sFuncName = string.Empty;
            string sQuery = string.Empty;
            DataView oDTView = new DataView();

            try
            {
                sFuncName = "Get_BitmapPath()";
                sQuery = "select BitmapPath  from OADP";
                oLog.WriteToDebugLogFile("Starting Function ", sFuncName);

                oLog.WriteToDebugLogFile("Calling SQLQuery " + sQuery, sFuncName);
                if (oDTCompanyList != null && oDTCompanyList.Tables.Count > 0)
                {
                    oDTView = oDTCompanyList.Tables[0].DefaultView;

                    oDTView.RowFilter = "U_DBName= '" + sCompany + "'";
                    if (oDTView != null && oDTView.Count > 0)
                    {
                        oDataset = SqlHelper.ExecuteDataSet(oDTView[0]["U_ConnString"].ToString(), CommandType.Text, sQuery);

                        oLog.WriteToDebugLogFile("Completed With SUCCESS  ", sFuncName);
                    }
                    else
                    {
                        oLog.WriteToDebugLogFile("No Data in OUSR table for the selected Company", sFuncName);
                        return oDataset;
                    }
                }
                else
                {
                    oLog.WriteToDebugLogFile("There is No Company List in the Holding Company ", sFuncName);
                    return oDataset;
                }
                return oDataset;
            }
            catch (Exception Ex)
            {
                sErrDesc = Ex.Message.ToString();
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                throw Ex;
            }
        }

        public string SendEmail(DataTable oDTCallInfo, DataTable oDTItemInfo, DataTable oDTAdditionalItemInfo, string sCompany)
        {
            string sFuncName = "SendEmail";
            string sResult = string.Empty;
            oLog.WriteToDebugLogFile("Starting function", sFuncName);
            try
            {
                DataRow dr = oDTCallInfo.Rows[0];

                if (dr["SvcCall"].ToString() != string.Empty && dr["CallType"].ToString() == "1") // First Template
                {
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    DataSet ds = new DataSet();
                    using (SqlConnection connection = new SqlConnection(ConnectionString))
                    using (SqlCommand cmd = connection.CreateCommand())
                    {
                        connection.Open();
                        adapter.SelectCommand = new SqlCommand("select IsNull(U_FirstEmailSentDate,'') [value] from ODLN where DocEntry = " + dr["DocEntry"].ToString() + " and DocNum = " + dr["DocNum"].ToString() + "", connection);
                        adapter.Fill(ds);
                        connection.Close();
                    }

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (ds.Tables[0].Rows[0][0].ToString().Length == 0)
                        {
                            sResult = SendEmailTemplate1(oDTCallInfo, oDTItemInfo, oDTAdditionalItemInfo, ref sErrDesc);
                            if (sResult == "SUCCESS")
                            {
                                using (SqlConnection connection = new SqlConnection(ConnectionString))
                                using (SqlCommand command = connection.CreateCommand())
                                {
                                    oLog.WriteToDebugLogFile("Before Updating UDF", sFuncName);
                                    string sTime = System.DateTime.Now.TimeOfDay.ToString().Substring(0, 5).Replace(":", "");
                                    command.CommandText = "UPDATE ODLN SET U_FirstEmailSentDate = '" + DateTime.Now.Date + "',U_FirstEmailSentTime = '" + sTime + "'" +
                                                          " where DocEntry = " + dr["DocEntry"].ToString() + " and DocNum = " + dr["DocNum"].ToString() + "";
                                    connection.Open();
                                    command.ExecuteNonQuery();
                                    connection.Close();
                                    command.Dispose();
                                    oLog.WriteToDebugLogFile("After Updating UDF", sFuncName);
                                }
                            }
                        }

                        else
                        {
                            oLog.WriteToDebugLogFile("Email already send first time for this Doc Entry " + dr["DocEntry"].ToString(), sFuncName);
                            sResult = "Email already send first time for this Doc Entry " + dr["DocEntry"].ToString();
                        }
                    }
                }
                else //Second-Third Template
                {
                    // Checking the mail send status

                    DataSet ds = CheckEmailStatusforSecTemplate(sCompany, dr["DocNum"].ToString(), dr["DocEntry"].ToString());

                    if (ds.Tables[0].Rows[0][0].ToString() == "2")
                    {
                        sResult = SendEmailTemplate2(oDTCallInfo, oDTItemInfo, oDTAdditionalItemInfo, ref sErrDesc);
                        updatestatement(sResult, dr["DocEntry"].ToString(), dr["DocNum"].ToString(), "U_FirstEmailSentDate", "U_FirstEmailSentTime");
                    }
                    else if (ds.Tables[0].Rows[0][0].ToString() == "3")
                    {
                        sResult = SendEmailTemplate3(oDTCallInfo, oDTItemInfo, oDTAdditionalItemInfo, ref sErrDesc);
                        updatestatement(sResult, dr["DocEntry"].ToString(), dr["DocNum"].ToString(), "U_SecEmailSentDate", "U_SecEmailSentTime");
                    }
                    else if (ds.Tables[0].Rows[0][0].ToString() == "4")
                    {
                        oLog.WriteToDebugLogFile("Email already send first time for this Doc Entry " + dr["DocEntry"].ToString(), sFuncName);
                        sResult = "Email already send first time for this Doc Entry " + dr["DocEntry"].ToString();
                    }
                }
                oLog.WriteToDebugLogFile("Ending function", sFuncName);
            }
            catch (Exception ex)
            {
                sErrDesc = ex.Message.ToString();
                sResult = sErrDesc;
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
            }

            return sResult;
        }

        public string SendEmailTemplate1(DataTable oDTCallInfo, DataTable oDTItemInfo, DataTable oDTAdditionalItemInfo, ref string sErrDesc)
        {
            string functionReturnValue = string.Empty;

            string sFuncName = "SendEmailTemplate1";

            try
            {
                DataRow dr = oDTCallInfo.Rows[0];
                oLog.WriteToDebugLogFile("Starting function", sFuncName);
                oLog.WriteToDebugLogFile("Setting SMTP properties", sFuncName);
                SmtpClient smtpClient = new SmtpClient(sSMTPHost, iSMTPPort);

                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new System.Net.NetworkCredential(sSMTPUser, sSMTPPassword);

                smtpClient.EnableSsl = true;

                //string sSubject = dr["TYPE"].ToString() + " ETA - " + dr["ETADATE"].ToString() + " - " + dr["Customer"].ToString();
                string sSubject = oDTCallInfo.Rows[0]["Subject"].ToString();
                MailMessage message = new MailMessage();
                message.From = new MailAddress(sEmailFrom);
                message.To.Add(new MailAddress(sServiceMailId));
                message.SubjectEncoding = System.Text.Encoding.UTF8;
                message.Subject = sSubject;
                message.BodyEncoding = System.Text.Encoding.UTF8;
                message.Body = ComposeBody(oDTCallInfo, oDTItemInfo, oDTAdditionalItemInfo, sSubject, 1);
                message.IsBodyHtml = true;
                object userState = message;

                oLog.WriteToDebugLogFile("Sending Email Message", sFuncName);

                oLog.WriteToDebugLogFile("Sending Email Messages to : " + sServiceMailId, sFuncName);

                ServicePointManager.ServerCertificateValidationCallback = delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                { return true; };

                smtpClient.Send(message);

                message.Dispose();

                functionReturnValue = "SUCCESS";

                oLog.WriteToDebugLogFile("Function completed with Success", sFuncName);

            }
            catch (Exception ex)
            {
                functionReturnValue = ex.Message;
                sErrDesc = ex.Message;

                oLog.WriteToDebugLogFile("Function completed with Error", sFuncName);
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                oLog.WriteToErrorLogFile("Failed sending email to : " + " " + sServiceMailId, sFuncName);

            }
            finally
            {
            }
            return functionReturnValue;

        }

        public string SendEmailTemplate2(DataTable oDTCallInfo, DataTable oDTItemInfo, DataTable oDTAdditionalItemInfo, ref string sErrDesc)
        {
            string functionReturnValue = string.Empty;

            string sFuncName = "SendEmailTemplate2";

            try
            {
                DataRow dr = oDTCallInfo.Rows[0];
                oLog.WriteToDebugLogFile("Starting function", sFuncName);
                oLog.WriteToDebugLogFile("Setting SMTP properties", sFuncName);
                SmtpClient smtpClient = new SmtpClient(sSMTPHost, iSMTPPort);

                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new System.Net.NetworkCredential(sSMTPUser, sSMTPPassword);

                smtpClient.EnableSsl = true;

                //string sSubject = "Adventus " + dr["TYPE"].ToString() + "Delivery ETA - " + dr["ETADATE"].ToString() + " - " + dr["Customer"].ToString();
                string sSubject = oDTCallInfo.Rows[0]["Subject"].ToString();
                MailMessage message = new MailMessage();
                message.From = new MailAddress(sEmailFrom);
                message.To.Add(new MailAddress(dr["Email"].ToString()));
                message.SubjectEncoding = System.Text.Encoding.UTF8;
                message.Subject = sSubject;
                message.BodyEncoding = System.Text.Encoding.UTF8;
                message.Body = ComposeBody(oDTCallInfo, oDTItemInfo, oDTAdditionalItemInfo, sSubject, 2);
                message.IsBodyHtml = true;
                object userState = message;

                oLog.WriteToDebugLogFile("Sending Email Message", sFuncName);

                oLog.WriteToDebugLogFile("Sending Email Messages to : " + sServiceMailId, sFuncName);

                ServicePointManager.ServerCertificateValidationCallback = delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                { return true; };

                smtpClient.Send(message);

                message.Dispose();

                functionReturnValue = "SUCCESS";

                oLog.WriteToDebugLogFile("Function completed with Success", sFuncName);

            }
            catch (Exception ex)
            {
                functionReturnValue = ex.Message;
                sErrDesc = ex.Message;

                oLog.WriteToDebugLogFile("Function completed with Error", sFuncName);
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                oLog.WriteToErrorLogFile("Failed sending email to : " + " " + sServiceMailId, sFuncName);

            }
            finally
            {
            }
            return functionReturnValue;

        }

        public string SendEmailTemplate3(DataTable oDTCallInfo, DataTable oDTItemInfo, DataTable oDTAdditionalItemInfo, ref string sErrDesc)
        {
            string functionReturnValue = string.Empty;

            string sFuncName = "SendEmailTemplate3";

            try
            {
                DataRow dr = oDTCallInfo.Rows[0];
                oLog.WriteToDebugLogFile("Starting function", sFuncName);
                oLog.WriteToDebugLogFile("Setting SMTP properties", sFuncName);
                SmtpClient smtpClient = new SmtpClient(sSMTPHost, iSMTPPort);

                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new System.Net.NetworkCredential(sSMTPUser, sSMTPPassword);

                smtpClient.EnableSsl = true;

                //string sSubject = "Adventus " + dr["TYPE"].ToString() + "Delivery ETA - " + dr["ETADATE"].ToString() + " - " + dr["Customer"].ToString() + " (Change of ETA Date)";
                string sSubject = oDTCallInfo.Rows[0]["Subject"].ToString();
                MailMessage message = new MailMessage();
                message.From = new MailAddress(sEmailFrom);
                message.To.Add(new MailAddress(dr["Email"].ToString()));
                message.SubjectEncoding = System.Text.Encoding.UTF8;
                message.Subject = sSubject;
                message.BodyEncoding = System.Text.Encoding.UTF8;
                message.Body = ComposeBody(oDTCallInfo, oDTItemInfo, oDTAdditionalItemInfo, sSubject, 3);
                message.IsBodyHtml = true;
                object userState = message;

                oLog.WriteToDebugLogFile("Sending Email Message", sFuncName);

                oLog.WriteToDebugLogFile("Sending Email Messages to : " + sServiceMailId, sFuncName);

                ServicePointManager.ServerCertificateValidationCallback = delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                { return true; };

                smtpClient.Send(message);

                message.Dispose();

                functionReturnValue = "SUCCESS";

                oLog.WriteToDebugLogFile("Function completed with Success", sFuncName);

            }
            catch (Exception ex)
            {
                functionReturnValue = ex.Message;
                sErrDesc = ex.Message;

                oLog.WriteToDebugLogFile("Function completed with Error", sFuncName);
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                oLog.WriteToErrorLogFile("Failed sending email to : " + " " + sServiceMailId, sFuncName);

            }
            finally
            {
            }
            return functionReturnValue;

        }

        public string SendTonerPartsEmailTemplate(DataTable oDTCallInfo, DataTable oDTItemInfo, DataTable oDTAdditionalItemInfo, ref string sErrDesc)
        {
            string functionReturnValue = string.Empty;

            string sFuncName = "SendTonerPartsEmailTemplate";

            try
            {
                DataRow dr = oDTCallInfo.Rows[0];
                oLog.WriteToDebugLogFile("Starting function", sFuncName);
                oLog.WriteToDebugLogFile("Setting SMTP properties", sFuncName);
                SmtpClient smtpClient = new SmtpClient(sSMTPHost, iSMTPPort);

                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new System.Net.NetworkCredential(sSMTPUser, sSMTPPassword);

                smtpClient.EnableSsl = true;

                string sSubject = "Adventus DO - " + dr["DocNum"].ToString() + " - Delivery of Consumables/Parts to - " + dr["CustomerName"].ToString() + "";
                MailMessage message = new MailMessage();
                message.From = new MailAddress(sEmailFrom);
                message.To.Add(new MailAddress(dr["RecipientEmail"].ToString()));
                message.SubjectEncoding = System.Text.Encoding.UTF8;
                message.Subject = sSubject;
                message.BodyEncoding = System.Text.Encoding.UTF8;
                message.Body = ComposeBody(oDTCallInfo, oDTItemInfo, oDTAdditionalItemInfo, sSubject, 4);
                message.IsBodyHtml = true;
                object userState = message;

                oLog.WriteToDebugLogFile("Sending Email Message", sFuncName);

                oLog.WriteToDebugLogFile("Sending Email Messages to : " + sServiceMailId, sFuncName);

                ServicePointManager.ServerCertificateValidationCallback = delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                { return true; };

                smtpClient.Send(message);

                message.Dispose();

                functionReturnValue = "SUCCESS";

                oLog.WriteToDebugLogFile("Function completed with Success", sFuncName);

            }
            catch (Exception ex)
            {
                functionReturnValue = ex.Message;
                sErrDesc = ex.Message;

                oLog.WriteToDebugLogFile("Function completed with Error", sFuncName);
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                oLog.WriteToErrorLogFile("Failed sending email to : " + " " + sServiceMailId, sFuncName);

            }
            finally
            {
            }
            return functionReturnValue;

        }

        public string ComposeBody(DataTable oDTCallInfo, DataTable oDTItemInfo, DataTable oDTAdditionalItemInfo, string sSubject, int template)
        {
            string sFuncName = string.Empty;
            string stextBody = string.Empty;
            try
            {
                sFuncName = "ComposeBody";
                string sBodyDetail = string.Empty;
                string sBodyDetail1 = string.Empty;
                string sTableFormat = string.Empty;

                var sbMail = new StringBuilder();
                DataRow dr = oDTCallInfo.Rows[0];
                if (template == 1)
                {
                    string sTemplatePath = AppDomain.CurrentDomain.RelativeSearchPath;
                    int index = sTemplatePath.IndexOf("\\bin");
                    if (index > 0)
                        sTemplatePath = sTemplatePath.Substring(0, index) + "\\Email Template\\Format1.htm";
                    using (var sReader = new StreamReader(sTemplatePath))
                    {
                        sbMail.Append(sReader.ReadToEnd());

                        sbMail.Replace("{CallId}", System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(dr["SvcCall"].ToString()));
                        sbMail.Replace("{Subject}", System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(sSubject));
                        sbMail.Replace("{Customer}", System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(dr["Customer"].ToString()));
                        sbMail.Replace("{Driver}", System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(dr["DriverName"].ToString()));
                        sbMail.Replace("{ETA}", System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(dr["ETATIME"].ToString()));
                        sbMail.Replace("{ItemTable}", ComposeItemTableBody(oDTItemInfo));
                    }
                }
                else if (template == 2)
                {
                    string sTemplatePath = AppDomain.CurrentDomain.RelativeSearchPath;
                    int index = sTemplatePath.IndexOf("\\bin");
                    if (index > 0)
                        sTemplatePath = sTemplatePath.Substring(0, index) + "\\Email Template\\Format2.htm";

                    using (var sReader = new StreamReader(sTemplatePath))
                    {
                        sbMail.Append(sReader.ReadToEnd());

                        sbMail.Replace("{Customer}", System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(dr["Customer"].ToString()));
                        sbMail.Replace("{DocumentNumber}", System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(dr["DocNum"].ToString()));
                        sbMail.Replace("{Address}", System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(dr["Address"].ToString()));
                        sbMail.Replace("{DriverName}", System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(dr["DriverName"].ToString()));
                        sbMail.Replace("{DriverContact}", System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(dr["DriverContactNumber"].ToString()));
                        sbMail.Replace("{ETA}", System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(dr["ETATIME"].ToString()));
                        sbMail.Replace("{ItemTable}", ComposeItemTableBody(oDTItemInfo));
                    }
                }
                else if (template == 3)
                {
                    string sTemplatePath = AppDomain.CurrentDomain.RelativeSearchPath;
                    int index = sTemplatePath.IndexOf("\\bin");
                    if (index > 0)
                        sTemplatePath = sTemplatePath.Substring(0, index) + "\\Email Template\\Format3.htm";

                    using (var sReader = new StreamReader(sTemplatePath))
                    {
                        sbMail.Append(sReader.ReadToEnd());

                        sbMail.Replace("{Customer}", System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(dr["Customer"].ToString()));
                        sbMail.Replace("{DocumentNumber}", System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(dr["DocNum"].ToString()));
                        sbMail.Replace("{Address}", System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(dr["Address"].ToString()));
                        sbMail.Replace("{DriverName}", System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(dr["DriverName"].ToString()));
                        sbMail.Replace("{DriverContact}", System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(dr["DriverContactNumber"].ToString()));
                        sbMail.Replace("{ETA}", System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(dr["ETATIME"].ToString()));
                        sbMail.Replace("{ItemTable}", ComposeItemTableBody(oDTItemInfo));
                    }
                }
                else if (template == 4)
                {
                    string sTemplatePath = AppDomain.CurrentDomain.RelativeSearchPath;
                    int index = sTemplatePath.IndexOf("\\bin");
                    if (index > 0)
                        sTemplatePath = sTemplatePath.Substring(0, index) + "\\Email Template\\TonerPartsDeliveryStatus.htm";

                    using (var sReader = new StreamReader(sTemplatePath))
                    {
                        sbMail.Append(sReader.ReadToEnd());

                        string sTimeReplacedinDate = dr["ArrivalDate"].ToString().Split(' ')[0].ToString();

                        sbMail.Replace("{RecipientName}", System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(dr["RecipientName"].ToString()));
                        sbMail.Replace("{DO Number}", System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(dr["DocNum"].ToString()));
                        sbMail.Replace("{DO Date}", sTimeReplacedinDate);
                        sbMail.Replace("{DO Time}", System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(dr["ArrivalTime"].ToString()));
                        sbMail.Replace("{Delivery Personnel}", System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(dr["DriverName"].ToString()));
                        sbMail.Replace("{Printer Address}", System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(dr["Address"].ToString()));
                        sbMail.Replace("{Printer Location}", System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(dr["Department"].ToString()));
                        sbMail.Replace("{Model}", System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(dr["EquipmentName"].ToString()));
                        sbMail.Replace("{Serial Number}", System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(dr["SerialNum"].ToString()));
                        sbMail.Replace("{ItemTable}", ComposeItemTableBody1(oDTItemInfo));
                        sbMail.Replace("{AdditionalItemTable}", ComposeAdditionalItemTableBody(oDTAdditionalItemInfo, oDTItemInfo));
                    }

                }

                stextBody = sbMail.ToString();
            }
            catch (Exception ex)
            {
                sErrDesc = ex.Message.ToString();
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
            }
            return stextBody;
        }

        public string ComposeItemTableBody(DataTable dtBody)
        {
            string sBodyDetail = string.Empty;
            string sBodyDetail1 = string.Empty;
            string sTableFormat = string.Empty;
            foreach (DataRow item in dtBody.Rows)
            {
                sBodyDetail = "<tr><td>&nbsp;" + item["No"] + "</td><td>&nbsp;" + item["ItemCode"].ToString() + " </td> " +
                    " <td>&nbsp;" + item["ItemName"].ToString() + " </td><td>&nbsp;" + item["Quantity"].ToString() + " </td> " +
                    " <td>&nbsp;" + item["SerialNum"].ToString() + " </td></tr>";
                sBodyDetail1 = sBodyDetail1 + sBodyDetail;
            }
            sTableFormat = "<table border = '1' cellspacing = 0 cellpadding = 0 style='font-size:10.0pt;font-family:Arial;width: 85%;'> " +
                                "<tr><td><strong style='color: blue; background-color: transparent;'>&nbsp;No.&nbsp;</strong></td> " +
                                "<td><strong style='color: blue; background-color: transparent;'>&nbsp;Item Code &nbsp; </strong></td> " +
                                "<td><strong style='color: blue; background-color: transparent;'>&nbsp;Item Name &nbsp;</strong></td> " +
                                "<td><strong style='color: blue; background-color: transparent;'>&nbsp;Qty &nbsp;</strong></td> " +
                                "<td><strong style='color: blue; background-color: transparent;'>&nbsp;Serial Number &nbsp;</strong></td></tr> " +
                                sBodyDetail1 + " </table> ";

            string stextBody = sTableFormat;

            return stextBody;
        }

        public string ComposeItemTableBody1(DataTable dtBody)
        {
            string sBodyDetail = string.Empty;
            string sBodyDetail1 = string.Empty;
            string sTableFormat = string.Empty;
            foreach (DataRow item in dtBody.Rows)
            {
                sBodyDetail = "<tr><td>&nbsp;" + item["ItemCode"].ToString() + " </td> " +
                    " <td>&nbsp;" + item["ItemName"].ToString() + " </td><td>&nbsp;" + item["DOQty"].ToString() + " </td> " +
                    " <td>&nbsp;" + item["CollectedQty"].ToString() + " </td></tr>";
                sBodyDetail1 = sBodyDetail1 + sBodyDetail;
            }
            sTableFormat = "<table border = '1' cellspacing = 0 cellpadding = 0 style='font-size:10.0pt;font-family:Arial;width: 85%;'> " +
                                "<tr><td><strong style='color: blue; background-color: transparent;'>&nbsp;Item Code &nbsp; </strong></td> " +
                                "<td><strong style='color: blue; background-color: transparent;'>&nbsp;Description &nbsp;</strong></td> " +
                                "<td><strong style='color: blue; background-color: transparent;'>&nbsp;Delivered &nbsp;</strong></td> " +
                                "<td><strong style='color: blue; background-color: transparent;'>&nbsp;Collected &nbsp;</strong></td></tr> " +
                                sBodyDetail1 + " </table> ";

            string stextBody = sTableFormat;

            return stextBody;
        }

        public string ComposeAdditionalItemTableBody(DataTable dtAdditionalItemsBody, DataTable dtItems)
        {
            string sBodyDetail = string.Empty;
            string sBodyDetail1 = string.Empty;
            string sTableFormat = string.Empty;

            var badValues = new HashSet<Tuple<string, string>>(
            dtItems.AsEnumerable().
                        Select(row =>
                          new Tuple<string, string>(row.Field<string>("ItemCode"), row.Field<string>("ItemName"))));

            var result = dtAdditionalItemsBody.AsEnumerable().
                                                Where(row => !(badValues.Contains(
                                                new Tuple<string, string>(row.Field<string>("ItemCode"), row.Field<string>("ItemDescription")))));
            DataTable dtResult = new DataTable();
            if (result.AsDataView().Count > 0)
            {
                dtResult = result.CopyToDataTable();
            }

            foreach (DataRow item in dtResult.Rows)
            {
                sBodyDetail = "<tr><td>&nbsp;" + item["ItemCode"].ToString() + " </td> " +
                    " <td>&nbsp;" + item["ItemDescription"].ToString() + " </td><td>&nbsp;" + item["Qty"].ToString() + " </td></tr>";
                sBodyDetail1 = sBodyDetail1 + sBodyDetail;
            }
            sTableFormat = "<table border = '1' cellspacing = 0 cellpadding = 0 style='font-size:10.0pt;font-family:Arial;width: 85%;'> " +
                                "<tr><td><strong style='color: blue; background-color: transparent;'>&nbsp;Item Code &nbsp; </strong></td> " +
                                "<td><strong style='color: blue; background-color: transparent;'>&nbsp;Description &nbsp;</strong></td> " +
                                "<td><strong style='color: blue; background-color: transparent;'>&nbsp;Collected Qty &nbsp;</strong></td></tr> " +
                                sBodyDetail1 + " </table> ";

            string stextBody = sTableFormat;

            return stextBody;
        }

        public DataSet CheckEmailStatusforSecTemplate(string sCompany, string sDocNum, string sDocEntry)
        {
            DataSet oDataset = new DataSet();
            string sFuncName = string.Empty;
            string sProcName = string.Empty;
            DataView oDTView = new DataView();
            try
            {
                sFuncName = "CheckEmailStatusforSecTemplate()";
                sProcName = "VS_LG_SP026_Web_CheckEmailSentStatus";
                oLog.WriteToDebugLogFile("Starting Function ", sFuncName);
                DataSet oDSCompanyList = Get_CompanyList();
                if (oDSCompanyList != null && oDSCompanyList.Tables.Count > 0)
                {
                    oDTView = oDSCompanyList.Tables[0].DefaultView;

                    oDTView.RowFilter = "U_DBName= '" + sCompany + "'";
                    if (oDTView != null && oDTView.Count > 0)
                    {
                        oDataset = SqlHelper.ExecuteDataSet(oDTView[0]["U_ConnString"].ToString(), CommandType.StoredProcedure, sProcName,
                            Data.CreateParameter("@DocNum", sDocNum),
                             Data.CreateParameter("@DocEntry", sDocEntry));
                        oLog.WriteToDebugLogFile("Completed With SUCCESS  ", sFuncName);
                    }
                    else
                    {
                        oLog.WriteToDebugLogFile("No Data in OUSR table for the selected Company", sFuncName);
                    }
                }
                else
                {
                    oLog.WriteToDebugLogFile("There is No Company List in the Holding Company ", sFuncName);
                }

                return oDataset;
            }
            catch (Exception Ex)
            {
                sErrDesc = Ex.Message.ToString();
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                throw Ex;
            }
        }

        public void updatestatement(string sResult, string sDocEntry, string sDocNum, string sF1, string sF2)
        {
            string sFuncName = string.Empty;
            sFuncName = "updatestatement()";
            try
            {
                if (sResult == "SUCCESS")
                {
                    using (SqlConnection connection = new SqlConnection(ConnectionString))
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        oLog.WriteToDebugLogFile("Before Updating UDF", sFuncName);
                        string sTime = System.DateTime.Now.TimeOfDay.ToString().Substring(0, 5).Replace(":", "");
                        command.CommandText = "UPDATE ODLN SET " + sF1 + " = '" + DateTime.Now.Date + "'," + sF2 + " = '" + sTime + "'" +
                                              " where DocEntry = " + sDocEntry + " and DocNum = " + sDocNum + "";
                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                        command.Dispose();
                        oLog.WriteToDebugLogFile("After Updating UDF", sFuncName);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region Delivery Status Deals

        public DataSet GetItemList(DataSet oDSCompanyList, string sCompany)
        {
            DataSet oDataset = new DataSet();
            string sFuncName = string.Empty;
            string sProcName = string.Empty;
            DataView oDTView = new DataView();
            try
            {
                sFuncName = "GetItemList()";
                sProcName = "VS_LG_SP027_Web_GetItemList";
                oLog.WriteToDebugLogFile("Starting Function ", sFuncName);

                if (oDSCompanyList != null && oDSCompanyList.Tables.Count > 0)
                {
                    oDTView = oDSCompanyList.Tables[0].DefaultView;

                    oDTView.RowFilter = "U_DBName= '" + sCompany + "'";
                    if (oDTView != null && oDTView.Count > 0)
                    {
                        oDataset = SqlHelper.ExecuteDataSet(oDTView[0]["U_ConnString"].ToString(), CommandType.StoredProcedure, sProcName);
                        oLog.WriteToDebugLogFile("Completed With SUCCESS  ", sFuncName);
                    }
                    else
                    {
                        oLog.WriteToDebugLogFile("No Data in OUSR table for the selected Company", sFuncName);
                    }
                }
                else
                {
                    oLog.WriteToDebugLogFile("There is No Company List in the Holding Company ", sFuncName);
                }

                return oDataset;
            }
            catch (Exception Ex)
            {
                sErrDesc = Ex.Message.ToString();
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                throw Ex;
            }
        }

        public DataSet GetDeliveryStatusList(DataSet oDSCompanyList, string sCompany)
        {
            DataSet oDataset = new DataSet();
            string sFuncName = string.Empty;
            string sProcName = string.Empty;
            DataView oDTView = new DataView();
            try
            {
                sFuncName = "GetDeliveryStatusList()";
                sProcName = "VS_LG_SP029_Web_GetDeliveryStatus";
                oLog.WriteToDebugLogFile("Starting Function ", sFuncName);

                if (oDSCompanyList != null && oDSCompanyList.Tables.Count > 0)
                {
                    oDTView = oDSCompanyList.Tables[0].DefaultView;

                    oDTView.RowFilter = "U_DBName= '" + sCompany + "'";
                    if (oDTView != null && oDTView.Count > 0)
                    {
                        oDataset = SqlHelper.ExecuteDataSet(oDTView[0]["U_ConnString"].ToString(), CommandType.StoredProcedure, sProcName);
                        oLog.WriteToDebugLogFile("Completed With SUCCESS  ", sFuncName);
                    }
                    else
                    {
                        oLog.WriteToDebugLogFile("No Data in OUSR table for the selected Company", sFuncName);
                    }
                }
                else
                {
                    oLog.WriteToDebugLogFile("There is No Company List in the Holding Company ", sFuncName);
                }

                return oDataset;
            }
            catch (Exception Ex)
            {
                sErrDesc = Ex.Message.ToString();
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                throw Ex;
            }
        }

        public DataSet GetCustomercontactDetails(DataSet oDSCompanyList, string sCompany, string sCardCode)
        {
            DataSet oDataset = new DataSet();
            string sFuncName = string.Empty;
            string sProcName = string.Empty;
            DataView oDTView = new DataView();
            try
            {
                sFuncName = "GetCustomercontactDetails()";
                sProcName = "VS_LG_SP028_Web_GetCustomercontactDetails";
                oLog.WriteToDebugLogFile("Starting Function ", sFuncName);

                if (oDSCompanyList != null && oDSCompanyList.Tables.Count > 0)
                {
                    oDTView = oDSCompanyList.Tables[0].DefaultView;

                    oDTView.RowFilter = "U_DBName= '" + sCompany + "'";
                    if (oDTView != null && oDTView.Count > 0)
                    {
                        oDataset = SqlHelper.ExecuteDataSet(oDTView[0]["U_ConnString"].ToString(), CommandType.StoredProcedure, sProcName,
                            Data.CreateParameter("@CardCode", sCardCode));
                        oLog.WriteToDebugLogFile("Completed With SUCCESS  ", sFuncName);
                    }
                    else
                    {
                        oLog.WriteToDebugLogFile("No Data in OUSR table for the selected Company", sFuncName);
                    }
                }
                else
                {
                    oLog.WriteToDebugLogFile("There is No Company List in the Holding Company ", sFuncName);
                }

                return oDataset;
            }
            catch (Exception Ex)
            {
                sErrDesc = Ex.Message.ToString();
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                throw Ex;
            }
        }

        public DataSet GetDeliveryCustomercontactDetails(DataSet oDSCompanyList, string sCompany, string sDocNum)
        {
            DataSet oDataset = new DataSet();
            string sFuncName = string.Empty;
            string sProcName = string.Empty;
            DataView oDTView = new DataView();
            try
            {
                sFuncName = "GetDeliveryCustomercontactDetails()";
                sProcName = "VS_LG_SP033_Web_GetDeliveryCustomerContact";
                oLog.WriteToDebugLogFile("Starting Function ", sFuncName);

                if (oDSCompanyList != null && oDSCompanyList.Tables.Count > 0)
                {
                    oDTView = oDSCompanyList.Tables[0].DefaultView;

                    oDTView.RowFilter = "U_DBName= '" + sCompany + "'";
                    if (oDTView != null && oDTView.Count > 0)
                    {
                        oDataset = SqlHelper.ExecuteDataSet(oDTView[0]["U_ConnString"].ToString(), CommandType.StoredProcedure, sProcName,
                            Data.CreateParameter("@DocNum", sDocNum));
                        oLog.WriteToDebugLogFile("Completed With SUCCESS  ", sFuncName);
                    }
                    else
                    {
                        oLog.WriteToDebugLogFile("No Data in OUSR table for the selected Company", sFuncName);
                    }
                }
                else
                {
                    oLog.WriteToDebugLogFile("There is No Company List in the Holding Company ", sFuncName);
                }

                return oDataset;
            }
            catch (Exception Ex)
            {
                sErrDesc = Ex.Message.ToString();
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                throw Ex;
            }
        }

        public string UpdateDeliveryOrder(DataTable dtHeader, DataTable dtAttachments, DataTable dtItems, DataTable dtAdditionalItems, string sCompany)
        {
            string sFuncName = string.Empty;
            string sSaveAttachmentResult = string.Empty;
            try
            {
                sFuncName = "UpdateDeliveryOrder()";
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                using (SqlCommand command = connection.CreateCommand())
                {
                    DataSet ds = Get_BitmapPath(Get_CompanyList(), sCompany);
                    foreach (DataRow item in dtHeader.Rows)
                    {
                        string sSignaturePath = string.Empty;
                        if (item["DoNumber"].ToString() != string.Empty)
                        {
                            if (ds != null && ds.Tables.Count > 0)
                            {
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    sSignaturePath = SaveSignature(ds.Tables[0].Rows[0][0].ToString(), item["RecipientSignature"].ToString().Replace(" ", "+").ToString(), item["DoNumber"].ToString());
                                }
                            }

                            oLog.WriteToDebugLogFile("Before Updating UDFs", sFuncName);

                            string sQuery = string.Empty;
                            sQuery = "UPDATE ODLN SET U_OB_DeliveryDate = '" + item["ArrivalDate"] + "',U_OB_DeliveryTime='" + item["ArrivalTime"] + "'" +
                            " ,U_OB_AdditionalRmk='" + item["Remarks"] + "',U_OB_Priority='" + item["Priority"] + "',u_ob_liftaccess='" + item["LiftAccess"] + "'" +
                            " ,U_OB_DriverName1='" + item["Driver1"] + "',U_OB_DriverName2='" + item["Driver2"] + "'," +
                            " U_OB_SuppliesPage='" + item["SuppliesPrinted"] + "',U_OB_Signatory='" + item["RecipientName"] + "'" +
                            " ,U_OB_RecpEmail ='" + item["RecipientEmail"] + "',U_OB_RecpSignText = '" + item["RecipientSignature"].ToString().Replace(" ", "+").ToString() + "',U_OB_RecpSign = '" + sSignaturePath + "'" +
                            " where DocNum = '" + item["DoNumber"] + "' and DocEntry = '" + item["DocEntry"] + "'";

                            oLog.WriteToDebugLogFile("Update DeliveryOrder Header Query" + sQuery, sFuncName);

                            command.CommandText = sQuery;
                            connection.Open();
                            command.ExecuteNonQuery();
                            connection.Close();
                            command.Dispose();
                            oLog.WriteToDebugLogFile("After Updating UDFs", sFuncName);
                        }
                    }

                    if (dtItems != null && dtItems.Rows.Count > 0)
                    {
                        foreach (DataRow item in dtItems.Rows)
                        {
                            if (item["DocEntry"].ToString() != string.Empty)
                            {
                                oLog.WriteToDebugLogFile("Before updating the Item Details", sFuncName);

                                string sQuery = string.Empty;
                                sQuery = "update DLN1 Set U_OB_LineRemarks = '" + item["Remarks"] + "' ,U_OB_TabDelStatus = '" + item["DeliveryStatus"] + "', U_OB_DOQty = '" + item["DOQty"] + "'" +
                                    " where DocEntry = '" + item["DocEntry"] + "' and ItemCode = '" + item["ItemCode"] + "' and Dscription = '" + item["ItemDescription"] + "'" +
                                    " and SerialNum = '" + item["SerialNum"] + "'";

                                oLog.WriteToDebugLogFile("Update Items Query" + sQuery, sFuncName);

                                command.CommandText = sQuery;
                                connection.Open();
                                command.ExecuteNonQuery();
                                connection.Close();
                                command.Dispose();
                                oLog.WriteToDebugLogFile("After updating the Item Details", sFuncName);
                            }
                        }
                    }


                    if (dtAdditionalItems != null && dtAdditionalItems.Rows.Count > 0)
                    {
                        foreach (DataRow item in dtAdditionalItems.Rows)
                        {
                            if (item["DocNum"].ToString() != string.Empty)
                            {
                                oLog.WriteToDebugLogFile("Before Inserting the AdditionalItems", sFuncName);

                                string sQuery = string.Empty;
                                sQuery = "INSERT INTO ADV_AdditionalItems(DocEntry,DONumber,ItemCode,ItemName,Quantity,ItemCondition) " +
                                        " VALUES ('" + item["DocEntry"] + "','" + item["DocNum"] + "','" + item["ItemCode"] + "','" + item["ItemDescription"] + "'," +
                                        " '" + item["Qty"] + "','" + item["ItemCondition"] + "')";

                                oLog.WriteToDebugLogFile("Insert Additional Items Query" + sQuery, sFuncName);

                                command.CommandText = sQuery;
                                connection.Open();
                                command.ExecuteNonQuery();
                                connection.Close();
                                command.Dispose();
                                oLog.WriteToDebugLogFile("After Inserting the AdditionalItems", sFuncName);
                            }
                        }
                    }

                    // Function to update OSCL.Resolution
                    oLog.WriteToDebugLogFile("Starting method to update OSCL.Resolution", sFuncName);
                    string sOSCLUpdateResult = string.Empty;
                    sOSCLUpdateResult = GenerateResolutionString(dtHeader, dtItems);
                    if (!sOSCLUpdateResult.Contains("ERROR"))
                    {
                        oLog.WriteToDebugLogFile("Before Updating Resolution", sFuncName);

                        string sQuery = string.Empty;
                        sQuery = "UPDATE OSCL SET resolution = '" + sOSCLUpdateResult + "', [Status] = -1 where callID = '" + dtHeader.Rows[0]["SvcCall"] + "' and calltype <> 1";

                        oLog.WriteToDebugLogFile("Update OSCL resolution" + sQuery, sFuncName);

                        command.CommandText = sQuery;
                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                        command.Dispose();
                        oLog.WriteToDebugLogFile("After Updating Resolution", sFuncName);
                    }
                }
                if (dtAttachments != null && dtAttachments.Rows.Count > 0)
                {
                    if (dtAttachments.Rows[0]["DocNum"].ToString() != string.Empty)
                    {
                        oLog.WriteToDebugLogFile("Before Adding attachments", sFuncName);
                        sSaveAttachmentResult = SaveAttachments(dtAttachments, sCompany);
                        oLog.WriteToDebugLogFile("Attachment Result : " + sSaveAttachmentResult.ToString(), sFuncName);
                        oLog.WriteToDebugLogFile("After Adding attachments", sFuncName);
                    }
                }

                return "SUCCESS";
            }
            catch (Exception ex)
            {
                sErrDesc = ex.Message.ToString();
                oLog.WriteToDebugLogFile("Completed with ERROR", sFuncName);
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                return sErrDesc;
            }
        }

        public string GenerateResolutionString(DataTable dtHeader, DataTable dtItems)
        {
            string sFuncName = string.Empty;
            string sResult = string.Empty;
            int i = 0;
            try
            {
                sFuncName = "GenerateResolutionString";
                foreach (DataRow dr in dtItems.Rows)
                {
                    if (i == 0)
                    {
                        sResult = dtHeader.Rows[0]["DriverName"].ToString() + " " + dr["ItemCode"].ToString() + " " + dr["DeliveryStatus"].ToString()
                            + dtHeader.Rows[0]["ArrivalDate"].ToString().Split(' ')[0].ToString() + " " + dtHeader.Rows[0]["ArrivalTime"].ToString();
                        oLog.WriteToDebugLogFile("First Row" + sResult, sFuncName);
                    }
                    else
                    {
                        sResult = sResult + Environment.NewLine + dtHeader.Rows[0]["DriverName"].ToString() + " " + dr["ItemCode"].ToString() + " " + dr["DeliveryStatus"].ToString()
                                                  + dtHeader.Rows[0]["ArrivalDate"].ToString().Split(' ')[0].ToString() + " " + dtHeader.Rows[0]["ArrivalTime"].ToString();
                        oLog.WriteToDebugLogFile("Next Row" + sResult, sFuncName);
                    }
                }
            }
            catch (Exception ex)
            {
                sErrDesc = ex.Message.ToString();
                oLog.WriteToDebugLogFile("Completed with ERROR", sFuncName);
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                sResult = "ERROR " + sErrDesc;
            }
            return sResult;
        }

        public string Before(string value, string a)
        {
            int posA = value.IndexOf(a);
            if (posA == -1)
            {
                return "";
            }
            return value.Substring(0, posA);
        }

        public string SaveSignature(string sPath, string sJsonInput, string sDONumber)
        {
            string path = string.Empty;
            byte[] bytes = Convert.FromBase64String(sJsonInput);
            Image img;
            string sImageName = string.Empty;
            sImageName = MyExtensions.AppendTimeStamp(sDONumber);
            sDONumber = sDONumber + "";
            path = sPath + sImageName + ".Png";
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                img = Image.FromStream(ms);
                //   img.Save(path, ImageFormat.Png);
            }

            using (var bitmapImage = new Bitmap(img.Width, img.Height))
            {
                bitmapImage.SetResolution(img.HorizontalResolution, img.VerticalResolution);

                using (var g = Graphics.FromImage(bitmapImage))
                {
                    g.Clear(Color.White);
                    g.DrawImageUnscaled(img, 0, 0);
                }

                // Now save b as a JPEG like you normally would
                bitmapImage.Save(path, System.Drawing.Imaging.ImageFormat.Png);
            }
            return path;
        }

        #endregion

        #region Delivery Status Toner Parts

        public string UpdateDeliveryOrder_TonerParts(DataTable dtHeader, DataTable dtAttachments, DataTable dtItems, DataTable dtAdditionalItems, string sCompany)
        {
            string sFuncName = string.Empty;
            string sSaveAttachmentResult = string.Empty;
            try
            {
                sFuncName = "UpdateDeliveryOrder_TonerParts()";
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                using (SqlCommand command = connection.CreateCommand())
                {
                    //connection.BeginTransaction();
                    DataSet ds = Get_BitmapPath(Get_CompanyList(), sCompany);
                    foreach (DataRow item in dtHeader.Rows)
                    {
                        string sSignaturePath = string.Empty;
                        if (item["DoNumber"].ToString() != string.Empty)
                        {
                            if (ds != null && ds.Tables.Count > 0)
                            {
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    sSignaturePath = SaveSignature(ds.Tables[0].Rows[0][0].ToString(), item["RecipientSignature"].ToString().Replace(" ", "+").ToString(), item["DoNumber"].ToString());
                                }
                            }

                            oLog.WriteToDebugLogFile("Before Updating UDFs", sFuncName);

                            string sQuery = string.Empty;
                            sQuery = "UPDATE ODLN SET U_OB_DeliveryDate = '" + item["ArrivalDate"] + "',U_OB_DeliveryTime='" + item["ArrivalTime"] + "'" +
                            " ,U_OB_AdditionalRmk='" + item["Remarks"] + "',U_OB_Priority='" + item["Priority"] + "'" +
                            " ,U_OB_DriverName1='" + item["Driver1"] + "',U_OB_DriverName2='" + item["Driver2"] + "'," +
                            " U_OB_SuppliesPage='" + item["SuppliesPrinted"] + "',U_OB_Signatory ='" + item["RecipientName"] + "'" +
                            " ,U_OB_RecpEmail ='" + item["RecipientEmail"] + "',U_OB_RecpSignText = '" + item["RecipientSignature"].ToString().Replace(" ", "+").ToString() + "',U_OB_RecpSign = '" + sSignaturePath + "'" +
                            " where DocNum = '" + item["DoNumber"] + "' and DocEntry = '" + item["DocEntry"] + "'";

                            oLog.WriteToDebugLogFile("Update DeliveryOrder Header Query" + sQuery, sFuncName);

                            command.CommandText = sQuery;
                            connection.Open();
                            command.ExecuteNonQuery();
                            connection.Close();
                            command.Dispose();
                            oLog.WriteToDebugLogFile("After Updating UDFs", sFuncName);
                        }
                    }

                    if (dtItems != null && dtItems.Rows.Count > 0)
                    {
                        foreach (DataRow item in dtItems.Rows)
                        {
                            if (item["DocEntry"].ToString() != string.Empty)
                            {
                                oLog.WriteToDebugLogFile("Before updating the Item Details", sFuncName);

                                string sQuery = string.Empty;
                                sQuery = "update DLN1 Set U_OB_LineRemarks = '" + item["Remarks"] + "' ,U_OB_TabDelStatus = '" + item["DeliveryStatus"] + "', U_OB_DOQty = '" + item["DOQty"] + "'" +
                                    " , U_OB_TonerPercentage = '" + item["TonerPercentage"] + "'" +
                                    " where DocEntry = '" + item["DocEntry"] + "' and ItemCode = '" + item["ItemCode"] + "' and Dscription = '" + item["ItemDescription"] + "'" +
                                    " and SerialNum = '" + item["SerialNum"] + "'";

                                oLog.WriteToDebugLogFile("Update Items Query" + sQuery, sFuncName);

                                command.CommandText = sQuery;
                                connection.Open();
                                command.ExecuteNonQuery();
                                connection.Close();
                                command.Dispose();
                                oLog.WriteToDebugLogFile("After updating the Item Details", sFuncName);
                            }
                        }
                    }


                    if (dtAdditionalItems != null && dtAdditionalItems.Rows.Count > 0)
                    {
                        foreach (DataRow item in dtAdditionalItems.Rows)
                        {
                            if (item["DocNum"].ToString() != string.Empty)
                            {
                                oLog.WriteToDebugLogFile("Before Inserting the AdditionalItems", sFuncName);

                                string sQuery = string.Empty;
                                sQuery = "INSERT INTO ADV_AdditionalItems(DocEntry,DONumber,ItemCode,ItemName,Quantity,ItemCondition) " +
                                        " VALUES ('" + item["DocEntry"] + "','" + item["DocNum"] + "','" + item["ItemCode"] + "','" + item["ItemDescription"] + "'," +
                                        " '" + item["Qty"] + "','" + item["ItemCondition"] + "')";

                                oLog.WriteToDebugLogFile("Insert Additional Items Query" + sQuery, sFuncName);

                                command.CommandText = sQuery;
                                connection.Open();
                                command.ExecuteNonQuery();
                                connection.Close();
                                command.Dispose();
                                oLog.WriteToDebugLogFile("After Inserting the AdditionalItems", sFuncName);
                            }
                        }
                    }
                }
                if (dtAttachments != null && dtAttachments.Rows.Count > 0)
                {
                    if (dtAttachments.Rows[0]["DocNum"].ToString() != string.Empty)
                    {
                        oLog.WriteToDebugLogFile("Before Adding attachments", sFuncName);
                        sSaveAttachmentResult = SaveAttachments(dtAttachments, sCompany);
                        oLog.WriteToDebugLogFile("Attachment Result : " + sSaveAttachmentResult.ToString(), sFuncName);
                        oLog.WriteToDebugLogFile("After Adding attachments", sFuncName);
                    }
                }

                string sReturnResult = "SUCCESS";
                // To Send the email to the Receipient

                DataRow dr = dtHeader.Rows[0];
                string sEmail = dr["RecipientEmail"].ToString();
                if (sEmail != string.Empty)
                {
                    string sEmailResult = SendTonerPartsEmailTemplate(dtHeader, dtItems, dtAdditionalItems, ref sErrDesc);
                    if (sEmailResult == "SUCCESS")
                    {
                        sReturnResult = "MAILSUCCESS";
                    }
                }

                return sReturnResult;
            }
            catch (Exception ex)
            {
                sErrDesc = ex.Message.ToString();
                oLog.WriteToDebugLogFile("Completed with ERROR", sFuncName);
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                return sErrDesc;
            }
        }

        #endregion

        #region CompanyConnection
        public SAPbobsCOM.Company ConnectToTargetCompany(string sCompanyDB)
        {
            string sFuncName = string.Empty;
            string sReturnValue = string.Empty;
            DataSet oDTCompanyList = new DataSet();
            DataSet oDSResult = new DataSet();
            string sConnString = string.Empty;
            DataView oDTView = new DataView();

            try
            {
                sFuncName = "ConnectToTargetCompany()";

                oLog.WriteToDebugLogFile("Starting Function ", sFuncName);


                if (oDICompany != null)
                {
                    if (oDICompany.CompanyDB == sCompanyDB)
                    {
                        oLog.WriteToDebugLogFile("ODICompany Name " + oDICompany.CompanyDB, sFuncName);
                        oLog.WriteToDebugLogFile("SCompanyDB " + sCompanyDB, sFuncName);
                        return oDICompany;
                    }

                }

                oLog.WriteToDebugLogFile("Calling Get_Company_Details() ", sFuncName);
                oDTCompanyList = Get_CompanyList();

                oLog.WriteToDebugLogFile("Calling Filter Based on Company DB() ", sFuncName);
                oDTView = oDTCompanyList.Tables[0].DefaultView;
                oDTView.RowFilter = "U_DBName= '" + sCompanyDB + "'";

                oLog.WriteToDebugLogFile("Calling ConnectToTargetCompany() ", sFuncName);

                sConnString = oDTView[0]["U_ConnString"].ToString();

                oDICompany = ConnectToTargetCompany(oDICompany, oDTView[0]["U_SAPUserName"].ToString(), oDTView[0]["U_SAPPassword"].ToString()
                                   , oDTView[0]["U_DBName"].ToString(), oDTView[0]["U_Server"].ToString(), oDTView[0]["U_LicenseServer"].ToString()
                                   , oDTView[0]["U_DBUserName"].ToString(), oDTView[0]["U_DBPassword"].ToString(), sErrDesc);

                oLog.WriteToDebugLogFile("Completed With SUCCESS  ", sFuncName);

                return oDICompany;
            }
            catch (Exception Ex)
            {
                sErrDesc = Ex.Message.ToString();
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                throw Ex;
            }

        }

        public SAPbobsCOM.Company ConnectToTargetCompany(SAPbobsCOM.Company oCompany, string sUserName, string sPassword, string sDBName,
                                                        string sServer, string sLicServerName, string sDBUserName
                                                       , string sDBPassword, string sErrDesc)
        {
            string sFuncName = string.Empty;
            long lRetCode;

            try
            {
                sFuncName = "ConnectToTargetCompany()";

                oLog.WriteToDebugLogFile("Starting Function ", sFuncName);

                if (oCompany != null)
                {
                    oLog.WriteToDebugLogFile("Disconnecting the Company object - Company Name " + oCompany.CompanyName, sFuncName);
                    oCompany.Disconnect();
                }
                oLog.WriteToDebugLogFile("Before initializing ", sFuncName);

                oCompany = new SAPbobsCOM.Company();
                oLog.WriteToDebugLogFile("After Initializing Company Connection ", sFuncName);
                oCompany.Server = sServer;
                oCompany.LicenseServer = sLicServerName;
                oCompany.DbUserName = sDBUserName;
                oCompany.DbPassword = sDBPassword;
                oCompany.language = SAPbobsCOM.BoSuppLangs.ln_English;
                oCompany.UseTrusted = false;
                oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2012;


                oCompany.CompanyDB = sDBName;// sDataBaseName;
                oCompany.UserName = sUserName;
                oCompany.Password = sPassword;

                oLog.WriteToDebugLogFile("Connecting the Database...", sFuncName);

                lRetCode = oCompany.Connect();

                if (lRetCode != 0)
                {
                    throw new ArgumentException(oCompany.GetLastErrorDescription());
                }
                else
                {
                    oLog.WriteToDebugLogFile("Company Connection Established", sFuncName);
                    oLog.WriteToDebugLogFile("Completed With SUCCESS", sFuncName);
                    return oCompany;
                }
            }
            catch (Exception Ex)
            {

                sErrDesc = Ex.Message.ToString();
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                throw Ex;
            }
        }
        #endregion

        #region Add Activity
        private string AddActivityUsingActivityService(DataView oDv, string sCompany)
        {
            string functionReturnValue = string.Empty;

            string sFuncName = "AddActivityUsingActivityService";
            string sSQL = string.Empty;
            int sServCallId;
            SAPbobsCOM.Recordset oRecordSet = null;

            try
            {

                oLog.WriteToDebugLogFile("Starting Function ", sFuncName);

                oLog.WriteToDebugLogFile("Connecting Company", sFuncName);
                oDICompany = ConnectToTargetCompany(sCompany);
                oLog.WriteToDebugLogFile("Company Connected successfully", sFuncName);

                SAPbobsCOM.ServiceCalls oService = default(SAPbobsCOM.ServiceCalls);
                oService = oDICompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oServiceCalls);
                oRecordSet = oDICompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                for (int i = 0; i <= oDv.Count - 1; i++)
                {
                    sServCallId = Convert.ToInt32(oDv[i]["ServiceCallId"].ToString());

                    if (oService.GetByKey(sServCallId))
                    {

                        oLog.WriteToDebugLogFile("Processing Service call " + sServCallId, sFuncName);

                        //****************ACTIVITY PHONE CALL
                        if (!(oDv[i]["ServiceCallId"].ToString().Trim() == string.Empty))
                        {

                            oLog.WriteToDebugLogFile("Adding Activity for Phone Call", sFuncName);

                            string sActivityId = string.Empty;
                            int iLine = 0;
                            SAPbobsCOM.Contacts oActivity = default(SAPbobsCOM.Contacts);
                            oActivity = oDICompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oContacts);

                            oActivity.Activity = SAPbobsCOM.BoActivities.cn_Conversation;
                            sSQL = "SELECT Code FROM OCLT WHERE UPPER(Name) = 'General'";
                            oRecordSet.DoQuery(sSQL);
                            if (oRecordSet.RecordCount > 0)
                            {
                                oActivity.ActivityType = oRecordSet.Fields.Item("Code").Value;
                            }
                            oActivity.CardCode = oService.CustomerCode;
                            oActivity.StartDate = DateTime.Now.Date;
                            oActivity.EndDuedate = DateTime.Now.Date;
                            oActivity.StartTime = DateTime.Now;
                            oActivity.EndTime = DateTime.Now.AddMinutes(5);
                            oActivity.HandledBy = Convert.ToInt32(oDv[i]["AssignedEngineerId"]); // Attend User

                            oActivity.Add();

                            sActivityId = oDICompany.GetNewObjectKey();

                            sSQL = "SELECT COUNT(ClgCode) [LineCount] FROM OCLG WHERE parentType = '191' AND parentId = '" + sServCallId + "'";
                            oRecordSet.DoQuery(sSQL);
                            if (oRecordSet.RecordCount > 0)
                            {
                                iLine = oRecordSet.Fields.Item("LineCount").Value;
                            }

                            oService.Activities.Add();
                            oService.Activities.SetCurrentLine(iLine);
                            oService.Activities.ActivityCode = Convert.ToInt32(sActivityId);

                            if (oService.Update() != 0)
                            {
                                sErrDesc = "ERROR WHILE UPDATING THE SERVICE CALL(PHONE CALL ACTIVITY) ID " + sServCallId + " / " + oDICompany.GetLastErrorDescription();
                                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);

                                oLog.WriteToDebugLogFile(sErrDesc, sFuncName);
                                //Throw New ArgumentException(sErrDesc)
                                continue;
                            }

                            oLog.WriteToDebugLogFile("Activity - Job Added successfully to Service call", sFuncName);

                            // Update UDF fileds : 

                            using (SqlConnection connection = new SqlConnection(ConnectionString))
                            using (SqlCommand command = connection.CreateCommand())
                            {
                                oLog.WriteToDebugLogFile("Before Updating UDF", sFuncName);
                                string sTime = System.DateTime.Now.TimeOfDay.ToString().Substring(0, 5).Replace(":", "");
                                command.CommandText = "UPDATE SCL5 SET u_ob_enddate = '" + DateTime.Now.Date + "',u_ob_resp_site_time = '" + sTime + "',u_ob_result = 'Yes'," +
                                                      "u_ob_success = 'Yes',u_ob_resp_sms_date = '" + DateTime.Now.Date + "', u_ob_resp_sms_time = '" + sTime + "'" +
                                                      ",U_ob_activitytype = '" + oDv[i]["TypeOfSLA"] + "'" + " from SCL5 where SrvcCallId = " + 171449 + "";
                                connection.Open();
                                command.ExecuteNonQuery();
                                connection.Close();
                                command.Dispose();
                                oLog.WriteToDebugLogFile("After Updating UDF", sFuncName);
                            }
                        }
                    }
                }

                oLog.WriteToDebugLogFile("Completed with SUCCESS", sFuncName);
                functionReturnValue = RTN_SUCCESS;
            }
            catch (Exception ex)
            {
                sErrDesc = ex.Message;
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                oLog.WriteToDebugLogFile("Completed with ERROR", sFuncName);
                functionReturnValue = sErrDesc;
            }
            return functionReturnValue;
        }
        #endregion

        public DataSet FetchValueInDataset(string sSqlQuery)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter();
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    connection.Open();
                    adapter.SelectCommand = new SqlCommand(sSqlQuery, connection);
                    adapter.Fill(ds);
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }

        public string UpdateQuery(string sSqlQuery)
        {
            string sFuncName = string.Empty;
            try
            {
                sFuncName = "UpdateQuery()";
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                using (SqlCommand command = connection.CreateCommand())
                {
                    oLog.WriteToDebugLogFile("Before Updating UDF", sFuncName);
                    command.CommandText = sSqlQuery;
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                    command.Dispose();
                    oLog.WriteToDebugLogFile("After Updating UDF", sFuncName);
                    return "SUCCESS";
                }
            }
            catch (Exception ex)
            {
                sErrDesc = ex.Message.ToString();
                oLog.WriteToDebugLogFile("Completed with ERROR", sFuncName);
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                return sErrDesc;
            }

        }
    }
}