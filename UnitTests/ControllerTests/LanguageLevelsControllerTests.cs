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
    class LanguageLevelsControllerTests
    {
        List<LanguageLevelDataDto> languageLevels;
        Mock<ILanguageLevelBLL> mockLanguageLevelBLL;
        LanguageLevelsController languageLevelsController;
        public LanguageLevelsControllerTests()
        {
            languageLevels = new List<LanguageLevelDataDto>()
            {
                new LanguageLevelDataDto()
                {
                    Id=1,
                    LevelSignature="A1"
                },
                new LanguageLevelDataDto()
                {
                    Id=2,
                    LevelSignature="A2"
                },
                new LanguageLevelDataDto()
                {
                    Id=3,
                    LevelSignature="B1"
                }
            };

            mockLanguageLevelBLL = new Mock<ILanguageLevelBLL>();
            languageLevelsController = new LanguageLevelsController(mockLanguageLevelBLL.Object);
        }

        [Test]
        public void Get_Always_ReturnsCorrectResult()
        {
            mockLanguageLevelBLL.Setup(x => x.GetAll()).Returns(languageLevels);
            var actionResult = languageLevelsController.Get() as OkNegotiatedContentResult<List<LanguageLevelDataDto>>;
            Assert.That(actionResult.Content.Count, Is.EqualTo(3));
        }

        [Test]
        public void Get_WhenFail_ReturnsNotFound()
        {
            mockLanguageLevelBLL.Setup(x => x.GetAll()).Returns((List<LanguageLevelDataDto>)null);
            var actionResult = languageLevelsController.Get();
            Assert.IsInstanceOf(typeof(NotFoundResult), actionResult);
        }
    }
}
