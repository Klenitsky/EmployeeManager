using EmployeeService.Controllers;
using EmployeeService.DAL.Contexts;
using EmployeeService.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using ReportService.Controllers;
using ReportService.Structures.ParameterModels;
using ReportService.Structures.Reports.ExchangeRateOnDateRange;
using ReportService.Structures.Reports.PaymentOnDateRange;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ServicesTests.ReportServiceTests
{
    public class PaymentOnDateRangeReportControllerTest
    {
            private PaymentOnDateRangeReportController _paymentOnDateRangeReportController;
            private List<Office> _includedOffices;
            private List<Country> _includedCountries;
            private HttpClient _httpClient = new HttpClient();

            public PaymentOnDateRangeReportControllerTest()
            {
                _paymentOnDateRangeReportController = new PaymentOnDateRangeReportController(new PaymentOnDateRangeReportBuilder("http://localhost:5282/api/EmployeeService","http://localhost:5027/api/ExchangeRate"));
                _includedOffices = _httpClient.GetFromJsonAsync<IEnumerable<Office>>("http://localhost:5282/api/EmployeeService/Office").Result.Where(o => o.Id == 4 || o.Id == 5 || o.Id == 7).ToList() ;
                _includedCountries = _httpClient.GetFromJsonAsync<IEnumerable<Country>>("http://localhost:5282/api/EmployeeService/Country").Result.Where(o => o.Id == 3).ToList();
            }


            [Fact]
            public void GenerateReportSuccessTest()
            {
                PaymentParametersModel parametersModel = new PaymentParametersModel()
                {
                    StartDate = new DateTime(2024, 2, 1),
                    EndDate = new DateTime(2024, 2, 5),
                    IncludedCountries = _includedCountries,
                    IncludedOffices = _includedOffices
                };
                var resultActionSet  = _paymentOnDateRangeReportController.SetParameters(parametersModel);
                var resultAction = _paymentOnDateRangeReportController.GetOnDateRange();
                var result = resultAction as OkObjectResult;
                var report = result.Value as PaymentOnDateRangeReport;

                Assert.True(resultActionSet is OkResult);
                Assert.True(resultAction is OkObjectResult);
                Assert.NotNull(result);
                Assert.NotNull(report);
                Assert.Equal(new DateTime(2024, 2, 1), report.StartDate);
                Assert.Equal(new DateTime(2024, 2, 5), report.EndDate);
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
            public void GenerateReportBadRangeTest()
            {
            PaymentParametersModel parametersModel = new PaymentParametersModel()
            {
                StartDate = new DateTime(2024, 2, 1),
                EndDate = new DateTime(2024, 1, 5),
                IncludedCountries = _includedCountries,
                IncludedOffices = _includedOffices
            };
            var result = _paymentOnDateRangeReportController.SetParameters(parametersModel);
                Assert.False(result is OkResult);
            }

            [Fact]
            public void GenerateReportBadOfficeTest()
            {
                var offices = _includedOffices.ToList();
                offices.Add(new Office { Name ="qwe", CountryId=1000 });
                PaymentParametersModel parametersModel = new PaymentParametersModel()
                {
                    StartDate = new DateTime(2024, 2, 1),
                    EndDate = new DateTime(2024, 2, 5),
                    IncludedCountries = _includedCountries,
                    IncludedOffices = offices
                };
                var result = _paymentOnDateRangeReportController.SetParameters(parametersModel);
                Assert.False(result is OkResult);
            }

            [Fact]
            public void GenerateReportBadCountryTest()
            {
                var countries = _includedCountries.ToList();
                countries.Add(new Country { Name="avs", Abbreviation="aaa", CurrencyId=1000} );
                PaymentParametersModel parametersModel = new PaymentParametersModel()
                {
                    StartDate = new DateTime(2024, 2, 1),   
                    EndDate = new DateTime(2024, 2, 5),
                    IncludedCountries = countries,
                    IncludedOffices = _includedOffices
                };
                var result = _paymentOnDateRangeReportController.SetParameters(parametersModel);
                Assert.False(result is OkResult);
            }
    }
}
