using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageSchool.DataAccess;
using LanguageSchool.Model;
using System.Text.RegularExpressions;
using System.Data.Entity;
using LanguageSchool.Presentation;
using System.Collections.ObjectModel;
using LanguageSchool.Shared.Dtos;

namespace LanguageSchool.BusinessLogic
{
    public class StudentBLL: IStudentBLL
    {
        private IStudentDAL _studentDAL;
        private IClassDAL _classDAL;
        public StudentBLL(IStudentDAL studentDAL, IClassDAL classDAL)
        {
            _studentDAL = studentDAL;
            _classDAL = classDAL;
        }

        private bool IsValidData(string firstName, string lastName, string email, string phoneNumber = "")
        {
            string error = null;
            if (!Validator.IsFirstNameValid(firstName, ref error))
                throw new Exception(error);
            if (!Validator.IsLastNameValid(lastName, ref error))
                throw new Exception(error);
            if (!Validator.IsEmailValid(email, ref error))
                throw new Exception(error);
            if (phoneNumber != null && phoneNumber != "" && !Validator.IsPhoneNumberValid(phoneNumber, ref error))
                throw new Exception(error);

            return true;
        }
        public List<Student> GetAll()
        { 
            return _studentDAL.GetAll();
        }

        public StudentDataDto GetById (string id)
        {
            Student s = _studentDAL.GetById(id);

            StudentDataDto studentData = new StudentDataDto()
            {
                Id = s.Id,
                FirstName = s.FirstName,
                LastName = s.LastName,
                Email = s.Email,
                PhoneNumber = s.PhoneNumber
            };

            return studentData;
        }
        public void Add(string id, string firstName, string lastName, string email, string phoneNumber="")
        {
            try
            {
                Student existingStudent = _studentDAL.FindByEmail(email);
                if (existingStudent != null)
                    throw new Exception("Student with such email already exists");

                IsValidData(firstName, lastName, email, phoneNumber);

                Student student = new Student {
                    Id = id,
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    PhoneNumber = phoneNumber == "" ? null : phoneNumber,
                    Classes = new Collection<Class>()
                };

                _studentDAL.Add(student);
            }
            catch
            {
                throw;
            }
        }

        //public void SignForClass(string studentID, Class languageClass)
        //{
        //    Student student = studentDAL.FindByID(studentID);
        //    if (student != null && !student.Classes.Contains(languageClass))
        //        studentDAL.SignForClass(student, languageClass);
        //    else
        //        throw new Exception("Student is already registered for this class");
        //}

        public string SignForClass(string studentId, int classId)
        {
            Student student = _studentDAL.GetById(studentId);
            Class languageClass = _studentDAL.GetClassByID(classId);
            if (student == null)
                return "Student not found";
            if (languageClass == null)
                return "Class not found";
            if (student.Classes.Contains(languageClass))
                return "Student is already registered for this class";
            if (languageClass.Students.Count >= languageClass.StudentsMax)
                return "No vacancies";

            _studentDAL.SignForClass(student, languageClass);
            return null;
        }

        public string UnsubscribeFromClass(string studentId, int classId)
        {
            Student student = _studentDAL.GetById(studentId);
            Class languageClass = _studentDAL.GetClassByID(classId);
            if (student == null)
                return "Student not found";
            if (languageClass == null)
                return "Class not found";
            if (!student.Classes.Contains(languageClass))
                return "Student is not registered for this class";

            _studentDAL.UnsubscribeFromClass(student, languageClass);
            return null;
        }


        public string Update(string id, string firstName, string lastName, string email, string phoneNumber = "")
        {
            try
            {
                Student existingStudent = _studentDAL.FindByEmail(email);
                if (existingStudent != null && existingStudent.Id != id)
                    throw new Exception("Student with such email already exists");

                firstName = !String.IsNullOrEmpty(firstName) ? firstName : existingStudent.FirstName;
                lastName = !String.IsNullOrEmpty(lastName) ? lastName : existingStudent.LastName;
                email = !String.IsNullOrEmpty(email) ? email : existingStudent.Email;

                IsValidData(firstName, lastName, email, phoneNumber);
                _studentDAL.Update(id, firstName, lastName, email, phoneNumber == "" ? null : phoneNumber);
                return null;
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

        public (List<Student> students, int pageCount) Search(StudentFilter filter)
        {
            var query = _studentDAL.Search(filter.Filter, filter.Text, filter.IsSorted);
            var count = Math.Ceiling(((double)query.Count()) / filter.PageSize);
            var list = query.Skip(filter.PageSize * (filter.PageNumber - 1)).Take(filter.PageSize).ToList();

            return (list, (int)count);
        }

        public int[] GetClasses(string id)
        {
            var classes = _studentDAL.GetClasses(id);
            if (classes != null)
                return classes.Select(x => x.Id).ToArray();
            else
                return null;
        }
    }
}
