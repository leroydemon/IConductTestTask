namespace IConductTestService.Dtos
{
    //Should be Dto, Entity and Model and to map with Mapper
    public class EmployeeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ManagerId { get; set; }
        public bool Enable { get; set; }

        public List<EmployeeDto> Employees { get; set; } = new List<EmployeeDto>();
    }
}
