﻿using EmployeeManagement.Business;
using EmployeeManagement.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Test
{
    public class EmployeeFactoryTests
    {
        [Fact]
        public void CreateEmployee_ConstructInternalEmployee_SalaryMustBe2500()
        {
            var employeeFactory = new EmployeeFactory();

            var employee = (InternalEmployee)employeeFactory.CreateEmployee("Kanan", "Garazada");

            Assert.Equal(2500, employee.Salary);
        }

        [Fact]
        public void CreateEmployee_ConstructInternalEmployee_SalaryMustBeBetween2500And3500()
        {
            var employeeFactory = new EmployeeFactory();

            var employee = (InternalEmployee)employeeFactory.CreateEmployee("Kanan", "Garazada");

            Assert.True(employee.Salary >= 2500 && employee.Salary <=3500, "Salary not in acceptable range.");
        }

        [Fact]
        public void CreateEmployee_ConstructEmployee_SalaryMustBeBetween2500And3000Alternative()
        {
            var employeeFactory = new EmployeeFactory();

            var employee = (InternalEmployee)employeeFactory.CreateEmployee("Kanan", "Garazada");

            Assert.True(employee.Salary >= 2500);
            Assert.True(employee.Salary <= 3500);
        }

        [Fact]
        public void CreateEmployee_ConstructEmployee_SalaryMustBeBetween2500And3000AlternativeWithInRange()
        {
            var employeeFactory = new EmployeeFactory();

            var employee = (InternalEmployee)employeeFactory.CreateEmployee("Kanan", "Garazada");

            Assert.InRange(employee.Salary, 2500, 3500);
        }

        [Fact]
        public void CreateEmployee_ConstructEmployee_SalaryMustBe2500_PrecisionExample()
        {
            var employeeFactory = new EmployeeFactory();

            var employee = (InternalEmployee)employeeFactory.CreateEmployee("Kanan", "Garazada");
            employee.Salary = 2500.123m;

            Assert.Equal(2500, employee.Salary, 0);
        }
    }
}