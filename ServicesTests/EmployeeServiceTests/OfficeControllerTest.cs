using Azure;
using EmployeeService.Controllers;
using EmployeeService.DAL.Contexts;
using EmployeeService.DAL.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesTests.EmployeeServiceTests
{
    public class OfficeControllerTest
    {
        private OfficeController _controller;
        private int _countryToTestId;

        public OfficeControllerTest()
        {
            string[] args = { };
            _controller = new OfficeController(new ApplicationDbContextFactory().CreateDbContext(args));
            _countryToTestId = ((new CountryController(new ApplicationDbContextFactory().CreateDbContext(args)).GetAll() as OkObjectResult).Value as IEnumerable<Country>).First().Id;
        }



        [Fact]
        public void GetAllTest()
        {
            var resultAction = _controller.GetAll();
            var result = resultAction as OkObjectResult;
            var offices = result.Value as IEnumerable<Office>;
            Assert.True(resultAction is OkObjectResult);
            Assert.NotNull(result.Value);
            Assert.NotNull(offices);
        }

        [Theory]
        [InlineData(4, "Development Department")]
        [InlineData(6, "Head Department")]
        public void GetByIdTestSuccess(int id, string name)
        {
            var resultAction = _controller.Find(id);
            var result = resultAction as OkObjectResult;
            var office = result.Value as Office;
            Assert.True(resultAction is OkObjectResult);
            Assert.NotNull(result.Value);
            Assert.Equal(name, office.Name);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(1500)]
        public void GetByIdTestInvalidId(int id)
        {
            var resultAction = _controller.Find(id);
            Assert.False(resultAction is OkObjectResult);
        }

        [Fact]
        public void GetByCountryTestSuccess()
        {          
            var resultAction = _controller.FindByCountry(_countryToTestId);
            var result = resultAction as OkObjectResult;
            var offices = result.Value as IEnumerable<Office>;
            Assert.True(resultAction is OkObjectResult);
            Assert.NotNull(result.Value);
            Assert.NotNull(offices);
            foreach(var office in offices)
            {
                Assert.Equal(_countryToTestId, office.CountryId);
            }
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(1500)]
        public void GetByCountryTestInvalidId(int id)
        {
            var resultAction = _controller.FindByCountry(id);
            Assert.False(resultAction is OkObjectResult);
        }

        [Fact]
        public void AddTest()
        {
            Office testOffice = new Office() { Name = "QA", CountryId=_countryToTestId };
            var response = _controller.Add(testOffice);

            Assert.True(response is CreatedAtActionResult);
            Assert.True(((_controller.GetAll() as OkObjectResult).Value as IEnumerable<Office>).Where(o => o.Name == testOffice.Name).Count() > 0);

            testOffice = ((_controller.GetAll() as OkObjectResult).Value as IEnumerable<Office>).Where(o => o.Name == testOffice.Name).First();
            _controller.Delete(testOffice);
        }

        [Fact]
        public void AddRangeTest()
        {
            List<Office> testOffices = new List<Office>()
                {
                    new Office() { Name = "QA", CountryId= _countryToTestId },
                    new Office() { Name = "DevOps", CountryId= _countryToTestId}
                };
            var response = _controller.AddRange(testOffices);
            Assert.True(response is CreatedAtActionResult);
            foreach (var office in testOffices)
            {
                Assert.True(((_controller.GetAll() as OkObjectResult).Value as IEnumerable<Office>).Where(o => o.Name == office.Name).Count() > 0);
            }
            testOffices = ((_controller.GetAll() as OkObjectResult).Value as IEnumerable<Office>).Where(o => o.Name == "QA" || o.Name == "DevOps").ToList();
            _controller.DeleteRange(testOffices);

        }

        [Fact]
        public void UpdateTest()
        {
            Office testOffice = new Office() { Name = "QA", CountryId = _countryToTestId };
            _controller.Add(testOffice);
            var addedOfficeId = ((_controller.GetAll() as OkObjectResult).Value as IEnumerable<Office>).Where(o => o.Name == "QA").First().Id;
            testOffice.Name = "RQ";
            testOffice.Id = addedOfficeId;

            var response = _controller.Update(testOffice);


            Assert.True(response is OkResult);
            Assert.True(((_controller.GetAll() as OkObjectResult).Value as IEnumerable<Office>).Where(c => c.Id == addedOfficeId).First().Name == "RQ");

            _controller.Delete(testOffice);
        }

        [Fact]
        public void UpdateRangeTest()
        {
            List<Office> testOffices = new List<Office>()
                {
                    new Office() { Name = "QA", CountryId= _countryToTestId },
                    new Office() { Name = "DevOps", CountryId= _countryToTestId}
                };
            _controller.AddRange(testOffices);

            testOffices = ((_controller.GetAll() as OkObjectResult).Value as IEnumerable<Office>).Where(o => o.Name == "QA" || o.Name == "DevOps").ToList();
            testOffices[0].Name = "QB";
            testOffices[1].Name = "DEFOPS";

            var response = _controller.UpdateRange(testOffices);
            Assert.True(response is OkResult);
            foreach (var office in testOffices)
            {
                Assert.True(((_controller.GetAll() as OkObjectResult).Value as IEnumerable<Office>).Where(o => o.Name == office.Name).Count() > 0);

            }
            _controller.DeleteRange(testOffices);

        }

        [Fact]
        public void DeleteTest()
        {
            Office testOffice = new Office() { Name = "QA", CountryId = _countryToTestId };
            var responseOnAdd = _controller.Add(testOffice);
            testOffice = ((_controller.GetAll() as OkObjectResult).Value as IEnumerable<Office>).Where(o => o.Name == testOffice.Name).First();
            var response = _controller.Delete(testOffice);
            Assert.True(responseOnAdd is CreatedAtActionResult);
            Assert.True(response is OkResult);
            Assert.True(((_controller.GetAll() as OkObjectResult).Value as IEnumerable<Office>).Where(o => o.Name == testOffice.Name).Count() == 0);
        }

        [Fact]
        public void DeleteRangeTest()
        {
            List<Office> testOffices = new List<Office>()
                {
                    new Office() { Name = "QA", CountryId= _countryToTestId },
                    new Office() { Name = "DevOps", CountryId= _countryToTestId}
                };
            var responseOnAdd = _controller.AddRange(testOffices);
            testOffices = ((_controller.GetAll() as OkObjectResult).Value as IEnumerable<Office>).Where(o => o.Name == "QA" || o.Name == "DevOps").ToList();
            var response = _controller.DeleteRange(testOffices);
            Assert.True(responseOnAdd is CreatedAtActionResult);
            Assert.True(response is OkResult);
            foreach (var office in testOffices)
            {
                Assert.True(((_controller.GetAll() as OkObjectResult).Value as IEnumerable<Office>).Where(o => o.Name == office.Name).Count() == 0);
            }
        }

        [Fact]
        public void DeleteByIdTestSuccess()
        {
            Office testOffice = new Office() { Name = "QA", CountryId = _countryToTestId };
            var responseOnAdd = _controller.Add(testOffice);
            testOffice = ((_controller.GetAll() as OkObjectResult).Value as IEnumerable<Office>).Where(o => o.Name == testOffice.Name).First();
            var response = _controller.Delete(testOffice.Id);
            Assert.True(responseOnAdd is CreatedAtActionResult);
            Assert.True(response is OkResult);
            Assert.True(((_controller.GetAll() as OkObjectResult).Value as IEnumerable<Office>).Where(o => o.Name == testOffice.Name).Count() == 0);

        }


        [Theory]
        [InlineData(-1)]
        [InlineData(1500)]
        public void DeleteByIdTestInvalidId(int id)
        {
            var resultAction = _controller.Delete(id);
            Assert.False(resultAction is OkResult);
        }
    }
}
