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
using LanguageSchool.Shared.Dtos;

namespace LanguageSchool.BusinessLogic
{
    public class ClassBLL: IClassBLL
    {
        IClassDAL classDAL;

        public ClassBLL(IClassDAL _classDAL)
        {
            classDAL = _classDAL;
        }
        public List<ClassBasicDataDto> GetAll()
        {
            var classes =  classDAL.GetAll();
            List<ClassBasicDataDto> result = new List<ClassBasicDataDto>();

            foreach(Class c in classes)
            {
                ClassBasicDataDto classData = new ClassBasicDataDto()
                {
                    ClassName = c.ClassName,
                    Language = classDAL.GetLanguage(c.Id).LanguageName,
                    LanguageLevel = classDAL.GetLanguageLevel(c.Id).LanguageLevelSignature
                };
                result.Add(classData);
            }

            return result;
        }

        public ClassDataDto GetByID (int ID)
        {
            Class c = classDAL.GetByID(ID);
            ClassDataDto classData = new ClassDataDto()
            {
                ClassName = c.ClassName,
                Language = classDAL.GetLanguage(c.Id).LanguageName,
                LanguageLevel = classDAL.GetLanguageLevel(c.Id).LanguageLevelSignature,
                StartTime = c.StartTime,
                EndTime = c.EndTime,
                StudentsCount = GetStudentsCount(c.Id),
                StudentsMax = c.StudentsMax
            };

            return classData;
        }

        public List<ClassBasicDataDto> GetClasses(string language, string level)
        {
            var classes =  classDAL.GetClasess(language, level);
            List<ClassBasicDataDto> result = new List<ClassBasicDataDto>();

            foreach (Class c in classes)
            {
                ClassBasicDataDto classData = new ClassBasicDataDto()
                {
                    ClassName = c.ClassName,
                    Language = classDAL.GetLanguage(c.Id).LanguageName,
                    LanguageLevel = classDAL.GetLanguageLevel(c.Id).LanguageLevelSignature
                };
                result.Add(classData);
            }

            return result;
        }

        public List<ClassBasicDataDto> Search(ClassFilter filter)
        {
            var classes = classDAL.Search(filter.ClassName, filter.Language == null ? -1 : filter.Language.Id, filter.LanguageLevel == null ? -1 : filter.LanguageLevel.Id);
            List<ClassBasicDataDto> result = new List<ClassBasicDataDto>();

            foreach (Class c in classes)
            {
                ClassBasicDataDto classData = new ClassBasicDataDto()
                {
                    ClassName = c.ClassName,
                    Language = classDAL.GetLanguage(c.Id).LanguageName,
                    LanguageLevel = classDAL.GetLanguageLevel(c.Id).LanguageLevelSignature
                };
                result.Add(classData);
            }

            return result;
        }

        public List<ClassBasicDataDto> GetTopClasses(int count)
        {
            if (count <= 0)
                throw new ArgumentException("Invalid argument: count has to be > 0");

            var classes =  classDAL.GetTopClasses(count).ToList();
            List<ClassBasicDataDto> result = new List<ClassBasicDataDto>();

            foreach (Class c in classes)
            {
                ClassBasicDataDto classData = new ClassBasicDataDto()
                {
                    ClassName = c.ClassName,
                    Language = classDAL.GetLanguage(c.Id).LanguageName,
                    LanguageLevel = classDAL.GetLanguageLevel(c.Id).LanguageLevelSignature
                };
                result.Add(classData);
            }

            return result;
        }

        public List<ClassBasicDataDto> GetSuggestedClasses(int studentID, int count)
        {
            if (count <= 0)
                throw new ArgumentException("Invalid argument: count has to be > 0");
            var classes = classDAL.GetSuggestedClasses(studentID);
            classes.Take(Math.Min(classes.Count(), count)).ToList();
            List<ClassBasicDataDto> result = new List<ClassBasicDataDto>();

            foreach (Class c in classes)
            {
                ClassBasicDataDto classData = new ClassBasicDataDto()
                {
                    ClassName = c.ClassName,
                    Language = classDAL.GetLanguage(c.Id).LanguageName,
                    LanguageLevel = classDAL.GetLanguageLevel(c.Id).LanguageLevelSignature
                };
                result.Add(classData);
            }

            return result;
        }

        private int GetStudentsCount(int id)
        {
            var students = classDAL.GetStudents(id);
            if (students == null)
                throw new ArgumentException();
            return students.Count;
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
