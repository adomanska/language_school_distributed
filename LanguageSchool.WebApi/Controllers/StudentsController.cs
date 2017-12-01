using LanguageSchool.BusinessLogic;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LanguageSchool.WebApi.Controllers
{
    public class StudentsController : ApiController
    {
        IStudentBLL _studentService;
        IClassBLL _classService;

        public StudentsController(IStudentBLL studentService, IClassBLL classService)
        {
            _studentService = studentService;
            _classService = classService;
        }

        [Route("api/students/classes"), HttpGet]
        public IHttpActionResult GetClasses()
        {
            string userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var classesIDs = _studentService.GetClasses(userId);
            if (classesIDs == null)
                return BadRequest("Student not found");
            return Ok(classesIDs.Select(x => _classService.GetByID(x)));
        }
    }
}
