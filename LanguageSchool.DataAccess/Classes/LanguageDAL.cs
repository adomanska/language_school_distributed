using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageSchool.Model;
using System.Data.Entity;
using System.Collections.ObjectModel;
using LanguageSchool.DataAccess.Providers;

namespace LanguageSchool.DataAccess
{
    public class LanguageDAL: ILanguageDAL
    {
        private IlanguageSchoolContext contextProvider;

        public LanguageDAL(IlanguageSchoolContext provider)
        {
            contextProvider = provider;
        }
        public List<Language> GetAll()
        {
            using (var db = contextProvider.GetNewContext())
            {
                return db.Languages.ToList();
            }
        }

        public Language GetById(int Id)
        {
            using (var db = contextProvider.GetNewContext())
            {
                return db.Languages.Where(x => x.Id == Id).Select(x => x).FirstOrDefault();
            }
        }
    }
}
