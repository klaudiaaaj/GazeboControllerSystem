using Dapper;
using MetricMaster.Enums;
using MetricMaster.Repo;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace MetricMaster
{
    public class MetricsRepository : IMetricsRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string connectionString;

        public MetricsRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public void AddMetics(ApplicationEnum application, ActionEnum action, ApplicationEnum contactWith, DateTime date)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                var dictionary = new Dictionary<string, object>
                {
                    {"@app_id", (int)application },
                    {"@contact_app_id", (int)contactWith },
                    {"@action_id", (int)action },
                    {"@insert_date", date},
                };
                var parameter = new DynamicParameters(dictionary);
                var sql = "Insert into Metrics(app_id, contact_app_id, action_id, insert_date) values (@app_id, @contact_app_id,@action_id, @insert_date)";
                var affectedRow = connection.Execute(sql, parameter);

            }
        }
    }
}
