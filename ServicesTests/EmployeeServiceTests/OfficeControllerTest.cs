﻿using EmployeeService.Controllers;
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
            _countryToTestId = new CountryController(new ApplicationDbContextFactory().CreateDbContext(args)).GetAll().Value.First().Id;
        }



        [Fact]
        public void GetAllTest()
        {
            var result = _controller.GetAll();
            Assert.NotNull(result.Value);
            Assert.Equal(3, result.Value.Count());
        }

        [Theory]
        [InlineData(3, "Development")]
        [InlineData(4, "Head")]
        public void GetByIdTestSuccess(int id, string name)
        {
            var result = _controller.Find(id).Value;
            Assert.NotNull(result);
            Assert.Equal(name, result.Name);
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
            Office testOffice = new Office() { Name = "QA", CountryId=_countryToTestId };
            var response = _controller.Add(testOffice);

            Assert.True(response is CreatedAtActionResult);
            Assert.True(_controller.GetAll().Value.Where(o => o.Name == testOffice.Name).Count() > 0);

            testOffice = _controller.GetAll().Value.Where(o => o.Name == testOffice.Name).First();
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
                Assert.True(_controller.GetAll().Value.Where(o => o.Name == office.Name).Count() > 0);

            }
            testOffices = _controller.GetAll().Value.Where(o => o.Name == "QA" || o.Name == "DevOps").ToList();
            _controller.DeleteRange(testOffices);

        }

        [Fact]
        public void UpdateTest()
        {
            Office testOffice = new Office() { Name = "QA", CountryId = _countryToTestId };
            var addedOfficeId = _controller.GetAll().Value.Where(o => o.Name == "QA").First().Id;
            testOffice.Name = "RQ";
            testOffice.Id = addedOfficeId;

            var response = _controller.Update(testOffice);


            Assert.True(response is OkResult);
            Assert.True(_controller.GetAll().Value.Where(c => c.Id == addedOfficeId).First().Name == "RQ");

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

            testOffices = _controller.GetAll().Value.Where(o => o.Name == "QA" || o.Name == "DevOps").ToList();
            testOffices[0].Name = "QB";
            testOffices[1].Name = "DEFOPS";

            var response = _controller.UpdateRange(testOffices);
            Assert.True(response is OkResult);
            foreach (var office in testOffices)
            {
                Assert.True(_controller.GetAll().Value.Where(o => o.Name == office.Name).Count() > 0);

            }
            _controller.DeleteRange(testOffices);

        }

        [Fact]
        public void DeleteTest()
        {
            Office testOffice = new Office() { Name = "QA", CountryId = _countryToTestId };
            var response = _controller.Add(testOffice);
            testOffice = _controller.GetAll().Value.Where(o => o.Name == testOffice.Name).First();
            _controller.Delete(testOffice);
            Assert.True(response is OkResult);
            Assert.True(_controller.GetAll().Value.Where(o => o.Name == testOffice.Name).Count() == 0);
        }

        [Fact]
        public void DeleteRangeTest()
        {
            List<Office> testOffices = new List<Office>()
                {
                    new Office() { Name = "QA", CountryId= _countryToTestId },
                    new Office() { Name = "DevOps", CountryId= _countryToTestId}
                };
            var response = _controller.AddRange(testOffices);
            testOffices = _controller.GetAll().Value.Where(o => o.Name == "QA" || o.Name == "DevOps").ToList();
            _controller.DeleteRange(testOffices);
            Assert.True(response is OkResult);
            foreach (var office in testOffices)
            {
                Assert.True(_controller.GetAll().Value.Where(o => o.Name == office.Name).Count() == 0);
            }
        }

        [Fact]
        public void DeleteByIdTest()
        {
            Office testOffice = new Office() { Name = "QA", CountryId = _countryToTestId };
            var response = _controller.Add(testOffice);
            testOffice = _controller.GetAll().Value.Where(o => o.Name == testOffice.Name).First();
            _controller.Delete(testOffice.Id);
            Assert.True(response is OkResult);
            Assert.True(_controller.GetAll().Value.Where(o => o.Name == testOffice.Name).Count() == 0);

        }
    }
}