using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageSchool.DataAccess;
using LanguageSchool.Model;
using System.Data.Entity;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace LanguageSchool.BusinessLogic
{
    public class ClassBLL: IClassBLL
    {
        IClassDAL classDAL;

        public ClassBLL(IClassDAL _classDAL)
        {
            classDAL = _classDAL;
        }
        public List<Class> GetAll()
        {
            return classDAL.GetAll().ToList();
        }

        public Class GetByID (int ID)
        {
            return classDAL.GetByID(ID);
        }

        public List<Class> GetClasses(string language, string level)
        {
            return classDAL.GetClasess(language, level);
        }

        public (List<Class> classes, int pageCount) Search(ClassFilter filter)
        {
            var resultCollection = classDAL.Search(filter.ClassName, filter.Language == null ? -1 : filter.Language.Id, filter.LanguageLevel == null ? -1 : filter.LanguageLevel.Id);
            var count = Math.Ceiling(((double)resultCollection.Count()) / filter.PageSize);
            var list = resultCollection.OrderBy(x=> x.ClassName).Skip(filter.PageSize * (filter.PageNumber - 1)).Take(filter.PageSize).ToList();

            return (list, (int)count);
        }

        public List<Class> GetTopClasses(int count)
        {
            if (count <= 0)
                throw new ArgumentException("Invalid argument: count has to be > 0");
            if (count > classDAL.GetAll().Count())
                throw new ArgumentException("Invalid argument: count cannot be larger than classes count");

            return classDAL.GetTopClasses(count).ToList();
        }

        public List<Class> GetSuggestedClasses(int studentID, int count)
        {
            if (count <= 0)
                throw new ArgumentException("Invalid argument: count has to be > 0");
            var sugClasses = classDAL.GetSuggestedClasses(studentID);
            return sugClasses.Take(Math.Min(sugClasses.Count(), count)).ToList();
        }
    }

    public class ClassFilter
    {
        public string ClassName { get; set; }
        public Language Language { get; set; }
        public LanguageLevel LanguageLevel{ get; set; }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
