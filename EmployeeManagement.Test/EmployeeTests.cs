using EmployeeManagement.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Test
{
    public class EmployeeTests
    {
        [Fact]
        public void EmployeeFullNamePropertyGetter_InputFirstNameAndLastName_FullNameIsConcatenation()
        {
            var employee = new InternalEmployee("Kanan", "Garazada", 0, 2500, false, 1);

            employee.FirstName = "Michael";
            employee.LastName = "SCOFIELD";

            Assert.Equal("Michael Scofield", employee.FullName, ignoreCase: true);
        }

        [Fact]
        public void EmployeeFullNamePropertyGetter_InputFirstNameAndLastName_FullNameStartWithFirstName()
        {
            var employee = new InternalEmployee("Kanan", "Garazada", 0, 2500, false, 1);

            employee.FirstName = "Michael";
            employee.LastName = "Scofield";

            Assert.StartsWith(employee.FirstName, employee.FullName);
        }

        [Fact]
        public void EmployeeFullNamePropertyGetter_InputFirstNameAndLastName_FullNameEndWithLastName()
        {
            var employee = new InternalEmployee("Kanan", "Garazada", 0, 2500, false, 1);

            employee.FirstName = "Michael";
            employee.LastName = "Scofield";

            Assert.EndsWith(employee.LastName, employee.FullName);
        }

        [Fact]
        public void EmployeeFullNamePropertyGetter_InputFirstNameAndLastName_FullNameContainsPartOfConcatenation()
        {
            var employee = new InternalEmployee("Kanan", "Garazada", 0, 2500, false, 1);

            employee.FirstName = "Michael";
            employee.LastName = "Scofield";

            Assert.Contains("el Scof", employee.FullName);
        }
        
        [Fact]
        public void EmployeeFullNamePropertyGetter_InputFirstNameAndLastName_FullNameSoundsLikeConcatenation()
        {
            var employee = new InternalEmployee("Kanan", "Garazada", 0, 2500, false, 1);

            employee.FirstName = "Lucia";
            employee.LastName = "Shelton";

            Assert.Matches("Lu(c|s|z)ia Shel(t|d)on", employee.FullName);
        }
    }
}
