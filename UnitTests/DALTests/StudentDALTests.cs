using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageSchool.DataAccess;
using NUnit.Framework;
using LanguageSchool.Model;

namespace UnitTests
{
    [TestFixture]
    class StudentDALTests
    {
        LanguageSchoolMockContext context;
        StudentDAL studentDAL;

        public StudentDALTests()
        {
            context = new LanguageSchoolMockContext();
            studentDAL = new StudentDAL(context);
        }

        

        [Test]
        public void SearchByName_WhenNameExists_ReturnsCorrectStudent()
        {
            var result = studentDAL.Search(SearchBy.LastName, "Davis", false);
            Assert.That(result.First().Id, Is.EqualTo("5"));
        }

        [Test]
        public void SearchByEmail_WhenEmailExists_ReturnsCorrectStudent()
        {
            var result = studentDAL.Search(SearchBy.Email, "king@gmail.com", false);
            Assert.That(result.First().Id, Is.EqualTo("4"));
        }

        [Test]
        public void SearchByName_WhenIsAlphabeticallySorted_ReturnsCorrectFirstStudent()
        {
            var result = studentDAL.Search(SearchBy.LastName, "", true);
            Assert.That(result.First().Id, Is.EqualTo("2"));
        }

        [Test]
        public void SearchByEmail_WhenIsAlphabeticallySorted_ReturnsCorrectFirstStudent()
        {
            var result = studentDAL.Search(SearchBy.Email, "", true);
            Assert.That(result.First().Id, Is.EqualTo("5"));
        }

        [Test]
        public void Update_NonExisitingStudent_ThrowsException()
        {
            Assert.Throws<Exception>(() => studentDAL.Update("-1", "Tom", "Cruise", "tom@gmail.com", "503998452"));
        }

        [Test]
        public void Update_ExisitingStudent_ReturnCorrectResult()
        {
            studentDAL.Update("2", "John", "Cruise", "tom@gmail.com", "503998452");
            var firstName = context.Students.Where(x => x.Id == "2").First().FirstName;
            Assert.That(firstName, Is.EqualTo("John"));
        }

        [Test]
        public void FindByEmail_NonExistingEmail_ReturnsNull()
        {
            var result = studentDAL.FindByEmail("example@gmail.com");
            Assert.IsNull(result);
        }

        [TestCase("kate@gmail.com", 1)]
        [TestCase("elizabeth@gmail.com", 3)]
        public void FindByEmail_ExistingEmail_ReturnsCorrectStudent(string email, int id)
        {
            var result = studentDAL.FindByEmail(email);
            Assert.IsNotNull(result);
            Assert.That(result.Id, Is.EqualTo(id.ToString()));
        }

        [Test]
        public void FindByID_NonExistingID_ReturnsNull()
        {
            var result = studentDAL.GetById("-1");
            Assert.IsNull(result);
        }

        [TestCase(1)]
        [TestCase(3)]
        public void FindByID_ExistingID_ReturnsCorrectStudent(int id)
        {
            var result = studentDAL.GetById(id.ToString());
            Assert.IsNotNull(result);
            Assert.That(result.Id, Is.EqualTo(id.ToString()));
        }


        [Test]
        public void SignForClass_WhenStudentIsNotSigned_IncreaseStudentClassesCount()
        {
            Student s = context.Students.First();
            int countClassesBefore = s.Classes.Count;
            Class c = context.Classes.First();

            studentDAL.SignForClass(s, c);

            int countClassesAfter = s.Classes.Count;
            Assert.That(countClassesBefore + 1, Is.EqualTo(countClassesAfter));
        }

        [Test]
        public void UnsubscribeFromClass_WhenStudentIsSigned_DecreaseStudentClassesCount()
        {
            Student s = context.Students.First();
            int countClassesBefore = s.Classes.Count;
            Class c = context.Classes.First();

            studentDAL.UnsubscribeFromClass(s, c);

            int countClassesAfter = s.Classes.Count;
            Assert.That(countClassesBefore - 1, Is.EqualTo(countClassesAfter));
        }

        [Test]
        public void GetClasses_Always_ReturnCorrectResult()
        {
            List<Class> c = context.Classes.Take(2).ToList();
            var s = context.Students.FirstOrDefault();
            s.Classes.Add(c[0]);
            s.Classes.Add(c[1]);

            var result = studentDAL.GetClasses(s.Id);

            Assert.That(result.Count, Is.EqualTo(2));
        }

        [Test]
        public void Add_WhenNewStudentNonExists_IncreaseStudentsCount()
        {
            int count1 = context.Students.Count();
            var student = new Student
            {
                Id = "6",
                FirstName = "Paul",
                LastName = "Kingson",
                Email = "paulking@gmail.com",
                PhoneNumber = "789652314",
            };
            studentDAL.Add(student);

            int count2 = context.Students.Count();
            Assert.That(count1 + 1, Is.EqualTo(count2));
            context.Students.Remove(student);
        }

        [Test]
        public void GetAll_Always_ReturnAllStudents()
        {
            var result = studentDAL.GetAll().Count();
            Assert.That(result, Is.EqualTo(context.Students.Count()));
        }
    }
}
