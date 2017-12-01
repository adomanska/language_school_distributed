using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageSchool.Model;

namespace LanguageSchool.DataAccess
{
    public interface IStudentDAL
    {
        List<Student> GetAll();
        void Add(Student student);
        void SignForClass(Student student, Class _class);
        void UnsubscribeFromClass(Student student, Class languageCLass);
        void Update(string ID, string firstName, string lastName, string email, string phoneNumber);
        Student FindByEmail(string email);
        Student FindByID(string id);
        List<Student> Search(SearchBy type, string text, bool sorted);
        Class GetClassByID(int ID);
        List<Class> GetClasses(string id);

    }
}
