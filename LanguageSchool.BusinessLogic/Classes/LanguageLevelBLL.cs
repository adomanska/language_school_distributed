using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageSchool.Model;
using LanguageSchool.DataAccess;
using System.Data.Entity;

namespace LanguageSchool.BusinessLogic
{
    public class LanguageLevelBLL: ILanguageLevelBLL
    {
        ILanguageLevelDAL languageLevelDAL;

        public LanguageLevelBLL(ILanguageLevelDAL _languageLevelDAL)
        {
            languageLevelDAL = _languageLevelDAL;
        }
        public List<LanguageLevel> GetAll()
        {
             return languageLevelDAL.GetAll().ToList();
        }

        public List<string> GetLevels(string language)
        {
            return languageLevelDAL.GetLevels(language);
        }
    }
}
