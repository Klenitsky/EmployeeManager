using Microsoft.AspNetCore.Mvc;
using ReportService.Structures.ParameterModels;
using ReportService.Structures.Reports.ExchangeRateOnDateRange;
using ReportService.Structures.Reports.PaymentOnDateRange;
using ReportService.Structures.Reports.SalarySummary;
using ReportService.Web.DataReaders.EmployeeServiceReaders;
using ReportService.Web.DataReaders.ReportServiceReaders;
using System.Text.Json;

namespace ReportService.Web.Controllers
{
    public class SalarySummaryReportController : Controller
    {
        private SalarySummaryReportReader _reportReader;
        private OfficeReader _officeReader;
        private CountryReader _countryReader;

        public SalarySummaryReportController(SalarySummaryReportReader reportReader, OfficeReader officeReader, CountryReader countryReader)
        {
            _reportReader = reportReader;
            _officeReader = officeReader;
            _countryReader = countryReader;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Create");
        }

        [HttpGet]
        public IActionResult Create()
        {
            var offices = _officeReader.Read();
            ViewBag.Offices = offices.Select(o => (o.Id + " " + o.Name + " (" + o.CountryNavigation.Abbreviation+ ")")).ToArray();
            var countries = _countryReader.Read();
            ViewBag.Countries = countries.Select(c => (c.Id + " " + c.Abbreviation)).ToArray();
            return View();
        }
        [HttpPost]
        public IActionResult Create(DateTime date, string[] offices, string[] countries)
        {
            var args = new SalarySummaryParametersModel
            {
                Date = date,
                IncludedOffices = _officeReader.Read().Where(o => offices.Contains(o.Id + " " + o.Name + " (" + o.CountryNavigation.Abbreviation + ")")).ToList(),
                IncludedCountries = _countryReader.Read().Where(c => countries.Contains(c.Id + " " + c.Abbreviation)).ToList(),
            };
            var report = _reportReader.GetReport(args);
            TempData["report"] = JsonSerializer.Serialize<SalarySummaryReport>(report, new JsonSerializerOptions
            {
                Converters =
                {
                    new SalarySummaryReportJsonConverter()
                }
            });

            return RedirectToAction("Show");
        }


        [HttpGet]
        public IActionResult Show()
        {
            var report = JsonSerializer.Deserialize<SalarySummaryReport>((string)TempData["report"], new JsonSerializerOptions
            {
                Converters =
                {
                    new SalarySummaryReportJsonConverter()
                }
            });
            return View(report);
        }
    }
}
