﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LanguageSchool.Model;

namespace LanguageSchool.WebApi.Providers
{
    public class ContextProvider<T> : IContextProvider
        where T : ILanguageSchoolContext, new()
    {
        public ILanguageSchoolContext GetNewContext()
        {
            return new T();
        }
    }
}