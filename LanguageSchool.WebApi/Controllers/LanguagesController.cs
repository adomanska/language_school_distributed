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
        IlanguageSchoolContext contextProvider;
        ILanguageBLL languageBLL;

        public LanguagesController()
        {
            contextProvider = new ContextProvider<LanguageSchoolContext>();
            languageBLL = new LanguageBLL(new LanguageDAL(contextProvider));
        }
        public IHttpActionResult Get()
        {
            var languages = languageBLL.GetAll();
            return Ok(languages);
        }

    }
}
