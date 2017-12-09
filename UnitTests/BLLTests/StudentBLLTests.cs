using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageSchool.BusinessLogic;
using LanguageSchool.DataAccess;
using LanguageSchool.Model;
using NUnit.Framework;
using Moq;
using System.Collections.ObjectModel;

namespace UnitTests
{
    [TestFixture]
    public class StudentBLLTests
    {
        Mock<IStudentDAL> mockStudentDAL;
        Mock<IClassDAL> mockClassDAL;
        StudentBLL studentBLL;

        public StudentBLLTests()
        {
            mockStudentDAL = new Mock<IStudentDAL>();
            mockClassDAL = new Mock<IClassDAL>();
            studentBLL = new StudentBLL(mockStudentDAL.Object, mockClassDAL.Object);
        }

        

        [Test]
        public void GetAll_Always_ReturnAllStudents()
        {
            var mockStudentList = new List<Student>
            {
                new Student()
                {
                   Id = "1",
                   FirstName = "Kate",
                   LastName = "Smith",
                   Email = "kate@gmail.com",
                   PhoneNumber = "536987415"
                },
                new Student()
                {
                   Id = "2",
                   FirstName = "Tom",
                   LastName = "Brown",
                   Email = "tomb@gmail.com",
                   PhoneNumber = "236859714"
                },
                new Student()
                {
                   Id = "3",
                   FirstName = "Elizabeth",
                   LastName = "Jones",
                   Email = "elizabeth@gmail.com",
                   PhoneNumber = "444555236"
                }
            };

            mockStudentDAL.Setup(x => x.GetAll()).Returns(mockStudentList);
            var result = studentBLL.GetAll();
            Assert.That(result.Count, Is.EqualTo(3));
        }

        [TestCase("Tom", "Watson", "tom@gmail.com", "545898452")]
        public void Add_WhenEmailExists_ThrowException(string firstName, string lastName, string email, string phoneNumber = "")
        {
            mockStudentDAL.Setup(x => x.FindByEmail(email)).Returns(
                new Student()
                {
                    Id = "2",
                    FirstName = "Tom",
                    LastName = "Brown",
                    Email = "tomb@gmail.com",
                    PhoneNumber = "236859714"
                });
            mockStudentDAL.Setup(x => x.Add(new Student()));

            Assert.Throws<Exception>(() => studentBLL.Add(firstName, lastName, email, phoneNumber));
        }

        [TestCase("Emma34", "Wats4on", "emma@gmail.com", "412563987")]
        public void Add_WhenNameIsInvalid_ThrowException(string firstName, string lastName, string email, string phoneNumber = "")
        {
            mockStudentDAL.Setup(x => x.FindByEmail(email)).Returns((Student)null);
            mockStudentDAL.Setup(x => x.Add(new Student()));

            Assert.Throws<Exception>(() => studentBLL.Add(firstName, lastName, email, phoneNumber));
        }

        [TestCase("Emma", "Watson", "emailwp.pl", "545898452")]
        public void Add_WhenEmailIsInvalid_ThrowException(string firstName, string lastName, string email, string phoneNumber = "")
        {
            mockStudentDAL.Setup(x => x.FindByEmail(email)).Returns((Student)null);
            mockStudentDAL.Setup(x => x.Add(new Student()));

            Assert.Throws<Exception>(() => studentBLL.Add(firstName, lastName, email, phoneNumber));
        }

        [Test]
        public void SignForClass_WhenStudentIsSigned_ReturnsErrorMessage()
        {
            Class c = new Class()
            {
                Id = 1,
                ClassName = "English M1",
                LanguageRefID = 1,
                LanguageLevelRefID = 1,
                StartTime = "10:00",
                EndTime = "11:30",
                Day = DayOfWeek.Monday,
                Students = new Collection<Student>()
            };

            Student s = new Student()
            {
                Id = "2",
                FirstName = "Tom",
                LastName = "Brown",
                Email = "tomb@gmail.com",
                PhoneNumber = "236859714",
                Classes = new Collection<Class>()
            };

            s.Classes.Add(c);

            mockStudentDAL.Setup(x => x.GetById(s.Id)).Returns(s);
            mockStudentDAL.Setup(x => x.GetClassByID(c.Id)).Returns(c);
            mockStudentDAL.Setup(x => x.SignForClass(s, c));
            var result = studentBLL.SignForClass(s.Id, c.Id);
            Assert.That(result, Is.EqualTo("Student is already registered for this class"));
        }

