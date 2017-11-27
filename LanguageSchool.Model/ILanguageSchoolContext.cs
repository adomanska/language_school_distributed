using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageSchool.Model
{
    public interface IStudentContext : IDisposable
    {
        IDbSet<Student> Students { get; }
        int SaveChanges();
    }
    public interface ILanguageSchoolContext: IStudentContext, IDisposable
    {
        IDbSet<Class> Classes { get; }
        IDbSet<Language> Languages { get; }
        IDbSet<LanguageLevel> LanguageLevels { get; }

        DbEntityEntry Entry(Object o);
    }
}
