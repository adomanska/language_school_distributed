using LanguageSchool.BusinessLogic;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageSchool.DataAccess;
using LanguageSchool.Model;
using LanguageSchool.Shared.Dtos;

namespace UnitTests
{
    [TestFixture]
    public class ClassBLLTests
    {
        private Mock<IClassDAL> mockClassDAL;
        private List<Class> classes;
        private IClassBLL classBLL;
        private List<LanguageLevel> languageLevels;
        private List<Language> languages;

        public ClassBLLTests()
        {
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
                    StudentsMax = 20,
                    Students = new List<Student>()
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
                    StudentsMax = 20,
                    Students = new List<Student>()
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
                    StudentsMax = 20,
                    Students = new List<Student>()
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
                    StudentsMax = 20,
                    Students = new List<Student>()
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
                    StudentsMax = 20,
                    Students = new List<Student>()
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
                    StudentsMax = 20,
                    Students = new List<Student>()
                }
            };

            mockClassDAL = new Mock<IClassDAL>();
            classBLL = new ClassBLL(mockClassDAL.Object);
        }

        [Test]
        public void GetAll_Always_ReturnsAllClasses()
        {
            mockClassDAL.Setup(mr => mr.GetAll()).Returns(classes);
            mockClassDAL.Setup(mr => mr.GetLanguage(It.IsAny<int>())).Returns((int id) => classes.Where(x => x.Id == id).FirstOrDefault().Language);
            mockClassDAL.Setup(mr => mr.GetLanguageLevel(It.IsAny<int>())).Returns((int id) => classes.Where(x => x.Id == id).FirstOrDefault().LanguageLevel);
            mockClassDAL.Setup(mr => mr.GetStudents(It.IsAny<int>())).Returns((int id) => classes.Where(x => x.Id == id).FirstOrDefault().Students.ToList());
            var result = classBLL.GetAll().Count;
            Assert.That(result, Is.EqualTo(6));
        }

        [TestCase(-1)]
        [TestCase(10)]
        public void GetById_InvalidId_ReturnsNull(int ID)
        {
            mockClassDAL.Setup(mr => mr.GetByID(It.IsAny<int>())).Returns((int id) => classes.Where(x => x.Id == id).FirstOrDefault());
            var result = classBLL.GetByID(ID);
            Assert.IsNull(result);
        }

        [TestCase(1)]
        [TestCase(4)]
        public void GetById_ValidId_ReturnsClass(int ID)
        {
            mockClassDAL.Setup(mr => mr.GetByID(It.IsAny<int>())).Returns((int id) => classes.Where(x => x.Id == id).FirstOrDefault());

            var result = classBLL.GetByID(ID);
            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(ClassDataDto), result);
        }

        [TestCase("English", "C1", 1)]
        [TestCase("Russian", "A1", 1)]
        public void GetClasses_Always_ReturnsExpectedResult(string lang, string level, int count)
        {
            mockClassDAL.Setup(mr => mr.GetClasess(It.IsAny<string>(), It.IsAny<string>())).Returns(
               (string language, string languageLevel) => classes.Where(x => x.Language.LanguageName == language && x.LanguageLevel.LanguageLevelSignature == languageLevel).ToList());
            var result = classBLL.GetClasses(lang, level).Count;
            Assert.That(result, Is.EqualTo(count));
        }

        [TestCase("M",-1,-1,5)]
        [TestCase(null,1,-1,2)]
        public void Search_Always_ReturnsExpectedResult(string className, int languageID, int languageLevelID, int classCount)
        {
            mockClassDAL.Setup(mr => mr.Search(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns(
               (string name, int langID, int levelID) =>
               {
                   var result = classes;
                   if (name != null)
                       result = result.Where(x => x.ClassName.Contains(name)).ToList();
                   if (langID != -1)
                       result = result.Where(x => x.LanguageRefID == langID).ToList();
                   if (levelID != -1)
                       result = result.Where(x => x.LanguageLevelRefID == levelID).ToList();
                   return result;
               });
            ClassFilter filter = new ClassFilter()
            {
                ClassName = className,
                Language = languageID!=-1 ? languages.ElementAt(languageID - 1) : null,
                LanguageLevel = languageLevelID!=-1 ? languageLevels.ElementAt(languageLevelID - 1) : null,
            };

            var res = classBLL.Search(filter);
            Assert.That(res.Count, Is.EqualTo(classCount));
        }
    }
}
