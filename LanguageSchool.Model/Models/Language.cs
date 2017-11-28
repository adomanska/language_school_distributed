using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageSchool.Model
{
    public class Language: Entity
    {
        [Required]
        public string LanguageName { get; set; }

        public virtual ICollection<Class> Classes { get; set; }
    }
}
