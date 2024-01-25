using EmployeeService.DAL.Models;
using ReportService.Structures.Reports.PaymentOnDateRange;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ReportService.Structures.Reports.SalarySummary
{
    public class SalarySummaryReportJsonConverter : JsonConverter<SalarySummaryReport>
    {
        public override SalarySummaryReport? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return Parse(reader.GetString()!);
        }

        public override void Write(Utf8JsonWriter writer, SalarySummaryReport value, JsonSerializerOptions options)
        {
            writer.WriteRawValue(GetJsonString(value));
        }

        private static string GetJsonString(SalarySummaryReport value)
        {
            StringBuilder resultString = new StringBuilder();
            resultString.AppendLine("{");
            resultString.AppendLine("\"Date\": \"" + value.Date.ToString("yyyy-MM-dd") + "\",");
            resultString.AppendLine("\"IncludedCountries\": [");
            var countries = value.IncludedCountriesMetrics.Keys.Select(k => k).ToList();
            for (int i = 0; i < countries.Count; i++)
            {
                resultString.Append(JsonSerializer.Serialize(countries[i]));
                if (i != countries.Count - 1)
                {
                    resultString.Append(",");
                }
                resultString.AppendLine();
            }
            resultString.AppendLine("],");

            resultString.AppendLine("\"StatisticsOnCountries\": [");
            for (int i = 0; i < countries.Count; i++)
            {
                resultString.Append(JsonSerializer.Serialize(value.IncludedCountriesMetrics[countries[i]]));
                if (i != countries.Count - 1)
                {
                    resultString.Append(",");
                }
                resultString.AppendLine();
            }
            resultString.AppendLine("],");

            resultString.AppendLine("\"IncludedOffices\": [");
            var offices = value.IncludedOfficesMetrics.Keys.Select(o => o).ToList();
            for (int i = 0; i < offices.Count; i++)
            {
                resultString.Append(JsonSerializer.Serialize(offices[i]));
                if (i != offices.Count - 1)
                {
                    resultString.Append(",");
                }
                resultString.AppendLine();
            }
            resultString.AppendLine("],");

            resultString.AppendLine("\"StatisticsOnOffices\": [");
            for (int i = 0; i < offices.Count; i++)
            {
                resultString.Append(JsonSerializer.Serialize(value.IncludedOfficesMetrics[offices[i]]));
                if (i != offices.Count - 1)
                {
                    resultString.Append(",");
                }
                resultString.AppendLine();
            }
            resultString.AppendLine("],");

            resultString.AppendLine("\"IncludedEmployees\": [");
            var employees = value.IncludedEmployees.ToList();
            for (int i = 0; i < employees.Count; i++)
            {
                resultString.Append(JsonSerializer.Serialize(employees[i]));
                if (i != employees.Count - 1)
                {
                    resultString.Append(",");
                }
                resultString.AppendLine();
            }
            resultString.AppendLine("],");

            resultString.AppendLine("\"GeneralStatistics\": " + JsonSerializer.Serialize(value.GeneralStatistics));
            resultString.AppendLine("}");

            return resultString.ToString();
        }

        private static SalarySummaryReport Parse(string input)
        {
            SalarySummaryReport resultReport = new SalarySummaryReport();
            StringReader reader = new StringReader(input);

            reader.ReadLine();

            string currentLine = reader.ReadLine();
            currentLine = currentLine.Substring(9, 10);
            resultReport.Date = DateTime.Parse(currentLine);


            List<Country> countryList = new List<Country>();

            currentLine = reader.ReadLine();
            while (currentLine != "],")
            {
                currentLine = reader.ReadLine();
                if (currentLine[currentLine.Length - 1] == ',')
                {
                    currentLine = currentLine.Substring(0, currentLine.Length - 1);
                }
                countryList.Add(JsonSerializer.Deserialize<Country>(currentLine));
            }

            List<StatisticMetric> statisticsOnCountries = new List<StatisticMetric>();
            currentLine = reader.ReadLine();
            while (currentLine != "],")
            {
                currentLine = reader.ReadLine();
                if (currentLine[currentLine.Length - 1] == ',')
                {
                    currentLine = currentLine.Substring(0, currentLine.Length - 1);
                }
                statisticsOnCountries.Add(JsonSerializer.Deserialize<StatisticMetric>(currentLine));
            }

            for (int i = 0; i < countryList.Count; i++)
            {
                resultReport.IncludedCountriesMetrics.Add(countryList[i], statisticsOnCountries[i]);
            }


            List<Office> officeList = new List<Office>();

            currentLine = reader.ReadLine();
            while (currentLine != "],")
            {
                currentLine = reader.ReadLine();
                if (currentLine[currentLine.Length - 1] == ',')
                {
                    currentLine = currentLine.Substring(0, currentLine.Length - 1);
                }
                officeList.Add(JsonSerializer.Deserialize<Office>(currentLine));
            }

            List<StatisticMetric> statisticsOnOffices = new List<StatisticMetric>();
            currentLine = reader.ReadLine();
            while (currentLine != "],")
            {
                currentLine = reader.ReadLine();
                if (currentLine[currentLine.Length - 1] == ',')
                {
                    currentLine = currentLine.Substring(0, currentLine.Length - 1);
                }
                statisticsOnOffices.Add(JsonSerializer.Deserialize<StatisticMetric>(currentLine));
            }

            for (int i = 0; i < officeList.Count; i++)
            {
                resultReport.IncludedOfficesMetrics.Add(officeList[i], statisticsOnOffices[i]);
            }

            List<Employee> employeeList = new List<Employee>();

            currentLine = reader.ReadLine();
            while (currentLine != "],")
            {
                currentLine = reader.ReadLine();
                if (currentLine[currentLine.Length - 1] == ',')
                {
                    currentLine = currentLine.Substring(0, currentLine.Length - 1);
                }
                employeeList.Add(JsonSerializer.Deserialize<Employee>(currentLine));
            }

            resultReport.IncludedEmployees = employeeList;



            currentLine = reader.ReadLine();
            currentLine = currentLine.Substring(21);
            resultReport.GeneralStatistics = JsonSerializer.Deserialize<StatisticMetric>(currentLine);

            return resultReport;
        }
    }
}
