using EmployeeManagement.Business.EventArguments;
using EmployeeManagement.Business.Exceptions;
using EmployeeManagement.Business;
using EmployeeManagement.DataAccess.Entities;
using EmployeeManagement.Test.Fixtures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Test
{
    [Collection("EmployeeServiceCollection")]
    public class DataDrivenEmployeeServiceTests : IClassFixture<EmployeeServiceFixture>
    {
        private readonly EmployeeServiceFixture _fixture;
        public DataDrivenEmployeeServiceTests(EmployeeServiceFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GiveRaise_MinimumRaiseGiven_EmployeeMinimumRaiseGivenMustBeTrue()
        {
            var internalEmployee = new InternalEmployee("Brooklyn", "Cannon", 5, 3000, false, 1);

            await _fixture.EmployeeService.GiveRaiseAsync(internalEmployee, 100);

            Assert.True(internalEmployee.MinimumRaiseGiven);
        }


        [Fact]
        public async Task GiveRaise_MoreThanMinimumRaiseGiven_EmployeeMinimumRaiseGivenMustBeFalse()
        {
            var internalEmployee = new InternalEmployee("Brooklyn", "Cannon", 5, 3000, false, 1);

            await _fixture.EmployeeService.GiveRaiseAsync(internalEmployee, 200);

            Assert.False(internalEmployee.MinimumRaiseGiven);
        }

    }
}
