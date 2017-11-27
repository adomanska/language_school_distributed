using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageSchool.Model;

namespace LanguageSchool.WebApi.Providers
{
    public interface IContextProvider
    {
        ILanguageSchoolContext GetNewContext();
    }
}
