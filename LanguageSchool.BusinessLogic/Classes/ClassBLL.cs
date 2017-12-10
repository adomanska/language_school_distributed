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
        public List<ClassDataDto> GetAll()
        {
            try
            {
                var classes = classDAL.GetAll();
                List<ClassDataDto> result = new List<ClassDataDto>();

                foreach (Class c in classes)
                {
                ClassDataDto classData = new ClassDataDto()
                {
                    Id = c.Id,
                    ClassName = c.ClassName,
                    Language = classDAL.GetLanguage(c.Id).LanguageName,
                    LanguageLevel = classDAL.GetLanguageLevel(c.Id).LanguageLevelSignature,
                    StartTime = c.StartTime,
                    EndTime = c.EndTime,
                    StudentsCount = GetStudentsCount(c.Id),
                    StudentsMax = c.StudentsMax
                };
                result.Add(classData);
                }
                return result;
            }
            catch
            {
                return null;
            }
        }

        public ClassDataDto GetByID (int ID)
        {
            Class c = classDAL.GetByID(ID);

            if (c == null)
                return null;

            ClassDataDto classData = new ClassDataDto()
            {
                Id = c.Id,
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
            try
            {
                var classes = classDAL.GetClasess(language, level);
                List<ClassBasicDataDto> result = new List<ClassBasicDataDto>();

                foreach (Class c in classes)
                {
                    ClassBasicDataDto classData = new ClassBasicDataDto()
                    {
                        Id = c.Id,
                        ClassName = c.ClassName,
                        Language = classDAL.GetLanguage(c.Id).LanguageName,
                        LanguageLevel = classDAL.GetLanguageLevel(c.Id).LanguageLevelSignature,
                        StudentsCount = GetStudentsCount(c.Id)
                    };
                    result.Add(classData);
                }

                return result;
            }
            catch
            {
                return null;
            }
        }

        public (List<ClassBasicDataDto> classes, int pageCount) Search(ClassFilter filter)
        {
            var resultCollection = classDAL.Search(filter.ClassName, filter.LanguageId == 0 ? -1 : filter.LanguageId, filter.LanguageLevelId == 0 ? -1 : filter.LanguageLevelId);
            var count = Math.Ceiling(((double)resultCollection.Count()) / filter.PageSize);
            var classes = resultCollection.OrderBy(x => x.ClassName).Skip(filter.PageSize * (filter.PageNumber - 1)).Take(filter.PageSize).ToList();

            List<ClassBasicDataDto> result = new List<ClassBasicDataDto>();

            foreach (Class c in classes)
            {
                ClassBasicDataDto classData = new ClassBasicDataDto()
                {
                    Id = c.Id,
                    ClassName = c.ClassName,
                    Language = classDAL.GetLanguage(c.Id).LanguageName,
                    LanguageLevel = classDAL.GetLanguageLevel(c.Id).LanguageLevelSignature,
                    StudentsCount = GetStudentsCount(c.Id)
                };
                result.Add(classData);
            }

            return (result, (int)count);
        }

        public List<ClassBasicDataDto> GetTopClasses(int count)
        {
            if (count <= 0)
                throw new ArgumentException("Invalid argument: count has to be > 0");

            try
            {
                var classes = classDAL.GetTopClasses(count).ToList();
                List<ClassBasicDataDto> result = new List<ClassBasicDataDto>();

                foreach (Class c in classes)
                {
                    ClassBasicDataDto classData = new ClassBasicDataDto()
                    {
                        Id = c.Id,
                        ClassName = c.ClassName,
                        Language = classDAL.GetLanguage(c.Id).LanguageName,
                        LanguageLevel = classDAL.GetLanguageLevel(c.Id).LanguageLevelSignature,
                        StudentsCount = GetStudentsCount(c.Id)
                    };
                    result.Add(classData);
                }

                return result;
            }
            catch
            {
                throw new Exception("Loading data failed");
            }
        }

        public List<ClassBasicDataDto> GetSuggestedClasses(string studentID, int count)
        {
            if (count <= 0)
                throw new ArgumentException("Invalid argument: count has to be > 0");
            try
            {
                var classes = classDAL.GetSuggestedClasses(studentID);
                classes.Take(Math.Min(classes.Count(), count)).ToList();
                List<ClassBasicDataDto> result = new List<ClassBasicDataDto>();

                foreach (Class c in classes)
                {
                    ClassBasicDataDto classData = new ClassBasicDataDto()
                    {
                        Id = c.Id,
                        ClassName = c.ClassName,
                        Language = classDAL.GetLanguage(c.Id).LanguageName,
                        LanguageLevel = classDAL.GetLanguageLevel(c.Id).LanguageLevelSignature,
                        StudentsCount = GetStudentsCount(c.Id)
                    };
                    result.Add(classData);
                }

                return result;
            }
            catch
            {
                throw new Exception("Loading data failed");
            }
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
        public int LanguageId { get; set; }
        public int LanguageLevelId{ get; set; }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
