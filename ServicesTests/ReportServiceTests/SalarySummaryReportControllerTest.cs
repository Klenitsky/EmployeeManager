using EmployeeService.Controllers;
using EmployeeService.DAL.Contexts;
using EmployeeService.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using ReportService.Controllers;
using ReportService.Structures.ParameterModels;
using ReportService.Structures.Reports.PaymentOnDateRange;
using ReportService.Structures.Reports.SalarySummary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ServicesTests.ReportServiceTests
{
    public class SalarySummaryReportControllerTest
    {
        private SalarySummaryReportController _SalarySummaryReportController;
        private List<Office> _includedOffices;
        private List<Country> _includedCountries;
        private HttpClient _httpClient = new HttpClient();

        public SalarySummaryReportControllerTest()
        {
            _SalarySummaryReportController = new SalarySummaryReportController(new SalarySummaryReportBuilder("http://localhost:5282/api/EmployeeService", "http://localhost:5027/api/ExchangeRate"));
            string[] args = { };
            _includedOffices = _httpClient.GetFromJsonAsync<IEnumerable<Office>>("http://localhost:5282/api/EmployeeService/Office").Result.Where(o => o.Id == 4 || o.Id == 5 || o.Id == 7).ToList();
            _includedCountries = _httpClient.GetFromJsonAsync<IEnumerable<Country>>("http://localhost:5282/api/EmployeeService/Country").Result.Where(o => o.Id == 3).ToList();
        }


        [Fact]
        public void GenerateReportSuccessTest()
        {
            SalarySummaryParametersModel parametersModel = new SalarySummaryParametersModel()
            {
                Date = new DateTime(2023, 11, 1),
                IncludedCountries = _includedCountries,
                IncludedOffices = _includedOffices
            };
            var resultActionSet = _SalarySummaryReportController.SetParameters(parametersModel);
            var resultAction = _SalarySummaryReportController.GetOnDateRange();
            var result = resultAction as OkObjectResult;
            var report = result.Value as SalarySummaryReport;
            Assert.True(resultActionSet is OkResult);
            Assert.True(resultAction is OkObjectResult);
            Assert.NotNull(result.Value);
            Assert.NotNull(report);
            Assert.Equal(new DateTime(2023, 11, 1), report.Date);
            foreach (var includedCountry in _includedCountries)
            {
                Assert.Contains(includedCountry, report.IncludedCountriesMetrics.Keys.ToList());
            }
            foreach (var includedOffice in _includedOffices)
            {
                Assert.Contains(includedOffice, report.IncludedOfficesMetrics.Keys.ToList());
            }
        }

        [Fact]
        public void GenerateReportBadRangeDate()
        {
            SalarySummaryParametersModel parametersModel = new SalarySummaryParametersModel()
            {
                Date = new DateTime(2025, 11, 1),
                IncludedCountries = _includedCountries,
                IncludedOffices = _includedOffices
            };
            var resultAction = _SalarySummaryReportController.SetParameters(parametersModel);
            Assert.False(resultAction is OkResult);
        }

        [Fact]
        public void GenerateReportBadOfficeTest()
        {
            var offices = _includedOffices.ToList();
            offices.Add(new Office { Name = "qwe", CountryId = 1000 });
            SalarySummaryParametersModel parametersModel = new SalarySummaryParametersModel()
            {
                Date = new DateTime(2023, 11, 1),
                IncludedCountries = _includedCountries,
                IncludedOffices = offices
            };
            var resultAction = _SalarySummaryReportController.SetParameters(parametersModel);
            Assert.False(resultAction is OkResult);
        }

        [Fact]
        public void GenerateReportBadCountryTest()
        {
            var countries = _includedCountries.ToList();
            countries.Add(new Country { Name = "avs", Abbreviation = "aaa", CurrencyId = 1000 });
            SalarySummaryParametersModel parametersModel = new SalarySummaryParametersModel()
            {
                Date = new DateTime(2023, 11, 1),
                IncludedCountries = countries,
                IncludedOffices = _includedOffices
            };
            var result = _SalarySummaryReportController.SetParameters(parametersModel);
            Assert.False(result is OkResult);
        }
    }
}
