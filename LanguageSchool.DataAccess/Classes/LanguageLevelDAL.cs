using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageSchool.Model;
using System.Data.Entity;
using LanguageSchool.DataAccess.Providers;

namespace LanguageSchool.DataAccess
{
    public class LanguageLevelDAL: ILanguageLevelDAL
    {
        IlanguageSchoolContext contextProvider;
        public LanguageLevelDAL(IlanguageSchoolContext provider)
        {
            contextProvider = provider;
        }
        public List<LanguageLevel> GetAll()
        {
            using (var db = contextProvider.GetNewContext())
            {
                return db.LanguageLevels.ToList();
            }
        }

        public List<string> GetLevels(string language)
        {
            using (var db = contextProvider.GetNewContext())
            {
                var lan = db.Languages.Where(l => l.LanguageName == language).FirstOrDefault();
                if (lan != null)
                {
                    return db.Classes.Where(c => c.Language.Id == lan.Id).Select(x => x.LanguageLevel.LanguageLevelSignature).Distinct().ToList();
                }
                return null;
            }
        }
    }
}
