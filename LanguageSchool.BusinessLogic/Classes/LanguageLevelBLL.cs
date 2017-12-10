using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageSchool.Model;
using LanguageSchool.DataAccess;
using System.Data.Entity;
using LanguageSchool.Shared.Dtos;

namespace LanguageSchool.BusinessLogic
{
    public class LanguageLevelBLL: ILanguageLevelBLL
    {
        ILanguageLevelDAL languageLevelDAL;

        public LanguageLevelBLL(ILanguageLevelDAL _languageLevelDAL)
        {
            languageLevelDAL = _languageLevelDAL;
        }
        public List<LanguageLevelDataDto> GetAll()
        {
            var levels = languageLevelDAL.GetAll();
            List<LanguageLevelDataDto> result = new List<LanguageLevelDataDto>();
            foreach (var l in levels)
            {
                LanguageLevelDataDto languageData = new LanguageLevelDataDto()
                {
                    Id = l.Id,
                    LevelSignature = l.LanguageLevelSignature
                };
                result.Add(languageData);
            }

            return result;
        }

        public List<string> GetLevels(string language)
        {
            return languageLevelDAL.GetLevels(language);
        }
    }
}
