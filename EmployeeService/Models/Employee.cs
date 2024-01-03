namespace EmployeeService.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public int OfficeId { get; set; }
        public Office Office { get; set; }


        public int SalaryCurrencyId {  get; set; }
        public Currency SalaryCurrency { get; set; }

        public float Salary {  get; set; }
    }
}
