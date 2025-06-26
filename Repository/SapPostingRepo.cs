using Newtonsoft.Json;
using SAPbobsCOM;
using SD_Core_Api.Interfaces;
using SD_Core_Api.Models;
using System.Data;
using System.Data.SqlClient;
using System.Net.Http.Headers;
using System.Text;

namespace SD_Core_Api.Repository
{
    public class SapPostingRepo : ISapPosting
    {
        public SAPbobsCOM.Company oCompany = new Company();

        string Rc_Docnum = "";
        //public SAPbobsCOM.Documents BaseDocument;
        public SAPbobsCOM.Documents oPosting;
        //public SAPbobsCOM.Documents opor;
        //public SAPbobsCOM.Documents opdn;
        //public SAPbobsCOM.Documents ogrn;
        //public SAPbobsCOM.Documents Ordr;

        //private SAPbobsCOM.Recordset oRecSet;
        //private SAPbobsCOM.Recordset oRecSet1;
        //private SAPbobsCOM.Recordset oRecSetPrj;
        //private SAPbobsCOM.Recordset oRecLinenumSet;

        Int32 lRetCode;
        Int32 lErrCode;
        string sErrMsg = "";
        int RetVal;
        /*string tempStr*/
        int ErrCode;
        //string ErrMsg;
        //public new void Dispose()
        //{
        //    Disposes(true);
        //    GC.Collect();
        //    GC.SuppressFinalize(this);
        //}

        //protected void Disposes(bool disposing)
        //{
        //    if (disposing)
        //    {



        //        if (oCompany != null)
        //        {
        //            System.Runtime.InteropServices.Marshal.ReleaseComObject(oCompany);
        //            oCompany = null;
        //        }

        //        if (BaseDocument != null)
        //        {
        //            System.Runtime.InteropServices.Marshal.ReleaseComObject(BaseDocument);
        //            BaseDocument = null;
        //        }
        //        if (oPosting != null)
        //        {
        //            System.Runtime.InteropServices.Marshal.ReleaseComObject(oPosting);
        //            oPosting = null;
        //        }
        //        if (oRecSet != null)
        //        {
        //            System.Runtime.InteropServices.Marshal.ReleaseComObject(oRecSet);
        //            oRecSet = null;
        //        }




        //    }

        //}
        public string object_to_Json(dynamic table)
        {
            string JSONstring = string.Empty;
            JSONstring = JsonConvert.SerializeObject(table, Formatting.Indented);
            return JSONstring;
        }

        public static dynamic json_to_object(dynamic table)
        {
            dynamic myjson_object = JsonConvert.DeserializeObject(table);
            return myjson_object;
        }

