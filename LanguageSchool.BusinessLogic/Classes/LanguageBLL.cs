using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageSchool.Model;
using LanguageSchool.DataAccess;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Data.Entity;
using System.Collections.ObjectModel;
using static System.Net.Mime.MediaTypeNames;
using LanguageSchool.Shared.Dtos;

namespace LanguageSchool.BusinessLogic
{
    public class LanguageBLL: ILanguageBLL
    {
        ILanguageDAL languageDAL;

        public LanguageBLL(ILanguageDAL _languageDAL)
        {
            languageDAL = _languageDAL;
        }
        public List<LanguageDataDto> GetAll()
        {
            var languages =  languageDAL.GetAll();
            List<LanguageDataDto> result = new List<LanguageDataDto>();
            foreach(var l in languages)
            {
                LanguageDataDto languageData = new LanguageDataDto()
                {
                    LanguageName = l.LanguageName
                };
                result.Add(languageData);
            }

            return result;
        }
    
        public Language GetById(int Id)
        {
            return languageDAL.GetById(Id);
        }
    }
}
