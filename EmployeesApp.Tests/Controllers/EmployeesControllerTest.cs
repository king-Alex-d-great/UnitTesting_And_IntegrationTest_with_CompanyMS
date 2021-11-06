using System.Collections.Generic;
using System.Linq;
using EmployeesApp.Contracts;
using EmployeesApp.Controllers;
using EmployeesApp.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace EmployeesApp.Tests.Controllers
{
    public class EmployeesControllerTest
    {
        private Mock<IEmployeeRepository> _mockRepo;

        private EmployeesController _controller;

        public EmployeesControllerTest()
        {
            _mockRepo = new Mock<IEmployeeRepository>();

            _controller = new EmployeesController(_mockRepo.Object);
        }

        [Fact]
        public void Index_ActionExecutes_ReturnsActionResult()
        {
            var result = _controller.Index();
            Assert.True(result is ViewResult);
            //Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Index_ActionExecutes_ReturnsExactNumberOfEmployees()
        {
            //Setup sets up information for a particular method, for example here => whenever the getAll method is called
            //we have set it up to always return two employee objects
            _mockRepo.Setup(a => a.GetAll()).Returns(new List<Employee>() { new Employee(), new Employee() });

            List<Employee> emp1 = _mockRepo.Object.GetAll().ToList();

            var result = _controller.Index();

            var viewResult = Assert.IsType<ViewResult>(result);

            var employees = Assert.IsType<List<Employee>>(viewResult.Model);

            Assert.Equal(emp1.Count, employees.Count);
        }

        [Fact]
        public void Create_ActionExecutes_ReturnsCreateView()
        {
            var result = _controller.Create();

            Assert.True(result is ViewResult);
        }

        [Fact]
        //Can a complex type be passed as an inline data???
        public void Create_InvalidModelState_ReturnsView()
        {
            //create a model state error
            _controller.ModelState.AddModelError("Name", "Name is required");
            //create a new invalid employee
            var emp = new Employee() { AccountNumber = "123-1234567890-23", Age = 23 };
            //try to do the create it
            var result = _controller.Create(emp);
            //check the result of the create method
            var viewResult = Assert.IsType<ViewResult>(result);
            //check if the result associated with the result is actuall an employee
            var employee = Assert.IsType<Employee>(viewResult.Model);
            //check its actually the emloyee we sent!
            Assert.Equal(emp.AccountNumber, employee.AccountNumber);

            Assert.Equal(emp.Age, employee.Age);
        }

        [Fact]
        public void Create_InvalidModelState_CreateEmployeeNeverExecutes()
        {
            //create a model state error
            _controller.ModelState.AddModelError("Name", "Name is required");
            //create a new invalid employee
            var emp = new Employee() { AccountNumber = "123-1234567890-23", Age = 23 };
            //try to do the create it
            var result = _controller.Create(emp);

            _mockRepo.Verify(x => x.CreateEmployee(It.IsAny<Employee>()), Times.Never);
            //this verifies that whatever is entering the createemployee method is an employee and because modelStae is invalid, that it never runs
        }

        [Fact]
        public void Create_validModelState_CreateEmployeeExecutesJustOnce()
        {
            Employee emp = null;
            //Setup sets up information for a particular method, for example here => whenever the createEmployee method is called
            //we have set it up to callback the method that assigns the newly created employee to emp declared above
            _mockRepo.Setup(a => a.CreateEmployee(It.IsAny<Employee>())).Callback<Employee>(a => emp = a);

            var employee = new Employee
            {
                Name = "Test Employee",
                Age = 32,
                AccountNumber = "123-5435789603-21"
            };
            _controller.Create(employee);

            _mockRepo.Verify(x => x.CreateEmployee(It.IsAny<Employee>()), Times.Once);

            Assert.Equal(emp.Name, employee.Name);

            Assert.Equal(emp.Age, employee.Age);

            Assert.Equal(emp.AccountNumber, employee.AccountNumber);
        }

        [Fact]
        public void Create_validModelState_RedirectsToIndexAction()
        {
            var employee = new Employee
            {
                Name = "Test Employee",
                Age = 32,
                AccountNumber = "123-5435789603-21"
            };
            var result = _controller.Create(employee);

            var redirectToAction = Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal("Index", redirectToAction.ActionName);
        }
    }
}
