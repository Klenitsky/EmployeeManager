using EmployeeService.DAL.Models;
using ExchangeRateService.DAL.BasicStructures.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OfficeOpenXml;
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


        public void PrintToTXT(string filename)
        {
            using (StreamWriter writer = new StreamWriter(filename))
            {
                writer.Write("Date:");
                foreach (var currency in IncludedCurrencies)
                {
                    writer.Write("  ");
                    writer.Write(currency.Key.Abbreviation + "(" + ExchangeRates.Where(r => (r.Abbreviation == currency.Key.Abbreviation)).First().Scale + ")");
                }
                writer.WriteLine();

                var currentDate = StartDate;

       
                while (currentDate <= EndDate)
                {
                    
                    writer.Write(currentDate.ToString("yyyy-MM-dd"));
                    foreach (var currency in IncludedCurrencies)
                    { 
                        writer.Write("  ");
                        writer.Write(ExchangeRates.Where(r => (r.Abbreviation == currency.Key.Abbreviation) && (r.Date == currentDate)).First().Rate);
                    }
                    writer.WriteLine();
                    currentDate = currentDate.AddDays(1);                   
                }

                writer.Write("Average:  ");
                foreach (var currency in IncludedCurrencies)
                {
                    writer.Write( currency.Value+"  ");
                }
            }
        }


        public void PrintToXLSX(string filename)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var package = new ExcelPackage();
            var sheet = package.Workbook.Worksheets.Add("Salary Report");
            sheet.Cells["A1"].Value = "Date";
            int index = 2;
            foreach(var currency in IncludedCurrencies)
            {
                sheet.Cells[index,1].Value = currency.Key.Abbreviation +"("+ ExchangeRates.Where(r => (r.Abbreviation == currency.Key.Abbreviation)).First().Scale+")";
                index++;
            }

            var currentDate = StartDate;

            index = 2;
            int horizontalIndex = 2;
            while (currentDate <=EndDate)
            {
              
                sheet.Cells[1,index].Value = currentDate.ToString("yyyy-MM-dd");
                horizontalIndex = 2;
                foreach (var currency in IncludedCurrencies)
                {
                    sheet.Cells[horizontalIndex, index].Value = ExchangeRates.Where(r => (r.Abbreviation == currency.Key.Abbreviation) && (r.Date == currentDate)).First().Rate;
                }
                currentDate = currentDate.AddDays(1);
                index++;
            }

            sheet.Cells[1, index].Value = "Average:";
            horizontalIndex = 2;
            foreach (var currency in IncludedCurrencies)
            {
                sheet.Cells[horizontalIndex, index].Value = currency.Value;
                
                horizontalIndex++;
            }
            sheet.Cells[1, 1, horizontalIndex, index].AutoFitColumns();
            for(int i=1; i <= horizontalIndex;i++)
            {
                for(int j = 1; j <= index; j++)
                {
                    sheet.Cells[i, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                }
            }



            byte[] reportData = package.GetAsByteArray();
            File.WriteAllBytes(filename, reportData);
        }

    }
}
