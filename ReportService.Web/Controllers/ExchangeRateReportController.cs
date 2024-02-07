using Microsoft.AspNetCore.Mvc;
using ReportService.Structures.ParameterModels;
using ReportService.Structures.Reports.ExchangeRateOnDateRange;
using ReportService.Web.DataReaders.EmployeeServiceReaders;
using ReportService.Web.DataReaders.ReportServiceReaders;

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
            return View();
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
            return RedirectToAction("Show",report);
        }


        [HttpGet]
        public IActionResult Show(ExchangeRateOnDateRangeReport report)
        {
            return View(report);
        }



    }
}
