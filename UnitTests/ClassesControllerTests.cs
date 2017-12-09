using LanguageSchool.BusinessLogic;
using LanguageSchool.Model;
using LanguageSchool.Shared.Dtos;
using LanguageSchool.WebApi.Controllers;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Web.Http.Results;
using System.Web.Http;

namespace LanguageSchool.Tests
{
    [TestFixture]
    public class ClassesControllerTests
    {
        List<ClassBasicDataDto> classesBasicData;
        List<ClassDataDto> classesData;
        Mock<IClassBLL> mockClassBLL;
        Mock<IStudentBLL> mockStudentBLL;
        ClassesController classesController;
        String userId = "abc";
        public ClassesControllerTests()
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

            classesBasicData = classes.Select(x => new ClassBasicDataDto()
            {
                ClassName = x.ClassName,
                Language = x.Language.LanguageName,
                LanguageLevel = x.LanguageLevel.LanguageLevelSignature
            }
                ).ToList();

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
            classesController = new ClassesController(mockStudentBLL.Object, mockClassBLL.Object)
            {
                CurrentUserId = () => "abc",
                CurrentUserName = () => "TestUser"
            };
        }

        [TestCase]
        public void GetAll_Always_ReturnsAllClasses()
        {
            mockClassBLL.Setup(mr => mr.GetAll()).Returns(classesData);
            var actionResult = classesController.Get() as OkNegotiatedContentResult<List<ClassDataDto>>;
            Assert.That(actionResult.Content.Count, Is.EqualTo(6));
        }

        [TestCase(-1)]
        [TestCase(10)]
        public void GetById_InvalidId_ReturnsNotFound(int ID)
        {
            mockClassBLL.Setup(mr => mr.GetByID(It.IsAny<int>())).Returns((int id) => classesData.Where(x => x.Id == id).FirstOrDefault());
            var actionResult = classesController.Get(ID);
            Assert.IsInstanceOf(typeof(NotFoundResult), actionResult);
        }

        [TestCase(1)]
        [TestCase(4)]
        public void GetById_ValidId_ReturnsClass(int ID)
        {
            mockClassBLL.Setup(mr => mr.GetByID(It.IsAny<int>())).Returns((int id) => classesData.Where(x => x.Id == id).FirstOrDefault());
            var actionResult = classesController.Get(ID);
            Assert.IsNotInstanceOf(typeof(NotFoundResult), actionResult);
            Assert.IsInstanceOf(typeof(ClassDataDto), (actionResult as OkNegotiatedContentResult<ClassDataDto>).Content);
        }

        [TestCase("English", "C1", 1)]
        [TestCase("Russian", "A1", 1)]
        public void GetClasses_Always_ReturnsExpectedResult(string lang, string level, int count)
        {
            mockClassBLL.Setup(mr => mr.GetClasses(It.IsAny<string>(), It.IsAny<string>())).Returns(
               (string language, string languageLevel) => classesBasicData.Where(x => x.Language == language && x.LanguageLevel == languageLevel).ToList());
            var actionResult = classesController.Get(lang, level) as OkNegotiatedContentResult<List<ClassBasicDataDto>>;
            Assert.That(actionResult.Content.Count, Is.EqualTo(count));
        }

        [Test]
        public void GetTop_WhenLessThan0_ReturnsBadRequest()
        {
            var actionResult = classesController.GetTop(-2);
            Assert.IsInstanceOf(typeof(BadRequestErrorMessageResult), actionResult);
        }

        [Test]
        public void GetTop_WhenSthWentWrong_ReturnsNotFound()
        {
            mockClassBLL.Setup(x => x.GetTopClasses(It.IsAny<int>())).Returns((List<ClassBasicDataDto>)null);
            var actionResult = classesController.GetTop(5);
            Assert.IsInstanceOf(typeof(NotFoundResult), actionResult);
        }

        [Test]
        public void GetTop_Always_ReturnsCorrectResult()
        {
            mockClassBLL.Setup(x => x.GetTopClasses(It.IsAny<int>())).Returns(classesBasicData.Take(5).ToList());
            var actionResult = classesController.GetTop(5) as OkNegotiatedContentResult<List<ClassBasicDataDto>>;
            Assert.That(actionResult.Content.Count, Is.EqualTo(5));
        }

        [Test]
        public void GetSuggested_Always_ReturnsCorrectResult()
        {
            mockClassBLL.Setup(x => x.GetSuggestedClasses(userId, 1)).Returns(classesBasicData.Take(2).ToList());
            var actionResult = classesController.GetSuggested() as OkNegotiatedContentResult<List<ClassBasicDataDto>>;
            Assert.That(actionResult.Content.Count, Is.EqualTo(2));
        }

        [Test]
        public void SignFor_WhenIdLessThan0_ReturnsBadRequest()
        {
            var actionResult = classesController.SignFor(-2);
            Assert.IsInstanceOf(typeof(BadRequestErrorMessageResult), actionResult);
        }

        [Test]
        public void SignFor_WhenErrorOccurs_ReturnsBadRequest()
        {
            mockStudentBLL.Setup(x => x.SignForClass(userId, It.IsAny<int>())).Returns("Error message");
            var actionResult = classesController.SignFor(1);
            Assert.IsInstanceOf(typeof(BadRequestErrorMessageResult), actionResult);
        }

        [Test]
        public void SignFor_Always_ReturnsOk()
        {
            mockStudentBLL.Setup(x => x.SignForClass(userId, It.IsAny<int>())).Returns((string)null);
            var actionResult = classesController.SignFor(1);
            Assert.IsInstanceOf(typeof(OkResult), actionResult);
        }

        [Test]
        public void DeleteSubscription_WhenIdLessThan0_ReturnsBadRequest()
        {
            var actionResult = classesController.DeleteSubscription(-2);
            Assert.IsInstanceOf(typeof(BadRequestErrorMessageResult), actionResult);
        }

        [Test]
        public void DeleteSubscription_WhenErrorOccurs_ReturnsBadRequest()
        {
            mockStudentBLL.Setup(x => x.UnsubscribeFromClass(userId, It.IsAny<int>())).Returns("Error message");
            var actionResult = classesController.DeleteSubscription(1);
            Assert.IsInstanceOf(typeof(BadRequestErrorMessageResult), actionResult);
        }

        [Test]
        public void DeleteSubscription_Always_ReturnsOk()
        {
            mockStudentBLL.Setup(x => x.UnsubscribeFromClass(userId, It.IsAny<int>())).Returns((string)null);
            var actionResult = classesController.DeleteSubscription(1);
            Assert.IsInstanceOf(typeof(OkResult), actionResult);
        }
        
    }
}
