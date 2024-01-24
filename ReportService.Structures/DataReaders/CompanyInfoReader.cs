using EmployeeService.DAL.Models;
using ExchangeRateService.DAL.BasicStructures.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ReportService.Structures.DataReaders
{
    public class CompanyInfoReader
    {
        private string _connectionString;
        private HttpClient _httpClient;

        public CompanyInfoReader(string connectionString)
        {
            _connectionString = connectionString;
            _httpClient = new HttpClient();
        }

        public IEnumerable<Employee> GetEmployees()
        {
            string url = _connectionString + "/Employee";
            List<Employee> employees = _httpClient.GetFromJsonAsync<IEnumerable<Employee>>(url).Result.ToList();
            return employees;

        }

        public Employee FindEmployeeById(int id)
        {
            string url = _connectionString + "/Employee/Find/" + id;
            Employee employee = _httpClient.GetFromJsonAsync<Employee>(url).Result;
            return employee;

        }

        public IEnumerable<Employee> FindEmployeesByOffice(int officeId)
        {
            string url = _connectionString + "/Employee/FindByOffice/" + officeId;
            List<Employee> employees = _httpClient.GetFromJsonAsync<IEnumerable<Employee>>(url).Result.ToList();
            return employees;

        }

        public IEnumerable<Office> GetOffices()
        {
            string url = _connectionString + "/Office";
            List<Office> offices = _httpClient.GetFromJsonAsync<IEnumerable<Office>>(url).Result.ToList();
            return offices;

        }

        public Office FindOfficeById(int id)
        {
            string url = _connectionString + "/Office/Find/" + id;
            Office office = _httpClient.GetFromJsonAsync<Office>(url).Result;
            return office;

        }

        public IEnumerable<Office> FindOfficesByCountry(int countryId)
        {
            string url = _connectionString + "/Office/FindByCountry/" + countryId;
            List<Office> offices = _httpClient.GetFromJsonAsync<IEnumerable < Office >> (url).Result.ToList();
            return offices;

        }


        public IEnumerable<Country> GetCountries()
        {
            string url = _connectionString + "/Country";
            List<Country> countries = _httpClient.GetFromJsonAsync<IEnumerable<Country>>(url).Result.ToList();
            return countries;

        }

        public Country FindCountryById(int id)
        {
            string url = _connectionString + "/Country/Find/" + id;
            Country country = _httpClient.GetFromJsonAsync<Country>(url).Result;
            return country;

        }

        public IEnumerable<Currency> GetCurrencies()
        {
            List<Currency> currencies= _httpClient.GetFromJsonAsync<IEnumerable<Currency>>(_connectionString + "/Currency").Result.ToList();
            return currencies;

        }

        public Currency FindCurrrencyById(int id)
        {
            Currency currency = _httpClient.GetFromJsonAsync<Currency>(_connectionString + "/Country/Find/" + id).Result;
            return currency;

        }
    }
}
