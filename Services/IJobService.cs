namespace SD_Core_Api.Services
{
    public interface IJobService
    {
        void FireandForgetJob();
        void ReccuringJob();
        void DelayedJob();
        void ContinuationJob();
        void ExecuteJob();
    }
}
