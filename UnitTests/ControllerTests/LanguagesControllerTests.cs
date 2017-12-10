using LanguageSchool.BusinessLogic;
using LanguageSchool.Shared.Dtos;
using LanguageSchool.WebApi.Controllers;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace UnitTests
{
    class LanguagesControllerTests
    {
        List<LanguageDataDto> languages;
        Mock<ILanguageBLL> mockLanguageBLL;
        LanguagesController languagesController;
        public LanguagesControllerTests()
        {
            languages = new List<LanguageDataDto>()
            {
                new LanguageDataDto()
                {
                    Id=1,
                    LanguageName = "English"
                },
                new LanguageDataDto()
                {
                    Id=2,
                    LanguageName="Spanish"
                },
                new LanguageDataDto()
                {
                    Id=3,
                    LanguageName="Russian"
                }
            };

            mockLanguageBLL = new Mock<ILanguageBLL>();
            languagesController = new LanguagesController(mockLanguageBLL.Object);
        }

        [Test]
        public void Get_Always_ReturnsCorrectResult()
        {
            mockLanguageBLL.Setup(x => x.GetAll()).Returns(languages);
            var actionResult = languagesController.Get() as OkNegotiatedContentResult<List<LanguageDataDto>>;
            Assert.That(actionResult.Content.Count, Is.EqualTo(3));
        }

        [Test]
        public void Get_WhenFail_ReturnsNotFound()
        {
            mockLanguageBLL.Setup(x => x.GetAll()).Returns((List<LanguageDataDto>)null);
            var actionResult = languagesController.Get();
            Assert.IsInstanceOf(typeof(NotFoundResult), actionResult);
        }
    }
}
