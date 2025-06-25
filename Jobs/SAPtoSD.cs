using Hangfire;
using Hangfire.Common;

namespace SD_Core_Api.Jobs
{
    public class SAPtoSD : JobFilterAttribute
    {
        [AutomaticRetry(Attempts = 0)]
        [Queue("queue-a_ap")]
        public void SAPSDAP()
        {
            Repository.SapPostingRepo SAPtoSDPost = new Repository.SapPostingRepo();
            SAPtoSDPost.SAP_SD_AP_Data();
        }

    }
}
