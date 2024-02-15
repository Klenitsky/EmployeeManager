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
using System.Text;
using System.Threading.Tasks;

namespace ServicesTests.ReportServiceTests
{
    public class PaymentOnDateRangeReportControllerTest
    {
            private PaymentOnDateRangeReportController _paymentOnDateRangeReportController;
            private List<Office> _includedOffices;
            private List<Country> _includedCountries;


            public PaymentOnDateRangeReportControllerTest()
            {
                _paymentOnDateRangeReportController = new PaymentOnDateRangeReportController(new PaymentOnDateRangeReportBuilder("https://localhost:44316/api/EmployeeService","https://localhost:44341/api/ExchangeRate"));
                string[] args = { };
                _includedOffices = new List<Office>
                {
                        (new OfficeController(new ApplicationDbContextFactory().CreateDbContext(args)).Find(1) as OkObjectResult).Value as Office
                };

                _includedCountries = new List<Country>
                {
                        (new CountryController(new ApplicationDbContextFactory().CreateDbContext(args)).Find(2) as OkObjectResult).Value as Country
                };
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
                var resultAction  = _paymentOnDateRangeReportController.SetParameters(parametersModel);
                resultAction = _paymentOnDateRangeReportController.GetOnDateRange();
                var result = resultAction as OkObjectResult;
                var report = result.Value as PaymentOnDateRangeReport;
                
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
                Assert.False(result is OkObjectResult);
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
                Assert.False(result is OkObjectResult);
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
                Assert.False(result is OkObjectResult);
            }
    }
}
