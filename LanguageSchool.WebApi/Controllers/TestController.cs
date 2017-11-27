using LanguageSchool.Model;
using LanguageSchool.WebApi.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LanguageSchool.WebApi.Controllers
{
    public class TestController : ApiController
    {
        public IHttpActionResult Get()
        {
            //var provider = new ContextProvider<LanguageSchoolContext>();
            using (var context = new LanguageSchoolContext())
            {
                Student s = new Student()
                {
                    Email = "email@gmail.com",
                    HashedPassword = "password",
                    Salt = "dfsd",
                    FirstName = "Monica",
                    LastName = "Cruise",
                    PhoneNumber = "503698745"
                };
                context.Students.Add(s);
                context.SaveChanges();
            }

            return Ok();
        }
    }
}
