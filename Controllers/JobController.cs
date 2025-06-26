using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SD_Core_Api.Interfaces;
using SD_Core_Api.Models;

namespace SD_Core_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly ISapPosting _sapPosting;
        public JobController(ISapPosting sapPosting)
        {
            _sapPosting = sapPosting;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("SD_Status")]
        public SDData SD_Status(List<Acknowledgment> objData)
        {
            try
            {
                SDData result = _sapPosting.SD_Data(objData);

                return result;
            }
            catch (Exception e) { throw e; }




        }
        [AllowAnonymous]
        [HttpPost]
        [Route("SD_SAP_Data_Sync")]
        public SAPDocTransJson SD_SAP_Data_Sync()
        {

            SAPDocTransJson result = _sapPosting.SAP_SD_AP_Data();

            return  result;



        }
        [AllowAnonymous]
        [HttpPost]
        [Route("SAP_Sync_Update_Response")]
        public void SAP_Sync_Update_Response(SAPAcknowledgementDocTrans data)
        {
            try
            {
                _sapPosting.UpdateDoctransResponse(data);

            }
            catch (Exception e) { throw e; }


        }





    }
}
