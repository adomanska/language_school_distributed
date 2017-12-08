using System;
using Owin;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Autofac.Integration.WebApi;
using Autofac;
using System.Reflection;
using LanguageSchool.BusinessLogic;
using LanguageSchool.WebApi.Controllers;
using LanguageSchool.DataAccess;
using LanguageSchool.Model;


[assembly: OwinStartup(typeof(LanguageSchool.WebApi.Startup))]
namespace LanguageSchool.WebApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var builder = new ContainerBuilder();
            HttpConfiguration config = new HttpConfiguration();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<ClassBLL>().As<IClassBLL>();
            builder.RegisterType<ClassDAL>().As<IClassDAL>();
            builder.RegisterType<StudentBLL>().As<IStudentBLL>();
            builder.RegisterType<StudentDAL>().As<IStudentDAL>();
            builder.RegisterType<LanguageDAL>().As<ILanguageDAL>();
            builder.RegisterType<LanguageBLL>().As<ILanguageBLL>();
            builder.RegisterType<LanguageLevelDAL>().As<ILanguageLevelDAL>();
            builder.RegisterType<LanguageLevelBLL>().As<ILanguageLevelBLL>();


            builder.RegisterType<LanguageSchoolContext>().As<ILanguageSchoolContext>();

            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            ConfigureOAuth(app);

            WebApiConfig.Register(config);
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseAutofacMiddleware(container);
            app.UseAutofacWebApi(config);
            app.UseWebApi(config);
        }

        public void ConfigureOAuth(IAppBuilder app)
        {
            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = new SimpleAuthorizationServerProvider()
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

        }


    }
}