using EmployeeService.DAL.Models;
using ReportService.Structures.Reports.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportService.Structures.Reports.SalarySummary
{
    public class SalarySummaryReport : IReport
    {
        public DateTime Date { get; set; } 

        public Dictionary<Country, StatisticMetric> IncludedCountriesMetrics { get; set; }

        public Dictionary<Office, StatisticMetric>  IncludedOfficesMetrics { get; set; }
        
        public IEnumerable<Employee> IncludedEmployees { get; set; }


        public StatisticMetric GeneralStatistics { get; set; }




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
