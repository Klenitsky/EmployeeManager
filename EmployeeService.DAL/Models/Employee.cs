namespace EmployeeService.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public int OfficeId { get; set; }
        public Office OfficeNavigation { get; set; }


        public int CurrencyId {  get; set; }
        public Currency CurrencyNavigation { get; set; }

        public float Salary {  get; set; }
    }
}
