using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using LanguageSchool.BusinessLogic;
using LanguageSchool.Shared.Dtos;
using LanguageSchool.WebApi.Controllers;
using LanguageSchool.Model;
using System.Web.Http.Results;
using System.Web.Http.Controllers;
using System.Web;
using System.Security.Principal;
using System.Web.Http;
using LanguageSchool.WebApi;

namespace LanguageSchool.Tests
{
    [TestFixture]
    public class StudentsControllerTests
    {
        List<ClassBasicDataDto> classesBasicData;
        List<ClassDataDto> classesData;
        Mock<IClassBLL> mockClassBLL;
        Mock<IStudentBLL> mockStudentBLL;
        StudentsController studentsController;
        public StudentsControllerTests()
        {
            List<Language> languages;
            List<LanguageLevel> languageLevels;
            List<Class> classes;
            List<Student> students;
            languages = new List<Language>()
            {
                new Language()
                {
                    Id=1,
                    LanguageName = "English"
                },
                new Language()
                {
                    Id=2,
                    LanguageName="Spanish"
                },
                new Language()
                {
                    Id=3,
                    LanguageName="Russian"
                }
            };

            languageLevels = new List<LanguageLevel>()
            {
                new LanguageLevel()
                {
                    Id=1,
                    LanguageLevelSignature="A1"
                },
                new LanguageLevel()
                {
                    Id=2,
                    LanguageLevelSignature="A2"
                },
                new LanguageLevel()
                {
                    Id=3,
                    LanguageLevelSignature="B1"
                },
                new LanguageLevel()
                {
                    Id=4,
                    LanguageLevelSignature="B2"
                },
                new LanguageLevel()
                {
                    Id=5,
                    LanguageLevelSignature="C1"
                },
                new LanguageLevel()
                {
                    Id=6,
                    LanguageLevelSignature="C2"
                }
            };

            classes = new List<Class>()
            {
                new Class()
                {
                    Id=1,
                    ClassName="English M1",
                    LanguageRefID=1,
                    Language = languages.ElementAt(0),
                    LanguageLevelRefID=1,
                    LanguageLevel = languageLevels.ElementAt(0),
                    StartTime="10:00",
                    EndTime="11:30",
                    Day=DayOfWeek.Monday,
                    StudentsMax = 5
                },
                new Class()
                {
                    Id=2,
                    ClassName="English M14",
                    LanguageRefID=1,
                    Language = languages.ElementAt(0),
                    LanguageLevelRefID=5,
                    LanguageLevel = languageLevels.ElementAt(4),
                    StartTime="10:00",
                    EndTime="11:30",
                    Day=DayOfWeek.Tuesday,
                    StudentsMax =20
                },
                new Class()
                {
                    Id=3,
                    ClassName="Spanish M2",
                    LanguageRefID=2,
                    Language = languages.ElementAt(1),
                    LanguageLevelRefID=1,
                    LanguageLevel = languageLevels.ElementAt(0),
                    StartTime="11:00",
                    EndTime="12:30",
                    Day=DayOfWeek.Monday,
                    StudentsMax =20
                },
                new Class()
                {
                    Id=4,
                    ClassName="Spanish Conversations",
                    LanguageRefID=2,
                    Language = languages.ElementAt(1),
                    LanguageLevelRefID=4,
                    LanguageLevel = languageLevels.ElementAt(3),
                    StartTime="10:00",
                    EndTime="11:30",
                    Day=DayOfWeek.Thursday,
                    StudentsMax =20
                },
                new Class()
                {
                    Id=5,
                    ClassName="Russian M15",
                    LanguageRefID=3,
                    Language = languages.ElementAt(2),
                    LanguageLevelRefID=5,
                    LanguageLevel = languageLevels.ElementAt(4),
                    StartTime="12:00",
                    EndTime="13:30",
                    Day=DayOfWeek.Wednesday,
                    StudentsMax =20
                },
                new Class()
                {
                    Id=6,
                    ClassName="Russian M1",
                    LanguageRefID=3,
                    LanguageLevelRefID=1,
                    LanguageLevel = languageLevels.ElementAt(0),
                    Language = languages.ElementAt(2),
                    StartTime="10:00",
                    EndTime="11:30",
                    Day=DayOfWeek.Friday,
                    StudentsMax =20
                }
            };

            classesData = classes.Select(x => new ClassDataDto()
            {
                Id = x.Id,
                ClassName = x.ClassName,
                Language = x.Language.LanguageName,
                LanguageLevel = x.LanguageLevel.LanguageLevelSignature,
                StudentsCount = x.Students.Count,
                StudentsMax = x.StudentsMax,
                StartTime = x.StartTime,
                EndTime = x.EndTime
            }
                ).ToList();

            mockClassBLL = new Mock<IClassBLL>();
            mockStudentBLL = new Mock<IStudentBLL>();
            studentsController = new StudentsController(mockStudentBLL.Object, mockClassBLL.Object)
            {
                CurrentUserId = () => "abc",
                CurrentUserName = () => "TestUser"
            };
        }

