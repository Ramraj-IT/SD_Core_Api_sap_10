using Newtonsoft.Json;
using SAPbobsCOM;
using Sha_Chiper;
using System.Data.SqlClient;
using System.Net;
using System.Net.Sockets;

namespace SD_Core_Api
{


    public class Connectivity
    {
        IConfiguration configuration;
        public SAPbobsCOM.Company oCompany = new Company();
        public string server;
        public string db_name;
        public string db_version;
        public string db_username;
        public string secret_key;
        public string db_password;
        public string sap_username;
        public string sap_password;
        public string sap_licenseserver;
        public Int32 Branch;
        public string companycode;
        public string endPoint_ap;
        public string endPoint_dn;
        public string endPoint_cn;
        public string endPoint_ocr;
        public string endPoint_nrdc;
        public string endPoint_grn;
        public string endPoint_stk;
        public string endPoint_bp;
        public string url;



        public string bank_myFilePath;

        public Connectivity()
        {

            ClientsAppsetting[] ClientsAppsetting = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("ClientsAppsetting").Get<ClientsAppsetting[]>();



            var clientsetting = ClientsAppsetting[0];



            server = clientsetting.server;

            db_name = clientsetting.db_name;
            db_version = clientsetting.db_version;
            db_username = clientsetting.db_username;
            secret_key = clientsetting.secret_key;
            db_password = Cipher.Decrypt(clientsetting.db_password, secret_key);
            sap_username = clientsetting.sap_username;
            sap_password = Cipher.Decrypt(clientsetting.sap_password, secret_key);
            //sap_password = clientsetting.sap_password;
            sap_licenseserver = clientsetting.sap_licenseserver;
            companycode = clientsetting.companycode;
            endPoint_ap = clientsetting.endPoint_ap;
            endPoint_dn = clientsetting.endPoint_dn;
            endPoint_cn = clientsetting.endPoint_cn;
            endPoint_ocr = clientsetting.endPoint_ocr;
            endPoint_nrdc = clientsetting.endPoint_nrdc;
            endPoint_grn = clientsetting.endPoint_grn;
            endPoint_stk = clientsetting.endPoint_stk;
            endPoint_bp = clientsetting.endPoint_bp;
            url = clientsetting.url;






        }


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

        public string Connection_string()
        {
            SqlConnectionStringBuilder sb = new SqlConnectionStringBuilder();
            sb.DataSource = server;
            sb.InitialCatalog = db_name;
            sb.UserID = db_username;
            sb.Password = db_password;
            return sb.ConnectionString;
        }


        public string SAP_Connection_string()
        {
            SqlConnectionStringBuilder sb = new SqlConnectionStringBuilder();
            sb.DataSource = server;
            sb.InitialCatalog = db_name;
            sb.UserID = db_username;
            sb.Password = db_password;
            return sb.ConnectionString;
        }


    }


    public class ClientsAppsetting
    {
        public string server { get; set; }
        public string db_name { get; set; }
        public string db_version { get; set; }
        public string db_username { get; set; }
        public string secret_key { get; set; }
        public string db_password { get; set; }
        public string sap_username { get; set; }
        public string sap_password { get; set; }
        public string sap_licenseserver { get; set; }
        public string companycode { get; set; }
        public string url { get; set; }
        public string endPoint_ap { get; set; }
        public string endPoint_dn { get; set; }
        public string endPoint_cn { get; set; }
        public string endPoint_ocr { get; set; }
        public string endPoint_nrdc { get; set; }
        public string endPoint_grn { get; set; }
        public string endPoint_stk { get; set; }
        public string endPoint_bp { get; set; }

    }



    public class Utility
    {

        public static T CloneObject<T>(T obj)
        {
            if (obj == null)
                return obj;
            string ser = JsonConvert.SerializeObject(obj, Formatting.Indented,
                new JsonSerializerSettings()
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects
                });
            return (T)JsonConvert.DeserializeObject(ser, obj.GetType());
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }






    }



}