        public SDData SD_Data(List<Acknowledgment> jsonData)
        {
            var current_dt = DateTime.Now;

            SDData FinalResult = new SDData();
            List<Acknowledgment> AckdatalIST = new List<Acknowledgment>();

            Connectivity con = new Connectivity();

            oCompany.Server = con.server;
            oCompany.CompanyDB = con.db_name;

            if (con.db_version.ToString() == "2008")
            {
                oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2008;
            }
            else if (con.db_version.ToString() == "2012")
            {
                oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2012;
            }
            else if (con.db_version.ToString() == "2017")
            {
                oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2017;
            }
            else if (con.db_version.ToString() == "2019")
            {
                oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2019;
            }
            else
            {
                oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2017;
            }

            oCompany.DbUserName = con.db_username;
            oCompany.DbPassword = con.db_password;
            oCompany.UserName = con.sap_username;
            oCompany.Password = con.sap_password;
            oCompany.language = SAPbobsCOM.BoSuppLangs.ln_English;
            oCompany.UseTrusted = false;

            oCompany.LicenseServer = con.sap_licenseserver;
            lRetCode = oCompany.Connect();

            if (lRetCode != 0)
            {
                oCompany.GetLastError(out lErrCode, out sErrMsg);
                FinalResult.status = "Failed";
                FinalResult.message = sErrMsg;
                // Dispose();
                return FinalResult;

            }
            else
            {

                foreach (Acknowledgment dtl in jsonData)
                {

                    try
                    {
                        //string approvalId = dtl.approvalId;
                        //string ApprovalStatus = dtl.approvalStatus;
                        //string CompanyCode = dtl.CompanyCode;
                        //string Comments = dtl.reason;
                        //string status = dtl.status;
                        //int TransTypeID = dtl.transType;
                        oPosting = oCompany.GetBusinessObject((SAPbobsCOM.BoObjectTypes)(dtl.transType));

                        int Docentry = Convert.ToInt32(dtl.approvalId);

                        oPosting.GetByKey(Docentry);

                        if (dtl.approvalStatus == "A")
                        {
                            oPosting.UserFields.Fields.Item("U_SD_App_Status").Value = 1;
                        }

                        RetVal = oPosting.Update();

                        if (RetVal != 0)
                        {
                            //Acknowledgment Ackdata = new Acknowledgment();
                            oCompany.GetLastError(out lErrCode, out sErrMsg);
                            dtl.status = "Failed";
                            //Ackdata.approvalId = approvalId.ToString();
                            //Ackdata.CompanyCode = CompanyCode.ToString();
                            dtl.reason = sErrMsg;
                            dtl.approvalStatus = lErrCode.ToString();
                            //dtl.transType = dtl.transType;
                            AckdatalIST.Add(dtl);
                        }
                        else
                        {
                            //oCompany.GetNewObjectCode(out tempStr);
                            //string opch_DocEntry = oCompany.GetNewObjectKey();

                            //Acknowledgment Ackdata = new Acknowledgment();
                            //Ackdata.status = "Success";
                            //Ackdata.approvalId = approvalId.ToString();
                            //Ackdata.CompanyCode = CompanyCode.ToString();
                            //Ackdata.reason = "Approved";
                            //Ackdata.approvalStatus = "A";
                            //Ackdata.transType = dtl.transType;
                            AckdatalIST.Add(dtl);

                        }


                    }

                    catch (Exception e)
                    {
                        if (oCompany.Connected) oCompany.Disconnect();
                        dtl.status = "Failed";
                        dtl.reason = e.InnerException.Message.ToString();
                        dtl.approvalStatus = "0";
                        AckdatalIST.Add(dtl);
                    }
                }


                if (oCompany.Connected) oCompany.Disconnect();
                FinalResult.status = "Success";
                FinalResult.message = "Success";
                FinalResult.data = AckdatalIST;
                return FinalResult;
            }
        }

        public SAPDocTransJson SAP_SD_AP_Data()

