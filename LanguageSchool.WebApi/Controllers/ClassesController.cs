using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LanguageSchool.DataAccess.Providers;
using LanguageSchool.Model;
using LanguageSchool.BusinessLogic;
using LanguageSchool.DataAccess;
using System.Web;
using LanguageSchool.Shared.Dtos;

namespace LanguageSchool.WebApi.Controllers
{
    public class ClassesController : ApiController
    {
        IContextProvider contextProvider;
        ClassBLL classBLL;

        public ClassesController()
        {
            contextProvider = new ContextProvider<LanguageSchoolContext>();
            classBLL = new ClassBLL(new ClassDAL(contextProvider));
        }

        public IHttpActionResult Get()
        {
            var classes = classBLL.GetAll();
            return Ok(classes);
        }

        public IHttpActionResult Get(int id)
        {
            var _class = classBLL.GetByID(id);
            if (_class == null)
                return NotFound();
            else
                return Ok(_class);
        }

        public IHttpActionResult Get(string language, string languageLevel)
        {
            var classes = classBLL.GetClasses(language, languageLevel);
            return Ok(classes);
        }

        [Route("api/classes/top/{count:int}"),HttpGet]
        public IHttpActionResult GetTop(int count)
        {
            var topClasses = classBLL.GetTopClasses(count);
            return Ok(topClasses);
        }

        [Authorize]
        [Route("api/classes/suggested"), HttpGet]
        public IHttpActionResult GetSuggested()
        {
            //string username = HttpContext.Current.User.Identity.Name;
            var suggestedClasses = classBLL.GetSuggestedClasses(1, 1);
            return Ok(suggestedClasses);
        }

    }
}
