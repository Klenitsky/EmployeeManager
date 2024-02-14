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
    public class EmployeeControllerTest
    {
        private EmployeeController _controller;
        private int _officeToTestId;
        private int _currencyToTestId;

        public EmployeeControllerTest()
        {
            string[] args = { };
            _controller = new EmployeeController(new ApplicationDbContextFactory().CreateDbContext(args));
            _officeToTestId = new OfficeController(new ApplicationDbContextFactory().CreateDbContext(args)).GetAll().Value.First().Id;
            _currencyToTestId = new CurrencyController(new ApplicationDbContextFactory().CreateDbContext(args)).GetAll().Value.First().Id;
        }



        [Fact]
        public void GetAllTest()
        {
            var result = _controller.GetAll();
            Assert.NotNull(result.Value);
            Assert.Equal(8, result.Value.Count());
        }

        [Theory]
        [InlineData(3, "Anna")]
        [InlineData(4, "Ivan")]
        public void GetByIdTestSuccess(int id, string name)
        {
            var result = _controller.Find(id).Value;
            Assert.NotNull(result);
            Assert.Equal(name, result.FirstName);
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
            Employee testEmployee = new Employee { FirstName = "Asdfgh", LastName = "Zxc", EmploymentDate =new DateTime(2022,01,13), DismissalDate=null, CurrencyId= _currencyToTestId, OfficeId =_officeToTestId, Email= "aaa@gmail.com", Salary =777 };
            var response = _controller.Add(testEmployee);

            Assert.True(response is CreatedAtActionResult);
            Assert.True(_controller.GetAll().Value.Where(e => e.FirstName == testEmployee.FirstName).Count() > 0);

            testEmployee = _controller.GetAll().Value.Where(o => e.FirstName == testEmployee.FirstName).First();
            _controller.Delete(testEmployee);
        }

        [Fact]
        public void AddRangeTest()
        {
            List<Employee> testEmployees = new List<Employee>()
                {
                    new Employee { FirstName = "Asdfgh", LastName = "Zxc", EmploymentDate =new DateTime(2022,01,13), DismissalDate=null, CurrencyId= _currencyToTestId, OfficeId =_officeToTestId, Email= "aaa@gmail.com", Salary =777 },
                    new Employee { FirstName = "Qwett", LastName = "Zxc", EmploymentDate = new DateTime(2021, 01, 13), DismissalDate = null, CurrencyId = _currencyToTestId, OfficeId = _officeToTestId, Email = "bbb@gmail.com", Salary = 888 }
                };
            var response = _controller.AddRange(testEmployees);
            Assert.True(response is CreatedAtActionResult);
            foreach (var employee in testEmployees)
            {
                Assert.True(_controller.GetAll().Value.Where(e => e.FirstName == employee.FirstName).Count() > 0);

            }
            testEmployees = _controller.GetAll().Value.Where(e => e.FirstName == "Asdfgh" || e.FirstName == "Qwett").ToList();
            _controller.DeleteRange(testEmployees);

        }

        [Fact]
        public void UpdateTest()
        {
            Employee testEmployee = new Employee { FirstName = "Asdfgh", LastName = "Zxc", EmploymentDate = new DateTime(2022, 01, 13), DismissalDate = null, CurrencyId = _currencyToTestId, OfficeId = _officeToTestId, Email = "aaa@gmail.com", Salary = 777 };
            var addedEmployeeId = _controller.GetAll().Value.Where(e => e.FirstName == "Asdfgh").First().Id;
            testEmployee.FirstName = "RQ";
            testEmployee.Id = addedEmployeeId;

            var response = _controller.Update(testEmployee);


            Assert.True(response is OkResult);
            Assert.True(_controller.GetAll().Value.Where(e => e.Id == addedEmployeeId).First().FirstName == "RQ");

            _controller.Delete(testEmployee);
        }

        [Fact]
        public void UpdateRangeTest()
        {
            List<Employee> testEmployees = new List<Employee>()
                {
                    new Employee { FirstName = "Asdfgh", LastName = "Zxc", EmploymentDate =new DateTime(2022,01,13), DismissalDate=null, CurrencyId= _currencyToTestId, OfficeId =_officeToTestId, Email= "aaa@gmail.com", Salary =777 },
                    new Employee { FirstName = "Qwett", LastName = "Zxc", EmploymentDate = new DateTime(2021, 01, 13), DismissalDate = null, CurrencyId = _currencyToTestId, OfficeId = _officeToTestId, Email = "bbb@gmail.com", Salary = 888 }
                };
            _controller.AddRange(testEmployees);

            testEmployees = _controller.GetAll().Value.Where(o => o.FirstName == "Asdfgh" || o.FirstName == "Qwett").ToList();
            testEmployees[0].FirstName = "QB";
            testEmployees[1].FirstName = "DEFOPS";

            var response = _controller.UpdateRange(testEmployees);
            Assert.True(response is OkResult);
            foreach (var employee in testEmployees)
            {
                Assert.True(_controller.GetAll().Value.Where(e=> e.FirstName == employee.FirstName).Count() > 0);

            }
            _controller.DeleteRange(testEmployees);

        }

        [Fact]
        public void DeleteTest()
        {
            Employee testEmployee = new Employee { FirstName = "Asdfgh", LastName = "Zxc", EmploymentDate = new DateTime(2022, 01, 13), DismissalDate = null, CurrencyId = _currencyToTestId, OfficeId = _officeToTestId, Email = "aaa@gmail.com", Salary = 777 };
            var response = _controller.Add(testEmployee);
            testEmployee = _controller.GetAll().Value.Where(e => e.FirstName == testEmployee.FirstName).First();
            _controller.Delete(testEmployee);
            Assert.True(response is OkResult);
            Assert.True(_controller.GetAll().Value.Where(e => e.FirstName == testEmployee.FirstName).Count() == 0);
        }

        [Fact]
        public void DeleteRangeTest()
        {
            List<Employee> testEmployees = new List<Employee>()
                {
                    new Employee { FirstName = "Asdfgh", LastName = "Zxc", EmploymentDate =new DateTime(2022,01,13), DismissalDate=null, CurrencyId= _currencyToTestId, OfficeId =_officeToTestId, Email= "aaa@gmail.com", Salary =777 },
                    new Employee { FirstName = "Qwett", LastName = "Zxc", EmploymentDate = new DateTime(2021, 01, 13), DismissalDate = null, CurrencyId = _currencyToTestId, OfficeId = _officeToTestId, Email = "bbb@gmail.com", Salary = 888 }
                };
            var response = _controller.AddRange(testEmployees);
            testEmployees = _controller.GetAll().Value.Where(e => e.FirstName == "Asdfgh" || e.FirstName == "Qwett").ToList();
            _controller.DeleteRange(testEmployees);
            Assert.True(response is OkResult);
            foreach (var employee in testEmployees)
            {
                Assert.True(_controller.GetAll().Value.Where(e => e.FirstName == employee.FirstName).Count() == 0);
            }
        }

        [Fact]
        public void DeleteByIdTest()
        {
            Employee testEmployee = new Employee { FirstName = "Asdfgh", LastName = "Zxc", EmploymentDate = new DateTime(2022, 01, 13), DismissalDate = null, CurrencyId = _currencyToTestId, OfficeId = _officeToTestId, Email = "aaa@gmail.com", Salary = 777 };
            var response = _controller.Add(testEmployee);
            testEmployee = _controller.GetAll().Value.Where(e => e.FirstName == testEmployee.FirstName).First();
            _controller.Delete(testEmployee.Id);
            Assert.True(response is OkResult);
            Assert.True(_controller.GetAll().Value.Where(e => e.FirstName == testEmployee.FirstName).Count() == 0);

        }
    }
}
