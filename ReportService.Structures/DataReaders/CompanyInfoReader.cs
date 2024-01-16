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
            List<Employee> employees = _httpClient.GetFromJsonAsync<IEnumerable<Employee>>(_connectionString + "api/EmployeeService/Employee").Result.ToList();
            return employees;

        }

        public Employee FindEmployeeById(int id)
        {
            Employee employee = _httpClient.GetFromJsonAsync<Employee>(_connectionString + "api/EmployeeService/Employee/Find/"+id).Result;
            return employee;

        }

        public IEnumerable<Employee> FindEmployeesByOffice(int officeId)
        {
            List<Employee> employees = _httpClient.GetFromJsonAsync<IEnumerable<Employee>>(_connectionString + "api/EmployeeService/Employee/FindByOffice/"+officeId).Result.ToList();
            return employees;

        }

        public IEnumerable<Office> GetOffices()
        {
            List<Office> offices = _httpClient.GetFromJsonAsync<IEnumerable<Office>>(_connectionString + "api/EmployeeService/Office").Result.ToList();
            return offices;

        }

        public Office FindOfficeById(int id)
        {
            Office office = _httpClient.GetFromJsonAsync<Office>(_connectionString + "api/EmployeeService/Office/Find/" + id).Result;
            return office;

        }

        public IEnumerable<Office> FindOfficesByOffice(int countryId)
        {
            List<Office> offices = _httpClient.GetFromJsonAsync<IEnumerable < Office >> (_connectionString + "api/EmployeeService/Office/FindByCountry/" + countryId).Result.ToList();
            return offices;

        }


        public IEnumerable<Country> GetCountries()
        {
            List<Country> countries = _httpClient.GetFromJsonAsync<IEnumerable<Country>>(_connectionString + "api/EmployeeService/Country").Result.ToList();
            return countries;

        }

        public Country FindCountryById(int id)
        {
            Country country = _httpClient.GetFromJsonAsync<Country>(_connectionString + "api/EmployeeService/Country/Find/" + id).Result;
            return country;

        }

        public IEnumerable<Currency> GetCurrencies()
        {
            List<Currency> currencies= _httpClient.GetFromJsonAsync<IEnumerable<Currency>>(_connectionString + "api/EmployeeService/Currency").Result.ToList();
            return currencies;

        }

        public Currency FindCurrrencyById(int id)
        {
            Currency currency = _httpClient.GetFromJsonAsync<Currency>(_connectionString + "api/EmployeeService/Country/Find/" + id).Result;
            return currency;

        }
    }
}
