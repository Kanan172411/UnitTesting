using EmployeeManagement.Business;
using EmployeeManagement.Business.EventArguments;
using EmployeeManagement.Business.Exceptions;
using EmployeeManagement.DataAccess.Entities;
using EmployeeManagement.Services.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Test
{
    public class EmployeeServiceTests
    {
        [Fact]
        public void CreateInternalEmployee_InternalEmployeeCreated_MustHaveAttendedFirstObligatoryCourse_WithObject()
        {
            var employeeManagementTestDataRepository = new EmployeeManagementTestDataRepository();
            var employeeService = new EmployeeService (employeeManagementTestDataRepository, new EmployeeFactory());
            var obligatoryCourse = employeeManagementTestDataRepository.GetCourse(Guid.Parse("37e03ca7-c730-4351-834c-b66f280cdb01"));

            var internalEmployee = employeeService.CreateInternalEmployee("Kanan", "Garazada");

            Assert.Contains(obligatoryCourse, internalEmployee.AttendedCourses);
        }

        [Fact]
        public void CreateInternalEmployee_InternalEmployeeCreated_MustHaveAttendedFirstObligatoryCourse_WithPredicate()
        {
            var employeeManagementTestDataRepository = new EmployeeManagementTestDataRepository();
            var employeeService = new EmployeeService (employeeManagementTestDataRepository, new EmployeeFactory());

            var internalEmployee = employeeService.CreateInternalEmployee("Kanan", "Garazada");

            Assert.Contains(internalEmployee.AttendedCourses, 
                course => course.Id == Guid.Parse("37e03ca7-c730-4351-834c-b66f280cdb01"));
        }

        [Fact]
        public void CreateInternalEmployee_InternalEmployeeCreated_MustHaveAttendedSecondObligatoryCourse_WithPredicate()
        {
            var employeeManagementTestDataRepository = new EmployeeManagementTestDataRepository();
            var employeeService = new EmployeeService(employeeManagementTestDataRepository, new EmployeeFactory());

            var internalEmployee = employeeService.CreateInternalEmployee("Kanan", "Garazada");

            Assert.Contains(internalEmployee.AttendedCourses,
                course => course.Id == Guid.Parse("1fd115cf-f44c-4982-86bc-a8fe2e4ff83e"));
        }

        [Fact]
        public void CreateInternalEmployee_InternalEmployeeCreated_AttendedCoursesMustMatchObligatoryCourse()
        {
            var employeeManagementTestDataRepository = new EmployeeManagementTestDataRepository();
            var employeeService = new EmployeeService(employeeManagementTestDataRepository, new EmployeeFactory());
            var obligatoryCourses = employeeManagementTestDataRepository.GetCourses(
                Guid.Parse("37e03ca7-c730-4351-834c-b66f280cdb01"),
                Guid.Parse("1fd115cf-f44c-4982-86bc-a8fe2e4ff83e"));

            var internalEmployee = employeeService.CreateInternalEmployee("Kanan", "Garazada");

            Assert.Equal(obligatoryCourses, internalEmployee.AttendedCourses);
        }

        [Fact]
        public void CreateInternalEmployee_InternalEmployeeCreated_AttendedCoursesMustNotBeNew()
        {
            var employeeManagementTestDataRepository = new EmployeeManagementTestDataRepository();
            var employeeService = new EmployeeService(employeeManagementTestDataRepository, new EmployeeFactory());

            var internalEmployee = employeeService.CreateInternalEmployee("Kanan", "Garazada");

            Assert.All(internalEmployee.AttendedCourses, course => Assert.False(course.IsNew));
        }

        [Fact]
        public async Task CreateInternalEmployee_InternalEmployeeCreated_AttendedCoursesMustMatchObligatoryCourse_Async()
        {
            var employeeManagementTestDataRepository = new EmployeeManagementTestDataRepository();
            var employeeService = new EmployeeService(employeeManagementTestDataRepository, new EmployeeFactory());
            var obligatoryCourses = await employeeManagementTestDataRepository.GetCoursesAsync(
                Guid.Parse("37e03ca7-c730-4351-834c-b66f280cdb01"),
                Guid.Parse("1fd115cf-f44c-4982-86bc-a8fe2e4ff83e"));

            var internalEmployee = await employeeService.CreateInternalEmployeeAsync("Kanan", "Garazada");

            Assert.Equal(obligatoryCourses, internalEmployee.AttendedCourses);
        }

        [Fact]
        public async Task GiveRaise_RaiseBelowMinimumGiven_EmployeeInvalidRaiseExceptionMustBeThrown()
        {
            var employeeService = new EmployeeService(new EmployeeManagementTestDataRepository(), new EmployeeFactory());
            var internalEmployee = new InternalEmployee("Kanan", "Garazada", 3, 3000, false, 1);

            await Assert.ThrowsAsync<EmployeeInvalidRaiseException>(
                async () => 
                    await employeeService.GiveRaiseAsync(internalEmployee, 50)
            );
        }

        [Fact]
        public void NotifyOfAbsence_EmployeeIsAbsent_OnEmployeeIsAbsentMustBeTriggered()
        {
            var employeeService = new EmployeeService(new EmployeeManagementTestDataRepository(), new EmployeeFactory());
            var internalEmployee = new InternalEmployee("Kanan", "Garazada", 3, 3000, false, 1);

            Assert.Raises<EmployeeIsAbsentEventArgs>(
                handler => employeeService.EmployeeIsAbsent += handler,
                handler => employeeService.EmployeeIsAbsent += handler,
                () => employeeService.NotifyOfAbsence(internalEmployee));
        }

        [Fact]
        public void CreateEmployee_IsExternalTrue_ReturnTypeMustBeExternalEmployee()
        {
            var factory = new EmployeeFactory();

            var employee = factory.CreateEmployee("Kanan", "Garazada", "Microsoft", true);

            Assert.IsType<ExternalEmployee>(employee);
        }
    }
}
