using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageSchool.Model;
using System.Data.Entity;
using System.Collections.ObjectModel;

namespace LanguageSchool.DataAccess
{
    public class LanguageDAL: ILanguageDAL
    {
        private ILanguageSchoolContext _context;

        public LanguageDAL(ILanguageSchoolContext context)
        {
            _context = context;
        }
        public List<Language> GetAll()
        {
            return _context.Languages.ToList();
        }

        public Language GetById(int Id)
        {
            return _context.Languages.Where(x => x.Id == Id).Select(x => x).FirstOrDefault();
        }
    }
}
