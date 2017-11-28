using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageSchool.Model
{
    class LanguageSchoolContextInitializer : CreateDatabaseIfNotExists<LanguageSchoolContext>
    {
        protected override void Seed(LanguageSchoolContext context)
        {
            context.LanguageLevels.Add(new LanguageLevel { LanguageLevelSignature = "A1" });
            context.LanguageLevels.Add(new LanguageLevel { LanguageLevelSignature = "A2" });
            context.LanguageLevels.Add(new LanguageLevel { LanguageLevelSignature = "B1" });
            context.LanguageLevels.Add(new LanguageLevel { LanguageLevelSignature = "B2" });
            context.LanguageLevels.Add(new LanguageLevel { LanguageLevelSignature = "C1" });
            context.LanguageLevels.Add(new LanguageLevel { LanguageLevelSignature = "C2" });

            Student s = new Student()
            {
                Email = "test@gmail.com",
                HashedPassword = "password",
                Salt = "dfsd",
                FirstName = "Tom",
                LastName = "Cruise",
                PhoneNumber = "503698745"
            };
            Student s2 = new Student()
            {
                Email = "test2@gmail.com",
                HashedPassword = "password2",
                Salt = "dfsd",
                FirstName = "Patrick",
                LastName = "Swayze",
                PhoneNumber = "503698745"
            };
            context.Students.Add(s);
            context.Students.Add(s2);

            Language lang = new Language() { LanguageName = "English" };
            context.Languages.Add(lang);
            context.SaveChanges();
            Class[] classes = new Class[]
            {
                new Class()
                {
                    ClassName = "English M1",
                    LanguageLevelRefID = 1,
                    LanguageRefID = 1,
                    StartTime = "10:00",
                    EndTime = "11:30",
                    StudentsMax=20,
                    Day = DayOfWeek.Thursday
                },
                new Class()
                {
                    ClassName = "English M14",
                    LanguageLevelRefID = 5,
                    LanguageRefID = 1,
                    StartTime = "10:00",
                    EndTime = "11:30",
                    StudentsMax=20,
                    Day = DayOfWeek.Thursday
                },
                new Class()
                {
                    ClassName = "English Conversations",
                    LanguageLevelRefID = 4,
                    LanguageRefID = 1,
                    StartTime = "10:00",
                    EndTime = "11:30",
                    StudentsMax=20,
                    Day = DayOfWeek.Thursday
                },
                new Class()
                {
                    ClassName = "English in Science",
                    LanguageLevelRefID = 3,
                    LanguageRefID = 1,
                    StartTime = "10:00",
                    EndTime = "11:30",
                    StudentsMax=20,
                    Day = DayOfWeek.Thursday
                }
            };

            classes[0].Students.Add(s);
            classes[0].Students.Add(s2);
            classes[1].Students.Add(s);
            classes[1].Students.Add(s2);
            classes[2].Students.Add(s2);
            foreach (Class c in classes)
                context.Classes.Add(c);
            context.SaveChanges();
        }
    }
}
