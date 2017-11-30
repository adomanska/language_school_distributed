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
using Microsoft.AspNet.Identity;

namespace LanguageSchool.WebApi.Controllers
{
    public class ClassesController : ApiController
    {
        IClassBLL _classService;
        IStudentBLL _studentService;

        public ClassesController(IStudentBLL studentService, IClassBLL classService)
        {
            _studentService = studentService;
            _classService = classService;
        }

        public IHttpActionResult Get()
        {
            var classes = _classService.GetAll();
            return Ok(classes);
        }

        public IHttpActionResult Get(int id)
        {
            var _class = _classService.GetByID(id);
            if (_class == null)
                return NotFound();
            else
                return Ok(_class);
        }

        public IHttpActionResult Get(string language, string languageLevel)
        {
            var classes = _classService.GetClasses(language, languageLevel);
            return Ok(classes);
        }

        [Route("api/classes/top/{count:int}"),HttpGet]
        public IHttpActionResult GetTop(int count)
        {
            var topClasses = _classService.GetTopClasses(count);
            return Ok(topClasses);
        }

        [Authorize]
        [Route("api/classes/suggested"), HttpGet]
        public IHttpActionResult GetSuggested()
        {
            string userId = RequestContext.Principal.Identity.GetUserId();
            var suggestedClasses = _classService.GetSuggestedClasses(userId, 1);
            return Ok(suggestedClasses);
        }

        [Authorize]
        [Route("api/classes/sign/{id:int}"), HttpGet]
        public IHttpActionResult SignFor(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid class id");

            string userId = HttpContext.Current.User.Identity.GetUserId();
            var error = _studentService.SignForClass(userId, id);
            if (error != null)
                return BadRequest(error);

            return Ok();
        }

        [Authorize]
        [Route("api/classes/{id:int}"), HttpDelete]
        public IHttpActionResult Unsubscribe(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid class id");

            string userId = HttpContext.Current.User.Identity.GetUserId();
            var error = _studentService.UnsubscribeFromClass(userId, id);
            if (error != null)
                return BadRequest(error);

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
