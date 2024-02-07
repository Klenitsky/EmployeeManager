using Microsoft.AspNetCore.Mvc;
using ReportService.Structures.ParameterModels;
using ReportService.Structures.Reports.ExchangeRateOnDateRange;
using ReportService.Web.DataReaders.EmployeeServiceReaders;
using ReportService.Web.DataReaders.ReportServiceReaders;

namespace ReportService.Web.Controllers
{
    public class PaymentOnDateRangeReportController : Controller
    {
        private PaymentOnDateRangeReportReader _reportReader;
        private OfficeReader _officeReader;
        private CountryReader _countryReader;

        public PaymentOnDateRangeReportController(PaymentOnDateRangeReportReader reportReader, OfficeReader officeReader, CountryReader countryReader)
        {
            _reportReader = reportReader;
            _officeReader = officeReader;
            _countryReader = countryReader;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            var offices = _officeReader.Read();
            ViewBag.Offices = offices.Select(o => (o.Id +" "+ o.Name)).ToArray();
            var countries = _countryReader.Read();
            ViewBag.Countries = countries.Select(c => (c.Id + " " + c.Abbreviation)).ToArray();
            return View();
        }
        [HttpPost]
        public IActionResult Create(DateTime startDate, DateTime endDate, string[] offices, string[]  countries)
        {
            var args = new PaymentParametersModel
            {
                StartDate = startDate,
                EndDate = endDate,
                IncludedOffices = _officeReader.Read().Where(o => offices.Contains(o.Id + " " + o.Name)).ToList(),
                IncludedCountries = _countryReader.Read().Where(c => countries.Contains(c.Id + " " + c.Abbreviation)).ToList(),
            };
            var report = _reportReader.GetReport(args);
            return RedirectToAction("Show", report);
        }


        [HttpGet]
        public IActionResult Show(ExchangeRateOnDateRangeReport report)
        {
            return View(report);
        }


    }
}
