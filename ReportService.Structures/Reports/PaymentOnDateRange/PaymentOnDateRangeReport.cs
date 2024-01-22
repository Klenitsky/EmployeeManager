using EmployeeService.DAL.Models;
using OfficeOpenXml;
using ReportService.Structures.Reports.Interfaces;
using ReportService.Structures.Reports.SalarySummary;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ReportService.Structures.Reports.PaymentOnDateRange
{
    public class PaymentOnDateRangeReport : IReport
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Dictionary<Country, float> IncludedCountriesMetrics { get; set; }

        public Dictionary<Office, float> IncludedOfficesMetrics { get; set; }

        public Dictionary<Employee,float> IncludedEmployees { get; set; }


        public float TotalPayment { get; set; }

        public void PrintToTXT(string filename)
        {
            using (StreamWriter writer = new StreamWriter(filename))
            {
                writer.WriteLine("Payment Report on daterange from" + StartDate.ToString("yyyy-MM-dd")+ " to "+ EndDate.ToString("yyyy-MM-dd"));
                foreach (var country in IncludedCountriesMetrics)
                {
                    writer.WriteLine(country.Key.Name+":");
                    foreach (var office in IncludedOfficesMetrics.Where(o => o.Key.CountryId == country.Key.Id))
                    {
                        writer.WriteLine(office.Key.Name);
                        foreach (var employee in IncludedEmployees.Where(e => e.Key.OfficeId == office.Key.Id))
                        {
                            writer.WriteLine(employee.Key.FirstName + " " + employee.Key.LastName + " " + employee.Key.CurrencyNavigation.Abbreviation + " " + employee.Key.Salary+" Amount to Pay: "+ employee.Value);
                        }
                        writer.WriteLine("Office total payment " + office.Value);
                    }

                    writer.WriteLine("Country total payment: " + country.Value);
                }
            }
        }

        public void PrintToXLSX(string filename)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var package = new ExcelPackage();
            var sheet = package.Workbook.Worksheets.Add("Salary Report");

            sheet.Cells[1, 1, 1, 7].Merge = true;
            sheet.Cells[1, 1, 1, 7].Value = "Payment Report on date range " + StartDate.ToString("yyyy-MM-dd")+" - "+EndDate.ToString("yyyy-MM-dd");
            sheet.Cells[1, 1, 1, 7].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            sheet.Cells[1, 1, 1, 7].AutoFitColumns();
            sheet.Cells[2, 1, 3, 1].Merge = true;
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
            sheet.Cells[2, 3, 2, 7].Merge = true;
            sheet.Cells[2, 3, 2, 7].Value = "Employees";
            sheet.Cells[2, 3, 2, 7].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            sheet.Cells[2, 3, 2, 7].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            sheet.Cells[2, 3, 2, 7].AutoFitColumns();
            sheet.Cells[3, 3].Value = "First Name";
            sheet.Cells[3, 3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            sheet.Cells[3, 4].Value = "Last Name";
            sheet.Cells[3, 4].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            sheet.Cells[3, 5].Value = "Currency";
            sheet.Cells[3, 5].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            sheet.Cells[3, 6].Value = "Salary Amount";
            sheet.Cells[3, 6].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            sheet.Cells[3, 7].Value = "Total Payment";
            sheet.Cells[3, 7].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            sheet.Cells[3, 3, 3, 7].AutoFitColumns();

            int verticalIndex = 4;

            foreach (var country in IncludedCountriesMetrics)
            {
                sheet.Cells[verticalIndex, 1].Value = country.Key.Name;
                sheet.Cells[verticalIndex, 6].Value = "Total:";
                sheet.Cells[verticalIndex, 7].Value = country.Value;
              
                verticalIndex++;
                foreach (var office in IncludedOfficesMetrics.Where(o => o.Key.CountryId == country.Key.Id))
                {
                    sheet.Cells[verticalIndex, 2].Value = office.Key.Name;
                    sheet.Cells[verticalIndex, 6].Value = "Total:";
                    sheet.Cells[verticalIndex, 7].Value = office.Value;
                    verticalIndex++;

                    foreach (var employee in IncludedEmployees.Where(e => e.Key.OfficeId == office.Key.Id))
                    {
                        sheet.Cells[verticalIndex, 3].Value = employee.Key.FirstName;
                        sheet.Cells[verticalIndex, 4].Value = employee.Key.LastName;
                        sheet.Cells[verticalIndex, 5].Value = employee.Key.CurrencyNavigation.Abbreviation;
                        sheet.Cells[verticalIndex, 6].Value = employee.Key.Salary;
                        sheet.Cells[verticalIndex, 7].Value = employee.Value;
                        verticalIndex++;
                    }

                }
            }
            sheet.Cells[verticalIndex, 1].Value = "Summary:";
            sheet.Cells[verticalIndex, 6].Value = "Total:";
            sheet.Cells[verticalIndex, 7].Value = TotalPayment;


            for (int i = 1; i <= 7; i++)
            {
                for (int j = 4; j <= verticalIndex; j++)
                {
                    sheet.Cells[j, i].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    sheet.Cells.AutoFitColumns();
                }
            }
            package.Save();


            byte[] reportData = package.GetAsByteArray();
            File.WriteAllBytes(filename, reportData);
        }
    }
}
