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
        public ActionResult SD_SAP_Data_Sync()
        {

            var result = _sapPosting.SAP_SD_AP_Data();

            return Ok(result);



        }





    }
}
