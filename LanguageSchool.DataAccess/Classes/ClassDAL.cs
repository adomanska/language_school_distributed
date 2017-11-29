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
    public class ClassDAL: IClassDAL
    {
        private IContextProvider contextProvider;

        public ClassDAL(IContextProvider provider)
        {
            contextProvider = provider;
        }
        public List<Class> GetAll()
        {
            IQueryable<Class> classes;
            using (var db = contextProvider.GetNewContext())
            {
                classes = db.Classes;

                return classes.ToList();
            }
        }

        public Class GetByID(int ID)
        {
            Class _class;
            using (var db = contextProvider.GetNewContext())
            {
                _class = db.Classes.Where(x => x.Id == ID).Select(x => x).FirstOrDefault();
            }
            return _class;
        }
        
        public List<Class> GetClasess(string language, string level)
        {
            List<Class> classes;
            using (var db = contextProvider.GetNewContext())
            {
                classes = db.Classes.Where(x => x.LanguageLevel.LanguageLevelSignature == level && x.Language.LanguageName == language).ToList();
            }
            return classes;
        }

        public List<Class> Search (string className, int languageID, int languageLevelID)
        {
            IQueryable<Class> resultCollection;
            List<Class> classes;
            using (var db = contextProvider.GetNewContext())
            {
                resultCollection = db.Classes.AsQueryable();

                if (languageID != -1)
                    resultCollection = resultCollection.Where(x => x.LanguageRefID == languageID);
                if (languageLevelID != -1)
                    resultCollection = resultCollection.Where(x => x.LanguageLevelRefID == languageLevelID);
                if (className != null)
                    resultCollection = resultCollection.Where(x => x.ClassName.Contains(className));
                classes = resultCollection.ToList();
            }
            return classes;
        }

        public List<Class> GetTopClasses(int count)
        {
            List<Class> topClasses;
            using (var db = contextProvider.GetNewContext())
            {
                var classes = db.Classes.OrderByDescending(x => x.Students.Count);
                topClasses = classes.Take(Math.Min(classes.Count(), count)).ToList();
            }
            return topClasses;
        }

        public List<Class> GetSuggestedClasses(int id)
        {
            List<Class> suggestedClasses;
            using (var db = contextProvider.GetNewContext())
            {
                Student s = db.Students.Where(x => x.Id == id).FirstOrDefault();
                if (s == null)
                    throw new ArgumentException("Invalid id");
                suggestedClasses = s.Classes.SelectMany(x => x.Students).SelectMany(y => y.Classes).Distinct().Except(s.Classes).AsQueryable().ToList();
            }
            return suggestedClasses;
        }
     
    }
}
