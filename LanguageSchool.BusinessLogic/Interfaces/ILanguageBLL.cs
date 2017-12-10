using LanguageSchool.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageSchool.Shared.Dtos;

namespace LanguageSchool.BusinessLogic
{
    public interface ILanguageBLL
    {
        List<LanguageDataDto> GetAll();
        Language GetById(int Id);
        Language GetByName(int name);
    }
}
