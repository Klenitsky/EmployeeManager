using ExchangeRateService.DAL.BasicStructures.RateReaders;
using ExchangeRateService.DAL.NbrbAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace ExchangeRateService.DAL.NbrbAPI.RateReaders
{
    public class RateReader : IRateReader<Rate>
    {
        public Rate ReadRate(DateTime date, string currencyCode)
        {
            string requestString = "https://api.nbrb.by/exrates/rates/" + currencyCode.ToUpper() + "?parammode=2&ondate=" + date.ToString("yyyy-MM-dd");

            HttpClient httpClient = new HttpClient();
            if (currencyCode.ToUpper() != "BYN")
            {
                Rate rate = httpClient.GetFromJsonAsync<Rate>(requestString).Result;
                requestString = "https://api.nbrb.by/exrates/rates/USD?parammode=2&ondate=" + date.ToString("yyyy-MM-dd");
                Rate rateUSD = httpClient.GetFromJsonAsync<Rate>(requestString).Result;

                rate.Cur_OfficialRate = rate.Cur_OfficialRate / rateUSD.Cur_OfficialRate;
                return rate;
            }
            else
            {
                requestString = "https://api.nbrb.by/exrates/rates/USD?parammode=2&ondate=" + date.ToString("yyyy-MM-dd");
                Rate rateUSD = httpClient.GetFromJsonAsync<Rate>(requestString).Result;
                rateUSD.Cur_OfficialRate = 1/rateUSD.Cur_OfficialRate;
                return rateUSD;
            }
        }
    }
}
