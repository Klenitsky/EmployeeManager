using EmployeeService.DAL.Models;
using ReportService.Structures.Reports.ExchangeRateOnDateRange;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ReportService.Structures.Reports.PaymentOnDateRange
{
    public class PaymentOnDateRangeJsonConverter : JsonConverter<PaymentOnDateRangeReport>
    {
        public override PaymentOnDateRangeReport? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using (var jsonDoc = JsonDocument.ParseValue(ref reader))
            {
                string jsonText = jsonDoc.RootElement.GetRawText();
                return Parse(jsonText);
            }
        }

        public override void Write(Utf8JsonWriter writer, PaymentOnDateRangeReport value, JsonSerializerOptions options)
        {
           writer.WriteRawValue(GetJsonString(value));
        }


        private static string GetJsonString(PaymentOnDateRangeReport value)
        {
            StringBuilder resultString = new StringBuilder();
            resultString.AppendLine("{");
            resultString.AppendLine("\"StartDate\": \"" + value.StartDate.ToString("yyyy-MM-dd") + "\",");
            resultString.AppendLine("\"EndDate\": \"" + value.EndDate.ToString("yyyy-MM-dd") + "\",");
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

            resultString.AppendLine("\"PaymentOnCountries\": [");
            for (int i = 0; i < countries.Count; i++)
            {
                resultString.Append(value.IncludedCountriesMetrics[countries[i]].ToString(System.Globalization.CultureInfo.GetCultureInfo("en-US")));
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

            resultString.AppendLine("\"PaymentOnOffices\": [");
            for (int i = 0; i < offices.Count; i++)
            {
                resultString.Append(value.IncludedOfficesMetrics[offices[i]].ToString(System.Globalization.CultureInfo.GetCultureInfo("en-US")));
                if (i != offices.Count - 1)
                {
                    resultString.Append(",");
                }
                resultString.AppendLine();
            }
            resultString.AppendLine("],");

            resultString.AppendLine("\"IncludedEmployees\": [");
            var employees = value.IncludedEmployees.Keys.Select(e => e).ToList();
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

            resultString.AppendLine("\"PaymentOnEmployees\": [");
            for (int i = 0; i < employees.Count; i++)
            {
                resultString.Append(value.IncludedEmployees[employees[i]].ToString(System.Globalization.CultureInfo.GetCultureInfo("en-US")));
                if (i != employees.Count - 1)
                {
                    resultString.Append(",");
                }
                resultString.AppendLine();
            }
            resultString.AppendLine("],");

            resultString.AppendLine("\"TotalPayment\": " + value.TotalPayment.ToString(System.Globalization.CultureInfo.GetCultureInfo("en-US")));
            resultString.AppendLine("}");

            return resultString.ToString();
        }

        private static PaymentOnDateRangeReport Parse(string input)
        {
            PaymentOnDateRangeReport resultReport = new PaymentOnDateRangeReport();
            StringReader reader = new StringReader(input);

            reader.ReadLine();

            string currentLine = reader.ReadLine();
            currentLine = currentLine.Substring(14, 10);
            resultReport.StartDate = DateTime.Parse(currentLine);

            currentLine = reader.ReadLine();
            currentLine = currentLine.Substring(12, 10);
            resultReport.EndDate = DateTime.Parse(currentLine);

            List<Country> countryList = new List<Country>();

            reader.ReadLine();
            currentLine = reader.ReadLine();
            while (!currentLine.StartsWith(']'))
            {
                if (currentLine[currentLine.Length - 1] == ',')
                {
                    currentLine = currentLine.Substring(0, currentLine.Length - 1);
                }
                countryList.Add(JsonSerializer.Deserialize<Country>(currentLine));
                currentLine = reader.ReadLine();
            }

            List<float> totalOnCountries = new List<float>();
            reader.ReadLine();
            currentLine = reader.ReadLine();
            while (!currentLine.StartsWith(']'))
            {
                if (currentLine[currentLine.Length - 1] == ',')
                {
                    currentLine = currentLine.Substring(0, currentLine.Length - 1);
                }
                totalOnCountries.Add(float.Parse(currentLine, System.Globalization.CultureInfo.GetCultureInfo("en-US")));
                currentLine = reader.ReadLine();
            }

            resultReport.IncludedCountriesMetrics = new Dictionary<Country, float>();
            for (int i = 0; i < countryList.Count; i++)
            {
                resultReport.IncludedCountriesMetrics.Add(countryList[i], totalOnCountries[i]);
            }


            List<Office> officeList = new List<Office>();
            reader.ReadLine();
            currentLine = reader.ReadLine();
            while (!currentLine.StartsWith(']'))
            {
                if (currentLine[currentLine.Length - 1] == ',')
                {
                    currentLine = currentLine.Substring(0, currentLine.Length - 1);
                }
                officeList.Add(JsonSerializer.Deserialize<Office>(currentLine));
                currentLine = reader.ReadLine();
            }

            List<float> totalOnOffices = new List<float>();
            reader.ReadLine();
            currentLine = reader.ReadLine();
            while (!currentLine.StartsWith(']'))
            {
                if (currentLine[currentLine.Length - 1] == ',')
                {
                    currentLine = currentLine.Substring(0, currentLine.Length - 1);
                }
                totalOnOffices.Add(float.Parse(currentLine, System.Globalization.CultureInfo.GetCultureInfo("en-US")));
                currentLine = reader.ReadLine();
            }

            resultReport.IncludedOfficesMetrics = new Dictionary<Office, float>();
            for (int i = 0; i < officeList.Count; i++)
            {
                resultReport.IncludedOfficesMetrics.Add(officeList[i], totalOnOffices[i]);
            }

            List<Employee> employeeList = new List<Employee>();
            reader.ReadLine();
            currentLine = reader.ReadLine();
            while (!currentLine.StartsWith(']'))
            {
                if (currentLine[currentLine.Length - 1] == ',')
                {
                    currentLine = currentLine.Substring(0, currentLine.Length - 1);
                }
                employeeList.Add(JsonSerializer.Deserialize<Employee>(currentLine));
                currentLine = reader.ReadLine();
            }

            List<float> paymentOnEmployees = new List<float>();
            reader.ReadLine();
            currentLine = reader.ReadLine();
            while (!currentLine.StartsWith(']'))
            {
                if (currentLine[currentLine.Length - 1] == ',')
                {
                    currentLine = currentLine.Substring(0, currentLine.Length - 1);
                }
                paymentOnEmployees.Add(float.Parse(currentLine, System.Globalization.CultureInfo.GetCultureInfo("en-US")));
                currentLine = reader.ReadLine();
            }

            resultReport.IncludedEmployees = new Dictionary<Employee, float>();
            for (int i = 0; i < employeeList.Count; i++)
            {
                resultReport.IncludedEmployees.Add(employeeList[i], paymentOnEmployees[i]);
            }

            currentLine = reader.ReadLine();
            currentLine = currentLine.Substring(16);
            resultReport.TotalPayment = float.Parse(currentLine, System.Globalization.CultureInfo.GetCultureInfo("en-US"));

            return resultReport;
        }
    }
}
