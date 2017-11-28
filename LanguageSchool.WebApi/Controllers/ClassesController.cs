using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LanguageSchool.WebApi.Providers;
using LanguageSchool.Model;
using LanguageSchool.BusinessLogic;
using LanguageSchool.DataAccess;

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
            return Ok(classes.Select(x=>x.ClassName));
        }

        public IHttpActionResult Get(int id)
        {
            Class _class = classBLL.GetByID(id);
            if (_class == null)
                return NotFound();
            else
                return Ok(_class.ClassName);
        }

        public IHttpActionResult Get(string language, string languageLevel)
        {
            var classes = classBLL.GetClasses(language, languageLevel);
            return Ok(classes.Select(x=>x.ClassName));
        }

        [Route("api/classes/top/{count:int}"),HttpGet]
        public IHttpActionResult GetTop(int count)
        {
            var topClasses = classBLL.GetTopClasses(count);
            return Ok(topClasses.Select(x => x.ClassName));
        }

        [Route("api/classes/suggested"), HttpGet]
        public IHttpActionResult GetSuggested()
        {
            var suggestedClasses = classBLL.GetSuggestedClasses(1, 1);
            return Ok(suggestedClasses.Select(x => x.ClassName));
        }

    }
}
