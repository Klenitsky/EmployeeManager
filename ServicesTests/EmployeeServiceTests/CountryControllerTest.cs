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
    public  class CountryControllerTest
    {
            private CountryController _controller;

            public CountryControllerTest()
            {
                string[] args = { };
                _controller = new CountryController(new ApplicationDbContextFactory().CreateDbContext(args));
            }


            [Fact]
            public void GetAllTest()
            {
                var result = _controller.GetAll().Value;
                Assert.NotNull(result);
                Assert.Equal(3, result.Count());
            }

            [Theory]
            [InlineData(4, "USA")]
            [InlineData(5, "GBR")]
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
                Country testCountry = new Country() { Name = "Republic of Poland", Abbreviation = "PLN", Currency= new Currency() { Name = "EURO", Abbreviation = "EUR", Code = 444 } };
                var response = _controller.Add(testCountry);

                Assert.True(response is CreatedAtActionResult);
                Assert.True(_controller.GetAll().Value.Where(c => c.Abbreviation == testCountry.Abbreviation).Count() > 0);

                testCountry = _controller.GetAll().Value.Where(c => c.Abbreviation == testCountry.Abbreviation).First();
                _controller.Delete(testCountry);
            }

            [Fact]
            public void AddRangeTest()
            {
                List<Country> testCountries = new List<Country>()
                {
                    new Country() { Name = "Republic of Poland", Abbreviation = "PLN", Currency= new Currency() { Name = "EURO", Abbreviation = "EUR", Code = 444 } },
                    new Country() { Name = "Republic of France", Abbreviation = "FRC", Currency= new Currency() { Name = "EURO", Abbreviation = "EUR", Code = 444 } },
                };
                var response = _controller.AddRange(testCountries);
                Assert.True(response is CreatedAtActionResult);
                foreach (var country in testCountries)
                {
                    Assert.True(_controller.GetAll().Value.Where(c => c.Abbreviation == country.Abbreviation).Count() > 0);

                }
                testCountries = _controller.GetAll().Value.Where(c => c.Abbreviation == "PLN" || c.Abbreviation == "FRC").ToList();
                _controller.DeleteRange(testCountries);

            }

            [Fact]
            public void UpdateTest()
            {
                Country testCountry = new Country() { Name = "Republic of Poland", Abbreviation = "PLN", Currency = new Currency() { Name = "EURO", Abbreviation = "EUR", Code = 444 } };
                _controller.Add(testCountry);
                var addedCountryId = _controller.GetAll().Value.Where(c => c.Abbreviation == "PLN").First().Id;
                testCountry.Abbreviation = "RRB";
                testCountry.Id = addedCountryId;

                var response = _controller.Update(testCountry);


                Assert.True(response is OkResult);
                Assert.True(_controller.GetAll().Value.Where(c => c.Id == addedCountryId).First().Abbreviation == "RRB");

                _controller.Delete(testCountry);
            }

            [Fact]
            public void UpdateRangeTest()
            {
                List<Country> testCountries = new List<Country>()
                {
                    new Country() { Name = "Republic of Poland", Abbreviation = "PLN", Currency= new Currency() { Name = "EURO", Abbreviation = "EUR", Code = 444 } },
                    new Country() { Name = "Republic of France", Abbreviation = "FRC", Currency= new Currency() { Name = "EURO", Abbreviation = "EUR", Code = 444 } },
                };
                _controller.AddRange(testCountries);

                testCountries = _controller.GetAll().Value.Where(c => c.Abbreviation == "PLN" || c.Abbreviation == "FRC").ToList();
                testCountries[0].Abbreviation = "ABC";
                testCountries[1].Abbreviation = "DEF";

                var response = _controller.UpdateRange(testCountries);
                Assert.True(response is OkResult);
                foreach (var currency in testCountries)
                {
                    Assert.True(_controller.GetAll().Value.Where(c => c.Abbreviation == currency.Abbreviation).Count() > 0);

                }
                _controller.DeleteRange(testCountries);

            }

            [Fact]
            public void DeleteTest()
            {
                Country testCountry = new Country() { Name = "Republic of Poland", Abbreviation = "PLN", Currency = new Currency() { Name = "EURO", Abbreviation = "EUR", Code = 444 } };
                var response = _controller.Add(testCountry);
                testCountry = _controller.GetAll().Value.Where(c => c.Abbreviation == testCountry.Abbreviation).First();
                _controller.Delete(testCountry);
                Assert.True(response is OkResult);
                Assert.True(_controller.GetAll().Value.Where(c => c.Abbreviation == testCountry.Abbreviation).Count() == 0);
            }

            [Fact]
            public void DeleteRangeTest()
            {
                List<Country> testCountries = new List<Country>()
                {
                    new Country() { Name = "Republic of Poland", Abbreviation = "PLN", Currency= new Currency() { Name = "EURO", Abbreviation = "EUR", Code = 444 } },
                    new Country() { Name = "Republic of France", Abbreviation = "FRC", Currency= new Currency() { Name = "EURO", Abbreviation = "EUR", Code = 444 } },
                };
                var response = _controller.AddRange(testCountries);
                testCountries = _controller.GetAll().Value.Where(c => c.Abbreviation == "PLN" || c.Abbreviation == "FRC").ToList();
                _controller.DeleteRange(testCountries);
                Assert.True(response is OkResult);
                foreach (var currency in testCountries)
                {
                    Assert.True(_controller.GetAll().Value.Where(c => c.Abbreviation == currency.Abbreviation).Count() == 0);
                }
            }

            [Fact]
            public void DeleteByIdTest()
            {
            Country testCountry = new Country() { Name = "Republic of Poland", Abbreviation = "PLN", Currency = new Currency() { Name = "EURO", Abbreviation = "EUR", Code = 444 } };
            var response = _controller.Add(testCountry);
                testCountry = _controller.GetAll().Value.Where(c => c.Abbreviation == testCountry.Abbreviation).First();
                _controller.Delete(testCountry.Id);
                Assert.True(response is OkResult);
                Assert.True(_controller.GetAll().Value.Where(c => c.Abbreviation == testCountry.Abbreviation).Count() == 0);

            }
        }
}