        {


            Connectivity con = new Connectivity();
            ;
            List<Acknowledgment> AckdatalIST = new List<Acknowledgment>();
            DataTable AP_lst_dt = new DataTable();
            string AP_Api_URL = con.url;
            string AP_Api_EndPoint = con.endPoint_ap;
            //string Objectcode = "AP";


            //SqlDataAdapter adpt;
            SqlConnection conn = new SqlConnection(con.Connection_string());
            if (conn.State == ConnectionState.Closed) conn.Open();




            List<SAPDocTransResult> results = new List<SAPDocTransResult>();
            SAPDocTransJson sAPDocTransJson = new SAPDocTransJson()
            {
                message = "Success",
                status = "Success",
                data = results,
            };
            using (SqlCommand cmd = new SqlCommand("[@SdUpdated_Get_Api_Data]", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        results.Add(new SAPDocTransResult
                        {
                            // Assuming your class has these properties
                            DocNum = Convert.ToInt32(reader["DocNum"].ToString() ?? "0"),
                            CardCode = reader["CardCode"].ToString() ?? "",
                            CANCELED = reader["CANCELED"].ToString() ?? "",
                            DocDate = Convert.ToDateTime(reader["DocDate"]),
                            DocEntry = Convert.ToInt32(reader["DocEntry"].ToString() ?? "0"),
                            DocRefNo = reader["DocRefNo"].ToString() ?? "",
                            DocRefDate = Convert.ToDateTime(reader["DocRefDate"]),
                            Series = reader["Series"].ToString() ?? "",
                            PIndicator = reader["PIndicator"].ToString() ?? "",
                            TransType = reader["TransType"].ToString() ?? "",
                            DocTotal = Convert.ToDecimal(reader["DocTotal"].ToString() ?? "0"),
                            GSTIN = reader["GSTIN"].ToString() ?? "",
                            STATUS = Convert.ToInt32(reader["STATUS"].ToString() ?? "0"),
                            Category = reader["Category"].ToString() ?? "",
                            WFStatusID = Convert.ToInt32(reader["WFStatusID"].ToString() ?? "0"),
                            WFAssignedToUserIDs = reader["WFAssignedToUserIDs"].ToString() ?? "",
                            IsWFClosed = Convert.ToBoolean(reader["IsWFClosed"]),
                            CostCenter = reader["CostCenter"].ToString() ?? "",
                            BaseAmnt = Convert.ToDecimal(reader["BaseAmnt"].ToString() ?? "0"),
                            GSTTax = Convert.ToDecimal(reader["GSTTax"].ToString() ?? "0"),
                            WTax = Convert.ToDecimal(reader["WTax"].ToString() ?? "0"),
                            PaymentTerms = reader["PaymentTerms"].ToString() ?? "",
                            DocDueDate = Convert.ToDateTime(reader["DocDueDate"]),
                            UserSign = reader["UserSign"].ToString() ?? "",
                            Department = reader["Department"].ToString() ?? "",
                            Branch = reader["Branch"].ToString() ?? "",
                            ContactPerson = reader["ContactPerson"].ToString() ?? "",
                            PAYMODE = reader["PAYMODE"].ToString() ?? "",
                            TaxCode = reader["TaxCode"].ToString() ?? "",
                            WTName = reader["WTName"].ToString() ?? "",
                            AdvanceAdjust = reader["AdvanceAdjust"].ToString() ?? "",
                            Applied = reader["Applied"].ToString() ?? "",
                            BalanceDue = reader["BalanceDue"].ToString() ?? "",
                            WhatsAppNumber = reader["WhatsAppNumber"].ToString() ?? "",
                            CardName = reader["CardName"].ToString() ?? "",
                            Company = reader["Company"].ToString() ?? "",
                            DocTypeID = Convert.ToInt32(reader["DocTypeID"].ToString() ?? "0"),
                            // Add other fields as necessary
                            // ...map other fields
                        });
                    }
                }
            }
            return sAPDocTransJson;


            //}


        }

        public void UpdateDoctransResponse(SAPAcknowledgementDocTrans responseResult)
        {
            Connectivity con = new Connectivity();
            //    //SqlDataAdapter adpt;
            SqlConnection conn = new SqlConnection(con.Connection_string());
            if (conn.State == ConnectionState.Closed) conn.Open();

            List<ResponseData> list = responseResult.data.ToList();

            foreach (ResponseData data in list)
            {
                if (data.DocTypeID == 1)
                {
                    SqlCommand cmd = new SqlCommand("update OPCH set U_SyncStatus = 1 where docentry = '" + data.DocEntry + "'", conn);
                    cmd.ExecuteNonQuery();
                }
                else if (data.DocTypeID == 9)
                {
                    SqlCommand cmd = new SqlCommand("update ORPC set U_SyncStatus = 1 where docentry = '" + data.DocEntry + "'", conn);
                    cmd.ExecuteNonQuery();
                }
                else if (data.DocTypeID == 11)
                {
                    SqlCommand cmd = new SqlCommand("update ORIN set U_SyncStatus = 1 where docentry = '" + data.DocEntry + "'", conn);
                    cmd.ExecuteNonQuery();

                }
            }
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
        }





    }

}
