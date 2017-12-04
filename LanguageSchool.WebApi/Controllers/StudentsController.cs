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

        [Authorize]
        [Route("api/student/classes"), HttpGet]
        public IHttpActionResult GetClasses()
        {
            string userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var classesIDs = _studentService.GetClasses(userId);
            if (classesIDs == null)
                return BadRequest("Student not found");
            return Ok(classesIDs.Select(x => _classService.GetByID(x)));
        }

        [Authorize]
        [Route("api/student"), HttpGet]
        public IHttpActionResult GetInformations()
        {
            string userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var studentInfo = _studentService.GetById(userId);
            if(studentInfo == null)
                return BadRequest("Student not found");

            studentInfo.UserName = System.Web.HttpContext.Current.User.Identity.GetUserName();
            return Ok(studentInfo);
        }

        [Authorize]
        [Route("api/student"), HttpPut]
        public IHttpActionResult PutInformations(EditProfileModel editModel)
        {
            string userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
            string error = _studentService.Update(userId, editModel.FirstName, editModel.LastName, editModel.Email, editModel.PhoneNumber);

            if (error != null)
                return BadRequest(error);

            return StatusCode(HttpStatusCode.NoContent);
        }


    }
}
