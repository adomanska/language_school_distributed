using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using LanguageSchool.Model;
using System.Linq.Expressions;
namespace LanguageSchool.DataAccess
{
    public enum SearchBy { Email, LastName };
    public class StudentDAL: IStudentDAL
    {
        ILanguageSchoolContext _context;

        public StudentDAL(ILanguageSchoolContext context)
        {
            _context = context;
        }
        public List<Student> GetAll()
        {
            IQueryable<Student> students;
            students = _context.Students;
            return students.ToList();
        }
        public void Add(Student student)
        {
            try
            {
                _context.Students.Add(student);
                _context.SaveChanges();
            }
            catch
            {
                throw new Exception("Student with such email address already exists in the database");
            }
        }

        public Student FindByEmail(string email)
        {
            Student student = _context.Students.FirstOrDefault(x => x.Email == email);
            return student;
        }

        public Student GetById(string id)
        {
            Student student = _context.Students.FirstOrDefault(x => x.Id == id);
            return student;
        }

        public void Update(string id, string firstName, string lastName, string email, string phoneNumber)
        {
            try
            {
                Student existingStudent = _context.Students.Where(x => x.Id == id).FirstOrDefault();

                existingStudent.FirstName = firstName;
                existingStudent.LastName = lastName;
                existingStudent.Email = email;
                existingStudent.PhoneNumber = phoneNumber;

                if (_context.Entry(existingStudent) != null)
                    _context.Entry(existingStudent).State = System.Data.Entity.EntityState.Modified;
                _context.SaveChanges();
            }
            catch
            {
                throw new Exception("Update failed");
            }
        }

        public List<Student> Search(SearchBy type, string text, bool sorted)
        {
            var query = _context.Students.AsQueryable();
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

        public void SignForClass(Student student, Class languageClass)
        {
            student.Classes.Add(languageClass);
            _context.SaveChanges();
        }

        public void UnsubscribeFromClass(Student student, Class languageCLass)
        {
            student.Classes.Remove(languageCLass);
            _context.SaveChanges();
        }

        public Class GetClassByID(int ID)
        {
            Class _class;
            _class = _context.Classes.Where(x => x.Id == ID).Select(x => x).FirstOrDefault();
            return _class;
        }

        public List<Class> GetClasses(string id)
        {
            var student =  _context.Students.Where(x => x.Id == id).FirstOrDefault();
            if (student != null)
                return student.Classes.ToList();
            else
                return null;
        }
    }

}
