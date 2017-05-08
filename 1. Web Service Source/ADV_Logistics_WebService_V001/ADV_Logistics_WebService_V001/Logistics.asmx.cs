using System.Collections.Generic;
using System.Web.Services;
using ADV_Logistics_BLL;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Data;
using System;
using System.Text.RegularExpressions;
using System.Globalization;


namespace ADV_Logistics_WebService_V001
{
    /// <summary>
    /// Summary description for Logistics
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Logistics : System.Web.Services.WebService
    {
        #region Objects

        clsLog oLog = new clsLog();
        clsDataAccess oDataAccess = new clsDataAccess();
        public string sErrDesc = string.Empty;
        List<result> lstResult = new List<result>();
        JavaScriptSerializer js = new JavaScriptSerializer();

        //public static string EngineerAvailabilitySetup = System.Configuration.ConfigurationManager.AppSettings["EngineerAvailabilitySetup"].ToString();

        #endregion

        #region Web Methods

        #region Login

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public void LoginValidation(string sJsonInput)
        {
            string sFuncName = string.Empty;
            try
            {
                sFuncName = "LoginValidation()";
                oLog.WriteToDebugLogFile("Starting Function ", sFuncName);

                string sUserName = string.Empty;
                string sPassword = string.Empty;
                string sCompany = string.Empty;

                sJsonInput = "[" + sJsonInput + "]";
                //Split JSON to Individual String
                oLog.WriteToDebugLogFile("Getting the Json Input from Mobile  '" + sJsonInput + "'", sFuncName);
                oLog.WriteToDebugLogFile("Before Deserialize the Json Input ", sFuncName);
                List<Json_UserInfo> lstDeserialize = js.Deserialize<List<Json_UserInfo>>(sJsonInput);
                oLog.WriteToDebugLogFile("After Deserialize the Json Input ", sFuncName);
                if (lstDeserialize.Count > 0)
                {
                    Json_UserInfo objUserInfo = lstDeserialize[0];
                    sUserName = objUserInfo.sUserName;
                    sPassword = objUserInfo.sPassword;
                    sCompany = objUserInfo.sCompany;
                }

                DataSet oDTCompanyList = new DataSet();
                oLog.WriteToDebugLogFile("Before calling the Get_CompanyList() ", sFuncName);
                oDTCompanyList = oDataAccess.Get_CompanyList();
                oLog.WriteToDebugLogFile("After calling the Get_CompanyList() ", sFuncName);
                oLog.WriteToDebugLogFile("Before calling the Method LoginValidation() ", sFuncName);
                DataSet ds = oDataAccess.LoginValidation(oDTCompanyList, sUserName, sPassword, sCompany);
                oLog.WriteToDebugLogFile("After calling the Method LoginValidation() ", sFuncName);
                if (ds != null && ds.Tables.Count > 0)
                {
                    List<UserInfo> lstUserInfo = new List<UserInfo>();
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        UserInfo _userInfo = new UserInfo();
                        _userInfo.UserId = r["UserId"].ToString();
                        _userInfo.UserName = r["UserName"].ToString();
                        _userInfo.ContactNumber = r["ContactNumber"].ToString();
                        _userInfo.CompanyCode = r["CompanyCode"].ToString();
                        _userInfo.Message = r["Message"].ToString();
                        lstUserInfo.Add(_userInfo);
                    }
                    oLog.WriteToDebugLogFile("Before Serializing the UserInformation ", sFuncName);
                    Context.Response.Output.Write(js.Serialize(lstUserInfo));
                    oLog.WriteToDebugLogFile("After Serializing the UserInformation , the Serialized data is ' " + js.Serialize(lstUserInfo) + " '", sFuncName);
                }
                else
                {
                    List<UserInfo> lstUserInfo = new List<UserInfo>();
                    UserInfo objUserInfo = new UserInfo();
                    objUserInfo.UserId = string.Empty;
                    objUserInfo.UserName = string.Empty;
                    objUserInfo.ContactNumber = string.Empty;
                    objUserInfo.CompanyCode = sCompany;
                    objUserInfo.Message = "UserName/ Password is Incorrect";
                    lstUserInfo.Add(objUserInfo);

                    Context.Response.Output.Write(js.Serialize(lstUserInfo));
                }
            }
            catch (Exception ex)
            {
                sErrDesc = ex.Message.ToString();
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                result objResult = new result();
                objResult.Result = "Error";
                objResult.DisplayMessage = sErrDesc;
                lstResult.Add(objResult);
                Context.Response.Output.Write(js.Serialize(lstResult));
            }
        }

        #endregion

