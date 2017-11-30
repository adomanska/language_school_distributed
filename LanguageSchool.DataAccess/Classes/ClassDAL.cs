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
        private ILanguageSchoolContext _context;

        public ClassDAL(ILanguageSchoolContext context)
        {
            _context = context;
        }
        public List<Class> GetAll()
        {
            IQueryable<Class> classes;
            classes = _context.Classes;

            return classes.ToList();
        }

        public Class GetByID(int ID)
        {
            Class _class;
            _class = _context.Classes.Where(x => x.Id == ID).Select(x => x).FirstOrDefault();
            
            return _class;
        }
        
        public List<Class> GetClasess(string language, string level)
        {
            List<Class> classes;
            
            classes = _context.Classes.Where(x => x.LanguageLevel.LanguageLevelSignature == level && x.Language.LanguageName == language).ToList();
            
            return classes;
        }

        public List<Class> Search (string className, int languageID, int languageLevelID)
        {
            IQueryable<Class> resultCollection;
            List<Class> classes;
            resultCollection = _context.Classes.AsQueryable();

            if (languageID != -1)
                resultCollection = resultCollection.Where(x => x.LanguageRefID == languageID);
            if (languageLevelID != -1)
                resultCollection = resultCollection.Where(x => x.LanguageLevelRefID == languageLevelID);
            if (className != null)
                resultCollection = resultCollection.Where(x => x.ClassName.Contains(className));
            classes = resultCollection.ToList();
            
            return classes;
        }

        public List<Class> GetTopClasses(int count)
        {
            List<Class> topClasses;
            
            var classes = _context.Classes.OrderByDescending(x => x.Students.Count);
            topClasses = classes.Take(Math.Min(classes.Count(), count)).ToList();
            
            return topClasses;
        }

        public List<Class> GetSuggestedClasses(string id)
        {
            List<Class> suggestedClasses;
            
            Student s = _context.Students.Where(x => x.Id == id).FirstOrDefault();
            if (s == null)
                throw new ArgumentException("Invalid id");
            suggestedClasses = s.Classes.SelectMany(x => x.Students).SelectMany(y => y.Classes).Distinct().Except(s.Classes).AsQueryable().ToList();
            
            return suggestedClasses;
        }

        public List<Student> GetStudents(int id)
        {
            var _class = _context.Classes.Where(x => x.Id == id).FirstOrDefault();
            return _class.Students.ToList();
        }

        public Language GetLanguage(int id)
        {
            var _class = _context.Classes.Where(x => x.Id == id).FirstOrDefault();
            return _class.Language;
        }

        public LanguageLevel GetLanguageLevel(int id)
        {
            var _class = _context.Classes.Where(x => x.Id == id).FirstOrDefault();
            return _class.LanguageLevel;
        }
     
    }
}
