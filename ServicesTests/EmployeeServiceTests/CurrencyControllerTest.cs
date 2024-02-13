using EmployeeService.Controllers;
using EmployeeService.DAL.Contexts;
using EmployeeService.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesTests.EmployeeServiceTests
{
    public class CurrencyControllerTest
    {
        private CurrencyController _controller;

        public CurrencyControllerTest()
        {
            string[] args = { };
            _controller = new CurrencyController(new ApplicationDbContextFactory().CreateDbContext(args));
        }


        [Fact]
        public void GetAllTest()
        {
            var result = _controller.GetAll().Value;
            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
        }

        [Theory]
        [InlineData(4, "USD")]
        [InlineData(5, "GBP")]
        public void GetByIdTestSuccess(int id, string abbreviation)
        {
            var result = _controller.Find(id).Value;
            Assert.NotNull(result);
            Assert.Equal(abbreviation, result.Abbreviation);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(1500)]
        public void GetByIdTestInvalidId(int id)
        {
            var result = _controller.Find(id).Value;
            Assert.Null(result);
        }

        [Fact]
        public void AddTest()
        {
            Currency testCurrency = new Currency() { Name = "Russian Ruble", Abbreviation = "RUB", Code = 871 };
            var response = _controller.Add(testCurrency);

            Assert.True(response is CreatedAtActionResult);
            Assert.True(_controller.GetAll().Value.Where(c => c.Abbreviation == testCurrency.Abbreviation).Count() > 0);

            testCurrency = _controller.GetAll().Value.Where(c => c.Abbreviation == testCurrency.Abbreviation).First();
            _controller.Delete(testCurrency);
        }

        [Fact]
        public void AddRangeTest()
        {
            List<Currency> testCurrencies = new List<Currency>()
            {
                new Currency() { Name = "Russian Ruble", Abbreviation = "RUB", Code = 871 },
                new Currency() { Name = "EURO", Abbreviation = "EUR", Code = 444 }
            };
            var response = _controller.AddRange(testCurrencies) as CreatedAtActionResult;
            Assert.True(response is CreatedAtActionResult);
            foreach (var currency in testCurrencies)
            {
                Assert.True(_controller.GetAll().Value.Where(c => c.Abbreviation == currency.Abbreviation).Count() > 0);

            }
            testCurrencies = _controller.GetAll().Value.Where(c => c.Abbreviation == "RUB" || c.Abbreviation == "EUR").ToList();
            _controller.DeleteRange(testCurrencies);

        }

        [Fact]
        public void UpdateTest()
        {
            Currency testCurrency = new Currency() { Name = "Russian Ruble", Abbreviation = "RUB", Code = 871 };
            _controller.Add(testCurrency);
            var addedCurrencyId = _controller.GetAll().Value.Where(c => c.Abbreviation == "RUB").First().Id;
            testCurrency.Abbreviation = "RRB";
            testCurrency.Id = addedCurrencyId;

            var response = _controller.Update(testCurrency);


            Assert.True(response is OkResult);
            Assert.True(_controller.GetAll().Value.Where(c => c.Id == addedCurrencyId).First().Abbreviation == "RRB");

            _controller.Delete(testCurrency);
        }

        [Fact]
        public void UpdateRangeTest()
        {
            List<Currency> testCurrencies = new List<Currency>()
            {
                new Currency() { Name = "Russian Ruble", Abbreviation = "RUB", Code = 871 },
                new Currency() { Name = "EURO", Abbreviation = "EUR", Code = 444 }
            };
            _controller.AddRange(testCurrencies);

            testCurrencies = _controller.GetAll().Value.Where(c => c.Abbreviation == "RUB" || c.Abbreviation == "EUR").ToList();
            testCurrencies[0].Abbreviation = "ABC";
            testCurrencies[1].Abbreviation = "DEF";

            var response = _controller.UpdateRange(testCurrencies);
            Assert.True(response is OkResult);
            foreach (var currency in testCurrencies)
            {
                Assert.True(_controller.GetAll().Value.Where(c => c.Abbreviation == currency.Abbreviation).Count() > 0);

            }
            _controller.DeleteRange(testCurrencies);

        }

        [Fact]
        public void DeleteTest()
        {
            Currency testCurrency = new Currency() { Name = "Russian Ruble", Abbreviation = "RUB", Code = 871 };
            var response = _controller.Add(testCurrency);
            testCurrency = _controller.GetAll().Value.Where(c => c.Abbreviation == testCurrency.Abbreviation).First();
            _controller.Delete(testCurrency);
            Assert.True(response is OkResult);
            Assert.True(_controller.GetAll().Value.Where(c => c.Abbreviation == testCurrency.Abbreviation).Count() == 0);

        }

        [Fact]
        public void DeleteRangeTest()
        {
            List<Currency> testCurrencies = new List<Currency>()
            {
                new Currency() { Name = "Russian Ruble", Abbreviation = "RUB", Code = 871 },
                new Currency() { Name = "EURO", Abbreviation = "EUR", Code = 444 }
            };
            var response = _controller.AddRange(testCurrencies);
            testCurrencies = _controller.GetAll().Value.Where(c => c.Abbreviation == "RUB" || c.Abbreviation == "EUR").ToList();
            _controller.DeleteRange(testCurrencies);
            Assert.True(response is OkResult);
            foreach (var currency in testCurrencies)
            {
                Assert.True(_controller.GetAll().Value.Where(c => c.Abbreviation == currency.Abbreviation).Count() == 0);
            }
        }

        [Fact]
        public void DeleteByIdTest()
        {
            Currency testCurrency = new Currency() { Name = "Russian Ruble", Abbreviation = "RUB", Code = 871 };
            var response = _controller.Add(testCurrency);
            testCurrency = _controller.GetAll().Value.Where(c => c.Abbreviation == testCurrency.Abbreviation).First();
            _controller.Delete(testCurrency.Id);
            Assert.True(response is OkResult);
            Assert.True(_controller.GetAll().Value.Where(c => c.Abbreviation == testCurrency.Abbreviation).Count() == 0);

        }
    }
}