        #region Driver Management Module

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public void GetCompanyList()
        {
            string sFuncName = string.Empty;
            try
            {
                sFuncName = "GetCompanyList";
                oLog.WriteToDebugLogFile("Starting Function ", sFuncName);

                oLog.WriteToDebugLogFile("Before calling the Get_CompanyList() ", sFuncName);
                DataSet ds = oDataAccess.Get_CompanyList();
                oLog.WriteToDebugLogFile("After calling the Get_CompanyList() ", sFuncName);
                if (ds != null && ds.Tables.Count > 0)
                {
                    List<Company> lstCompany = new List<Company>();
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        Company _company = new Company();
                        _company.U_DBName = r["U_DBName"].ToString();
                        _company.U_CompName = r["U_CompName"].ToString();
                        _company.U_SAPUserName = r["U_SAPUserName"].ToString();
                        _company.U_SAPPassword = r["U_SAPPassword"].ToString();
                        _company.U_DBUserName = r["U_DBUserName"].ToString();
                        _company.U_DBPassword = r["U_DBPassword"].ToString();
                        _company.U_ConnString = r["U_ConnString"].ToString();
                        _company.U_Server = r["U_Server"].ToString();
                        _company.U_LicenseServer = r["U_LicenseServer"].ToString();
                        lstCompany.Add(_company);
                    }
                    oLog.WriteToDebugLogFile("Before Serializing the Company List ", sFuncName);
                    Context.Response.Output.Write(js.Serialize(lstCompany));
                    oLog.WriteToDebugLogFile("After Serializing the Company List , the Serialized data is ' " + js.Serialize(lstCompany) + " '", sFuncName);
                }
                else
                {
                    List<Company> lstCompany = new List<Company>();
                    Context.Response.Output.Write(js.Serialize(lstCompany));
                }
            }
            catch (Exception ex)
            {
                sErrDesc = ex.Message.ToString();
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                result objResult = new result();
                objResult.Result = "Error";
                objResult.DisplayMessage = sErrDesc;
                lstResult.Add(objResult);
                Context.Response.Output.Write(js.Serialize(lstResult));
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public void InsertUpdateDeleteDriver(string sJsonInput, string sCompany)
        {
            //{
            //    "Id": "",
            //    "DriverId": "",
            //    "DriverName": "",
            //    "UserName": "",
            //    "Password": "",
            //    "ContactNumber" : ""
            //    "IsActive": "",
            //    "TypeofOperation": ""
            //}
            string sFuncName = string.Empty;
            try
            {
                sFuncName = "InsertUpdateDeleteDriver()";
                oLog.WriteToDebugLogFile("Starting Function ", sFuncName);

                oLog.WriteToDebugLogFile("Before calling the Get_CompanyList() ", sFuncName);
                DataSet dsCompanyList = oDataAccess.Get_CompanyList();
                oLog.WriteToDebugLogFile("After calling the Get_CompanyList() ", sFuncName);

                sJsonInput = "[" + sJsonInput + "]";
                oLog.WriteToDebugLogFile("Getting the Json Input from Mobile  '" + sJsonInput + "'", sFuncName);
                DataTable dtDriverList = JsonStringToDataTable(sJsonInput);

                oLog.WriteToDebugLogFile("Before calling the Method InsertUpdateDeleteDriver() ", sFuncName);
                DataSet ds = oDataAccess.InsertUpdateDeleteDriver(dsCompanyList, dtDriverList, sCompany);
                oLog.WriteToDebugLogFile("After calling the Method InsertUpdateDeleteDriver() ", sFuncName);

                oLog.WriteToDebugLogFile("Completed With SUCCESS ", sFuncName);
                result objResult = new result();
                objResult.Result = ds.Tables[0].Rows[0]["Status"].ToString();
                objResult.DisplayMessage = ds.Tables[0].Rows[0]["Message"].ToString();
                lstResult.Add(objResult);
                Context.Response.Output.Write(js.Serialize(lstResult));
            }
            catch (Exception ex)
            {
                sErrDesc = ex.Message.ToString();
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                result objResult = new result();
                objResult.Result = "Error";
                objResult.DisplayMessage = sErrDesc;
                lstResult.Add(objResult);
                Context.Response.Output.Write(js.Serialize(lstResult));
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public void GetDriverList(string sCompany)
        {
            string sFuncName = string.Empty;
            try
            {
                sFuncName = "GetDriverList()";
                oLog.WriteToDebugLogFile("Starting Function ", sFuncName);

                oLog.WriteToDebugLogFile("Before calling the Get_CompanyList() ", sFuncName);
                DataSet dsCompanyList = oDataAccess.Get_CompanyList();
                oLog.WriteToDebugLogFile("After calling the Get_CompanyList() ", sFuncName);

                oLog.WriteToDebugLogFile("Before calling the Method LoginValidation() ", sFuncName);
                DataSet dsDriverList = oDataAccess.GetAllDriverList(dsCompanyList, sCompany);
                oLog.WriteToDebugLogFile("After calling the Method LoginValidation() ", sFuncName);
                if (dsDriverList != null && dsDriverList.Tables.Count > 0)
                {
                    List<JSON_DriverList> lstDriverInfo = new List<JSON_DriverList>();
                    foreach (DataRow r in dsDriverList.Tables[0].Rows)
                    {
                        JSON_DriverList _driverInfo = new JSON_DriverList();
                        _driverInfo.Id = r["Id"].ToString();
                        _driverInfo.DriverId = r["DriverId"].ToString();
                        _driverInfo.DriverName = r["DriverName"].ToString();
                        _driverInfo.UserName = r["UserName"].ToString();
                        _driverInfo.Password = r["Password"].ToString();
                        _driverInfo.ContactNumber = r["ContactNumber"].ToString();
                        _driverInfo.IsActive = r["IsActive"].ToString();
                        _driverInfo.TypeofOperation = string.Empty;
                        lstDriverInfo.Add(_driverInfo);
                    }
                    oLog.WriteToDebugLogFile("Before Serializing the Driver Information ", sFuncName);
                    Context.Response.Output.Write(js.Serialize(lstDriverInfo));
                    oLog.WriteToDebugLogFile("After Serializing the  Driver Information , the Serialized data is ' " + js.Serialize(lstDriverInfo) + " '", sFuncName);
                }
                else
                {
                    List<JSON_DriverList> lstDriverInfo = new List<JSON_DriverList>();
                    JSON_DriverList _driverInfo = new JSON_DriverList();
                    _driverInfo.Id = string.Empty;
                    _driverInfo.DriverId = string.Empty;
                    _driverInfo.DriverName = string.Empty;
                    _driverInfo.UserName = string.Empty;
                    _driverInfo.Password = string.Empty;
                    _driverInfo.ContactNumber = string.Empty;
                    _driverInfo.IsActive = string.Empty;
                    _driverInfo.TypeofOperation = string.Empty;
                    lstDriverInfo.Add(_driverInfo);

                    Context.Response.Output.Write(js.Serialize(lstDriverInfo));
                }
            }
            catch (Exception ex)
            {
                sErrDesc = ex.Message.ToString();
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                result objResult = new result();
                objResult.Result = "Error";
                objResult.DisplayMessage = sErrDesc;
                lstResult.Add(objResult);
                Context.Response.Output.Write(js.Serialize(lstResult));
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public void GetDriverNames(string sCompany)
        {
            string sFuncName = string.Empty;
            try
            {
                sFuncName = "GetDriverNames()";
                oLog.WriteToDebugLogFile("Starting Function ", sFuncName);

                oLog.WriteToDebugLogFile("Before calling the Get_CompanyList() ", sFuncName);
                DataSet dsCompanyList = oDataAccess.Get_CompanyList();
                oLog.WriteToDebugLogFile("After calling the Get_CompanyList() ", sFuncName);

                oLog.WriteToDebugLogFile("Before calling the Method LoginValidation() ", sFuncName);
                DataSet dsDriverList = oDataAccess.GetAllDriverNames(dsCompanyList, sCompany);
                oLog.WriteToDebugLogFile("After calling the Method LoginValidation() ", sFuncName);
                if (dsDriverList != null && dsDriverList.Tables.Count > 0)
                {
                    List<DriverNames> lstDriverInfo = new List<DriverNames>();
                    foreach (DataRow r in dsDriverList.Tables[0].Rows)
                    {
                        DriverNames _driverInfo = new DriverNames();
                        _driverInfo.DriverId = r["DriverId"].ToString();
                        _driverInfo.DriverName = r["DriverName"].ToString();
                        lstDriverInfo.Add(_driverInfo);
                    }
                    oLog.WriteToDebugLogFile("Before Serializing the Driver Information ", sFuncName);
                    Context.Response.Output.Write(js.Serialize(lstDriverInfo));
                    oLog.WriteToDebugLogFile("After Serializing the  Driver Information , the Serialized data is ' " + js.Serialize(lstDriverInfo) + " '", sFuncName);
                }
                else
                {
                    List<DriverNames> lstDriverInfo = new List<DriverNames>();
                    DriverNames _driverInfo = new DriverNames();
                    _driverInfo.DriverId = string.Empty;
                    _driverInfo.DriverName = string.Empty;
                    lstDriverInfo.Add(_driverInfo);

                    Context.Response.Output.Write(js.Serialize(lstDriverInfo));
                }
            }
            catch (Exception ex)
            {
                sErrDesc = ex.Message.ToString();
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                result objResult = new result();
                objResult.Result = "Error";
                objResult.DisplayMessage = sErrDesc;
                lstResult.Add(objResult);
                Context.Response.Output.Write(js.Serialize(lstResult));
            }
        }

        #endregion

        #region Call Assignment

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public void SearchCallAssignment(string sJsonInput, string sCompany)
        {
            string sFuncName = string.Empty;
            try
            {
                sFuncName = "GetCallAssignment()";
                oLog.WriteToDebugLogFile("Starting Function ", sFuncName);

                sJsonInput = "[" + sJsonInput + "]";
                oLog.WriteToDebugLogFile("Getting the Json Input 1 from Mobile  '" + sJsonInput + "'", sFuncName);
                DataTable dtSearch = JsonStringToNewDataTable(sJsonInput);

                oLog.WriteToDebugLogFile("Before calling the Get_CompanyList() ", sFuncName);
                DataSet dsCompanyList = oDataAccess.Get_CompanyList();
                oLog.WriteToDebugLogFile("After calling the Get_CompanyList() ", sFuncName);

                oLog.WriteToDebugLogFile("Before calling the Method GetCallAssignement() ", sFuncName);
                DataSet dsCallList = oDataAccess.SearchCallAssignment(dsCompanyList, sCompany, dtSearch);
                oLog.WriteToDebugLogFile("After calling the Method GetCallAssignement() ", sFuncName);

                if (dsCallList != null && dsCallList.Tables.Count > 0)
                {
                    DataTable dtt = dsCallList.Tables[0];
                    List<CallAssignment> lstCallAssignement = dtt.DataTableToList<CallAssignment>();
                    oLog.WriteToDebugLogFile("Before Serializing the Call Information ", sFuncName);
                    Context.Response.Output.Write(js.Serialize(lstCallAssignement));
                    oLog.WriteToDebugLogFile("After Serializing the  call Information , the Serialized data is ' " + js.Serialize(lstCallAssignement) + " '", sFuncName);
                }
                else
                {
                    DataTable dtt = new DataTable();
                    List<CallAssignment> lstCallAssignement = dtt.DataTableToList<CallAssignment>();
                    CallAssignment _callInfo = new CallAssignment();
                    lstCallAssignement.Add(_callInfo);

                    Context.Response.Output.Write(js.Serialize(lstCallAssignement));
                }
            }
            catch (Exception ex)
            {
                sErrDesc = ex.Message.ToString();
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                result objResult = new result();
                objResult.Result = "Error";
                objResult.DisplayMessage = sErrDesc;
                lstResult.Add(objResult);
                Context.Response.Output.Write(js.Serialize(lstResult));
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public void SearchCallAssignment_ItemList(string sJsonInput, string sCompany)
        {
            string sFuncName = string.Empty;
            try
            {
                sFuncName = "GetCallAssignment_ItemList()";
                oLog.WriteToDebugLogFile("Starting Function ", sFuncName);

                sJsonInput = "[" + sJsonInput + "]";
                oLog.WriteToDebugLogFile("Getting the Json Input 1 from Mobile  '" + sJsonInput + "'", sFuncName);
                DataTable dtSearch = JsonStringToNewDataTable(sJsonInput);

                oLog.WriteToDebugLogFile("Before calling the Get_CompanyList() ", sFuncName);
                DataSet dsCompanyList = oDataAccess.Get_CompanyList();
                oLog.WriteToDebugLogFile("After calling the Get_CompanyList() ", sFuncName);

                oLog.WriteToDebugLogFile("Before calling the Method GetCallAssignement_ItemList() ", sFuncName);
                DataSet dsCallList = oDataAccess.SearchCallAssignment_ItemList(dsCompanyList, sCompany, dtSearch);
                oLog.WriteToDebugLogFile("After calling the Method GetCallAssignement_ItemList() ", sFuncName);

                if (dsCallList != null && dsCallList.Tables.Count > 0)
                {
                    DataTable dtt = dsCallList.Tables[0];
                    List<CallAssignment_ItemList> lstCallAssignement = dtt.DataTableToList<CallAssignment_ItemList>();
                    oLog.WriteToDebugLogFile("Before Serializing the Call ItemList Information ", sFuncName);
                    Context.Response.Output.Write(js.Serialize(lstCallAssignement));
                    oLog.WriteToDebugLogFile("After Serializing the  call ItemList Information , the Serialized data is ' " + js.Serialize(lstCallAssignement) + " '", sFuncName);
                }
                else
                {
                    DataTable dtt = new DataTable();
                    List<CallAssignment_ItemList> lstCallAssignement = dtt.DataTableToList<CallAssignment_ItemList>();
                    CallAssignment_ItemList _callInfo = new CallAssignment_ItemList();
                    lstCallAssignement.Add(_callInfo);

                    Context.Response.Output.Write(js.Serialize(lstCallAssignement));
                }
            }
            catch (Exception ex)
            {
                sErrDesc = ex.Message.ToString();
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                result objResult = new result();
                objResult.Result = "Error";
                objResult.DisplayMessage = sErrDesc;
                lstResult.Add(objResult);
                Context.Response.Output.Write(js.Serialize(lstResult));
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public void GetCallAssignment(string sCompany, string sDriverName)
        {
            string sFuncName = string.Empty;
            try
            {
                sFuncName = "GetCallAssignment()";
                oLog.WriteToDebugLogFile("Starting Function ", sFuncName);

                oLog.WriteToDebugLogFile("Before calling the Get_CompanyList() ", sFuncName);
                DataSet dsCompanyList = oDataAccess.Get_CompanyList();
                oLog.WriteToDebugLogFile("After calling the Get_CompanyList() ", sFuncName);

                oLog.WriteToDebugLogFile("Before calling the Method GetCallAssignement() ", sFuncName);
                DataSet dsCallList = oDataAccess.GetCallAssignement(dsCompanyList, sCompany, sDriverName);
                oLog.WriteToDebugLogFile("After calling the Method GetCallAssignement() ", sFuncName);

                if (dsCallList != null && dsCallList.Tables.Count > 0)
                {
                    DataTable dtt = dsCallList.Tables[0];
                    List<CallAssignment> lstCallAssignement = dtt.DataTableToList<CallAssignment>();
                    oLog.WriteToDebugLogFile("Before Serializing the Call Information ", sFuncName);
                    Context.Response.Output.Write(js.Serialize(lstCallAssignement));
                    oLog.WriteToDebugLogFile("After Serializing the  call Information , the Serialized data is ' " + js.Serialize(lstCallAssignement) + " '", sFuncName);
                }
                else
                {
                    DataTable dtt = new DataTable();
                    List<CallAssignment> lstCallAssignement = dtt.DataTableToList<CallAssignment>();
                    CallAssignment _callInfo = new CallAssignment();
                    lstCallAssignement.Add(_callInfo);

                    Context.Response.Output.Write(js.Serialize(lstCallAssignement));
                }
            }
            catch (Exception ex)
            {
                sErrDesc = ex.Message.ToString();
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                result objResult = new result();
                objResult.Result = "Error";
                objResult.DisplayMessage = sErrDesc;
                lstResult.Add(objResult);
                Context.Response.Output.Write(js.Serialize(lstResult));
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public void GetCallAssignment_ItemList(string sCompany, string sDriverName)
        {
            string sFuncName = string.Empty;
            try
            {
                sFuncName = "GetCallAssignment_ItemList()";
                oLog.WriteToDebugLogFile("Starting Function ", sFuncName);

                oLog.WriteToDebugLogFile("Before calling the Get_CompanyList() ", sFuncName);
                DataSet dsCompanyList = oDataAccess.Get_CompanyList();
                oLog.WriteToDebugLogFile("After calling the Get_CompanyList() ", sFuncName);

                oLog.WriteToDebugLogFile("Before calling the Method GetCallAssignement_ItemList() ", sFuncName);
                DataSet dsCallList = oDataAccess.GetCallAssignement_ItemList(dsCompanyList, sCompany, sDriverName);
                oLog.WriteToDebugLogFile("After calling the Method GetCallAssignement_ItemList() ", sFuncName);

                if (dsCallList != null && dsCallList.Tables.Count > 0)
                {
                    DataTable dtt = dsCallList.Tables[0];
                    List<CallAssignment_ItemList> lstCallAssignement = dtt.DataTableToList<CallAssignment_ItemList>();
                    oLog.WriteToDebugLogFile("Before Serializing the Call ItemList Information ", sFuncName);
                    Context.Response.Output.Write(js.Serialize(lstCallAssignement));
                    oLog.WriteToDebugLogFile("After Serializing the  call ItemList Information , the Serialized data is ' " + js.Serialize(lstCallAssignement) + " '", sFuncName);
                }
                else
                {
                    DataTable dtt = new DataTable();
                    List<CallAssignment_ItemList> lstCallAssignement = dtt.DataTableToList<CallAssignment_ItemList>();
                    CallAssignment_ItemList _callInfo = new CallAssignment_ItemList();
                    lstCallAssignement.Add(_callInfo);

                    Context.Response.Output.Write(js.Serialize(lstCallAssignement));
                }
            }
            catch (Exception ex)
            {
                sErrDesc = ex.Message.ToString();
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                result objResult = new result();
                objResult.Result = "Error";
                objResult.DisplayMessage = sErrDesc;
                lstResult.Add(objResult);
                Context.Response.Output.Write(js.Serialize(lstResult));
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public void GetCompletedCall(string sCompany, string sDriverName)
        {
            string sFuncName = string.Empty;
            try
            {
                sFuncName = "GetCompletedCall()";
                oLog.WriteToDebugLogFile("Starting Function ", sFuncName);

                oLog.WriteToDebugLogFile("Before calling the Get_CompanyList() ", sFuncName);
                DataSet dsCompanyList = oDataAccess.Get_CompanyList();
                oLog.WriteToDebugLogFile("After calling the Get_CompanyList() ", sFuncName);

                oLog.WriteToDebugLogFile("Before calling the Method GetCompletedCall() ", sFuncName);
                DataSet dsCallList = oDataAccess.GetCompletedCall(dsCompanyList, sCompany, sDriverName);
                oLog.WriteToDebugLogFile("After calling the Method GetCompletedCall() ", sFuncName);

                if (dsCallList != null && dsCallList.Tables.Count > 0)
                {
                    DataTable dtt = dsCallList.Tables[0];
                    //List<CompletedCall> lstCallAssignement = dtt.DataTableToList<CompletedCall>();

                    List<CompletedCall> lstCallAssignement = new List<CompletedCall>();

                    DataView view = new DataView(dtt);
                    DataTable distinctValues = view.ToTable(true, "EmailSentDate", "EmailSentTime", "TYPE", "DocNum", "DocEntry", "DocDate", "SvcCall",
                        "CustomerCode", "CustomerName", "ContactPerson", "Email", "CompletionTime");

                    for (int i = 0; i <= distinctValues.Rows.Count - 1; i++)
                    {
                        CompletedCall completedCall = new CompletedCall();
                        string sDocNum = "DocNum = '" + distinctValues.Rows[i]["DocNum"].ToString() + "'";

                        DataView dv = dsCallList.Tables[0].DefaultView;
                        dv.RowFilter = sDocNum;
                        List<Attachments> lstAttachment = new List<Attachments>();
                        foreach (DataRowView rowView in dv)
                        {
                            DataRow row = rowView.Row;
                            Attachments _docAttachment = new Attachments();
                            if (row["FilePath"].ToString() != string.Empty)
                            {
                                _docAttachment.DocEntry = row["DocNum"].ToString();
                                _docAttachment.FilePath = row["FilePath"].ToString();
                                _docAttachment.FileName = row["FileName"].ToString();
                                _docAttachment.FileExtension = row["FileExtension"].ToString();
                                _docAttachment.IsNew = "N";
                                lstAttachment.Add(_docAttachment);
                            }
                        }

                        completedCall.No = string.Empty;
                        completedCall.EmailSentDate = distinctValues.Rows[i]["EmailSentDate"].ToString();
                        completedCall.EmailSentTime = distinctValues.Rows[i]["EmailSentTime"].ToString();
                        completedCall.TYPE = distinctValues.Rows[i]["TYPE"].ToString();
                        completedCall.DocNum = distinctValues.Rows[i]["DocNum"].ToString();
                        completedCall.DocEntry = distinctValues.Rows[i]["DocEntry"].ToString();
                        completedCall.DocDate = distinctValues.Rows[i]["DocDate"].ToString();
                        completedCall.SvcCall = distinctValues.Rows[i]["SvcCall"].ToString();
                        completedCall.CustomerCode = distinctValues.Rows[i]["CustomerCode"].ToString();
                        completedCall.CustomerName = distinctValues.Rows[i]["CustomerName"].ToString();
                        completedCall.ContactPerson = distinctValues.Rows[i]["ContactPerson"].ToString();
                        completedCall.Email = distinctValues.Rows[i]["Email"].ToString();
                        completedCall.CompletionTime = distinctValues.Rows[i]["CompletionTime"].ToString();
                        completedCall.Attachments = lstAttachment;

                        lstCallAssignement.Add(completedCall);
                    }

                    oLog.WriteToDebugLogFile("Before Serializing the Call Information ", sFuncName);
                    Context.Response.Output.Write(js.Serialize(lstCallAssignement));
                    oLog.WriteToDebugLogFile("After Serializing the  call Information , the Serialized data is ' " + js.Serialize(lstCallAssignement) + " '", sFuncName);
                }
                else
                {
                    DataTable dtt = new DataTable();
                    List<CompletedCall> lstCallAssignement = dtt.DataTableToList<CompletedCall>();
                    CompletedCall _callInfo = new CompletedCall();
                    lstCallAssignement.Add(_callInfo);

                    Context.Response.Output.Write(js.Serialize(lstCallAssignement));
                }
            }
            catch (Exception ex)
            {
                sErrDesc = ex.Message.ToString();
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                result objResult = new result();
                objResult.Result = "Error";
                objResult.DisplayMessage = sErrDesc;
                lstResult.Add(objResult);
                Context.Response.Output.Write(js.Serialize(lstResult));
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public void GetCallAssignment_CompletedItemList(string sCompany, string sDriverName)
        {
            string sFuncName = string.Empty;
            try
            {
                sFuncName = "GetCallAssignment_ItemList()";
                oLog.WriteToDebugLogFile("Starting Function ", sFuncName);

                oLog.WriteToDebugLogFile("Before calling the Get_CompanyList() ", sFuncName);
                DataSet dsCompanyList = oDataAccess.Get_CompanyList();
                oLog.WriteToDebugLogFile("After calling the Get_CompanyList() ", sFuncName);

                oLog.WriteToDebugLogFile("Before calling the Method GetCallAssignement_CompletedItemsList() ", sFuncName);
                DataSet dsCallList = oDataAccess.GetCallAssignement_CompletedItemsList(dsCompanyList, sCompany, sDriverName);
                oLog.WriteToDebugLogFile("After calling the Method GetCallAssignement_CompletedItemsList() ", sFuncName);

                if (dsCallList != null && dsCallList.Tables.Count > 0)
                {
                    DataTable dtt = dsCallList.Tables[0];
                    List<CallAssignment_CompletedItemList> lstCallAssignement = dtt.DataTableToList<CallAssignment_CompletedItemList>();
                    oLog.WriteToDebugLogFile("Before Serializing the Call ItemList Information ", sFuncName);
                    Context.Response.Output.Write(js.Serialize(lstCallAssignement));
                    oLog.WriteToDebugLogFile("After Serializing the  call ItemList Information , the Serialized data is ' " + js.Serialize(lstCallAssignement) + " '", sFuncName);
                }
                else
                {
                    DataTable dtt = new DataTable();
                    List<CallAssignment_CompletedItemList> lstCallAssignement = dtt.DataTableToList<CallAssignment_CompletedItemList>();
                    CallAssignment_CompletedItemList _callInfo = new CallAssignment_CompletedItemList();
                    lstCallAssignement.Add(_callInfo);

                    Context.Response.Output.Write(js.Serialize(lstCallAssignement));
                }
            }
            catch (Exception ex)
            {
                sErrDesc = ex.Message.ToString();
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                result objResult = new result();
                objResult.Result = "Error";
                objResult.DisplayMessage = sErrDesc;
                lstResult.Add(objResult);
                Context.Response.Output.Write(js.Serialize(lstResult));
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public void GetCallAssignment_UpdateDragOrder(string sJsonInput, string sCompany)
        {
            string sFuncName = string.Empty;
            try
            {
                sFuncName = "GetCallAssignment_UpdateDragOrder()";
                oLog.WriteToDebugLogFile("Starting Function ", sFuncName);

                sJsonInput = "[" + sJsonInput + "]";
                oLog.WriteToDebugLogFile("Getting the Json Input from Mobile  '" + sJsonInput + "'", sFuncName);
                DataTable dtUpdateOrder = JsonStringToDataTable(sJsonInput);

                oLog.WriteToDebugLogFile("Before calling the Method UpdateDragOrder() ", sFuncName);
                string sResult = oDataAccess.UpdateDragOrder(dtUpdateOrder, sCompany);
                oLog.WriteToDebugLogFile("After calling the Method UpdateDragOrder() ", sFuncName);
                if (sResult == "SUCCESS")
                {
                    oLog.WriteToDebugLogFile("Completed With SUCCESS ", sFuncName);
                    result objResult = new result();
                    objResult.Result = "SUCCESS";
                    objResult.DisplayMessage = "Sort Order Successfully Updated";
                    lstResult.Add(objResult);
                    Context.Response.Output.Write(js.Serialize(lstResult));
                }
                else
                {
                    oLog.WriteToDebugLogFile("Completed With Error ", sFuncName);
                    result objResult = new result();
                    objResult.Result = "Failure";
                    objResult.DisplayMessage = sResult;
                    lstResult.Add(objResult);
                    Context.Response.Output.Write(js.Serialize(lstResult));
                }
            }
            catch (Exception ex)
            {
                sErrDesc = ex.Message.ToString();
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                result objResult = new result();
                objResult.Result = "Error";
                objResult.DisplayMessage = sErrDesc;
                lstResult.Add(objResult);
                Context.Response.Output.Write(js.Serialize(lstResult));
            }
        }

        #endregion

        #region ETA

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public void UpdateETA(string sJsonInput, string sJsonInput1, string sCompany)
        {
            string sFuncName = string.Empty;
            try
            {
                sFuncName = "UpdateETA()";
                oLog.WriteToDebugLogFile("Starting Function ", sFuncName);

                oLog.WriteToDebugLogFile("Before calling the Get_CompanyList() ", sFuncName);
                DataSet dsCompanyList = oDataAccess.Get_CompanyList();
                oLog.WriteToDebugLogFile("After calling the Get_CompanyList() ", sFuncName);

                sJsonInput = "[" + sJsonInput + "]";
                oLog.WriteToDebugLogFile("Getting the Json Input 1 from Mobile  '" + sJsonInput + "'", sFuncName);
                DataTable dtCallUpdate = JsonStringToDataTable(sJsonInput);

                sJsonInput1 = "[" + sJsonInput1 + "]";
                oLog.WriteToDebugLogFile("Getting the Json Input 2 from Mobile  '" + sJsonInput1 + "'", sFuncName);
                DataTable dtItemList = JsonStringToDataTable(sJsonInput1);
                DataTable dtAdditionalItemList = new DataTable();

                oLog.WriteToDebugLogFile("Before calling the Method UpdateETA() ", sFuncName);
                DataSet ds = oDataAccess.UpdateETA(dsCompanyList, dtCallUpdate, sCompany);
                oLog.WriteToDebugLogFile("After calling the Method UpdateETA() ", sFuncName);

                if (ds.Tables[0].Rows[0]["Status"].ToString() == "SUCCESS")
                {
                    string sSendEmailResult = oDataAccess.SendEmail(dtCallUpdate, dtItemList, dtAdditionalItemList, sCompany);
                    if (sSendEmailResult == "SUCCESS")
                    {
                        oLog.WriteToDebugLogFile("Completed With SUCCESS ", sFuncName);
                        result objResult = new result();
                        objResult.Result = "SUCCESS";
                        objResult.DisplayMessage = "ETA Updated and Mail send successfully";
                        lstResult.Add(objResult);
                        Context.Response.Output.Write(js.Serialize(lstResult));
                    }
                    else
                    {
                        oLog.WriteToDebugLogFile("Completed With SUCCESS ", sFuncName);
                        result objResult = new result();
                        objResult.Result = "SUCCESS";
                        objResult.DisplayMessage = "ETA Updated successfully";
                        lstResult.Add(objResult);
                        Context.Response.Output.Write(js.Serialize(lstResult));
                    }
                }
            }
            catch (Exception ex)
            {
                sErrDesc = ex.Message.ToString();
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                result objResult = new result();
                objResult.Result = "Error";
                objResult.DisplayMessage = sErrDesc;
                lstResult.Add(objResult);
                Context.Response.Output.Write(js.Serialize(lstResult));
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public void SaveAttachments(string sJsonInput, string sCompany)
        {
            string sFuncName = string.Empty;
            try
            {
                sFuncName = "SaveAttachments()";
                oLog.WriteToDebugLogFile("Starting Function ", sFuncName);

                sJsonInput = "[" + sJsonInput + "]";
                oLog.WriteToDebugLogFile("Getting the Json Input from Mobile  '" + sJsonInput + "'", sFuncName);
                DataTable dtAttachments = JsonStringToDataTable(sJsonInput);

                oLog.WriteToDebugLogFile("Before calling the Method SaveAttachments() ", sFuncName);
                string sResult = oDataAccess.SaveAttachments(dtAttachments, sCompany);
                oLog.WriteToDebugLogFile("After calling the Method SaveAttachments() ", sFuncName);
                if (sResult == "SUCCESS")
                {
                    oLog.WriteToDebugLogFile("Completed With SUCCESS ", sFuncName);
                    result objResult = new result();
                    objResult.Result = "SUCCESS";
                    objResult.DisplayMessage = "Attachment Added successfully";
                    lstResult.Add(objResult);
                    Context.Response.Output.Write(js.Serialize(lstResult));
                }
                else if (sResult == string.Empty)
                {
                    oLog.WriteToDebugLogFile("No New Attachments Added", sFuncName);
                    result objResult = new result();
                    objResult.Result = "SUCCESS";
                    objResult.DisplayMessage = "No New Attachments Added";
                    lstResult.Add(objResult);
                    Context.Response.Output.Write(js.Serialize(lstResult));
                }
                else
                {
                    oLog.WriteToDebugLogFile("Completed With Error ", sFuncName);
                    result objResult = new result();
                    objResult.Result = "Failure";
                    objResult.DisplayMessage = sResult;
                    lstResult.Add(objResult);
                    Context.Response.Output.Write(js.Serialize(lstResult));
                }
            }
            catch (Exception ex)
            {
                sErrDesc = ex.Message.ToString();
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                result objResult = new result();
                objResult.Result = "Error";
                objResult.DisplayMessage = sErrDesc;
                lstResult.Add(objResult);
                Context.Response.Output.Write(js.Serialize(lstResult));
            }
        }

        #endregion

        #region Delivery Status Deals

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public void GetItemList(string sCompany)
        {
            string sFuncName = string.Empty;
            try
            {
                sFuncName = "GetItemList()";
                oLog.WriteToDebugLogFile("Starting Function ", sFuncName);

                oLog.WriteToDebugLogFile("Before calling the Get_CompanyList() ", sFuncName);
                DataSet dsCompanyList = oDataAccess.Get_CompanyList();
                oLog.WriteToDebugLogFile("After calling the Get_CompanyList() ", sFuncName);

                oLog.WriteToDebugLogFile("Before calling the Method GetItemList() ", sFuncName);
                DataSet dsItemlList = oDataAccess.GetItemList(dsCompanyList, sCompany);
                oLog.WriteToDebugLogFile("After calling the Method GetItemList() ", sFuncName);

                if (dsItemlList != null && dsItemlList.Tables.Count > 0)
                {
                    DataTable dtt = dsItemlList.Tables[0];
                    List<DeliveryStatus_Items> lstItems = dtt.DataTableToList<DeliveryStatus_Items>();
                    oLog.WriteToDebugLogFile("Before Serializing the Item Information ", sFuncName);
                    Context.Response.Output.Write(js.Serialize(lstItems));
                    oLog.WriteToDebugLogFile("After Serializing the  Item Information , the Serialized data is ' " + js.Serialize(lstItems) + " '", sFuncName);
                }
                else
                {
                    DataTable dtt = new DataTable();
                    List<DeliveryStatus_Items> lstItem = dtt.DataTableToList<DeliveryStatus_Items>();
                    DeliveryStatus_Items _callInfo = new DeliveryStatus_Items();
                    lstItem.Add(_callInfo);

                    Context.Response.Output.Write(js.Serialize(lstItem));
                }
            }
            catch (Exception ex)
            {
                sErrDesc = ex.Message.ToString();
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                result objResult = new result();
                objResult.Result = "Error";
                objResult.DisplayMessage = sErrDesc;
                lstResult.Add(objResult);
                Context.Response.Output.Write(js.Serialize(lstResult));
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public void GetCustomercontactDetails(string sCompany, string sCardCode)
        {
            string sFuncName = string.Empty;
            try
            {
                sFuncName = "GetCustomercontactDetails()";
                oLog.WriteToDebugLogFile("Starting Function ", sFuncName);

                oLog.WriteToDebugLogFile("Before calling the Get_CompanyList() ", sFuncName);
                DataSet dsCompanyList = oDataAccess.Get_CompanyList();
                oLog.WriteToDebugLogFile("After calling the Get_CompanyList() ", sFuncName);

                oLog.WriteToDebugLogFile("Before calling the Method GetCustomercontactDetails() ", sFuncName);
                DataSet dsItemlList = oDataAccess.GetCustomercontactDetails(dsCompanyList, sCompany, sCardCode);
                oLog.WriteToDebugLogFile("After calling the Method GetCustomercontactDetails() ", sFuncName);

                if (dsItemlList != null && dsItemlList.Tables.Count > 0)
                {
                    DataTable dtt = dsItemlList.Tables[0];
                    List<Customercontact> lstContact = dtt.DataTableToList<Customercontact>();
                    oLog.WriteToDebugLogFile("Before Serializing the Customer Information ", sFuncName);
                    Context.Response.Output.Write(js.Serialize(lstContact));
                    oLog.WriteToDebugLogFile("After Serializing the Customer Information , the Serialized data is ' " + js.Serialize(lstContact) + " '", sFuncName);
                }
                else
                {
                    DataTable dtt = new DataTable();
                    List<Customercontact> lstContact = dtt.DataTableToList<Customercontact>();
                    Customercontact _ContactInfo = new Customercontact();
                    lstContact.Add(_ContactInfo);

                    Context.Response.Output.Write(js.Serialize(lstContact));
                }
            }
            catch (Exception ex)
            {
                sErrDesc = ex.Message.ToString();
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                result objResult = new result();
                objResult.Result = "Error";
                objResult.DisplayMessage = sErrDesc;
                lstResult.Add(objResult);
                Context.Response.Output.Write(js.Serialize(lstResult));
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public void GetDeliveryCustomercontactDetails(string sCompany, string sDocNum)
        {
            string sFuncName = string.Empty;
            try
            {
                sFuncName = "GetDeliveryCustomercontactDetails()";
                oLog.WriteToDebugLogFile("Starting Function ", sFuncName);

                oLog.WriteToDebugLogFile("Before calling the Get_CompanyList() ", sFuncName);
                DataSet dsCompanyList = oDataAccess.Get_CompanyList();
                oLog.WriteToDebugLogFile("After calling the Get_CompanyList() ", sFuncName);

                oLog.WriteToDebugLogFile("Before calling the Method GetCustomercontactDetails() ", sFuncName);
                DataSet dsItemlList = oDataAccess.GetDeliveryCustomercontactDetails(dsCompanyList, sCompany, sDocNum);
                oLog.WriteToDebugLogFile("After calling the Method GetCustomercontactDetails() ", sFuncName);

                if (dsItemlList != null && dsItemlList.Tables.Count > 0)
                {
                    DataTable dtt = dsItemlList.Tables[0];
                    List<Customercontact> lstContact = dtt.DataTableToList<Customercontact>();
                    oLog.WriteToDebugLogFile("Before Serializing the Customer Information ", sFuncName);
                    Context.Response.Output.Write(js.Serialize(lstContact));
                    oLog.WriteToDebugLogFile("After Serializing the Customer Information , the Serialized data is ' " + js.Serialize(lstContact) + " '", sFuncName);
                }
                else
                {
                    DataTable dtt = new DataTable();
                    List<Customercontact> lstContact = dtt.DataTableToList<Customercontact>();
                    Customercontact _ContactInfo = new Customercontact();
                    lstContact.Add(_ContactInfo);

                    Context.Response.Output.Write(js.Serialize(lstContact));
                }
            }
            catch (Exception ex)
            {
                sErrDesc = ex.Message.ToString();
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                result objResult = new result();
                objResult.Result = "Error";
                objResult.DisplayMessage = sErrDesc;
                lstResult.Add(objResult);
                Context.Response.Output.Write(js.Serialize(lstResult));
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public void UpdateDeliveryOrder(string Header, string Attachments, string Items, string AdditionalItems, string sCompany)
        {
            string sFuncName = string.Empty;
            try
            {
                sFuncName = "UpdateDeliveryOrder()";
                oLog.WriteToDebugLogFile("Starting Function ", sFuncName);

                Header = "[" + Header + "]";
                Attachments = "[" + Attachments + "]";
                Items = "[" + Items + "]";
                AdditionalItems = "[" + AdditionalItems + "]";
                oLog.WriteToDebugLogFile("Getting the Json Input 1 from Mobile  '" + Header + "'", sFuncName);
                oLog.WriteToDebugLogFile("Getting the Json Input 2 from Mobile  '" + Attachments + "'", sFuncName);
                oLog.WriteToDebugLogFile("Getting the Json Input 3 from Mobile  '" + Items + "'", sFuncName);
                oLog.WriteToDebugLogFile("Getting the Json Input 4 from Mobile  '" + AdditionalItems + "'", sFuncName);
                DataTable dtHeader = JsonStringToDataTable(Header);
                DataTable dtAttachments = JsonStringToDataTable(Attachments);
                DataTable dtItems = JsonStringToDataTable(Items);
                DataTable dtAdditionalItems = JsonStringToDataTable(AdditionalItems);

                oLog.WriteToDebugLogFile("Before calling the Method UpdateDeliveryOrder() ", sFuncName);
                string sResult = oDataAccess.UpdateDeliveryOrder(dtHeader, dtAttachments, dtItems, dtAdditionalItems, sCompany);
                oLog.WriteToDebugLogFile("After calling the Method UpdateDeliveryOrder() ", sFuncName);
                if (sResult == "SUCCESS")
                {
                    oLog.WriteToDebugLogFile("Completed With SUCCESS ", sFuncName);
                    result objResult = new result();
                    objResult.Result = "SUCCESS";
                    objResult.DisplayMessage = "Delivery Order Completed successfully";
                    lstResult.Add(objResult);
                    Context.Response.Output.Write(js.Serialize(lstResult));
                }
                else
                {
                    oLog.WriteToDebugLogFile("Completed With Error ", sFuncName);
                    result objResult = new result();
                    objResult.Result = "FAILURE";
                    objResult.DisplayMessage = sResult;
                    lstResult.Add(objResult);
                    Context.Response.Output.Write(js.Serialize(lstResult));
                }
            }
            catch (Exception ex)
            {
                sErrDesc = ex.Message.ToString();
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                result objResult = new result();
                objResult.Result = "Error";
                objResult.DisplayMessage = sErrDesc;
                lstResult.Add(objResult);
                Context.Response.Output.Write(js.Serialize(lstResult));
            }

        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public void GetDeliveryStatusList(string sCompany)
        {
            string sFuncName = string.Empty;
            try
            {
                sFuncName = "GetDeliveryStatusList()";
                oLog.WriteToDebugLogFile("Starting Function ", sFuncName);

                oLog.WriteToDebugLogFile("Before calling the Get_CompanyList() ", sFuncName);
                DataSet dsCompanyList = oDataAccess.Get_CompanyList();
                oLog.WriteToDebugLogFile("After calling the Get_CompanyList() ", sFuncName);

                oLog.WriteToDebugLogFile("Before calling the Method GetItemList() ", sFuncName);
                DataSet dsItemlList = oDataAccess.GetDeliveryStatusList(dsCompanyList, sCompany);
                oLog.WriteToDebugLogFile("After calling the Method GetItemList() ", sFuncName);

                if (dsItemlList != null && dsItemlList.Tables.Count > 0)
                {
                    DataTable dtt = dsItemlList.Tables[0];
                    List<DeliveryStatus_List> lstItems = dtt.DataTableToList<DeliveryStatus_List>();
                    oLog.WriteToDebugLogFile("Before Serializing the Item Information ", sFuncName);
                    Context.Response.Output.Write(js.Serialize(lstItems));
                    oLog.WriteToDebugLogFile("After Serializing the  Item Information , the Serialized data is ' " + js.Serialize(lstItems) + " '", sFuncName);
                }
                else
                {
                    DataTable dtt = new DataTable();
                    List<DeliveryStatus_List> lstItem = dtt.DataTableToList<DeliveryStatus_List>();
                    DeliveryStatus_List _callInfo = new DeliveryStatus_List();
                    lstItem.Add(_callInfo);

                    Context.Response.Output.Write(js.Serialize(lstItem));
                }
            }
            catch (Exception ex)
            {
                sErrDesc = ex.Message.ToString();
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                result objResult = new result();
                objResult.Result = "Error";
                objResult.DisplayMessage = sErrDesc;
                lstResult.Add(objResult);
                Context.Response.Output.Write(js.Serialize(lstResult));
            }
        }

        #endregion

        #region Delivery Status Toner Parts
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public void UpdateDeliveryOrder_TonerParts(string Header, string Attachments, string Items, string AdditionalItems, string sCompany)
        {
            string sFuncName = string.Empty;
            try
            {
                sFuncName = "UpdateDeliveryOrder_TonerParts()";
                oLog.WriteToDebugLogFile("Starting Function ", sFuncName);

                Header = "[" + Header + "]";
                Attachments = "[" + Attachments + "]";
                Items = "[" + Items + "]";
                AdditionalItems = "[" + AdditionalItems + "]";
                oLog.WriteToDebugLogFile("Getting the Json Input 1 from Mobile  '" + Header + "'", sFuncName);
                oLog.WriteToDebugLogFile("Getting the Json Input 2 from Mobile  '" + Attachments + "'", sFuncName);
                oLog.WriteToDebugLogFile("Getting the Json Input 3 from Mobile  '" + Items + "'", sFuncName);
                oLog.WriteToDebugLogFile("Getting the Json Input 4 from Mobile  '" + AdditionalItems + "'", sFuncName);
                DataTable dtHeader = JsonStringToDataTable(Header);
                DataTable dtAttachments = JsonStringToDataTable(Attachments);
                DataTable dtItems = JsonStringToDataTable(Items);
                DataTable dtAdditionalItems = JsonStringToDataTable(AdditionalItems);

                oLog.WriteToDebugLogFile("Before calling the Method UpdateDeliveryOrder_TonerParts() ", sFuncName);
                string sResult = oDataAccess.UpdateDeliveryOrder_TonerParts(dtHeader, dtAttachments, dtItems, dtAdditionalItems, sCompany);
                oLog.WriteToDebugLogFile("After calling the Method UpdateDeliveryOrder_TonerParts() ", sFuncName);
                if (sResult == "SUCCESS")
                {
                    oLog.WriteToDebugLogFile("Completed With SUCCESS ", sFuncName);
                    result objResult = new result();
                    objResult.Result = "SUCCESS";
                    objResult.DisplayMessage = "Delivery Order Completed successfully";
                    lstResult.Add(objResult);
                    Context.Response.Output.Write(js.Serialize(lstResult));
                }
                else if (sResult == "MAILSUCCESS")
                {
                    oLog.WriteToDebugLogFile("Completed With SUCCESS ", sFuncName);
                    result objResult = new result();
                    objResult.Result = "SUCCESS";
                    objResult.DisplayMessage = "Delivery Order Completed and Mail Sent successfully";
                    lstResult.Add(objResult);
                    Context.Response.Output.Write(js.Serialize(lstResult));
                }
                else
                {
                    oLog.WriteToDebugLogFile("Completed With Error ", sFuncName);
                    result objResult = new result();
                    objResult.Result = "FAILURE";
                    objResult.DisplayMessage = sResult;
                    lstResult.Add(objResult);
                    Context.Response.Output.Write(js.Serialize(lstResult));
                }
            }
            catch (Exception ex)
            {
                sErrDesc = ex.Message.ToString();
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                result objResult = new result();
                objResult.Result = "Error";
                objResult.DisplayMessage = sErrDesc;
                lstResult.Add(objResult);
                Context.Response.Output.Write(js.Serialize(lstResult));
            }

        }
        #endregion

        #endregion

        #region Classes

        class result
        {
            public string Result { get; set; }
            public string DisplayMessage { get; set; }
        }

        class Company
        {
            public string U_DBName { get; set; }
            public string U_CompName { get; set; }
            public string U_SAPUserName { get; set; }
            public string U_SAPPassword { get; set; }
            public string U_DBUserName { get; set; }
            public string U_DBPassword { get; set; }
            public string U_ConnString { get; set; }
            public string U_Server { get; set; }
            public string U_LicenseServer { get; set; }
        }

        class JSON_DriverList
        {
            public string Id { get; set; }
            public string DriverId { get; set; }
            public string DriverName { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }
            public string ContactNumber { get; set; }
            public string IsActive { get; set; }
            public string TypeofOperation { get; set; }
        }

        class UserInfo
        {
            public string UserId { get; set; }
            public string UserName { get; set; }
            public string ContactNumber { get; set; }
            public string CompanyCode { get; set; }
            public string Message { get; set; }
        }

        class Json_UserInfo
        {
            public string sUserName { get; set; }
            public string sPassword { get; set; }
            public string sCompany { get; set; }
        }

        class Json_CallAssignment
        {
            public string sCompany { get; set; }
        }

        class CallAssignment
        {
            public string No { get; set; }
            public string DragOrder { get; set; }
            public string PostalCode { get; set; }
            public string TYPE { get; set; }
            public string DocNum { get; set; }
            public string DocEntry { get; set; }
            public string DeliveryCode { get; set; }
            public string DeliveryType { get; set; }
            public string DocDate { get; set; }
            public string SvcCall { get; set; }
            public string CustomerCode { get; set; }
            public string CustomerName { get; set; }
            public string ContactPerson { get; set; }
            public string Remarks { get; set; }
            public string Item { get; set; }
            public string ETA { get; set; }
            public string ETADATE { get; set; }
            public string CallType { get; set; }
            public string DocDueDate { get; set; }
            public string DriverName { get; set; }
            public string DeliveryStatus { get; set; }
            public string SerialNum { get; set; }
            public string ActivityNo { get; set; }
            public string Address { get; set; }
            public string Department { get; set; }
            public string EquipmentCode { get; set; }
            public string EquipmentName { get; set; }
            public string LineDeliveryStatus { get; set; }
            public string LineRemarks { get; set; }
            public string LiftAccess { get; set; }
            public string Priority { get; set; }
            public string Email { get; set; }
            public string AMPM { get; set; }
            public string Subject { get; set; }
        }

        class CallAssignment_ItemList
        {
            public string No { get; set; }
            public string DocNum { get; set; }
            public string DocEntry { get; set; }
            public string ItemCode { get; set; }
            public string ItemName { get; set; }
            public string Quantity { get; set; }
            public string SerialNum { get; set; }
        }

        class CompletedCall
        {
            public string No { get; set; }
            public string EmailSentDate { get; set; }
            public string EmailSentTime { get; set; }
            public string TYPE { get; set; }
            public string DocNum { get; set; }
            public string DocEntry { get; set; }
            public string DocDate { get; set; }
            public string SvcCall { get; set; }
            public string CustomerCode { get; set; }
            public string CustomerName { get; set; }
            public string ContactPerson { get; set; }
            public string Email { get; set; }
            public string CompletionTime { get; set; }
            public List<Attachments> Attachments { get; set; }
        }

        class Attachments
        {
            public string DocEntry { get; set; }
            public string FilePath { get; set; }
            public string FileName { get; set; }
            public string FileExtension { get; set; }
            public string IsNew { get; set; }
        }

        class CallAssignment_CompletedItemList
        {
            public string No { get; set; }
            public string DocNum { get; set; }
            public string DocEntry { get; set; }
            public string ItemCode { get; set; }
            public string ItemName { get; set; }
            public string Quantity { get; set; }
            public string SerialNum { get; set; }
        }

        class DeliveryStatus_Items
        {
            public string ItemCode { get; set; }
            public string ItemName { get; set; }
        }

        class DeliveryStatus_List
        {
            public string Id { get; set; }
            public string Value { get; set; }
        }

        class Customercontact
        {
            public string RcpName { get; set; }
            public string RcpEmail { get; set; }
        }

        class DriverNames
        {
            public string DriverId { get; set; }
            public string DriverName { get; set; }
        }

        #endregion

        #region Public Methods

        public DataTable JsonStringToNewDataTable(string jsonString)
        {
            DataTable dt = new DataTable();
            string sFuncName = string.Empty;
            try
            {
                sFuncName = "JsonStringToNewDataTable()";
                oLog.WriteToDebugLogFile("Starting Function ", sFuncName);

                string[] jsonStringArray = Regex.Split(jsonString.Replace("[", "").Replace("]", ""), "},{");
                if (jsonStringArray[0].ToString() != string.Empty)
                {
                    List<string> ColumnsName = new List<string>();
                    foreach (string jSA in jsonStringArray)
                    {
                        string sjSA = jSA;
                        if (jSA.Contains("base64,"))
                        {
                            sjSA = jSA.Replace("base64,", "base64;");
                        }
                        string[] jsonStringData = Regex.Split(sjSA.Replace("{", "").Replace("}", ""), "\",");
                        foreach (string ColumnsNameData in jsonStringData)
                        {
                            try
                            {
                                int idx = ColumnsNameData.IndexOf(":");
                                string ColumnsNameString = ColumnsNameData.Substring(0, idx - 1).Replace("\"", "");
                                if (!ColumnsName.Contains(ColumnsNameString.Trim()))
                                {

                                    ColumnsName.Add(ColumnsNameString.Trim());
                                }
                            }
                            catch (Exception ex)
                            {
                                throw new Exception(string.Format("Error Parsing Column Name : {0}", ColumnsNameData));
                            }
                        }
                        break;
                    }
                    foreach (string AddColumnName in ColumnsName)
                    {
                        if (AddColumnName.Contains("Date"))
                        { dt.Columns.Add(AddColumnName, typeof(DateTime)); }
                        else
                        { dt.Columns.Add(AddColumnName); }

                    }
                    foreach (string jSA in jsonStringArray)
                    {
                        string sjSA = jSA;
                        if (jSA.Contains("base64,"))
                        {
                            sjSA = jSA.Replace("base64,", "base64;");
                        }
                        string[] RowData = Regex.Split(sjSA.Replace("{", "").Replace("}", ""), "\",");
                        DataRow nr = dt.NewRow();
                        foreach (string rowData in RowData)
                        {
                            try
                            {
                                string RowDataString = string.Empty;
                                int idx = rowData.Trim().IndexOf(":");
                                string RowColumns = rowData.Trim().Substring(0, idx - 1).Replace("\"", "");
                                if (rowData.Trim().Substring(idx + 1).Replace("\"", "").Contains("base64;"))
                                {
                                    RowDataString = rowData.Trim().Substring(idx + 1).Replace("\"", "").Replace("base64;", "base64,");
                                }
                                else
                                {
                                    RowDataString = rowData.Trim().Substring(idx + 1).Replace("\"", "");
                                }
                                if (RowColumns.Contains("Date"))
                                {
                                    if (RowDataString.Trim() != string.Empty)
                                    {
                                        oLog.WriteToDebugLogFile("Input Date: " + RowDataString.Trim(), sFuncName);
                                        nr[RowColumns] = DateTime.ParseExact(RowDataString.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                        oLog.WriteToDebugLogFile("Output Date: " + nr[RowColumns], sFuncName);
                                    }
                                    else
                                    {
                                        nr[RowColumns] = DBNull.Value;
                                    }
                                }
                                else
                                {
                                    nr[RowColumns] = RowDataString.Trim();
                                }
                            }
                            catch (Exception ex)
                            {
                                continue;
                            }
                        }
                        dt.Rows.Add(nr);
                    }
                }
            }
            catch (Exception ex)
            {
                sErrDesc = ex.Message.ToString();
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
            }
            return dt;
        }

        public DataTable JsonStringToDataTable(string jsonString)
        {
            DataTable dt = new DataTable();
            string sFuncName = string.Empty;
            try
            {
                sFuncName = "JsonStringToDataTable()";
                oLog.WriteToDebugLogFile("Starting Function ", sFuncName);

                string[] jsonStringArray = Regex.Split(jsonString.Replace("[", "").Replace("]", ""), "},{");
                if (jsonStringArray[0].ToString() != string.Empty)
                {
                    List<string> ColumnsName = new List<string>();
                    foreach (string jSA in jsonStringArray)
                    {
                        string sjSA = jSA;
                        if (jSA.Contains("base64,"))
                        {
                            sjSA = jSA.Replace("base64,", "base64;");
                        }
                        string[] jsonStringData = Regex.Split(sjSA.Replace("{", "").Replace("}", ""), "\",");
                        foreach (string ColumnsNameData in jsonStringData)
                        {
                            try
                            {
                                int idx = ColumnsNameData.IndexOf(":");
                                string ColumnsNameString = ColumnsNameData.Substring(0, idx - 1).Replace("\"", "");
                                if (!ColumnsName.Contains(ColumnsNameString.Trim()))
                                {

                                    ColumnsName.Add(ColumnsNameString.Trim());
                                }
                            }
                            catch (Exception ex)
                            {
                                throw new Exception(string.Format("Error Parsing Column Name : {0}", ColumnsNameData));
                            }
                        }
                        break;
                    }
                    foreach (string AddColumnName in ColumnsName)
                    {
                        if (AddColumnName.Contains("Date"))
                        { dt.Columns.Add(AddColumnName, typeof(DateTime)); }
                        else
                        { dt.Columns.Add(AddColumnName); }

                    }
                    foreach (string jSA in jsonStringArray)
                    {
                        string sjSA = jSA;
                        if (jSA.Contains("base64,"))
                        {
                            sjSA = jSA.Replace("base64,", "base64;");
                        }
                        string[] RowData = Regex.Split(sjSA.Replace("{", "").Replace("}", ""), "\",");
                        DataRow nr = dt.NewRow();
                        foreach (string rowData in RowData)
                        {
                            try
                            {
                                string RowDataString = string.Empty;
                                int idx = rowData.Trim().IndexOf(":");
                                string RowColumns = rowData.Trim().Substring(0, idx - 1).Replace("\"", "");
                                if (rowData.Trim().Substring(idx + 1).Replace("\"", "").Contains("base64;"))
                                {
                                    RowDataString = rowData.Trim().Substring(idx + 1).Replace("\"", "").Replace("base64;", "base64,");
                                }
                                else
                                {
                                    RowDataString = rowData.Trim().Substring(idx + 1).Replace("\"", "");
                                }

                                nr[RowColumns] = RowDataString.Trim();

                            }
                            catch (Exception ex)
                            {
                                continue;
                            }
                        }
                        dt.Rows.Add(nr);
                    }
                }
            }
            catch (Exception ex)
            {
                sErrDesc = ex.Message.ToString();
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
            }
            return dt;
        }

        #endregion

    }
}
