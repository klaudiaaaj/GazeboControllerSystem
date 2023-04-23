using MetricMaster.Enums;

namespace MetricMaster.Repo
{
    public interface IMetricsRepository
    {
        void AddMetics(ApplicationEnum application, ActionEnum action, ApplicationEnum contactWith, DateTime date);
    }
}
