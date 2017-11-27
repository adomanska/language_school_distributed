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
        IQueryable<Class> GetAll();
        Class GetByID(int ID);
        List<Class> GetClasess(string language, string level);
        IQueryable<Class> Search(string className, int languageID, int languageLevelID);
    }
}
