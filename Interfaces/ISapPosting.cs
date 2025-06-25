using SD_Core_Api.Models;

namespace SD_Core_Api.Interfaces
{
    public interface ISapPosting
    {
        SDData SD_Data(List<Acknowledgment> objData);
        SAPAcknowledgementDocTrans SAP_SD_AP_Data();
    }
}
