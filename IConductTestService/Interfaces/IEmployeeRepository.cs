using IConductTestService.Dtos;

namespace IConductTestService.Interfaces
{
    //Should be in Common
    public interface IEmployeeRepository
    {
        Task<List<EmployeeDto>> GetEmployeeSubtreeAsync(int id, CancellationToken cancellationToken);
        Task<SetEmployeeEnableResult> SetEmployeeEnableAsync(int id, bool enable, CancellationToken cancellationToken);
    }

}
