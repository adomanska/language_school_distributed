using LanguageSchool.Model;
using LanguageSchool.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageSchool.BusinessLogic
{
    public interface ILanguageLevelBLL
    {
        List<LanguageLevelDataDto> GetAll();
        List<string> GetLevels(string language);
    }
}
