using EmployeeService.DAL.Models;
using OfficeOpenXml;
using ReportService.Structures.Reports.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
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

        public void PrintToTXT(string filename)
        {
            using(StreamWriter writer= new StreamWriter(filename))
            {
                writer.WriteLine("Salary Report on date: " + Date.ToString("yyyy-MM-dd"));
                foreach(var country in IncludedCountriesMetrics)
                { 
                    writer.WriteLine(country.Key.Name);
                    foreach (var office in IncludedOfficesMetrics.Where(o=>o.Key.CountryId == country.Key.Id))
                    {
                        writer.WriteLine(office.Key.Name);
                        foreach(var employee in IncludedEmployees.Where(e=>e.OfficeId == office.Key.Id))
                        {
                            writer.WriteLine(employee.FirstName+" "+employee.LastName+" "+employee.CurrencyNavigation.Abbreviation+ " "+employee.Salary);
                        }
                        writer.WriteLine("Office summary: SalarySummary " + office.Value.SalarySummary + " EmployeeCount " + office.Value.NumberOfEmployees + " Average Salary " + office.Value.AverageSalary);
                    }

                        writer.WriteLine("Country summary: SalarySummary " + country.Value.SalarySummary + " EmployeeCount " + country.Value.NumberOfEmployees + " Average Salary " + country.Value.AverageSalary);
                }
            }
        }

        public void PrintToXLSX(string filename)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var package = new ExcelPackage();
            var sheet = package.Workbook.Worksheets.Add("Salary Report");

            sheet.Cells[1, 1, 1, 6].Merge = true;
            sheet.Cells[1, 1, 1, 6].Value = "Salary Report on " + Date.ToString("yyyy-MM-dd");
            sheet.Cells[1, 1, 1, 6].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            sheet.Cells[1, 1, 1, 6].AutoFitColumns();
            sheet.Cells[2,1,3,1].Merge= true;
            sheet.Cells[2, 1, 3, 1].Value = "Country";
            sheet.Cells[2, 1, 3, 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            sheet.Cells[2, 1, 3, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            sheet.Cells[2, 1, 3, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            sheet.Cells[2, 1, 3, 1].AutoFitColumns();
            sheet.Cells[2, 2, 3, 2].Merge = true;
            sheet.Cells[2, 2, 3, 2].Value = "Office";
            sheet.Cells[2, 2, 3, 2].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            sheet.Cells[2, 2, 3, 2].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            sheet.Cells[2, 2, 3, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            sheet.Cells[2, 2, 3, 2].AutoFitColumns();
            sheet.Cells[2, 3, 2, 6].Merge = true;
            sheet.Cells[2, 3, 2, 6].Value = "Employees";
            sheet.Cells[2, 3, 2, 6].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            sheet.Cells[2, 3, 2, 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            sheet.Cells[2, 3, 2, 6].AutoFitColumns();

            sheet.Cells[3, 3].Value = "First Name";
            sheet.Cells[3, 3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            sheet.Cells[3, 4].Value = "Last Name";
            sheet.Cells[3, 4].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            sheet.Cells[3, 5].Value = "Currency";
            sheet.Cells[3, 5].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            sheet.Cells[3, 6].Value = "Salary Amount";
            sheet.Cells[3, 6].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            sheet.Cells[3, 3, 3, 6].AutoFitColumns();
            int verticalIndex = 4;

            foreach (var country in IncludedCountriesMetrics)
            {
                sheet.Cells[verticalIndex,1].Value = country.Key.Name;
                sheet.Cells[verticalIndex, 3].Value = "Employees count";
                sheet.Cells[verticalIndex, 4].Value = country.Value.NumberOfEmployees;
                sheet.Cells[verticalIndex, 5].Value = "Salary Summary";
                sheet.Cells[verticalIndex, 6].Value = country.Value.SalarySummary;
                sheet.Cells[verticalIndex, 7].Value = "Average Salary";
                sheet.Cells[verticalIndex, 8].Value = country.Value.AverageSalary;
                verticalIndex++;
                foreach (var office in IncludedOfficesMetrics.Where(o => o.Key.CountryId == country.Key.Id))
                {
                    sheet.Cells[verticalIndex, 2].Value = office.Key.Name;
                    sheet.Cells[verticalIndex, 3].Value = "Employees count";
                    sheet.Cells[verticalIndex, 4].Value = office.Value.NumberOfEmployees;
                    sheet.Cells[verticalIndex, 5].Value = "Salary Summary";
                    sheet.Cells[verticalIndex, 6].Value = office.Value.SalarySummary;
                    sheet.Cells[verticalIndex, 7].Value = "Average Salary";
                    sheet.Cells[verticalIndex, 8].Value =  office.Value.AverageSalary;
                    verticalIndex++;

                    foreach (var employee in IncludedEmployees.Where(e => e.OfficeId == office.Key.Id))
                    {
                        sheet.Cells[verticalIndex, 3].Value = employee.FirstName;
                        sheet.Cells[verticalIndex, 4].Value = employee.LastName;
                        sheet.Cells[verticalIndex, 5].Value = employee.CurrencyNavigation.Abbreviation;
                        sheet.Cells[verticalIndex, 6].Value = employee.Salary;
                        verticalIndex++;
                    }
                    
                }
            }
            sheet.Cells[verticalIndex, 1].Value ="Summary:" ;
            sheet.Cells[verticalIndex, 3].Value = "Employees count";
            sheet.Cells[verticalIndex, 4].Value = GeneralStatistics.NumberOfEmployees;
            sheet.Cells[verticalIndex, 5].Value = "Salary Summary";
            sheet.Cells[verticalIndex, 6].Value = GeneralStatistics.NumberOfEmployees;
            sheet.Cells[verticalIndex, 7].Value = "Average Salary";
            sheet.Cells[verticalIndex, 8].Value = GeneralStatistics.NumberOfEmployees;


            for(int i=1; i<=6;i++)
            {
                for(int j = 4; j <= verticalIndex; j++)
                {
                    sheet.Cells[j, i].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                }
            }
            sheet.Cells[4, 1, verticalIndex, 8].AutoFitColumns();
            package.Save();


            byte[] reportData = package.GetAsByteArray();
            File.WriteAllBytes(filename, reportData);
        }

    }
}
