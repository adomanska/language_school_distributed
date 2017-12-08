using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LanguageSchool.DataAccess.Providers;
using LanguageSchool.DataAccess;
using LanguageSchool.BusinessLogic;
using LanguageSchool.Model;

namespace LanguageSchool.WebApi.Controllers
{
    public class LanguagesController : ApiController
    {
        ILanguageBLL _languageService;

        public LanguagesController(ILanguageBLL languageService)
        {
            _languageService = languageService;
        }
        public IHttpActionResult Get()
        {
            var languages = _languageService.GetAll();
            return Ok(languages);
        }

    }
}
