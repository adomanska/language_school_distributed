using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageSchool.DataAccess;
using LanguageSchool.Model;
using LanguageSchool.Shared.Dtos;

namespace LanguageSchool.BusinessLogic
{
    public interface IStudentBLL
    {
        List<Student> GetAll();
        StudentDataDto GetById(string id);
        void Add(string id, string firstName, string lastName, string email, string phoneNumber = "");
        string SignForClass(string studentId, int classId);
        string UnsubscribeFromClass(string studentId, int classId);
        string Update(string id, string firstName, string lastName, string email, string phoneNumber = "");
        (List<Student> students, int pageCount) Search(StudentFilter filter);
        int[] GetClasses(string id);

    }
}
