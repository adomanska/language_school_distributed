using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using LanguageSchool.DataAccess;
using LanguageSchool.BusinessLogic;
using LanguageSchool.Model;
using Moq;
using System.Data.Entity;

namespace UnitTests
{
    [TestFixture]
    class LanguageBLLTest
    {
        Mock<ILanguageDAL> mockLanguageDAL;
        ILanguageBLL languageBLL;
        List<Language> languages;

        public LanguageBLLTest()
        {
            mockLanguageDAL = new Mock<ILanguageDAL>();
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
            mockLanguageDAL.Setup(mr => mr.GetAll()).Returns(languages);
            languageBLL = new LanguageBLL(mockLanguageDAL.Object);
        }

        [Test]
        public void GetAll_ALways_RetunsAllLanguages()
        {
            var result = languageBLL.GetAll().Count;
            Assert.That(result, Is.EqualTo(3));
        }

        [TestCase(-1)]
        [TestCase(10)]
        public void GetById_InvalidId_ReturnsNull(int id)
        {
            mockLanguageDAL.Setup(mr => mr.GetById(It.IsAny<int>())).Returns((int ID) => languages.Where(x => x.Id == ID).FirstOrDefault());
            var result = languageBLL.GetById(id);
            Assert.IsNull(result);
        }

        [TestCase(1)]
        [TestCase(2)]
        public void GetById_ValidId_ReturnsLanguage(int id)
        {
            mockLanguageDAL.Setup(mr => mr.GetById(It.IsAny<int>())).Returns((int ID) => languages.Where(x => x.Id == ID).FirstOrDefault());
            var result = languageBLL.GetById(id);
            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(Language), result);
        }
    }
}