        [Test]
        public void UnsubscribeFromClass_WhenClassIdIsNotFound_ReturnsErrorMessage()
        {
            Student s = new Student()
            {
                Id = "2",
                FirstName = "Tom",
                LastName = "Brown",
                Email = "tomb@gmail.com",
                PhoneNumber = "236859714",
                Classes = new Collection<Class>()
            };

            mockStudentDAL.Setup(x => x.GetById(s.Id)).Returns(s);
            mockStudentDAL.Setup(x => x.GetClassByID(It.IsAny<int>())).Returns((Class)null);
            var result = studentBLL.UnsubscribeFromClass(s.Id, It.IsAny<int>());
            Assert.That(result, Is.EqualTo("Class not found"));
        }

        [Test]
        public void UnsubscribeFromClass_WhenStudentIsNotSigned_ReturnsErrorMessage()
        {
            Class c = new Class()
            {
                Id = 1,
                ClassName = "English M1",
                LanguageRefID = 1,
                LanguageLevelRefID = 1,
                StartTime = "10:00",
                EndTime = "11:30",
                Day = DayOfWeek.Monday,
                Students = new Collection<Student>()
            };

            Student s = new Student()
            {
                Id = "2",
                FirstName = "Tom",
                LastName = "Brown",
                Email = "tomb@gmail.com",
                PhoneNumber = "236859714",
                Classes = new Collection<Class>()
            };
            
            mockStudentDAL.Setup(x => x.GetById(s.Id)).Returns(s);
            mockStudentDAL.Setup(x => x.GetClassByID(c.Id)).Returns(c);
            mockStudentDAL.Setup(x => x.UnsubscribeFromClass(s, c));
            var result = studentBLL.UnsubscribeFromClass(s.Id, c.Id);
            Assert.That(result, Is.EqualTo("Student is not registered for this class"));
        }

        [Test]
        public void UnsubscribeFromClass_Always_ReturnsCorrectResult()
        {
            Class c = new Class()
            {
                Id = 1,
                ClassName = "English M1",
                LanguageRefID = 1,
                LanguageLevelRefID = 1,
                StartTime = "10:00",
                EndTime = "11:30",
                Day = DayOfWeek.Monday,
                Students = new Collection<Student>()
            };

            Student s = new Student()
            {
                Id = "2",
                FirstName = "Tom",
                LastName = "Brown",
                Email = "tomb@gmail.com",
                PhoneNumber = "236859714",
                Classes = new Collection<Class>()
            };

            s.Classes.Add(c);
            mockStudentDAL.Setup(x => x.GetById(s.Id)).Returns(s);
            mockStudentDAL.Setup(x => x.GetClassByID(c.Id)).Returns(c);
            mockStudentDAL.Setup(x => x.UnsubscribeFromClass(s, c));
            var result = studentBLL.UnsubscribeFromClass(s.Id, c.Id);
            Assert.IsNull(result);
        }

        [TestCase(100, "Sam", "Smith", "tomb@gmail.com", "545898452")]
        public void Update_WhenNewEmailExists_ReturnsErrorMessage(int id, string firstName, string lastName, string email, string phoneNumber = "")
        {
            mockStudentDAL.Setup(x => x.FindByEmail(email)).Returns(
                new Student()
                {
                    Id = "2",
                    FirstName = "Tom",
                    LastName = "Brown",
                    Email = "tomb@gmail.com",
                    PhoneNumber = "236859714"
                });
            mockStudentDAL.Setup(x => x.Update(id.ToString(), firstName, lastName, email, phoneNumber));

            var result = studentBLL.Update(id.ToString(), firstName, lastName, email, phoneNumber);
            Assert.That(result, Is.EqualTo("Student with such email already exists"));
        }

