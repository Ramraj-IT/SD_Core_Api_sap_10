using Hangfire.Common;

namespace SD_Core_Api.Services
{
    public class JobService : IJobService
    {
        private readonly ILogger _logger;

        public JobService(ILogger<JobService> logger)
        {
            _logger = logger;


        }
        public void FireandForgetJob()
        {
            _logger.LogInformation("FireandForgetJob ");
        }
        public void ReccuringJob()
        {
            _logger.LogInformation("ReccuringJob");
        }
        public void DelayedJob()
        {
            _logger.LogInformation("DelayedJob");
        }
         public void ContinuationJob()
        {
            _logger.LogInformation("ContinuationJob");
        }
        public void ExecuteJob()
        {
            _logger.LogInformation("ExecuteJob");
        }
        public void ScheduleJob()
        {
            _logger.LogInformation("ContinuationJob");
           
        }

    }
}
