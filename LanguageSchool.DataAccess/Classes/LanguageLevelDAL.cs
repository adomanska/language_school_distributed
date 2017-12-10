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
        ILanguageSchoolContext _context;
        public LanguageLevelDAL(ILanguageSchoolContext context)
        {
            _context = context;
        }
        public List<LanguageLevel> GetAll()
        {
                return _context.LanguageLevels.ToList();
        }

        public List<string> GetLevels(string language)
        {
                var lan = _context.Languages.Where(l => l.LanguageName == language).FirstOrDefault();
                if (lan != null)
                {
                    return _context.Classes.Where(c => c.Language.Id == lan.Id).Select(x => x.LanguageLevel.LanguageLevelSignature).Distinct().ToList();
                }
                return null;
        }

        public LanguageLevel GetBySignature(string signature)
        {
            return _context.LanguageLevels.Where(x => x.LanguageLevelSignature == signature).Select(x => x).FirstOrDefault();
        }
    }
}
