using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LanguageSchool.Model;

namespace LanguageSchool.DataAccess.Providers
{
    public class ContextProvider<T> : IlanguageSchoolContext
        where T : ILanguageSchoolContext, new()
    {
        public ILanguageSchoolContext GetNewContext()
        {
            return new T();
        }
    }
}