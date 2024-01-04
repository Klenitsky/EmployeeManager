namespace EmployeeService.Models
{
    public class Country
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }

        public int? NationalCurrencyId { get; set; }
        public Currency? NationalCurrency { get; set; }
    }
}
