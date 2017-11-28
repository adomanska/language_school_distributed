using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using LanguageSchool.Model;
using System.Linq.Expressions;
using LanguageSchool.WebApi.Providers;

namespace LanguageSchool.DataAccess
{
    public enum SearchBy { Email, LastName };
    public class StudentDAL: IStudentDAL
    {
        IContextProvider contextProvider;

        public StudentDAL(IContextProvider provider)
        {
            contextProvider = provider;
        }
        public List<Student> GetAll()
        {
            using (var db = contextProvider.GetNewContext())
            {
                return db.Students.ToList();
            }
        }
        public void Add(Student student)
        {
            using (var db = contextProvider.GetNewContext())
            {
                try
                {
                    db.Students.Add(student);
                    db.SaveChanges();
                }
                catch
                {
                    throw new Exception("Student with such email address already exists in the database");
                }
            }
        }

        public Student FindByEmail(string email)
        {
            using (var db = contextProvider.GetNewContext())
            {
                Student student = db.Students.FirstOrDefault(x => x.Email == email);
                return student;
            }
        }

        public Student FindByID(int id)
        {
            using (var db = contextProvider.GetNewContext())
            {
                Student student = db.Students.FirstOrDefault(x => x.Id == id);
                return student;
            }
        }

        public void Update(int id, string firstName, string lastName, string email, string phoneNumber)
        {
            using (var db = contextProvider.GetNewContext())
            {
                try
                {
                    Student existingStudent = db.Students.Where(x => x.Id == id).FirstOrDefault();

                    existingStudent.FirstName = firstName;
                    existingStudent.LastName = lastName;
                    existingStudent.Email = email;
                    existingStudent.PhoneNumber = phoneNumber;

                    if (db.Entry(existingStudent) != null)
                        db.Entry(existingStudent).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                catch
                {
                    throw new Exception("Update failed");
                }
            }
            
        }

        public List<Student> Search(SearchBy type, string text, bool sorted)
        {
            using (var db = contextProvider.GetNewContext())
            {
                var query = db.Students.AsQueryable();
                Expression<Func<Student, string>> expression;
                if (type == SearchBy.Email)
                {
                    expression = x => x.Email;
                    if (text != null) query = query.Where(x => x.Email.Contains(text));
                }
                else
                {
                    expression = x => x.LastName;
                    if (text != null) query = query.Where(x => x.LastName.Contains(text));
                }
                query = query.OrderBy(x => x.Id);
                if (sorted)
                    query = query.OrderBy(expression);
                return query.ToList();
            }
        }

        public void SignForClass(Student student, Class languageClass)
        {
            using (var db = contextProvider.GetNewContext())
            {
                student.Classes.Add(languageClass);
                db.SaveChanges();
            }
        }
    }
    
}
