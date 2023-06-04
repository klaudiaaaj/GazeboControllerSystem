using MetricMaster.Enums;

namespace MetricMaster.Repo
{
    public interface IMetricsRepository
    {
        Task AddMessage(CommunicationType type, DateTime sendTime, Guid id);
        void AddMetics(ApplicationEnum application, ActionEnum action, ApplicationEnum contactWith, DateTime date);
        Task UpdateMessageWithArrivalTime(DateTime arrivalTime, Guid id);
    }
}
