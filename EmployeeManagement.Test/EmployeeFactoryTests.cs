using EmployeeManagement.Business;
using EmployeeManagement.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Test
{
    public class EmployeeFactoryTests : IDisposable
    {
        private EmployeeFactory _factory;

        public EmployeeFactoryTests()
        {
            _factory = new EmployeeFactory();
        }

        public void Dispose()
        {
            //clean up the setup code, if required
        }

        [Fact]
        [Trait("Category", "EmployeeFactory_CreateEmployee_Salary")]
        public void CreateEmployee_ConstructInternalEmployee_SalaryMustBe2500()
        {
            var employee = (InternalEmployee)_factory.CreateEmployee("Kanan", "Garazada");

            Assert.Equal(2500, employee.Salary);
        }

        [Fact]
        [Trait("Category", "EmployeeFactory_CreateEmployee_Salary")]
        public void CreateEmployee_ConstructInternalEmployee_SalaryMustBeBetween2500And3500()
        {
            var employee = (InternalEmployee)_factory.CreateEmployee("Kanan", "Garazada");

            Assert.True(employee.Salary >= 2500 && employee.Salary <=3500, "Salary not in acceptable range.");
        }

        [Fact]
        [Trait("Category", "EmployeeFactory_CreateEmployee_Salary")]
        public void CreateEmployee_ConstructEmployee_SalaryMustBeBetween2500And3000Alternative()
        {
            var employee = (InternalEmployee)_factory.CreateEmployee("Kanan", "Garazada");

            Assert.True(employee.Salary >= 2500);
            Assert.True(employee.Salary <= 3500);
        }

        [Fact]
        public void CreateEmployee_ConstructEmployee_SalaryMustBeBetween2500And3000AlternativeWithInRange()
        {
            var employee = (InternalEmployee)_factory.CreateEmployee("Kanan", "Garazada");

            Assert.InRange(employee.Salary, 2500, 3500);
        }

        [Fact]
        [Trait("Category", "EmployeeFactory_CreateEmployee_Salary")]
        public void CreateEmployee_ConstructEmployee_SalaryMustBe2500_PrecisionExample()
        {
            var employee = (InternalEmployee)_factory.CreateEmployee("Kanan", "Garazada");
            employee.Salary = 2500.123m;

            Assert.Equal(2500, employee.Salary, 0);
        }

        [Fact]
        [Trait("Category","EmployeeFactory_CreateEmployee_ReturnType")]
        public void CreateEmployee_IsExternalTrue_ReturnTypeMustBeExternalEmployee()
        {
            var employee = _factory.CreateEmployee("Kanan", "Garazada", "Microsoft", true);

            Assert.IsType<ExternalEmployee>(employee);
        }
    }
}
