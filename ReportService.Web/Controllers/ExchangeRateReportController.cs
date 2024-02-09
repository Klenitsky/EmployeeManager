using Microsoft.AspNetCore.Mvc;
using ReportService.Structures.ParameterModels;
using ReportService.Structures.Reports.ExchangeRateOnDateRange;
using ReportService.Web.DataReaders.EmployeeServiceReaders;
using ReportService.Web.DataReaders.ReportServiceReaders;
using System.Text.Json;

namespace ReportService.Web.Controllers
{
    public class ExchangeRateReportController : Controller
    {


        private ExchangeRateOnDateRangeReportReader _reportReader;
        private CurrencyReader _currencyReader;

        public ExchangeRateReportController(ExchangeRateOnDateRangeReportReader reportReader, CurrencyReader currencyReader)
        {
            _reportReader = reportReader;
            _currencyReader = currencyReader;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Create");
        }

        [HttpGet]
        public IActionResult Create()
        {
            var currencies = _currencyReader.Read();
            ViewBag.Currencies = currencies.Select(c => c.Abbreviation).ToArray();
            return View();
        }
        [HttpPost]
        public IActionResult Create(DateTime startDate, DateTime endDate, string[] currencies)
        {
            var args = new ExchangeRateParametersModel()
            {
                StartDate = startDate,
                EndDate = endDate,
                IncludedCurrencies = _currencyReader.Read().Where(c => currencies.Contains(c.Abbreviation)).ToList(),
            };
            var report = _reportReader.GetReport(args);
            TempData["report"] = JsonSerializer.Serialize<ExchangeRateOnDateRangeReport>(report, new JsonSerializerOptions
            {
                Converters =
                {
                    new ExchangeRateOnDateRangeJsonConverter()
                }
            });
            return RedirectToAction("Show");
        }


        [HttpGet]
        public IActionResult Show()
        {
            var report = JsonSerializer.Deserialize<ExchangeRateOnDateRangeReport>((string)TempData["report"], new JsonSerializerOptions
            {
                Converters =
                {
                    new ExchangeRateOnDateRangeJsonConverter()
                }
            });

            return View(report);
        }



    }
}