        [Test]
        public void GetClasses_Always_ReturnsExpectedResult()
        {
            mockStudentBLL.Setup(x => x.GetClasses("abc")).Returns(new int[] { 1, 2});
            mockClassBLL.Setup(x => x.GetByID(It.IsAny<int>())).Returns((int id) => classesData.Where(x => x.Id == id).FirstOrDefault());

            var actionResult = studentsController.GetClasses() as OkNegotiatedContentResult<List<ClassDataDto>>;
            Assert.That(actionResult.Content.Count, Is.EqualTo(2));
        }
        [Test]
        public void GetClasses_WhenIdNotExist_ReturnsBadRequest()
        {
            mockStudentBLL.Setup(x => x.GetClasses(It.IsAny<string>())).Returns((int[])(null));

            IHttpActionResult actionResult = studentsController.GetInformations();
            Assert.IsInstanceOf(typeof(NotFoundResult), actionResult);
        }

        [Test]
        public void GetInformations_Always_ReturnsExpectedResult()
        {
            mockStudentBLL.Setup(x => x.GetById("abc")).Returns(
                new StudentDataDto
                {
                    Id = "abc",
                    FirstName = "Kasia",
                    LastName = "Nowak",
                    Email = "email@wp.pl",
                    PhoneNumber = "452369874"
                });

            var actionResult = studentsController.GetInformations() as OkNegotiatedContentResult<StudentDataDto>;
            Assert.That(actionResult.Content.LastName, Is.EqualTo("Nowak"));
            Assert.That(actionResult.Content.UserName, Is.EqualTo("TestUser"));
        }

        [Test]
        public void GetInformations_WhenIdNotExist_ReturnsBadRequest()
        {
            mockStudentBLL.Setup(x => x.GetById(It.IsAny<string>())).Returns((StudentDataDto)(null));

            IHttpActionResult actionResult = studentsController.GetInformations();
            Assert.IsInstanceOf(typeof(NotFoundResult), actionResult);
        }

        [Test]
        public void PutInformations_Always_ReturnsNoContentStatusCode()
        {
            EditProfileModel model = new EditProfileModel()
            {
                FirstName = "Kasia",
                LastName = "Nowak",
                Email = "kasia75@wp.pl",
                PhoneNumber = "456897123"
            };
            mockStudentBLL.Setup(x => x.Update("abc", "Kasia", "Nowak", "kasia75@wp.pl", "456897123")).Returns((string)null);

            IHttpActionResult actionResult = studentsController.PutInformations(model);
            Assert.IsInstanceOf(typeof(StatusCodeResult), actionResult);
        }

        [Test]
        public void PutInformations_WhenIdNotExist_ReturnsBadRequest()
        {
            EditProfileModel model = new EditProfileModel()
            {
                FirstName = "Kasia5",
                LastName = "Nowak",
                Email = "kasia75@wp.pl",
                PhoneNumber = "456897123"
            };
            mockStudentBLL.Setup(x => x.Update(It.IsAny<string>(), model.FirstName, model.LastName, model.Email, model.PhoneNumber)).Returns("Invalid First Name");

            IHttpActionResult actionResult = studentsController.PutInformations(model);
            Assert.IsInstanceOf(typeof(BadRequestErrorMessageResult), actionResult);
        }


    }
}
