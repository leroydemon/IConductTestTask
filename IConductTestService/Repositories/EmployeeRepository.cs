using IConductTestService.Dtos;
using IConductTestService.Interfaces;
using System.Data;
using System.Data.SqlClient;
using System.Threading;

namespace IConductTestService.Repositories
{
    // Should be in other project DAL
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly string _connectionString;

        public EmployeeRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string not found");
        }

        public async Task<List<EmployeeDto>> GetEmployeeSubtreeAsync(int id, CancellationToken cancellationToken)
        {
            await using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            await using var cmd = new SqlCommand("dbo.GetEmployeeTreeById", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            var result = new List<EmployeeDto>();

            await using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.SequentialAccess, cancellationToken);

            var idOrdinal = reader.GetOrdinal("Id");
            var nameOrdinal = reader.GetOrdinal("Name");
            var managerIdOrdinal = reader.GetOrdinal("ManagerId");
            var enableOrdinal = reader.GetOrdinal("Enable");

            while (await reader.ReadAsync(cancellationToken))
            {
                result.Add(new EmployeeDto
                {
                    Id = reader.GetInt32(idOrdinal),
                    Name = reader.GetString(nameOrdinal),
                    ManagerId = reader.IsDBNull(managerIdOrdinal) ? (int?)null : reader.GetInt32(managerIdOrdinal),
                    Enable = reader.GetBoolean(enableOrdinal),
                    Employees = new List<EmployeeDto>()
                });
            }

            return result;
        }


        public async Task<SetEmployeeEnableResult> SetEmployeeEnableAsync(int id, bool enable, CancellationToken cancellationToken)
        {
            await using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync(cancellationToken);

            await using var cmd = new SqlCommand("dbo.SetEmployeeEnable", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
            cmd.Parameters.Add("@Enable", SqlDbType.Bit).Value = enable;

            var affected = await cmd.ExecuteNonQueryAsync(cancellationToken);

            if (affected == 0)
            {
                return SetEmployeeEnableResult.NotFound;
            }

            return SetEmployeeEnableResult.Success;
        }

    }
}
