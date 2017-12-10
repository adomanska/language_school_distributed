using LanguageSchool.DataAccess;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageSchool.Model;

namespace UnitTests
{
    [TestFixture]
    class ClassDALTest
    {
        private LanguageSchoolMockContext context;
        private IClassDAL classDAL;

        public ClassDALTest()
        {
            context = new LanguageSchoolMockContext();
            classDAL = new ClassDAL(context);
        }

        [Test]
        public void GetAll_Always_ReturnsAllClasses()
        {
            var result = classDAL.GetAll().Count();
            Assert.That(result, Is.EqualTo(6));
        }

        [TestCase("English","C1",1)]
        [TestCase("Russian", "A1",1)]
        public void GetClasses_Always_ReturnExpectedResult(string language, string languageLevel, int count)
        {
            int result = classDAL.GetClasess(language, languageLevel).Count;
            Assert.That(result, Is.EqualTo(count));
        }

        [TestCase(20)]
        [TestCase(-1)]
        public void GetById_InvalidId_ReturnsNull(int id)
        {
            var result = classDAL.GetByID(id);
            Assert.IsNull(result);
        }

        [TestCase(1)]
        [TestCase(6)]
        public void GetById_Always_ReturnsClass(int id)
        {
            var result = classDAL.GetByID(id);
            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(Class), result);
        }

        [TestCase(null, 1,-1,2)]
        [TestCase("Spanish",-1,-1,2)]
        [TestCase(null,-1,1,3)]
        public void Search_Always_ReturnsExpectedResult(string className, int languageID, int languageLevelID, int count)
        {
            List<Class> result = classDAL.Search(className, languageID, languageLevelID);
            Assert.That(result.Count(), Is.EqualTo(count));
        }
    }
}
