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
            _officeToTestId = ((new OfficeController(new ApplicationDbContextFactory().CreateDbContext(args)).GetAll() as OkObjectResult).Value as IEnumerable<Office>).First().Id;
            _currencyToTestId = ((new CurrencyController(new ApplicationDbContextFactory().CreateDbContext(args)).GetAll() as OkObjectResult).Value as IEnumerable<Currency>).First().Id;
        }



        [Fact]
        public void GetAllTest()
        {
            var resultAction = _controller.GetAll();
            var result = resultAction as OkObjectResult;
            var employees = result.Value as IEnumerable<Employee>;
            Assert.True(resultAction is OkObjectResult);
            Assert.NotNull(result.Value);
            Assert.NotNull(employees);
        }

        [Theory]
        [InlineData(3, "Anna")]
        [InlineData(4, "Ivan")]
        public void GetByIdTestSuccess(int id, string name)
        {
            var resultAction = _controller.Find(id);
            var result = resultAction as OkObjectResult;
            var employee = result.Value as Employee;
            Assert.True(resultAction is OkObjectResult);
            Assert.NotNull(result.Value);
            Assert.NotNull(employee);
            Assert.Equal(name, employee.FirstName);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(1500)]
        public void GetByIdTestInvalidId(int id)
        {
            var result = _controller.Find(id);
            Assert.False(result is OkObjectResult);
        }

        [Fact]
        public void GetByOfficeTestSuccess()
        {
            var resultAction = _controller.FindByOffice(_officeToTestId);
            var result = resultAction as OkObjectResult;
            var employees = result.Value as IEnumerable<Employee>;
            Assert.True(resultAction is OkObjectResult);
            Assert.NotNull(result.Value);
            Assert.NotNull(employees);
            foreach(var employee in employees)
            {
                Assert.Equal(_officeToTestId, employee.OfficeId);
            }
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(1500)]
        public void GetByOfficeTestInvalidId(int id)
        {
            var result = _controller.FindByOffice(id);
            Assert.False(result is OkObjectResult);
        }

        [Fact]
        public void AddTest()
        {
            Employee testEmployee = new Employee { FirstName = "Asdfgh", LastName = "Zxc", EmploymentDate =new DateTime(2022,01,13), DismissalDate=null, CurrencyId= _currencyToTestId, OfficeId =_officeToTestId, Email= "aaa@gmail.com", Salary =777 };
            var response = _controller.Add(testEmployee);

            Assert.True(response is CreatedAtActionResult);
            Assert.True(((_controller.GetAll() as OkObjectResult).Value as IEnumerable<Employee>).Where(e => e.FirstName == testEmployee.FirstName).Count() > 0);

            testEmployee = ((_controller.GetAll() as OkObjectResult).Value as IEnumerable<Employee>).Where(e => e.FirstName == testEmployee.FirstName).First();
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
                Assert.True(((_controller.GetAll() as OkObjectResult).Value as IEnumerable<Employee>).Where(e => e.FirstName == employee.FirstName).Count() > 0);

            }
            testEmployees = ((_controller.GetAll() as OkObjectResult).Value as IEnumerable<Employee>).Where(e => e.FirstName == "Asdfgh" || e.FirstName == "Qwett").ToList();
            _controller.DeleteRange(testEmployees);

        }

        [Fact]
        public void UpdateTest()
        {
            Employee testEmployee = new Employee { FirstName = "Asdfgh", LastName = "Zxc", EmploymentDate = new DateTime(2022, 01, 13), DismissalDate = null, CurrencyId = _currencyToTestId, OfficeId = _officeToTestId, Email = "aaa@gmail.com", Salary = 777 };
            var addedEmployeeId = ((_controller.GetAll() as OkObjectResult).Value as IEnumerable<Employee>).Where(e => e.FirstName == "Asdfgh").First().Id;
            testEmployee.FirstName = "RQ";
            testEmployee.Id = addedEmployeeId;

            var response = _controller.Update(testEmployee);


            Assert.True(response is OkResult);
            Assert.True(((_controller.GetAll() as OkObjectResult).Value as IEnumerable<Employee>).Where(e => e.Id == addedEmployeeId).First().FirstName == "RQ");

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

            testEmployees = ((_controller.GetAll() as OkObjectResult).Value as IEnumerable<Employee>).Where(o => o.FirstName == "Asdfgh" || o.FirstName == "Qwett").ToList();
            testEmployees[0].FirstName = "QB";
            testEmployees[1].FirstName = "DEFOPS";

            var response = _controller.UpdateRange(testEmployees);
            Assert.True(response is OkResult);
            foreach (var employee in testEmployees)
            {
                Assert.True(((_controller.GetAll() as OkObjectResult).Value as IEnumerable<Employee>).Where(e=> e.FirstName == employee.FirstName).Count() > 0);

            }
            _controller.DeleteRange(testEmployees);

        }

        [Fact]
        public void DeleteTest()
        {
            Employee testEmployee = new Employee { FirstName = "Asdfgh", LastName = "Zxc", EmploymentDate = new DateTime(2022, 01, 13), DismissalDate = null, CurrencyId = _currencyToTestId, OfficeId = _officeToTestId, Email = "aaa@gmail.com", Salary = 777 };
            var response = _controller.Add(testEmployee);
            testEmployee = ((_controller.GetAll() as OkObjectResult).Value as IEnumerable<Employee>).Where(e => e.FirstName == testEmployee.FirstName).First();
            _controller.Delete(testEmployee);
            Assert.True(response is OkResult);
            Assert.True(((_controller.GetAll() as OkObjectResult).Value as IEnumerable<Employee>).Where(e => e.FirstName == testEmployee.FirstName).Count() == 0);
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
            testEmployees = ((_controller.GetAll() as OkObjectResult).Value as IEnumerable<Employee>).Where(e => e.FirstName == "Asdfgh" || e.FirstName == "Qwett").ToList();
            _controller.DeleteRange(testEmployees);
            Assert.True(response is OkResult);
            foreach (var employee in testEmployees)
            {
                Assert.True(((_controller.GetAll() as OkObjectResult).Value as IEnumerable<Employee>).Where(e => e.FirstName == employee.FirstName).Count() == 0);
            }
        }

        [Fact]
        public void DeleteByIdTestSuccess()
        {
            Employee testEmployee = new Employee { FirstName = "Asdfgh", LastName = "Zxc", EmploymentDate = new DateTime(2022, 01, 13), DismissalDate = null, CurrencyId = _currencyToTestId, OfficeId = _officeToTestId, Email = "aaa@gmail.com", Salary = 777 };
            var response = _controller.Add(testEmployee);
            testEmployee = ((_controller.GetAll() as OkObjectResult).Value as IEnumerable<Employee>).Where(e => e.FirstName == testEmployee.FirstName).First();
            _controller.Delete(testEmployee.Id);
            Assert.True(response is OkResult);
            Assert.True(((_controller.GetAll() as OkObjectResult).Value as IEnumerable<Employee>).Where(e => e.FirstName == testEmployee.FirstName).Count() == 0);

        }


        [Theory]
        [InlineData(-1)]
        [InlineData(10000)]
        public void DeleteByIdTestInvalidId(int id)
        {
            var result = _controller.Delete(id);
            Assert.False(result is OkObjectResult);
        }
    }
}
