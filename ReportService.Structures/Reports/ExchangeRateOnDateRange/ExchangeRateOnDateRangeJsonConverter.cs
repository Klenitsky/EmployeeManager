using EmployeeService.DAL.Models;
using ExchangeRateService.DAL.BasicStructures.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ReportService.Structures.Reports.ExchangeRateOnDateRange
{
    public class ExchangeRateOnDateRangeJsonConverter : JsonConverter<ExchangeRateOnDateRangeReport>
    {
        public override ExchangeRateOnDateRangeReport? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return Parse(reader.GetString()!);
        }

        public override void Write(Utf8JsonWriter writer, ExchangeRateOnDateRangeReport value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(GetJsonString(value));
        }

        private  string GetJsonString(ExchangeRateOnDateRangeReport value)
        {
            StringBuilder resultString = new StringBuilder();
            resultString.AppendLine("{");
            resultString.AppendLine("\"StartDate\": \"" + value.StartDate.ToString("yyyy-MM-dd") + "\",");
            resultString.AppendLine("\"EndDate\": \"" + value.EndDate.ToString("yyyy-MM-dd") + "\",");
            resultString.AppendLine("\"Currencies\": [");
            var currencies = value.IncludedCurrencies.Keys.Select(k => k).ToList();
            for (int i = 0; i < currencies.Count; i++)
            {
                resultString.Append(JsonSerializer.Serialize(currencies[i]));
                if (i != currencies.Count - 1)
                {
                    resultString.Append(",");
                }
                resultString.AppendLine();
            }
            resultString.AppendLine("],");

            resultString.AppendLine("\"AverageRates\": [");
            for (int i = 0; i < currencies.Count; i++)
            {
                resultString.Append(value.IncludedCurrencies[currencies[i]]);
                if (i != currencies.Count - 1)
                {
                    resultString.Append(",");
                }
                resultString.AppendLine();
            }
            resultString.AppendLine("],");

            resultString.AppendLine("\"ExchangeRates\": [");
            for (int i = 0; i < value.ExchangeRates.Count; i++)
            {
                resultString.Append(JsonSerializer.Serialize(value.ExchangeRates[i]));
                if (i != value.ExchangeRates.Count - 1)
                {
                    resultString.Append(",");
                }
                resultString.AppendLine();
            }

            resultString.AppendLine("]");
            resultString.AppendLine("}");

            return resultString.ToString();
        }

        private static ExchangeRateOnDateRangeReport Parse(string input)
        {
            ExchangeRateOnDateRangeReport resultReport = new ExchangeRateOnDateRangeReport();
            StringReader reader = new StringReader(input);

            reader.ReadLine();

            string currentLine = reader.ReadLine();
            currentLine = currentLine.Substring(14, 10);
            resultReport.StartDate = DateTime.Parse(currentLine);

            currentLine = reader.ReadLine();
            currentLine = currentLine.Substring(12, 10);
            resultReport.EndDate = DateTime.Parse(currentLine);

            List<Currency> currencyList = new List<Currency>();

            currentLine = reader.ReadLine();
            while (currentLine != "],")
            {
                currentLine = reader.ReadLine();
                if (currentLine[currentLine.Length - 1] == ',')
                {
                    currentLine = currentLine.Substring(0, currentLine.Length - 1);
                }
                currencyList.Add(JsonSerializer.Deserialize<Currency>(currentLine));
            }

            List<float> averageRates = new List<float>();
            currentLine = reader.ReadLine();
            while (currentLine != "],")
            {
                currentLine = reader.ReadLine();
                if (currentLine[currentLine.Length - 1] == ',')
                {
                    currentLine = currentLine.Substring(0, currentLine.Length - 1);
                }
                averageRates.Add(float.Parse(currentLine));
            }

            List<ExchangeRate> exchangeRates = new List<ExchangeRate>();
            currentLine = reader.ReadLine();
            while (currentLine != "]")
            {
                currentLine = reader.ReadLine();
                if (currentLine[currentLine.Length - 1] == ',')
                {
                    currentLine = currentLine.Substring(0, currentLine.Length - 1);
                }
                exchangeRates.Add(JsonSerializer.Deserialize<ExchangeRate>(currentLine));
            }

            resultReport.ExchangeRates = exchangeRates;
            for (int i = 0; i < currencyList.Count; i++)
            {
                resultReport.IncludedCurrencies.Add(currencyList[i], averageRates[i]);
            }

            return resultReport;

        }
    }
}
