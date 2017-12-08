using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LanguageSchool.WebApi.Controllers
{
    public class BaseController : ApiController
    {
        public Func<string> CurrentUserId { get; set; }
        public Func<string> CurrentUserName { get; set; }

        public BaseController()
        {
            CurrentUserId = (() => System.Web.HttpContext.Current.User.Identity.GetUserId());
            CurrentUserName = (() => System.Web.HttpContext.Current.User.Identity.GetUserName());
        }
    }
}
