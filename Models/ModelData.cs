using System.ComponentModel.DataAnnotations;

namespace SD_Core_Api.Models
{
    public class ModelData
    {
    }
    public class SAPDocTransJson
    {
        public string status { get; set; }
        public string message { get; set; }
        public IList<SAPDocTransResult> data { get; set; }

    }
    public class SAPDocTransResult
    {
        public int DocTypeID { get; set; }
        public string Company { get; set; }
        public int DocEntry { get; set; }
        public int DocNum { get; set; }
        public DateTime DocDate { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string DocRefNo { get; set; }
        public DateTime DocRefDate { get; set; }
        public string Series { get; set; }
        public string PIndicator { get; set; }
        public string TransType { get; set; }
        public decimal DocTotal { get; set; }
        public string GSTIN { get; set; }
        public int STATUS { get; set; }
        public string Category { get; set; }
        public int WFStatusID { get; set; }
        public string WFAssignedToUserIDs { get; set; }
        public bool IsWFClosed { get; set; }
        public string CostCenter { get; set; }
        public decimal BaseAmnt { get; set; }
        public decimal GSTTax { get; set; }
        public decimal WTax { get; set; }
        public string PaymentTerms { get; set; }
        public DateTime DocDueDate { get; set; }
        public string CANCELED { get; set; }
        public string UserSign { get; set; }
        public string Department { get; set; }
        public string Branch { get; set; }
        public string ContactPerson { get; set; }
        public string PAYMODE { get; set; }
        public string TaxCode { get; set; }
        public string WTName { get; set; }
        public string AdvanceAdjust { get; set; }
        public string Applied { get; set; }
        public string BalanceDue { get; set; }
        public string WhatsAppNumber { get; set; }

    }


    public class SAPAcknowledgementDocTrans
    {
        public string status { get; set; }
        public string message { get; set; }
        public IList<ResponseData> data { get; set; }

    }

    public class ResponseData
    {
        public int DocTypeID { get; set; }
        public string Company { get; set; }
        public int DocEntry { get; set; }
        public int DocNum { get; set; }
        public string reason { get; set; }
        public string status { get; set; }
    }
    public class Acknowledgment
    {
        [Key]
        public string approvalId { get; set; }
        public string approvalStatus { get; set; }
        public string CompanyCode { get; set; }
        public string reason { get; set; }
        public string status { get; set; }
        public int transType { get; set; }
    }
    public class SDData
    {
        public string status { get; set; }
        public string message { get; set; }
        public IList<Acknowledgment> data { get; set; }

    }

}
