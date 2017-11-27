using LanguageSchool.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageSchool.BusinessLogic
{
    public interface IClassBLL
    {
        List<Class> GetAll();
        Class GetByID(int ID);
        List<Class> GetClasses(string language, string level);
        (List<Class> classes, int pageCount) Search(ClassFilter filter);
        List<Class> GetTopClasses(int count);
        List<Class> GetSuggestedClasses(int studentID, int count);
    }
}
