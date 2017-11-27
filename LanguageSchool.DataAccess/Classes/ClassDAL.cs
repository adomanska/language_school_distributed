using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageSchool.Model;
using System.Data.Entity;

namespace LanguageSchool.DataAccess
{
    public class ClassDAL: IClassDAL
    {
        private ILanguageSchoolContext db;

        public ClassDAL(ILanguageSchoolContext context)
        {
            db = context;
        }
        public IQueryable<Class> GetAll()
        {
            try
            {
                return db.Classes;
            }
            catch
            {
                throw;
            }
        }

        public Class GetByID(int ID)
        {
            Class _class = db.Classes.Where(x => x.ClassID == ID).Select(x => x).FirstOrDefault();
            return _class;
        }
        
        public List<Class> GetClasess(string language, string level)
        {
            return db.Classes.Where(x => x.LanguageLevel.LanguageLevelSignature == level && x.Language.LanguageName == language).ToList();
        }

        public IQueryable<Class> Search (string className, int languageID, int languageLevelID)
        {
            IQueryable<Class> resultCollection = db.Classes.AsQueryable();

            if (languageID != -1)
                resultCollection = resultCollection.Where(x => x.LanguageRefID == languageID);
            if (languageLevelID != -1)
                resultCollection = resultCollection.Where(x => x.LanguageLevelRefID == languageLevelID);
            if (className != null)
                resultCollection = resultCollection.Where(x => x.ClassName.Contains(className));

            return resultCollection;
        }

        public IQueryable<Class> GetTopClasses(int count)
        {
            return db.Classes.OrderByDescending(x => x.Students.Count).Take(count);
        }

        public IQueryable<Class> GetSuggestedClasses(int id)
        {
            Student s = db.Students.Where(x => x.ID == id).FirstOrDefault();
            if (s == null)
                throw new ArgumentException("Invalid id");
            return s.Classes.SelectMany(x => x.Students).SelectMany(y => y.Classes).Distinct().Except(s.Classes).AsQueryable();
        }
     
    }
}
