using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace API.Extensions
{
    public class DataHealthCheck : IHealthCheck
    {
        private readonly string _connectionString;

        public DataHealthCheck(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync(cancellationToken);

                    var command = connection.CreateCommand();
                    
                    command.CommandText = "select count(*) from suppliers;";

                    var rowReturned = Convert.ToInt32(await command.ExecuteScalarAsync(cancellationToken));

                    if (rowReturned > 0)
                        return HealthCheckResult.Healthy();
                    else
                        return HealthCheckResult.Unhealthy();
                }
            }
            catch (Exception e)
            {
                return HealthCheckResult.Unhealthy(e.Message);
            }
        }
    }
}
