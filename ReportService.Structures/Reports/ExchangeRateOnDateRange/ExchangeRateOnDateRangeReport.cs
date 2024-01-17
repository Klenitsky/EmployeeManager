using EmployeeService.DAL.Models;
using ExchangeRateService.DAL.BasicStructures.Models;
using ReportService.Structures.Reports.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportService.Structures.Reports.ExchangeRateOnDateRange
{
    public class ExchangeRateOnDateRangeReport: IReport
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Dictionary<Currency,float> IncludedCurrencies { get; set; }
        public List<ExchangeRate> ExchangeRates { get; set; }

        public void PrintToCSV(string filename)
        {
            throw new NotImplementedException();
        }

        public void PrintToPDF(string filename)
        {
            throw new NotImplementedException();
        }

        public void PrintToTXT(string filename)
        {
            throw new NotImplementedException();
        }

        public void PrintToWord(string filename)
        {
            throw new NotImplementedException();
        }

        public void PrintToXLSX(string filename)
        {
            throw new NotImplementedException();
        }

        public void PrintToXML(string filename)
        {
            throw new NotImplementedException();
        }
    }
}
