using ExchangeRateService.Controllers;
using ExchangeRateService.DAL.NbrbAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesTests.ExchangeRateServiceTests
{
    public class ExchangeRateServiceOnRequestTests
    {
        private ExchangeRateOnRequestController _controller;

        public ExchangeRateServiceOnRequestTests()
        {
            _controller = new ExchangeRateOnRequestController(new NbrbExchangeRateRepo());
        }

        
        [Theory]
        [InlineData("01.05.2023")]
        [InlineData("03.01.2024")]
        public void OnDateTestSuccess(string dateString)
        {
            DateTime date = DateTime.Parse(dateString);
            var result = _controller.GetByDate(date).Value;
            Assert.NotNull(result);
            foreach (var rate in result)
            {
                Assert.NotNull(rate);
                Assert.Equal(date, rate.Date);
            }
        }


        [Theory]
        [InlineData("01.11.2000")]
        [InlineData("03.02.2025")]
        public void OnDateTestBadDate(string dateString)
        {
            DateTime date = DateTime.Parse(dateString);
            var result = _controller.GetByDate(date).Value;
            Assert.Null(result);
        }

        [Theory]
        [InlineData("01.05.2023", "05.05.2023")]
        [InlineData("03.01.2024", "05.01.2024")]
        public void OnDateRangeTestSuccess(string startDateString, string endDateString)
        {
            DateTime startDate = DateTime.Parse(startDateString);
            DateTime endDate = DateTime.Parse(endDateString);
            var result = _controller.GetByDateRange(startDate, endDate).Value;
            Assert.NotNull(result);
            foreach (var rate in result)
            {
                Assert.NotNull(rate);
                Assert.True(rate.Date >= startDate);
                Assert.True(rate.Date <= endDate);
            }
        }

        [Theory]
        [InlineData("01.11.2015", "05.11.2015")]
        [InlineData("03.02.2025", "05.02.2025")]
        [InlineData("03.02.2024", "01.02.2024")]
        public void OnDateRangeTestBadRange(string startDateString, string endDateString)
        {
            DateTime startDate = DateTime.Parse(startDateString);
            DateTime endDate = DateTime.Parse(endDateString);
            var result = _controller.GetByDateRange(startDate, endDate).Value;
            Assert.Null(result);
        }


        [Theory]
        [InlineData("01.04.2023", "USD")]
        [InlineData("03.01.2024", "GBP")]
        public void OnDateAndCurrencyTestSuccess(string dateString, string abbreviation)
        {
            var date = DateTime.Parse(dateString);
            var result = _controller.GetByDateAndCurrency(date, abbreviation).Value;
            Assert.NotNull(result);
            Assert.Equal(date, result.Date);
            Assert.Equal(abbreviation, result.Abbreviation);
        }



        [Theory]
        [InlineData("01.11.2000", "USD")]
        [InlineData("03.02.2025", "GBP")]
        public void OnDateAndCurrencyTestBadDate(string dateString, string abbreviation)
        {
            var date = DateTime.Parse(dateString);
            var result = _controller.GetByDateAndCurrency(date, abbreviation).Value;
            Assert.Null(result);
        }


        [Theory]
        [InlineData("01.11.2023", "SAdsa")]
        [InlineData("03.02.2024", "dss")]
        public void OnDateAndCurrencyTestBadCurrency(string dateString, string abbreviation)
        {
            var date = DateTime.Parse(dateString);
            var result = _controller.GetByDateAndCurrency(date, abbreviation).Value;
            Assert.Null(result);
        }


        [Theory]
        [InlineData("01.04.2023", "05.04.2023", "USD")]
        [InlineData("03.01.2024", "05.01.2024", "GBP")]
        public void OnDateRangeAndCurrencyTestSuccess(string startDateString, string endDateString, string abbreviation)
        {
            DateTime startDate = DateTime.Parse(startDateString);
            DateTime endDate = DateTime.Parse(endDateString);
            var result = _controller.GetByDateRangeAndCurrency(startDate, endDate, abbreviation).Value;
            Assert.NotNull(result);
            foreach (var rate in result)
            {
                Assert.NotNull(rate);
                Assert.True(rate.Date >= startDate);
                Assert.True(rate.Date <= endDate);
                Assert.True(rate.Abbreviation == abbreviation);
            }
        }

        [Theory]
        [InlineData("01.11.2015", "05.11.2015", "USD")]
        [InlineData("03.02.2025", "05.02.2025", "USD")]
        [InlineData("03.02.2024", "01.02.2024", "USD")]
        public void OnDateRangeAndCurrencyTestBadRange(string startDateString, string endDateString, string abbreviation)
        {
            DateTime startDate = DateTime.Parse(startDateString);
            DateTime endDate = DateTime.Parse(endDateString);
            var result = _controller.GetByDateRangeAndCurrency(startDate, endDate, abbreviation).Value;
            Assert.Null(result);
        }

        [Theory]
        [InlineData("01.11.2023", "05.11.2023", "sss")]
        [InlineData("03.02.2024", "05.02.2024", "qqq")]
        public void OnDateRangeAndCurrencyTestBadCurrency(string startDateString, string endDateString, string abbreviation)
        {
            DateTime startDate = DateTime.Parse(startDateString);
            DateTime endDate = DateTime.Parse(endDateString);
            var result = _controller.GetByDateRangeAndCurrency(startDate, endDate, abbreviation).Value;
            Assert.Null(result);
        }


    }
}
