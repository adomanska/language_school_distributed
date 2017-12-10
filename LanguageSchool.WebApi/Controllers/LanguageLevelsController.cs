using LanguageSchool.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LanguageSchool.WebApi.Controllers
{
    public class LanguageLevelsController : ApiController
    {
        ILanguageLevelBLL _languageLevelService;

        public LanguageLevelsController(ILanguageLevelBLL languageLevelService)
        {
            _languageLevelService = languageLevelService;
        }
        public IHttpActionResult Get()
        {
            var languageLevels = _languageLevelService.GetAll();
            if (languageLevels == null)
                return NotFound();
            return Ok(languageLevels);
        }
    }
}
