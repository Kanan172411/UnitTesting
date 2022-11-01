using EmployeeManagement.Business;
using EmployeeManagement.Business.EventArguments;
using EmployeeManagement.Business.Exceptions;
using EmployeeManagement.DataAccess.Entities;
using EmployeeManagement.Services.Test;
using EmployeeManagement.Test.Fixtures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace EmployeeManagement.Test
{
    [Collection("EmployeeServiceCollection")]
    public class EmployeeServiceTests 
    {
        private readonly EmployeeServiceFixture _fixture;
        private readonly ITestOutputHelper _testOutputHelper;
        public EmployeeServiceTests(EmployeeServiceFixture fixture, ITestOutputHelper testOutputHelper)
        {
            _fixture = fixture;
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void CreateInternalEmployee_InternalEmployeeCreated_MustHaveAttendedFirstObligatoryCourse_WithObject()
        {
            var obligatoryCourse = _fixture.EmployeeManagementTestDataRepository.GetCourse(Guid.Parse("37e03ca7-c730-4351-834c-b66f280cdb01"));

            var internalEmployee = _fixture.EmployeeService.CreateInternalEmployee("Kanan", "Garazada");
            _testOutputHelper.WriteLine($"Employee after Act:{internalEmployee.FirstName} {internalEmployee.LastName}");
            internalEmployee.AttendedCourses.ForEach(c => _testOutputHelper.WriteLine($"Attended course: {c.Id} {c.Title}"));

            Assert.Contains(obligatoryCourse, internalEmployee.AttendedCourses);
        }

        [Fact]
        public void CreateInternalEmployee_InternalEmployeeCreated_MustHaveAttendedFirstObligatoryCourse_WithPredicate()
        {
            var internalEmployee = _fixture.EmployeeService.CreateInternalEmployee("Kanan", "Garazada");

            Assert.Contains(internalEmployee.AttendedCourses, 
                course => course.Id == Guid.Parse("37e03ca7-c730-4351-834c-b66f280cdb01"));
        }

        [Fact]
        public void CreateInternalEmployee_InternalEmployeeCreated_MustHaveAttendedSecondObligatoryCourse_WithPredicate()
        {
            var internalEmployee = _fixture.EmployeeService.CreateInternalEmployee("Kanan", "Garazada");

            Assert.Contains(internalEmployee.AttendedCourses,
                course => course.Id == Guid.Parse("1fd115cf-f44c-4982-86bc-a8fe2e4ff83e"));
        }

        [Fact]
        public void CreateInternalEmployee_InternalEmployeeCreated_AttendedCoursesMustMatchObligatoryCourse()
        {
            var obligatoryCourses = _fixture.EmployeeManagementTestDataRepository.GetCourses(
                Guid.Parse("37e03ca7-c730-4351-834c-b66f280cdb01"),
                Guid.Parse("1fd115cf-f44c-4982-86bc-a8fe2e4ff83e"));

            var internalEmployee = _fixture.EmployeeService.CreateInternalEmployee("Kanan", "Garazada");

            Assert.Equal(obligatoryCourses, internalEmployee.AttendedCourses);
        }

        [Fact]
        public void CreateInternalEmployee_InternalEmployeeCreated_AttendedCoursesMustNotBeNew()
        {
            var internalEmployee = _fixture.EmployeeService.CreateInternalEmployee("Kanan", "Garazada");

            Assert.All(internalEmployee.AttendedCourses, course => Assert.False(course.IsNew));
        }

        [Fact]
        public async Task CreateInternalEmployee_InternalEmployeeCreated_AttendedCoursesMustMatchObligatoryCourse_Async()
        {
            var obligatoryCourses = await _fixture.EmployeeManagementTestDataRepository.GetCoursesAsync(
                Guid.Parse("37e03ca7-c730-4351-834c-b66f280cdb01"),
                Guid.Parse("1fd115cf-f44c-4982-86bc-a8fe2e4ff83e"));

            var internalEmployee = await _fixture.EmployeeService.CreateInternalEmployeeAsync("Kanan", "Garazada");

            Assert.Equal(obligatoryCourses, internalEmployee.AttendedCourses);
        }

        [Fact]
        public async Task GiveRaise_RaiseBelowMinimumGiven_EmployeeInvalidRaiseExceptionMustBeThrown()
        {
            var internalEmployee = new InternalEmployee("Kanan", "Garazada", 3, 3000, false, 1);

            await Assert.ThrowsAsync<EmployeeInvalidRaiseException>(
                async () => 
                    await _fixture.EmployeeService.GiveRaiseAsync(internalEmployee, 50)
            );
        }

        [Fact]
        public void NotifyOfAbsence_EmployeeIsAbsent_OnEmployeeIsAbsentMustBeTriggered()
        {
            var internalEmployee = new InternalEmployee("Kanan", "Garazada", 3, 3000, false, 1);

            Assert.Raises<EmployeeIsAbsentEventArgs>(
                handler => _fixture.EmployeeService.EmployeeIsAbsent += handler,
                handler => _fixture.EmployeeService.EmployeeIsAbsent += handler,
                () => _fixture.EmployeeService.NotifyOfAbsence(internalEmployee));
        }
    }
}
