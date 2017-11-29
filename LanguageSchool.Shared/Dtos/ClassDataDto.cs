using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageSchool.Shared.Dtos
{
    public class ClassDataDto
    {
        public string ClassName { get; set; }
        public string Language { get; set; }
        public string LanguageLevel { get; set; }
        public int StudentsCount { get; set; }
        public int StudentsMax { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
}
