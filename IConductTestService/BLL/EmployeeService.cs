using IConductTestService.Dtos;
using IConductTestService.Interfaces;

namespace IConductTestService.BLL
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repository;

        public EmployeeService(IEmployeeRepository repository)
        {
            _repository = repository;
        }

        public async Task<EmployeeDto?> GetEmployeeTreeAsync(int id, CancellationToken cancellationToken)
        {
            var list = await _repository.GetEmployeeSubtreeAsync(id, cancellationToken);

            if (list == null || list.Count == 0)
                return null;

            var dict = list.ToDictionary(e => e.Id);
            EmployeeDto? root = null;

            foreach (var emp in dict.Values)
            {
                if (emp.ManagerId.HasValue &&
                    dict.TryGetValue(emp.ManagerId.Value, out var manager))
                {
                    manager.Employees.Add(emp);
                }
                else
                {
                    root = emp;
                }
            }

            return root;
        }

        public Task<SetEmployeeEnableResult> SetEmployeeEnableAsync(int id, bool enable, CancellationToken cancellationToken)
            => _repository.SetEmployeeEnableAsync(id, enable, );
    }


}
