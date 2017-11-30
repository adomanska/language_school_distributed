using LanguageSchool.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageSchool.DataAccess
{
    public interface IClassDAL
    {
        List<Class> GetAll();
        Class GetByID(int ID);
        List<Class> GetClasess(string language, string level);
        List<Class> Search(string className, int languageID, int languageLevelID);
        List<Class> GetTopClasses(int count);
        List<Class> GetSuggestedClasses(string id);
        List<Student> GetStudents(int id);
        Language GetLanguage(int id);
        LanguageLevel GetLanguageLevel(int id);
    }
}
