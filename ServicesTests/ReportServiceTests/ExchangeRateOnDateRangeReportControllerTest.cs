using EmployeeService.Controllers;
using EmployeeService.DAL.Contexts;
using EmployeeService.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using ReportService.Controllers;
using ReportService.Structures.ParameterModels;
using ReportService.Structures.Reports.ExchangeRateOnDateRange;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ServicesTests.ReportServiceTests
{
    public class ExchangeRateOnDateRangeReportControllerTest
    {
        private ExchangeRateOnDateRangeReportController _exchangeRateOnDateRangeReportController;
        private List<Currency> _includedCurrencies;
        private HttpClient _httpClient = new HttpClient();

        public ExchangeRateOnDateRangeReportControllerTest()
        {
            _exchangeRateOnDateRangeReportController = new ExchangeRateOnDateRangeReportController(new ExchangeRateOnDateRangeReportBuilder("http://localhost:5027/api/ExchangeRate"));
            string[] args = Array.Empty<string>();
            _includedCurrencies = _httpClient.GetFromJsonAsync<IEnumerable<Currency>>("http://localhost:5282/api/EmployeeService/Currency").Result.ToList();
        }

        [Fact]
        public void GenerateReportSuccessTest()
        {
            ExchangeRateParametersModel parametersModel = new ExchangeRateParametersModel()
            {
                StartDate = new DateTime(2024, 2, 1),
                EndDate = new DateTime(2024, 2, 5),
                IncludedCurrencies = _includedCurrencies
            };
            var resultActionSet =  _exchangeRateOnDateRangeReportController.SetParameters(parametersModel);
            var resultAction = _exchangeRateOnDateRangeReportController.GetOnDateRange();
            var result = resultAction as OkObjectResult;
            var report = result.Value as ExchangeRateOnDateRangeReport;

            Assert.True(resultActionSet is OkResult);
            Assert.True(resultAction is OkObjectResult);
            Assert.NotNull(result.Value);
            Assert.NotNull(report);
            Assert.Equal(new DateTime(2024, 2, 1), report.StartDate);
            Assert.Equal(new DateTime(2024, 2, 5), report.EndDate);
            foreach(var includedCurrency in _includedCurrencies)
            {
                Assert.Contains(includedCurrency, report.IncludedCurrencies.Keys.ToList());
            }        
        }

        [Fact]
        public void GenerateReportBadRangeTest()
        {
            ExchangeRateParametersModel parametersModel = new ExchangeRateParametersModel()
            {
                StartDate = new DateTime(2024, 2, 1),
                EndDate = new DateTime(2024, 1, 5),
                IncludedCurrencies = _includedCurrencies
            };
            var result = _exchangeRateOnDateRangeReportController.SetParameters(parametersModel);
            Assert.False(result is OkResult);
        }

        [Fact]
        public void GenerateReportBadCurrenciesTest()
        {
            var currencies = _includedCurrencies.ToList();
            currencies.Add(new Currency { Abbreviation = "abc", Name = "bcd", Code = 212 });
            ExchangeRateParametersModel parametersModel = new ExchangeRateParametersModel()
            {
                StartDate = new DateTime(2024, 2, 1),
                EndDate = new DateTime(2024, 2, 5),
                IncludedCurrencies = currencies
            };
            var result = _exchangeRateOnDateRangeReportController.SetParameters(parametersModel);
            Assert.False(result is OkResult);
        }

    }
}
