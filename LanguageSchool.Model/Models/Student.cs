using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageSchool.Model
{
    public class Student: Entity
    {
        public Student()
        {
            this.Classes = new Collection<Class>();
        }

        [Required]
        //[Index(IsUnique = true)]
        public string Email { get; set; }
        public string HashedPassword { get; set; }
        public string Salt { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }

        public virtual ICollection<Class> Classes { get; set; }
    }
}
