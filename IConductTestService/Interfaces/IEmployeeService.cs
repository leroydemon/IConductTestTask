using IConductTestService.Dtos;

namespace IConductTestService.Interfaces
{
    public interface IEmployeeService
    {
        Task<EmployeeDto?> GetEmployeeTreeAsync(int id, CancellationToken cancellationToken);
        Task<SetEmployeeEnableResult> SetEmployeeEnableAsync(int id, bool enable, CancellationToken cancellationToken);
    }
}