        [TestCase(100, "Sam856", "Smith565", "sam@gmail.com", "545898452")]
        public void Update_WhenNameIsInvalid_ReturnsErrorMessage(int id, string firstName, string lastName, string email, string phoneNumber = "")
        {
            mockStudentDAL.Setup(x => x.FindByEmail(email)).Returns((Student)null);
            mockStudentDAL.Setup(x => x.Update(id.ToString(), firstName, lastName, email, phoneNumber));

            var result = studentBLL.Update(id.ToString(), firstName, lastName, email, phoneNumber);
            Assert.That(result, Is.EqualTo("Invalid First Name"));
        }

        [TestCase(100, "Sam", "Smith", "samgmail.com", "545898452")]
        public void Update_WhenEmailIsInvalid_ReturnsErrorMessage(int id, string firstName, string lastName, string email, string phoneNumber = "")
        {
            mockStudentDAL.Setup(x => x.FindByEmail(email)).Returns((Student)null);
            mockStudentDAL.Setup(x => x.Update(id.ToString(), firstName, lastName, email, phoneNumber));

            var result = studentBLL.Update(id.ToString(), firstName, lastName, email, phoneNumber);
            Assert.That(result, Is.EqualTo("Invalid Email Address"));
        }

        [TestCase(2, 2, 1, 2)]
        [TestCase(1, 2, 2, 2)]
        [TestCase(2, 1, 1, 3)]
        [TestCase(1, 3, 3, 1)]
        public void Search_WhenSpecifiedFilter_ReturnExpectedResult(int pageNumber, int pageSize, int resultStudentsCount, int resultPageCount)
        {
            StudentFilter filter = new StudentFilter
            {
                Filter = SearchBy.LastName,
                IsSorted = false,
                Text = "Smit",
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var mockFiltredStudentList = new List<Student>
            {
                new Student()
                {
                   Id = "1",
                   FirstName = "Kate",
                   LastName = "Smith",
                   Email = "kate@gmail.com",
                   PhoneNumber = "536987415"
                },
                new Student()
                {
                   Id = "2",
                   FirstName = "Tom",
                   LastName = "Smithway",
                   Email = "tomb@gmail.com",
                   PhoneNumber = "236859714"
                },
                new Student()
                {
                   Id = "3",
                   FirstName = "Elizabeth",
                   LastName = "Smithw",
                   Email = "elizabeth@gmail.com",
                   PhoneNumber = "444555236"
                }
            };

            mockStudentDAL.Setup(x => x.Search(filter.Filter, filter.Text, filter.IsSorted)).Returns(mockFiltredStudentList);
            List<Student> students;
            int pageCount;

            (students, pageCount) = studentBLL.Search(filter);

            Assert.That(students.Count, Is.EqualTo(resultStudentsCount));
            Assert.That(pageCount, Is.EqualTo(resultPageCount));
        }

        [Test]
        public void GetClasses_Always_ReturnsCorrectResult()
        {
            List<Class> classes = new List<Class>()
            {
                new Class(){
                    Id = 1,
                    ClassName = "English M1",
                    LanguageRefID = 1,
                    LanguageLevelRefID = 1,
                    StartTime = "10:00",
                    EndTime = "11:30",
                    Day = DayOfWeek.Monday,
                    Students = new Collection<Student>()
                },

                new Class(){
                    Id = 2,
                    ClassName = "English M1",
                    LanguageRefID = 1,
                    LanguageLevelRefID = 1,
                    StartTime = "10:00",
                    EndTime = "11:30",
                    Day = DayOfWeek.Monday,
                    Students = new Collection<Student>()
                }
            };

            mockStudentDAL.Setup(x => x.GetClasses(It.IsAny<string>())).Returns(classes);

            var result = studentBLL.GetClasses(It.IsAny<string>());
            Assert.That(result.Count, Is.EqualTo(2));
        }
    }
}
