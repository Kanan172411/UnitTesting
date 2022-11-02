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
using EmployeeManagement.Test.TestData;

namespace EmployeeManagement.Test
{
    [Collection("EmployeeServiceCollection")]
    public class DataDrivenEmployeeServiceTests //: IClassFixture<EmployeeServiceFixture>
    {
        private readonly EmployeeServiceFixture _fixture;
        public DataDrivenEmployeeServiceTests(EmployeeServiceFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [InlineData("1fd115cf-f44c-4982-86bc-a8fe2e4ff83e")]
        [InlineData("37e03ca7-c730-4351-834c-b66f280cdb01")]
        public void CreateInternalEmployee_InternalEmployeeCreated_MustHaveAttendedFirstObligatoryCourse(Guid courseId)
        {
            var internalEmployee = _fixture.EmployeeService.CreateInternalEmployee("Kanan", "Garazada");

            Assert.Contains(internalEmployee.AttendedCourses,
                course => course.Id == courseId);
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

        public static IEnumerable<object[]> ExampleTestDataForGiveRaise_WithProperty
        {
            get
            {
                return new List<object[]>
                {
                    new object[] { 100, true},
                    new object[] { 200, false}
                };
            }
        }

        public static TheoryData<int, bool> StronglyTypedExampleTestDataForGiveRaise_WithProperty
        {
            get
            {
                return new TheoryData<int, bool>()
                {
                        { 100, true },
                        { 200, false }
                };
            }
        }

        public static IEnumerable<object[]> ExampleTestDataForGiveRaise_WithMethod(int testDataInstancesToProvide)
        {
            var testData = new List<object[]>
            {
                new object[] { 100, true},
                new object[] { 200, false}
            };

            return testData.Take(testDataInstancesToProvide);
        }

        [Theory]
        [ClassData(typeof(StronglyTypedEmployeeServiceTestData))]
        public async Task GiveRaise_RaiseGiven_EmployeeMinimumRaiseGivenMatchesValue(int raiseGiven, bool expectedValueForMinimumRaiseGiven)
        {
            var internalEmployee = new InternalEmployee("Kanan", "Garazada", 3, 3000, false, 1);

            await _fixture.EmployeeService.GiveRaiseAsync(internalEmployee, raiseGiven);

            Assert.Equal(expectedValueForMinimumRaiseGiven, internalEmployee.MinimumRaiseGiven);
        }
 
        [Theory]
        [MemberData(nameof(DataDrivenEmployeeServiceTests.ExampleTestDataForGiveRaise_WithMethod), 1, MemberType = typeof(DataDrivenEmployeeServiceTests))]
        public async Task GiveRaise_RaiseGiven_EmployeeMinimumRaiseGivenMatchesValue_WithMemberData_WithMethod(int raiseGiven, bool expectedValueForMinimumRaiseGiven)
        {
            var internalEmployee = new InternalEmployee("Kanan", "Garazada", 3, 3000, false, 1);

            await _fixture.EmployeeService.GiveRaiseAsync(internalEmployee, raiseGiven);

            Assert.Equal(expectedValueForMinimumRaiseGiven, internalEmployee.MinimumRaiseGiven);
        }
  
        [Theory]
        [MemberData(nameof(StronglyTypedExampleTestDataForGiveRaise_WithProperty))]
        public async Task GiveRaise_RaiseGiven_EmployeeMinimumRaiseGivenMatchesValue_WithMemberData_WithProperty(int raiseGiven, bool expectedValueForMinimumRaiseGiven)
        {
            var internalEmployee = new InternalEmployee("Kanan", "Garazada", 3, 3000, false, 1);

            await _fixture.EmployeeService.GiveRaiseAsync(internalEmployee, raiseGiven);

            Assert.Equal(expectedValueForMinimumRaiseGiven, internalEmployee.MinimumRaiseGiven);
        }

        [Theory]
        [ClassData(typeof(StronglyTypedEmployeeServiceTestData_FromFile))]
        public async Task GiveRaise_RaiseGiven_EmployeeMinimumRaiseGivenMatchesValue_WithExternalData(int raiseGiven, bool expectedValueForMinimumRaiseGiven)
        {
            var internalEmployee = new InternalEmployee("Kanan", "Garazada", 3, 3000, false, 1);

            await _fixture.EmployeeService.GiveRaiseAsync(internalEmployee, raiseGiven);

            Assert.Equal(expectedValueForMinimumRaiseGiven, internalEmployee.MinimumRaiseGiven);
        }
    }
}
